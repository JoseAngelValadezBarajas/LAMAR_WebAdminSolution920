#!groovy​
pipeline {
    agent none

    options {
        buildDiscarder(logRotator(numToKeepStr: '10'))
     }

    environment {
        SERVICES_OUTPUT_PREFIX = 'PowerCampusWebAdministrationServices'
        UI_OUTPUT_PREFIX = 'PowerCampusWebAdministrationUI'
        OUTPUT_PREFIX = 'PowerCampusWebAdministration'
        OUTPUT_EXT = 'Installers.zip'
        INSTALLER_EXT = "Setup.exe"
        SOLUTION_NAME = 'WebAdminSolution.sln'
        UI_PROJECT_NAME = 'WebAdminUI\\WebAdminUI.csproj'
        SERVICES_PROJECT_NAME = 'WebAdminServices\\WebAdminServices.csproj'
        CONFIGURATION = 'Release'
        PROJECT_FOLDER = 'WebAdmin'
    }

    stages {
        stage('dotNet') {
            agent { node { label 'PowerCampus-NetApplications'}}

            environment {
                PROJECT_VERSION = readFile 'build/version.txt'
            }

            stages {
                stage('Build') {
                    steps {
                        script {
                            powershell '$outputFileName = ($ENV:OUTPUT_PREFIX + ($ENV:PROJECT_VERSION -replace \'[.]\', \'\') + $ENV:OUTPUT_EXT) | Out-File ($ENV:WORKSPACE + "\\build\\outputFileName.txt") -NoNewLine -Encoding ASCII'
                            env.OUTPUT_FILE = readFile 'build/outputFileName.txt'
                            powershell '$outputFileName = ($ENV:UI_OUTPUT_PREFIX + ($ENV:PROJECT_VERSION -replace \'[.]\', \'\') + $ENV:INSTALLER_EXT) | Out-File ($ENV:WORKSPACE + "\\build\\uiOutputFileName.txt") -NoNewLine -Encoding ASCII'
                            env.UI_OUTPUT_FILE = readFile 'build/uiOutputFileName.txt'
                            powershell '$outputFileName = ($ENV:SERVICES_OUTPUT_PREFIX + ($ENV:PROJECT_VERSION -replace \'[.]\', \'\') + $ENV:INSTALLER_EXT) | Out-File ($ENV:WORKSPACE + "\\build\\servicesOutputFileName.txt") -NoNewLine -Encoding ASCII'
                            env.SERVICES_OUTPUT_FILE = readFile 'build/servicesOutputFileName.txt'
                            echo 'Output File Name: ' + env.OUTPUT_FILE
                            echo 'UI installer file name:' + env.UI_OUTPUT_FILE
                            echo 'Services installer file name:' + env.SERVICES_OUTPUT_FILE
                            echo 'Product version: ' + env.PROJECT_VERSION
                        }    
                        powershell '.\\build\\UpdateAssemblyInfoFiles.ps1'
                        bat '''
                            nuget restore "%WORKSPACE%\\%SOLUTION_NAME%" -MSBuildVersion 15.9 -NonInteractive

                            "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\MSBuild\\15.0\\Bin\\MSBuild.exe" "%WORKSPACE%\\%SOLUTION_NAME%" /p:Configuration=%CONFIGURATION%  /t:build /nologo /m
                            "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\MSBuild\\15.0\\Bin\\MSBuild.exe" "%WORKSPACE%\\%UI_PROJECT_NAME%" /p:Configuration=%CONFIGURATION%;PackageLocation="%WORKSPACE%\\Output\\WebAdminUI.zip" /t:Package /nologo /m
                            "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\MSBuild\\15.0\\Bin\\MSBuild.exe" "%WORKSPACE%\\%SERVICES_PROJECT_NAME%" /p:Configuration=%CONFIGURATION%;PackageLocation="%WORKSPACE%\\Output\\WebAdminServices.zip" /t:Package /nologo /m
                        '''
                        powershell '''
                            .\\build\\exportBuildInfo.ps1 -outputFile "$ENV:WORKSPACE\\WebAdminUI\\obj\\$ENV:CONFIGURATION\\Package\\PackageTmp\\App_Data\\buildInfo.json"
                            .\\build\\exportBuildInfo.ps1 -outputFile "$ENV:WORKSPACE\\WebAdminServices\\obj\\$ENV:CONFIGURATION\\Package\\PackageTmp\\App_Data\\buildInfo.json"
                            '''
                    }
                    post {
                          success {
                              dir("WebAdminUI\\obj\\${CONFIGURATION}\\Package\\PackageTmp")
                              {
                                  stash includes: '**\\*', name: 'uiSourceforinstaller'
                              }
                              dir("WebAdminServices\\obj\\${CONFIGURATION}\\Package\\PackageTmp")
                              {
                                  stash includes: '**\\*', name: 'servicesSourceforinstaller'
                              }
                        }
                    }
                }
            }
            post {
                cleanup {
                    echo "Cleaning up workspace ${env.WORKSPACE}"
                    deleteDir()
                }
            }
        }
        stage('Installer')
        {
            agent { node { label 'PowerCampus-Backoffice'}}

            environment {
                PROJECT_VERSION =  readFile 'build/version.txt'
                UI_INSTALLER_NAME = 'Installer\\MVC.ism'
                SERVICES_INSTALLER_NAME = "Installer\\Services.ism"
                UI_PRODUCT_CODE = readFile 'build/mvcproductcode.txt'
                SERVICES_PRODUCT_CODE = readFile 'build/servicesproductcode.txt'
            }

            stages {
                stage('InstallShield') {
                    steps {
                        echo "Product version: ${env.PROJECT_VERSION}"
                        dir ('Installer/MVC/Source')
                        {
                            unstash 'uiSourceforinstaller'
                        }
                        dir('Installer/Services/Source')
                        {
                            unstash 'servicesSourceforinstaller'
                        }
                        rtDownload (
                            serverId: 'ArtifactoryProd',
                            spec: '''{
                                  "files": [
                                    {
                                      "pattern": "powercampus-generic/INSTALL_SUPPORT/2022/*.*",
                                      "target": "Installer/",
                                      "flat" : "true"
                                    },
                                    {
                                      "pattern": "powercampus-generic/UTILITIES/EDpfxScript.ps1",
                                      "target": "zip/",
                                      "flat" : "true"
                                    }
                                  ]
                            }'''
                        )
                        
                        bat '''
                            c:\\Windows\\SysWow64\\WindowsPowerShell\\v1.0\\powershell -executionpolicy bypass -file "%WORKSPACE%\\build\\updateInstallShieldProject.ps1" -project "%WORKSPACE%\\%UI_INSTALLER_NAME%" -buildNumber %BUILD_NUMBER% -version %PROJECT_VERSION% -productCode %UI_PRODUCT_CODE% -setupFile %UI_OUTPUT_PREFIX%
                             "%IS_LATEST_PATH%\\IsCmdBld.exe" -p "%WORKSPACE%\\%UI_INSTALLER_NAME%" -r networkImage -c COMP -a AutomatedBuild -l PATH_TO_CERT_FILES=c:\\Cert
                             "%SIGNTOOL_PATH%\\signtool.exe" sign /v /sm /s Root /n "Ellucian Company L.P." /t http://timestamp.digicert.com/scripts/timstamp.dll "%WORKSPACE%\\Installer\\MVC\\AutomatedBuild\\networkImage\\DiskImages\\DISK1\\%UI_OUTPUT_FILE%"

                             c:\\Windows\\SysWow64\\WindowsPowerShell\\v1.0\\powershell -executionpolicy bypass -file "%WORKSPACE%\\build\\updateInstallShieldProject.ps1" -project "%WORKSPACE%\\%SERVICES_INSTALLER_NAME%" -buildNumber %BUILD_NUMBER% -version %PROJECT_VERSION% -productCode %SERVICES_PRODUCT_CODE% -setupFile %SERVICES_OUTPUT_PREFIX%
                             "%IS_LATEST_PATH%\\IsCmdBld.exe" -p "%WORKSPACE%\\%SERVICES_INSTALLER_NAME%" -r networkImage -c COMP -a AutomatedBuild -l PATH_TO_CERT_FILES=c:\\Cert
                             "%SIGNTOOL_PATH%\\signtool.exe" sign /v /sm /s Root /n "Ellucian Company L.P." /t http://timestamp.digicert.com/scripts/timstamp.dll "%WORKSPACE%\\Installer\\Services\\AutomatedBuild\\networkImage\\DiskImages\\DISK1\\%SERVICES_OUTPUT_FILE%"
                         '''
                         powershell '''
                                $sourceFolder = "$ENV:WORKSPACE\\Installer\\MVC\\AutomatedBuild\\networkImage\\DiskImages\\DISK1"
                                robocopy "$sourceFolder" "$ENV:WORKSPACE\\zip" /MOV /S

                                if ($lastexitcode -eq 0)
                                 {
                                      write-host "Robocopy succeeded"
                                 }
                                else
                                {
                                      write-host "Robocopy returned exit code:" $lastexitcode
                                }
                                $sourceFolder = "$ENV:WORKSPACE\\Installer\\Services\\AutomatedBuild\\networkImage\\DiskImages\\DISK1"
                                robocopy "$sourceFolder" "$ENV:WORKSPACE\\zip" /MOV /S
                                if ($lastexitcode -eq 0)
                                 {
                                      write-host "Robocopy succeeded"
                                 }
                                else
                                {
                                      write-host "Robocopy returned exit code:" $lastexitcode
                                }

                                $host.SetShouldExit(0); exit
                            '''
                        zip archive: true, dir: 'zip', glob: '', zipFile: OUTPUT_FILE
                    }
                    post {
                        success {
                            script {
                                env.FOLDER = 'DEV'
                                try {
                                    echo branch
                                    if (branch.contains('feature')) {
                                        echo 'This is running on a FEATURE branch'
                                        env.FOLDER = 'FEATURE'
                                    }
                                    else if (branch.contains('release')) {
                                        echo 'This is running on a RELEASE branch'
                                        env.FOLDER = 'RELEASE'
                                    }
                                    else if (branch.contains('hotfix')) {
                                        echo 'This is running on a HOTFIX branch'
                                        env.FOLDER = 'HOTFIX'
                                    }
                                }
                                catch (ex) {
                                    echo 'branch parameter not defined, assuming working on DEV'
                                }
                                powershell '.\\build\\writeReadme.ps1'
                                rtUpload (
                                    serverId: 'ArtifactoryProd',
                                    spec: '''{
                                          "files": [
                                            {
                                              "pattern": "${OUTPUT_FILE}",
                                              "target": "powercampus-generic/${FOLDER}/${PROJECT_FOLDER}/" 
                                            },
                                            {
                                              "pattern": "${OUTPUT_FILE}",
                                              "target": "powercampus-generic/${FOLDER}/${PROJECT_FOLDER}/${OUTPUT_PREFIX}Latest${OUTPUT_EXT}" 
                                            },
                                            {
                                              "pattern": "readme.txt",
                                              "target": "powercampus-generic/${FOLDER}/${PROJECT_FOLDER}/" 
                                            }
                                         ]
                                    }'''
                                )
                            }
                        }
                    }
                    
                }
            }
            post {
                cleanup {
                    echo "Cleaning up workspace ${env.WORKSPACE}"
                    deleteDir()
                }
            }
        }
    }
}