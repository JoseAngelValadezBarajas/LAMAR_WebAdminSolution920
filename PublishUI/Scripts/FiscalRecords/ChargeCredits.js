$(document).ready(function () {

    var IsSubstitution = $("#IsSubstitution").val() == 'True';
    var IssuerTaxPayerId = $("#IssTaxPayerId");
    var IssuerExpAdd = $('#IssIssuingAddress');
    var IssPlaceIssue = $('#IssPlaceIssue');
    var IssTaxRegimenC = $('#IssuerTaxRegimenId');
    var IssuerTaxRegimenCode = "";
    var IssuerTaxRegimenDesc = "";
    var TaxPayers = [];
    var IssTaxPayers = [];
    var IssIssuingAddress;
    var IdIssTaxPayer = 0;
    var fIdentityNumber = "";
    var fResidency = "";
    var fResidencyDesc = "";
    var ExpeditionDate = "";
    var validateDate = moment(new Date()).endOf('month').add(3, "days").format('YYYY-MM-DD');
    var localeBrowser = "es";

    if (window.navigator !== undefined) {
        if (window.navigator.languages !== undefined) {
            localeBrowser = (window.navigator.languages[0] || window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
        else {
            localeBrowser = (window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
    }

    // #region Receiver

    if (IsSubstitution) {
        var RecTaxPayIdValue = $("#ReceiverTaxpayerId").val();
        $.ajax({
            url: urlGetReceiver,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: RecTaxPayIdValue },
            success: function (data) {
                TaxPayers = data;
                onTaxPayerIdFocusOut();
            }
        });
    }

    // EmisFact ACOLLI 04112021
    $("#IssueAlert").css('display', 'none');
    $("#IssueDate").datetimepicker({
        viewMode: 'days',
        format: 'DD/MM/YYYY',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $("#IssueTime").datetimepicker({
        // viewMode: 'days',
        format: 'HH:mm',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });
    // END EmisFact

    // EmisFact ACOLLI 04112021

    var strExpDate = String(moment(new Date()).format('YYYY-MM-DD'));
    // String(moment(new Date()).endOf('month').add(3,"days").format('YYYY-MM-DD'));
    var strExpTime = String(moment(new Date()).format('HH:mm'));
    ExpeditionDate = `${strExpDate} ${strExpTime}`;
    // new Date(`${strExpDate} ${strExpTime}`);
    // console.log((moment(ExpeditionDate) > moment(validateDate)) ? "La fecha y Hora de generación esta fuera de rango" : "pasa");

    $('#IssueDate').datetimepicker().on('dp.change', function (e) {
        var selectedDate = moment(new Date(e.date));
        // expDate = selectedDate.endOf('month').format('YYYY-MM-DD');
        // expDate = moment(expDate).add(3,"days");
        strExpDate = moment(new Date(e.date)).format('YYYY-MM-DD');
        ExpeditionDate = `${strExpDate} ${strExpTime}`;
        // console.log("mes corriente: ", validateDate, "fecha emision", ExpeditionDate);

        if (moment(ExpeditionDate) > moment(validateDate)) {
            // console.log($("#IssueAlert").is(":visible"));
            if (!$("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'block');
            }
        }
        else {
            // console.log($("#IssueAlert").is(":visible"));
            if ($("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'none');
            }
        }
        // console.log((moment(ExpeditionDate) > moment(validateDate)) ? "La fecha y Hora de generación esta fuera de rango" : "pasa");
        $(this).data("DateTimePicker").hide();
    });

    $("#IssueTime").datetimepicker().on('dp.change', function (e) {
        // console.log(moment(new Date(e.date)).format("HH:mm"));
        strExpTime = moment(new Date(e.date)).format("HH:mm");
        // console.log("concat data: ", strExpDate + " " + strExpTime);
        ExpeditionDate = `${strExpDate} ${strExpTime}`;
        // console.log("mes corriente: ", validateDate, "fecha emision", ExpeditionDate);
        if (moment(ExpeditionDate) > moment(validateDate)) {
            // console.log($("#IssueAlert").is(":visible"));
            if (!$("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'block');
            }
        }
        else {
            // console.log($("#IssueAlert").is(":visible"));
            if ($("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'none');
            }
        }
        // console.log((moment(ExpeditionDate) > moment(validateDate)) ? "La fecha y Hora de generación esta fuera de rango" : "pasa");
    });
    // END EmisFact ACOLLI 04112021

    $("#ReceiverTaxpayerId").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: urlGetReceiver,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === 'XEXX010101000') {
                            $("#CFDI").prop("disabled", false);
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber }
                        }
                        else {
                            $("#CFDI").prop("disabled", false);
                            return { label: item.TaxPayerId, value: item.TaxPayerId }
                        }

                    }))

                }
            })
        }
    });

    $("#ReceiverTaxpayerId").focusout(onTaxPayerIdFocusOut);

    function onTaxPayerIdFocusOut() {
        var IdTaxpayer = $("#ReceiverTaxpayerId").val();
        var RecTaxPayId = $("#ReceiverTaxpayerId");
        var response;
        var FiscalIdentityNumber;

        var res = IdTaxpayer.split("-");
        var taxPayerIdVal = res[0];
        if (res.length > 1) {
            FiscalIdentityNumber = res[1];
            response = $.grep(TaxPayers, function (element) { return element.FiscalIdentityNumber == FiscalIdentityNumber })

            $("#ReceiverCorporateName").val(response[0].CorporateName);
            $("#ReceiverFiscalResidency").val(response[0].FiscalResidency + "-" + response[0].FiscalResidencyDesc);
            fResidency = response[0].FiscalResidency;
            fResidencyDesc = response[0].FiscalResidencyDesc;
            $("#ReceiverFiscalIdentityNumber").val(response[0].FiscalIdentityNumber);
            fIdentityNumber = response[0].FiscalIdentityNumber;
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').css('display', 'none');
            $('#ReceiverTaxPayerGroup').removeClass('esg-has-error');
            $("#ReceiverTaxpayerId").val(taxPayerIdVal);
            $("#ReceiverTaxAddress").val(response[0].PostalCode);
            if (response[0].TaxRegimenCode !== undefined && response[0].TaxRegimenCode !== '') {
                $("#ReceiverTaxRegimen").val(response[0].TaxRegimenCode + " - " + response[0].TaxRegimenDesc);
            }
        }
        else
            response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == IdTaxpayer; })

        if ((IdTaxpayer.length > 0 || response.length > 0) && res.length === 1) {
            $("#ReceiverCorporateName").val(response[0].CorporateName);
            $("#ReceiverFiscalResidency").val(response[0].FiscalResidency + "-" + response[0].FiscalResidencyDesc);
            fResidency = response[0].FiscalResidency;
            fResidencyDesc = response[0].FiscalResidencyDesc;
            $("#ReceiverFiscalIdentityNumber").val(response[0].FiscalIdentityNumber);
            fIdentityNumber = response[0].FiscalIdentityNumber;
            $("#ReceiverTaxAddress").val(response[0].PostalCode);
            if (response[0].TaxRegimenCode !== undefined && response[0].TaxRegimenCode !== '') {
                $("#ReceiverTaxRegimen").val(response[0].TaxRegimenCode + " - " + response[0].TaxRegimenDesc);
            }
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').css('display', 'none');
            $("#RecTaxPayerDivLookup").show();
            $('#ReceiverTaxPayerGroup').removeClass('esg-has-error');
        }
        else if (response.length === 0 && res.length === 1) {
            $("#ReceiverCorporateName").val("");
            $("#ReceiverFiscalResidency").val("");
            $("#ReceiverFiscalIdentityNumber").val("");
            RecTaxPayId.parent().addClass('has-error');
            $('#RecTaxPayerIcon').css('display', 'block');
            $("#RecTaxPayerDivLookup").hide();
            $('#ReceiverTaxPayerGroup').addClass('esg-has-error');
        }
        $.ajax({
            url: urlGetReceiverCFDI,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { length: taxPayerIdVal.length },
            success: function (res) {
                $("#CFDI").empty();
                $("#CFDI").append($('<option></option>').val("").html(lblSelect));
                $.each(res, function (i, val) {
                    $("#CFDI").append($('<option></option>').val(val.Code).html(val.Description));
                });
                if (CfdiUsage !== "") {
                    $("#CFDI").val(CfdiUsage);
                    CfdiUsage = "";
                }
            }
        });
    }

    $("#ReceiverEmail").on('change', function () {

        var Email = $("#ReceiverEmail");

        if (!Email.val()) {

            Email.parent().addClass('has-error');
            $("#EmailGroup").addClass('esg-has-error');
            $("#EmailIcon").css('display', 'block');

        }
        else {

            $("#EmailGroup").removeClass('esg-has-error');
            $("#EmailIcon").css('display', 'none');
            Email.parent().removeClass('has-error');
        }

    });

    // #endregion Receiver

    // #region Issuer

    if (IsSubstitution) {
        var IssTaxPayerIdValue = $("#IssTaxPayerId").val();
        $.ajax({
            url: urlGetIssuer,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: IssTaxPayerIdValue },
            success: function (data) {
                IssTaxPayers = data;
                onIssTaxPayerIdFocusOut();
            }
        });
    }
    else {
        LoadIssuerSetUp();
    }

    function LoadIssuerSetUp() {
        $.ajax({
            url: urlSelectIssuerSetUp,
            type: "GET",
            cache: false,
            success: function (data) {

                if (data.IssInvoiceOrganizationId != 0 && data.IssInvoiceOrganizationId != null) { //Si existe registro en la tabla de IssuerDefault se valida esa información

                    var IssuerTaxpayer = $('#IssTaxPayerId').val();

                    if (IssuerTaxpayer === data.IssInvoiceTaxpayerId) {
                        $("#IssSerial").empty();
                        $("#IssIssuingAddress").empty();
                        $("#IssIssuingAddress").append($('<option></option>').val(data.IssInvoiceExpeditionId).html(data.IssIssuingAddress));
                        $("#IssSerial").append($('<option></option>').val(data.IssSerialNumber).html(data.IssSerialNumber));
                        var SerialNum = $("#IssSerial").val().trim();


                        var InvoiceOrganizationId = data.IssInvoiceOrganizationId;
                        var InvoiceExpId = data.IssInvoiceExpeditionId;

                        $.ajax({
                            url: urlGetTaxRegimen,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: InvoiceOrganizationId },
                            success: function (response) {

                                if (response.IssTaxRegimen != null) {

                                    var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element });
                                    IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                                    IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                                    $("#IssuerTaxRegimenId").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);

                                }
                                else {
                                    $("#IssTaxPayerDivLookup").hide();
                                    $("#IssuerTaxRegimenId").val('');
                                }

                                if (response.IssIssuingAdd != null) {

                                    IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                                    $.each(IssIssuingAddress, function (i, val) {

                                        if (InvoiceExpId != val.IssInoviceExpeditionId) {
                                            $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                        }
                                    });

                                    var IssuingAddress = $.grep(response.IssIssuingAdd, function (elem) { return elem.IssInoviceExpeditionId == InvoiceExpId });

                                    if (IssuingAddress[0].IssPlaceIssue != null) {
                                        $("#IssPlaceIssue").val(IssuingAddress[0].IssPlaceIssue);
                                    }

                                    if (IssIssuingAddress[0].IssByExpedition === 0) {
                                        InvoiceExpId = null;
                                    }
                                    //GET SerialNumber
                                    $.ajax({
                                        url: urlGetSerialNumber,
                                        type: "GET",
                                        cache: false,
                                        dataType: "json",
                                        data: { id: InvoiceOrganizationId, expId: InvoiceExpId },
                                        success: function (response) {

                                            $("#IssSerial").empty();
                                            $("#lblWarnSerial").css('display', 'none');

                                            if (response.IssSerial != null) {
                                                var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                                var SerialPrefer = $.grep(IssSerial, function (e) { return e.IssSerialNumber == SerialNum })

                                                if (SerialPrefer[0].IssLastFolio == '2147483647') {

                                                    $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 1));

                                                }
                                                else {
                                                    $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 0));

                                                }

                                                $.each(IssSerial, function (i, val) {
                                                    if ($("#IssSerial").val() != val.IssInvoiceReceipt) {

                                                        if (val.IssLastFolio == '2147483647') {
                                                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                                                        }
                                                        else {
                                                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                                                        }
                                                    }
                                                });

                                                var selected = $("#IssSerial").find('option:selected');
                                                var optionVal = selected.attr('id');

                                                if (optionVal == "1") {
                                                    $("#lblWarnSerial").css('display', 'block');
                                                }
                                                else {
                                                    $("#lblWarnSerial").css('display', 'none');
                                                }

                                            }
                                            else {

                                                $("#IssSerial").empty();
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    }
                    else { //En caso contrario se va a cargar la información del Issuer que fue configurado en la factura electrónica
                        $("#IssSerial").empty();
                        $("#IssIssuingAddress").empty();
                        var IssuerInvoiceOrganizationId = $("#IssInvoiceOrganizationId").val();

                        $.ajax({
                            url: urlGetTaxRegimen,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: IssuerInvoiceOrganizationId },
                            success: function (response) {

                                var InoviceExpeditionId;

                                IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                                $.each(IssIssuingAddress, function (i, val) {
                                    $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                });

                                if (IssIssuingAddress[0].IssByExpedition === 0) {
                                    InoviceExpeditionId = null;
                                }
                                else {
                                    InoviceExpeditionId = IssIssuingAddress[0].IssInoviceExpeditionId;
                                }

                                //Get Serial
                                $.ajax({
                                    url: urlGetSerialNumber,
                                    type: "GET",
                                    cache: false,
                                    dataType: "json",
                                    data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                                    success: function (response) {

                                        $("#lblWarnSerial").css('display', 'none');

                                        if (response.IssSerial != null) {
                                            var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                            $.each(IssSerial, function (i, val) {

                                                if (val.IssLastFolio == '2147483647') {
                                                    $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                                                }
                                                else {
                                                    $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                                                }

                                            });
                                        }
                                        else {

                                            $("#IssSerial").empty();
                                        }

                                        var selected = $("#IssSerial").find('option:selected');
                                        var optionVal = selected.attr('id');

                                        if (optionVal == "1") {
                                            $("#lblWarnSerial").css('display', 'block');
                                        }
                                        else {
                                            $("#lblWarnSerial").css('display', 'none');
                                        }
                                    }
                                });

                            }

                        });
                    }
                }
                else { //En caso contrario se va a cargar la información del Issuer que fue configurado en la factura electrónica
                    $("#IssSerial").empty();
                    $("#IssIssuingAddress").empty();
                    var IssuerInvoiceOrganizationId = $("#IssInvoiceOrganizationId").val();

                    $.ajax({
                        url: urlGetTaxRegimen,
                        type: "GET",
                        cache: false,
                        dataType: "json",
                        data: { id: IssuerInvoiceOrganizationId },
                        success: function (response) {

                            var InoviceExpeditionId;

                            IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                            $.each(IssIssuingAddress, function (i, val) {
                                $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                            });

                            if (IssIssuingAddress[0].IssByExpedition === 0) {
                                InoviceExpeditionId = null;
                            }
                            else {
                                InoviceExpeditionId = IssIssuingAddress[0].IssInoviceExpeditionId;
                            }

                            //Get Serial
                            $.ajax({
                                url: urlGetSerialNumber,
                                type: "GET",
                                cache: false,
                                dataType: "json",
                                data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                                success: function (response) {

                                    $("#lblWarnSerial").css('display', 'none');

                                    if (response.IssSerial != null) {
                                        var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                        $.each(IssSerial, function (i, val) {

                                            if (val.IssLastFolio == '2147483647') {
                                                $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                                            }
                                            else {
                                                $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                                            }

                                        });
                                    }
                                    else {

                                        $("#IssSerial").empty();
                                    }

                                    var selected = $("#IssSerial").find('option:selected');
                                    var optionVal = selected.attr('id');

                                    if (optionVal == "1") {
                                        $("#lblWarnSerial").css('display', 'block');
                                    }
                                    else {
                                        $("#lblWarnSerial").css('display', 'none');
                                    }
                                }
                            });

                        }

                    });
                }

            }
        });
    }

    $("#IssTaxPayerId").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: urlGetIssuer,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    IssTaxPayers = data;
                    response($.map(data, function (item) {
                        return { value: item.IssTaxpayerId, id: item.IssTaxpayerId }
                    }))

                }
            })
        }
    });

    $("#IssTaxPayerId").focusout(onIssTaxPayerIdFocusOut);

    function onIssTaxPayerIdFocusOut() {
        IdIssTaxPayer = $("#IssTaxPayerId").val();

        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];

        if (!response == "") {

            $("#IssNameCorpName").val("");
            $("#IssuerTaxRegimenId").empty();
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").empty();
            $("#IssPlaceIssue").val("");

            IssuerTaxPayerId.parent().removeClass('has-error');
            $('#IssTaxPayerIcon').css('display', 'none');
            $('#IssTaxPayerGroup').parent().removeClass('esg-has-error');
            $("#IssTaxPayerDivLookup").show();
            $("#IssNameCorpName").val(response.IssCorporateName);
            $("#IssuingAddressGroup").removeClass('esg-has-error');
            IssuerExpAdd.removeClass('has-error');

            //GET TAXREGIMEN
            $.ajax({
                url: urlGetTaxRegimen,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: response.IssInvoiceOrganizationId },
                success: function (response) {

                    if (response.IssTaxRegimen != null) {

                        var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element });
                        IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                        IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                        $("#IssuerTaxRegimenId").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);

                    }
                    else {
                        $("#IssTaxPayerDivLookup").hide();
                        $("#IssuerTaxRegimenId").val('');
                    }

                    if (response.IssIssuingAdd != null) {

                        var InoviceExpeditionId;

                        IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                        $.each(IssIssuingAddress, function (i, val) {
                            $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                        });

                        var issPlaceIssueFound;
                        if (SubsInvoiceExpeditionId != null && SubsInvoiceExpeditionId > 0) {
                            issPlaceIssueFound = IssIssuingAddress.find(a => a.IssInoviceExpeditionId == SubsInvoiceExpeditionId);
                            if (issPlaceIssueFound == null) {
                                issPlaceIssueFound = IssIssuingAddress[0];
                            }
                        }
                        else {
                            issPlaceIssueFound = IssIssuingAddress[0];
                        }
                        SubsInvoiceExpeditionId = null;

                        if (issPlaceIssueFound.IssByExpedition === 0) {
                            InvoiceExpeditionId = null;
                        }
                        else {
                            InoviceExpeditionId = IssIssuingAddress[0].IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound != null && issPlaceIssueFound.IssPlaceIssue != null) {
                            $("#IssIssuingAddress").val(issPlaceIssueFound.IssInoviceExpeditionId);
                            $("#IssPlaceIssue").val(IssIssuingAddress[0].IssPlaceIssue);
                            $("#IssPlaceIssueIcon").css('display', 'none');
                            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-error');
                        }
                        else {
                            $("#IssPlaceIssue").val("");
                            $("#IssPlaceIssueIcon").css('display', 'block');
                            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-success');
                        }

                        //GET SerialNumber
                        $.ajax({
                            url: urlGetSerialNumber,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                            success: function (response) {

                                $("#lblWarnSerial").css('display', 'none');

                                if (response.IssSerial != null) {
                                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                    $.each(IssSerial, function (i, val) {

                                        if (val.IssLastFolio == '2147483647') {
                                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                                        }
                                        else {
                                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                                        }

                                    });
                                    if (SusbsSerialNumber != null && IssSerial.length > 0 && IssSerial.find(s => s.IssSerialNumber == SusbsSerialNumber)) {
                                        $("#IssSerial").val(SusbsSerialNumber);
                                    }
                                }
                                else {

                                    $("#IssSerial").empty();
                                }
                                SusbsSerialNumber = null;

                                var selected = $("#IssSerial").find('option:selected');
                                var optionVal = selected.attr('id');

                                if (optionVal == "1") {
                                    $("#lblWarnSerial").css('display', 'block');
                                }
                                else {
                                    $("#lblWarnSerial").css('display', 'none');
                                }

                            }
                        });

                        $("#IssIssuingAddressIcon").css('display', 'none');
                        $("#IssIssuingAddressGroup").parent().removeClass('esg-has-error');
                        IssuerExpAdd.parent().removeClass('has-error');

                    }
                    else {
                        $("#IssIssuingAddress").empty();
                        $("#IssIssuingAddressIcon").css('display', 'block');
                        $("#IssuingAddressGroup").parent().addClass('esg-has-error');
                        $("#IssIssuingAddress").parent().addClass('esg-has-error');
                        IssuerExpAdd.parent().addClass('has-error');
                        $("#IssPlaceIssue").val("");
                        $("#IssPlaceIssueIcon").css('display', 'block');
                        $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                        IssPlaceIssue.parent().addClass('has-error');
                    }
                }
            });

        }
        else {

            if (IdIssTaxPayer == "") {
                IssuerTaxPayerId.parent().addClass('has-error');
                $('#IssTaxPayerIcon').css('display', 'block');
                $('#IssTaxPayerGroup').parent().addClass('esg-has-error');
                $("#IssIssuingAddressIcon").css('display', 'block');
                $("#IssuingAddressGroup").addClass('esg-has-error');
                IssuerExpAdd.addClass('has-error');
                $("#IssuerTaxRegimenId").val("");
                $("#IssNameCorpName").val("");
                $("#IssSerial").empty();
                $("#IssIssuingAddress").val("");
                $("#IssPlaceIssue").val("");
                $("#IssTaxPayerDivLookup").hide();

            }
        }
    }

    $('#IssIssuingAddress').change(function () {

        var InoviceExpeditionId = 0;
        var val = $('#IssIssuingAddress option:selected').text();
        var PlaceIssue = $.grep(IssIssuingAddress, function (element) { return element.IssIssuingAddress == val });
        var IssByExpedition = PlaceIssue[0].IssByExpedition;

        if (IssByExpedition === 0) {
            InoviceExpeditionId = null;
        }
        else {
            InoviceExpeditionId = PlaceIssue[0].IssInoviceExpeditionId;
        }

        //GET SerialNumber
        $.ajax({
            url: urlGetSerialNumber,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: PlaceIssue[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
            success: function (response) {

                if (response.IssSerial != null) {
                    $("#IssSerial").empty();
                    $("#lblWarnSerial").css('display', 'none');

                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                    $.each(IssSerial, function (i, val) {

                        if (val.IssLastFolio == '2147483647') {
                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                        }
                        else {
                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                        }

                    });

                    var selected = $("#IssSerial").find('option:selected');
                    var optionVal = selected.attr('id');

                    if (optionVal == "1") {
                        $("#lblWarnSerial").css('display', 'block');
                    }
                    else {
                        $("#lblWarnSerial").css('display', 'none');
                    }

                }
                else {

                    $("#IssSerial").empty();
                }
            }
        });

        $("#IssPlaceIssue").val(PlaceIssue[0].IssPlaceIssue);

    });

    $('#IssPlaceIssue').focusout(function () {

        if (IssPlaceIssue.val() != "") {

            $("#IssPlaceIssueIcon").css('display', 'none');
            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-error');
        }
        else {

            $("#IssPlaceIssueIcon").css('display', 'block');
            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-success').addClass('has-error');

        }

    });

    $("#IssSerial").change(function () {

        var selected = $(this).find('option:selected');
        var optionVal = selected.attr('id');

        if (optionVal == "0") {
            $("#lblWarnSerial").css('display', 'none');
        }
        else {
            $("#lblWarnSerial").css('display', 'block');
        }

    });

    // #endregion Issuer

    // #region Payment

    $("#PaymentCondition").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }
    });

    // #endregion Payment

    // #region Charge

    $(".ChargeDesc").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }

    });

    $('#UnitDescription').keyup(function (e) {
        var inputElement = $(this);
        if (inputElement.val().length > 20) {
            $('#msgUnitMaxLength').show();
            $('#UnitDescriptionGroup').addClass('esg-has-error');
            $('#UnitDescriptionIcon').show();
        }
        else {
            $('#msgUnitMaxLength').hide();
            $('#UnitDescriptionGroup').removeClass('esg-has-error');
            $('#UnitDescriptionIcon').hide();
        }
    });
    // #endregion Charge

    // #region Process

    var createPPD = $("#formCreatePPD");
    $('#btnProcessPPD').click(function () {

        $('.errorMessageDiv').hide();
        $('.errorMessageRequiredDiv').hide();
        var unitMaxLengthError = $('#UnitDescription').val().length > 20;

        if (unitMaxLengthError) {
            $('.errorMessageRequiredDiv').show();
            $('.successMessageDiv').hide();
            return false;
        }

        createPPD.validate();

        if (moment(ExpeditionDate) > moment(validateDate)) {
            // console.log($("#IssueAlert").is(":visible"));
            if (!$("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'block');
            }
        }
        else {
            // console.log($("#IssueAlert").is(":visible"));
            if ($("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'none');
            }

            if (createPPD.valid()) { //Process PPD
                $('#btnProcessPPD').css('display', 'none');

                var CFDISelected = $("#CFDI option:selected").text();
                var CFDICode = $("#CFDI option:selected").val();
                var CFDIDesc = CFDISelected.substring(CFDICode.length + 1, CFDISelected.length);

                var PaymentTypeSelected = $("#PaymentType").val();
                var PaymentType = PaymentTypeSelected.split('-');
                var PaymentTypeCode = PaymentType[0].trim();
                var PaymentTypeDesc = PaymentType[1].trim();
                var PaymentMethodSelected = $("#PaymentMethod").val();
                var PaymentMethod = PaymentMethodSelected.split('-');
                var PaymentMethodCode = PaymentMethod[0].trim();
                var PaymentMethodDesc = PaymentMethod[1].trim();

                var SerialNumber = $("#IssSerial option:selected").text();
                var cols = [];
                var frd = new Object();

                frd.ProductServiceKey = $("#ProductServiceKey").val();
                frd.ChargeCreditCodeId = $("#ChargeCreditCodeId").val();
                frd.ChargeCreditCode = $("#ChargeCreditCode").val();
                frd.UnitDescription = $("#UnitDescription").val();
                frd.Description = $("#Description").val();
                cols.push(frd);

                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                var ParamInsInvoiceHeaderList = {
                    CancelReasonKey: "",
                    CancelReasonName: CancelReasonName,
                    CFDIRelated: CFDIRelated,
                    CFDIRelatedId: CFDIRelatedId,
                    CFDIRelationTypeKey: "",
                    CFDIRelationTypeName: null,
                    CFDIUsageCode: CFDICode,
                    CFDIUsageDesc: CFDIDesc,
                    ChargeCreditNumber: $('#ChargeCreditNumberHidden').val(),
                    CityOfIssue: $("#IssPlaceIssue").val(),
                    Comments: "",
                    Currency: $("#Currency").val(),
                    Detail: cols,
                    EndDate: null,
                    ExchangeRate: 0.00,
                    FiscalIdentityNumber: fIdentityNumber,
                    FiscalResidency: fResidency,
                    FiscalResidencyDesc: fResidencyDesc,
                    InvoiceExpeditionId: $("#IssIssuingAddress").val(),
                    IssuerTaxPayerId: $("#IssTaxPayerId").val(),
                    PaymentAccountNumber: "",
                    PaymentCondition: $("#PaymentCondition").val(),
                    PaymentMethod: PaymentMethodCode,
                    PaymentMethodDesc: PaymentMethodDesc,
                    PaymentType: PaymentTypeCode,
                    PaymentTypeDesc: PaymentTypeDesc,
                    peopleOrgCodeId: $('#PeopleOrgCodeIdHidden').val(),
                    ReceiptNumber: null,
                    ReceiverEmail: $("#ReceiverEmail").val(),
                    ReceiverTaxpayerId: $("#ReceiverTaxpayerId").val(),
                    SerialNumber: SerialNumber,
                    StartDate: null,
                    Subtotal: $("#SubTotal").val(),
                    TaxRegimen: IssuerTaxRegimenCode,
                    TaxRegimenDesc: IssuerTaxRegimenDesc,
                    Total: $("#Total").val(),
                    TotalTransferTaxes: $("#TotalTT").val(),
                    // ACOLLI 08112021 EMISIONFACT
                    ExpeditionDate: ExpeditionDate
                    //END

                };
                $.ajax({
                    url: urlProcessPPD,
                    dataType: "json",
                    type: "POST",
                    cache: false,
                    data: ParamInsInvoiceHeaderList,
                    success: function (response) {
                        if (response.id <= 0) {
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            $('#btnProcessPPD').css('display', 'block');
                            $('.errorMessageDiv').show();
                            $(".errorMessageResult").html(response.message);
                        }
                        else {
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            $('.successMessageDiv').show();
                            $('.errorMessageDiv').hide();
                            $(".successMessageResult").html(response.message);
                            if (IsSubstitution) {
                                window.location.href = urlViewAll;
                            }
                            else {
                                window.location.href = urlViewPeoOrgId;
                            }
                        }
                    }
                });
            }

            else {
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                $('.errorMessageRequiredDiv').show();
                $('.successMessageDiv').hide();
                $('#btnProcessPPD').css('display', 'block');
            }
        }
    });

    // #endregion Process
});