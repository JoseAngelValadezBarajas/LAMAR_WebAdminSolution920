// --------------------------------------------------------------------
// <copyright file="CatalogsController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.BusinessInterfaces;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// CatalogsController
    /// </summary>
    /// <seealso cref="T:System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class CatalogsController : ApiController
    {
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult Get([FromUri] Catalog name, [FromUri] FiscalRecordCatalog fiscalRecordCatalog)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();
            ICatalogServices catalogServices = new CatalogServices();
            if (name == Catalog.TaxRegimen)
                return Ok(catalogServices.GetTaxRegimen());
            if (name == Catalog.CFDIUsage)
                return Ok(catalogServices.GetCFDIUsage());
            if (name == Catalog.ChargeCredit)
                return Ok(catalogServices.GetChargeCreditCatalogNonTax(fiscalRecordCatalog.Code));
            if (name == Catalog.ReceiptCharge)
                return Ok(catalogServices.GetRecieptChargeCodes());
            if (name == Catalog.RecordType)
                return Ok(catalogServices.GetAllRecordTypes());
            if (name == Catalog.Credit)
                return Ok(catalogServices.GetAllChargeCredits());
            if (name == Catalog.States)
                return Ok(catalogServices.GetStates());
            List<FiscalRecordCatalog> fiscalRecordCatalogList;
            if (name == Catalog.Country || name == Catalog.UnityKey || name == Catalog.ProductServiceKey || name == Catalog.PostalCode)
                fiscalRecordCatalogList = catalogServices.GetFiscalRecordCatalogByKey(name, fiscalRecordCatalog);
            else if (fiscalRecordCatalog.Code == null)
                fiscalRecordCatalogList = catalogServices.GetFiscalRecordCatalog(name);
            else
                fiscalRecordCatalogList = catalogServices.GetFiscalRecordCatalogByAttribute(name, fiscalRecordCatalog);

            if (fiscalRecordCatalogList?.Count > 0)
                return Ok(fiscalRecordCatalogList);
            return NotFound();
        }

        /// <summary>
        /// Gets the cfdi.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult GetCFDI(int length)
        {
            List<CFDIUsageCatalog> catalogListCFDI = null;
            ICatalogServices catalogServices = new CatalogServices();

            if (length.Equals(12))
            {
                catalogListCFDI = catalogServices.GetCFDIUsage();

                catalogListCFDI = catalogListCFDI.Where(m => m.AppliesToMoralPerson && m.AppliesToPhysicalPerson).Select(m => new CFDIUsageCatalog
                {
                    Code = m.Code,
                    Description = m.Description
                }).ToList();
            }
            else
                catalogListCFDI = catalogServices.GetCFDIUsage();

            return Ok(catalogListCFDI);
        }

        /// <summary>
        /// Gets the cfdi usage.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/Catalogs/CFDIUsage")]
        public IHttpActionResult GetCFDIUsage()
        {
            ICatalogServices catalogServices = new CatalogServices();

            List<CFDIUsageCatalog> catalogList = catalogServices.GetCFDIUsage();

            return Ok(catalogList);
        }
    }
}