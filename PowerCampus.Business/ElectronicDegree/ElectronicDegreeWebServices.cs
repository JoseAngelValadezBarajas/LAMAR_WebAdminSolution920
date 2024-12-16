// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeWebServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.SepService;
using System;
using System.Threading.Tasks;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// Manages all the request and responses from the SEP WebServices
    /// </summary>
    public class ElectronicDegreeWebServices
    {
        /// <summary>
        /// The password
        /// </summary>
        private readonly string password;

        /// <summary>
        /// The sep webservice client
        /// </summary>
        private readonly SepService.TitulosPortTypeClient sepClient;

        /// <summary>
        /// The user
        /// </summary>
        private readonly string user;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeWebServices"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public ElectronicDegreeWebServices(string user, string password)
        {
            this.password = password;
            this.user = user;
            this.sepClient = new SepService.TitulosPortTypeClient();
        }

        /// <summary>
        /// Cancels the document.
        /// </summary>
        /// <param name="folioControl">The folio control.</param>
        /// <param name="cancelationReason">The cancelation reason.</param>
        /// <returns></returns>
        public async Task<cancelaTituloElectronicoResponse1> CancelDocument(string folioControl, string cancelationReason)
        {
            cancelaTituloElectronicoRequest cancelaRequest = new cancelaTituloElectronicoRequest();
            cancelaRequest.autenticacion = new autenticacionType() { usuario = user, password = password };
            cancelaRequest.folioControl = folioControl;
            cancelaRequest.motCancelacion = cancelationReason;

            cancelaTituloElectronicoResponse1 response = await sepClient.cancelaTituloElectronicoAsync(cancelaRequest);
            return response;
        }

        /// <summary>
        /// Checks the document.
        /// </summary>
        /// <param name="lote">The lote.</param>
        /// <returns></returns>
        public async Task<consultaProcesoTituloElectronicoResponse1> CheckDocument(string lote)
        {
            consultaProcesoTituloElectronicoRequest consultaRequest = new consultaProcesoTituloElectronicoRequest();
            consultaRequest.numeroLote = lote;
            consultaRequest.autenticacion = new autenticacionType() { usuario = user, password = password };
            consultaProcesoTituloElectronicoResponse1 response = await sepClient.consultaProcesoTituloElectronicoAsync(consultaRequest);
            return response;
        }

        /// <summary>
        /// Downloads the document.
        /// </summary>
        /// <param name="numeroLote">The numero lote.</param>
        /// <returns></returns>
        public async Task<descargaTituloElectronicoResponse1> DownloadDocument(string numeroLote)
        {
            descargaTituloElectronicoRequest descargaRequest = new descargaTituloElectronicoRequest();
            descargaRequest.autenticacion = new autenticacionType() { usuario = user, password = password };
            descargaRequest.numeroLote = numeroLote;

            descargaTituloElectronicoResponse1 response = await sepClient.descargaTituloElectronicoAsync(descargaRequest);
            return response;
        }

        /// <summary>
        /// Sends the document.
        /// </summary>
        /// <param name="base64">The base64.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public async Task<cargaTituloElectronicoResponse1> SendDocument(string base64, string fileName)
        {
            cargaTituloElectronicoRequest sepRequest = new cargaTituloElectronicoRequest();
            sepRequest.archivoBase64 = Convert.FromBase64String(base64);
            sepRequest.autenticacion = new autenticacionType() { usuario = user, password = password };
            sepRequest.nombreArchivo = fileName;

            cargaTituloElectronicoResponse1 responseCarga = await sepClient.cargaTituloElectronicoAsync(sepRequest);
            return responseCarga;
        }
    }
}