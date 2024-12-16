using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PowerCampus.MVC.Controllers
{
    [AllowAnonymous]
    public class PowerCampusRequestController : Controller
    {
        // GET: PowerCampusRequest
        public IHtmlString Index(string guid)
        {
            Guid id = new Guid();
            string dbValuesretrivedAs = string.Empty;
            if (Guid.TryParse(guid, out id))
            {
                dbValuesretrivedAs = dbValuesretrivedAs + "GUID is :" + guid + "</br>";
                dbValuesretrivedAs = dbValuesretrivedAs + "People ID is :" + "P00000258" + "</br>";
                dbValuesretrivedAs = dbValuesretrivedAs + "Invoice ID is :" + "XYZTV" + "</br>";
                dbValuesretrivedAs = dbValuesretrivedAs + "Window Called from is  :" + "w_invoice details" + "</br>";
                dbValuesretrivedAs = dbValuesretrivedAs + "Pwermission is :" + "READ write Create etc" + "</br>";
                dbValuesretrivedAs = dbValuesretrivedAs + "Session is :" + this.Session.SessionID + "</br>";
            }
            else
            {
                dbValuesretrivedAs = "Site is available but cant acess the desired page due to permission issue.";
              //  OAuthGrantResourceOwnerCredentialsContext context = new OAuthGrantResourceOwnerCredentialsContext(this.,)
              //  AuthorizationServerProvider.GrantResourceOwnerCredentials()
                Response.Redirect(Url.Action("", "Home"));
            }
            return new HtmlString(dbValuesretrivedAs);
        }
       
    }
}
