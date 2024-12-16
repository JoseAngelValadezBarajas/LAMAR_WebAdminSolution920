using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerCampus.WebApi
{
    public class AccountController : ApiController
    {
        public AccountController() { }

        // POST api/login
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(int id, string pwd)
        {
            if (id > 0) // testing - not authenticating right now
            {
                var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, id.ToString()));
                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                var currentUtc = new SystemClock().UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
                var token = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<object>(new
                    {
                        UserName = id.ToString(),
                        AccessToken = token
                    }, Configuration.Formatters.JsonFormatter)
                };
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        // POST api/token
        [Route("token")]
        [HttpPost]
        public HttpResponseMessage Token(int id, string pwd)
        {
            // Never reaches here. Do I need this method?
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}