using Microsoft.Deployment.WindowsInstaller;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace WebAdminUpgrade
{
    public class ServicesConfigUpgrade
    {
        private const string webConfigFile = "web.config";

        [CustomAction]
        public static ActionResult GetServicesSettingsFromOldVersion(Session session)
        {
            string installDirOld = session["INSTALLDIROLD"];

            if (string.IsNullOrWhiteSpace(installDirOld))
            {
                session.Log("ERROR: INSTALLDIROLD has not been set.");
                return ActionResult.Failure;
            }

            string xmlPath = Path.Combine(installDirOld, webConfigFile);
            try
            {
                XDocument xmlDoc = XDocument.Load(xmlPath);

                // .. let's find the name of the default database first
                XElement dataConfig = xmlDoc.Descendants("dataConfiguration").FirstOrDefault();
                string defaultDB = dataConfig != null ? dataConfig.Attribute("defaultDatabase").Value : "PowerCampusDbContext";
                // .. now find the connection string
                XElement connStrings = xmlDoc.Descendants("connectionStrings").FirstOrDefault();

                XElement addNode = connStrings.Descendants("add")
                    .FirstOrDefault(el => el.Attribute("name")?.Value == defaultDB);

                string connectionString = addNode != null ? addNode.Attribute("connectionString").Value : string.Empty;

                // .. get the remaining appSettings
                XElement appSettings = xmlDoc.Descendants("appSettings").FirstOrDefault();

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault((el => el.Attribute("key")?.Value == "TokenExpirationTime"));
                string token = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault((el => el.Attribute("key")?.Value == "SepUser"));
                string sepUser = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                addNode = appSettings.Descendants("add")
                    .FirstOrDefault((el => el.Attribute("key")?.Value == "SepPassword"));
                string sepPwd = addNode != null ? addNode.Attribute("value").Value : string.Empty;

                session["OLDCONNSTR"] = connectionString;
                session["OLDTOKEN"] = token;
                session["OLDSEPUSER"] = sepUser;
                session["OLDSEPPWD"] = sepPwd;
            }
            catch(Exception e)
            {
                session["DEBUG"] = e.ToString();
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult SetServicesSettingsInNewVersion(Session session)
        {
            if (!session.GetMode(InstallRunMode.Scheduled))
            {
                session.Log("ERROR - SetServicesSettingsInNewVersion is designed to be a deferred custom action, but you are calling it in a different mode");
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
                string connectionString = customActionDataArray[1];
                string token = customActionDataArray[2];
                string sepUser = customActionDataArray[3];
                string sepPwd = customActionDataArray[4];

                try
                {
                    // .. change the Config/EmailSettings.config first
                    string filePath = Path.Combine(installDir, webConfigFile);
                    XDocument xmlDoc = XDocument.Load(filePath, LoadOptions.PreserveWhitespace);

                    // .. let's find the name of the default database first
                    XElement dataConfig = xmlDoc.Descendants("dataConfiguration").FirstOrDefault();
                    string defaultDB = dataConfig != null ? dataConfig.Attribute("defaultDatabase").Value : "PowerCampusDbContext";
                    // .. now find the connection string
                    XElement connStrings = xmlDoc.Descendants("connectionStrings").FirstOrDefault();
                    XElement addNode = connStrings.Descendants("add")
                        .FirstOrDefault(el => el.Attribute("name")?.Value == defaultDB);

                    if (addNode.Attribute("connectionString") != null)
                    {
                        addNode.Attribute("connectionString").Value = connectionString;
                    }
                    else
                    {
                        addNode.Add(new XAttribute("connectionString", connectionString));
                    }

                    // .. get the remaining appSettings
                    XElement appSettings = xmlDoc.Descendants("appSettings").FirstOrDefault();

                    addNode = appSettings.Descendants("add")
                        .FirstOrDefault((el => el.Attribute("key")?.Value == "TokenExpirationTime"));
                    if (addNode.Attribute("value") != null)
                    {
                        addNode.Attribute("value").Value = token;
                    }
                    else
                    {
                        addNode.Add(new XAttribute("value", token));
                    }

                    addNode = appSettings.Descendants("add")
                        .FirstOrDefault((el => el.Attribute("key")?.Value == "SepUser"));
                    if (addNode.Attribute("value") != null)
                    {
                        addNode.Attribute("value").Value = sepUser;
                    }
                    else
                    {
                        addNode.Add(new XAttribute("value", sepUser));
                    }

                    addNode = appSettings.Descendants("add")
                        .FirstOrDefault((el => el.Attribute("key")?.Value == "SepPassword"));
                    if (addNode.Attribute("value") != null)
                    {
                        addNode.Attribute("value").Value = sepPwd;
                    }
                    else
                    {
                        addNode.Add(new XAttribute("value", sepPwd));
                    }
                    xmlDoc.Save(filePath);
                }
                catch(Exception e)
                {
                    session.Log(e.ToString());
                }
            }
            return ActionResult.Success;
        }
    }
}
