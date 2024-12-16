using Microsoft.Deployment.WindowsInstaller;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace WebAdminUpgrade
{
    public class EmailConfigUpgrade
    {
        [CustomAction]
        public static ActionResult GetPreviousValues(Session session)
        {
            string installDirOld = session["INSTALLDIROLD"];

            if (string.IsNullOrWhiteSpace(installDirOld))
            {
                session.Log("ERROR: INSTALLDIROLD has not been set.");
                return ActionResult.Failure;
            }

            string xmlPath = Path.Combine(installDirOld, "web.config");
            try
            {
                XDocument xmlDoc = XDocument.Load(xmlPath);

                XElement appSettings = xmlDoc.Descendants("configuration").FirstOrDefault()
                    .Descendants("appSettings").FirstOrDefault();

                XElement addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "SmtpHost");

                string smtpHost = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "SmtpPort");

                string smtpPort = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "EmailFrom");

                string emailFrom = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "EmailUserName");

                string emailUser = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "EmailPassword");

                string emailPwd = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "EmailSubject");

                string emailSubject = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "EmailBody");

                string emailBody = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                if (appSettings.Descendants("add").Where(el => el.Attribute("key")?.Value == "WebAdminServicesBaseAddress").Count() == 2)
                {
                    addNode = appSettings.Descendants("add")
                        .LastOrDefault(el => el.Attribute("key")?.Value == "WebAdminServicesBaseAddress");
                }
                else if (appSettings.Descendants("add").Where(el => el.Attribute("key")?.Value == "WebAdminServicesBaseAddress").Count() < 2)
                {
                    addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "WebAdminServicesBaseAddress");
                }

                string baseAddress = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("key")?.Value == "TokenExpirationTime");

                string token = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                XElement sessionState = xmlDoc.Descendants("configuration").FirstOrDefault()
                    .Descendants("system.web").FirstOrDefault()
                    .Descendants("sessionState").FirstOrDefault();

                string timeout = sessionState.Attribute("timeout").Value;

                session["SMTPHOST"] = smtpHost;
                session["SMTPPORT"] = smtpPort;
                session["EMAILFROM"] = emailFrom;
                session["EMAILUSERNAME"] = emailUser;
                session["EMAILPWD"] = emailPwd;
                session["EMAILSUBJECT"] = emailSubject;
                session["EMAILBODY"] = emailBody;
                session["SERVICESBASEADDRESS"] = baseAddress;
                session["TOKEN"] = token;
                session["TIMEOUT"] = timeout;
            }
            catch (Exception e)
            {
                session["DEBUG"] = e.ToString();
                session.Log(e.ToString());
            }
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult SetValuesOnNewFiles(Session session)
        {
            if (!session.GetMode(InstallRunMode.Scheduled))
            {
                session.Log("ERROR - SetValuesOnNewFiles is designed to be a deferred custom action, but you are calling it in a different mode");
                return ActionResult.Failure;
            }

            string customActionData = session["CustomActionData"];

            if (string.IsNullOrWhiteSpace(customActionData))
            {
                session.Log("ERROR: CustomActionData has not been set.");
                return ActionResult.Failure;
            }

            // .. parsing the values from CustomActionData
            string[] customActionDataArray = customActionData.Split('|');
            if (customActionDataArray.Length > 1)
            {
                string installDir = customActionDataArray[0];
                string smtpHost = customActionDataArray[1];
                string smtpPort = customActionDataArray[2];
                string emailFrom = customActionDataArray[3];
                string emailUser = customActionDataArray[4];
                string emailPwd = customActionDataArray[5];
                string emailSubject = customActionDataArray[6];
                string emailBody = customActionDataArray[7];
                string baseAddress = customActionDataArray[8];
                string token = customActionDataArray[9];
                string timeout = customActionDataArray[10];

                try
                {
                    // .. update web.config first
                    string filePath = Path.Combine(installDir, "web.config");
                    XDocument xmlDoc = XDocument.Load(filePath, LoadOptions.PreserveWhitespace);

                    XElement appSettings = xmlDoc.Descendants("appSettings").FirstOrDefault();

                    XElement addNode = appSettings.Descendants("add")
                        .FirstOrDefault(el => el.Attribute("key")?.Value == "WebAdminServicesBaseAddress");

                    if (addNode.Attribute("value") != null)
                    {
                        addNode.Attribute("value").Value = baseAddress;
                    }
                    else
                    {
                        addNode.Add(new XAttribute("value", baseAddress));
                    }

                    addNode = appSettings.Descendants("add")
                        .FirstOrDefault(el => el.Attribute("key")?.Value == "TokenExpirationTime");

                    if (addNode.Attribute("value") != null)
                    {
                        addNode.Attribute("value").Value = token;
                    }
                    else
                    {
                        addNode.Add(new XAttribute("value", token));
                    }

                    XElement sessionState = xmlDoc.Descendants("configuration").FirstOrDefault()
                        .Descendants("system.web").FirstOrDefault()
                        .Descendants("sessionState").FirstOrDefault();

                    if (sessionState.Attribute("timeout") != null)
                    {
                        sessionState.Attribute("timeout").Value = timeout;
                    }
                    else
                    {
                        sessionState.Add(new XAttribute("timeout", timeout));
                    }
                    xmlDoc.Save(filePath);
                }
                catch (Exception e)
                {
                    session.Log("Exception found when modifying Web.config");
                    session.Log(e.ToString());
                }
                
                try
                {
                    // .. now change the Config/EmailSettings.config 
                    string filePath = Path.Combine(installDir, "Config\\EmailSettings.config");
                    XDocument xmlDoc = XDocument.Load(filePath, LoadOptions.PreserveWhitespace);

                    XElement FiscalRecordsSetting = xmlDoc.Descendants("emailSettings").FirstOrDefault()
                        .Descendants("FiscalRecordsSetting").FirstOrDefault();


                    if (FiscalRecordsSetting.Attribute("EmailSubject") != null)
                    {
                        FiscalRecordsSetting.Attribute("EmailSubject").Value = emailSubject;
                    }
                    else
                    {
                        FiscalRecordsSetting.Add(new XAttribute("EmailSubject", emailSubject));
                    }

                    if (FiscalRecordsSetting.Attribute("EmailBody") != null)
                    {
                        FiscalRecordsSetting.Attribute("EmailBody").Value = emailBody;
                    }
                    else
                    {
                        FiscalRecordsSetting.Add(new XAttribute("EmailBody", emailBody));
                    }
                    xmlDoc.Save(filePath);
                }
                catch (Exception e)
                {
                    session.Log("Exception found when modifying Config\\EmailSettings.config");
                    session.Log(e.ToString());
                }

                try
                {
                    string filePath = Path.Combine(installDir, "Config\\SmtpSettings.config");
                    XDocument xmlDoc = XDocument.Load(filePath, LoadOptions.PreserveWhitespace);

                    XElement SmtpSettings = xmlDoc.Descendants("smtp").FirstOrDefault();
                    if (SmtpSettings.Attribute("from") != null)
                    {
                        SmtpSettings.Attribute("from").Value = emailFrom;
                    }
                    else
                    {
                        SmtpSettings.Add(new XAttribute("from", emailFrom));
                    }

                    XElement NetworkSettings = xmlDoc.Descendants("smtp").FirstOrDefault()
                        .Descendants("network").FirstOrDefault();

                    if (NetworkSettings.Attribute("host") != null)
                    {
                        NetworkSettings.Attribute("host").Value = smtpHost;
                    }
                    else
                    {
                        NetworkSettings.Add(new XAttribute("host", smtpHost));
                    }

                    if (NetworkSettings.Attribute("password") != null)
                    {
                        NetworkSettings.Attribute("password").Value = emailPwd;
                    }
                    else
                    {
                        NetworkSettings.Add(new XAttribute("password", emailPwd));
                    }

                    if (NetworkSettings.Attribute("port") != null)
                    {
                        NetworkSettings.Attribute("port").Value = smtpPort;
                    }
                    else
                    {
                        NetworkSettings.Add(new XAttribute("port", smtpPort));
                    }

                    if (NetworkSettings.Attribute("userName") != null)
                    {
                        NetworkSettings.Attribute("userName").Value = emailUser;
                    }
                    else
                    {
                        NetworkSettings.Add(new XAttribute("userName", emailUser));
                    }
                    xmlDoc.Save(filePath);
                }
                catch (Exception e)
                {
                    session.Log("Exception found when trying to modify Config\\SmtpSettings.config");
                    session.Log(e.ToString());
                }
            }
            return ActionResult.Success;
        }
    }
}
