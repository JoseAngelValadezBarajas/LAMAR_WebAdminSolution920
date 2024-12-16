// --------------------------------------------------------------------
// <copyright file="AboutController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Reflection;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// About API
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class AboutController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult Get()
        {
            string version = string.Empty;
            Assembly assembly = Assembly.GetExecutingAssembly();

            object[] objects = assembly.GetCustomAttributes(false);
            foreach (object o in objects)
            {
                if (o is System.Reflection.AssemblyFileVersionAttribute)
                {
                    AssemblyFileVersionAttribute versionAttr = (AssemblyFileVersionAttribute)o;
                    version = versionAttr.Version;
                }
            }
            return Ok(version);
        }
    }
}