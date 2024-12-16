$(document).ready(function () {
    var localeBrowser = "es";

    // ACOLLI EmisFact 08112021
    var ExpeditionDate = "";
    var validateDate = moment(new Date()).endOf('month').add(3, "days").format('YYYY-MM-DD');
    //END

    var IsSubstitution = $('#IsSubstitution').val() == 'True';
    var IssuerTaxPayerId = $('#IssTaxPayerId');
    var IssuerExpAdd = $('#IssIssuingAddress');
    var IssPlaceIssue = $('#IssPlaceIssue');
    var IssTaxRegimenC = $('#IssTaxRegimen');
    var IssuerTaxRegimenCode = '';
    var IssuerTaxRegimenDesc = '';
    var fIdentityNumber = '';
    var fResidency = '';
    var fResidencyDesc = '';

    var TaxPayers = [];
    var IssTaxPayers = [];
    var IssIssuingAddress;

    var IdIssTaxPayer = 0;

    if (window.navigator !== undefined) {
        if (window.navigator.languages !== undefined) {
            localeBrowser = (window.navigator.languages[0] || window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
        else {
            localeBrowser = (window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
    }

    // EmisFact ACOLLI 04112021
    $("#IssueAlert").css('display', 'none');

    $("#IssueDate").datetimepicker({
        viewMode: 'days',
        format: 'DD/MM/YYYY',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date(),
        ignoreReadonly: true
    });

    // $('#IssueDate').data('DateTimePicker').minDate(defDay);

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
    console.log("concat data: ", ExpeditionDate);

    $('#IssueDate').datetimepicker().on('dp.change', function (e) {
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
        $(this).data("DateTimePicker").hide();
    });

    $("#IssueTime").datetimepicker().on('dp.change', function (e) {
        // console.log(moment(new Date(e.date)).format("HH:mm"));
        strExpTime = moment(new Date(e.date)).format("HH:mm");
        // console.log("concat data: ", strExpDate + " " + strExpTime);
        ExpeditionDate = `${strExpDate} ${strExpTime}`;
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
    });
    // END EmisFact ACOLLI 04112021

    $('#btnBack').click(function () {
        showProcessing();
        $('#btnBack').prop('disabled', true);
        window.location.href = urlEdit;
    });

    // #region Receiver

    if (IsSubstitution) {
        LoadReceiverInfoForInvoice();
    }

    function LoadReceiverInfoForInvoice() {
        var RecTaxPayIdValue = $('#TaxPayerId').val();
        $.ajax({
            url: urlGetReceivers,
            type: 'GET',
            cache: false,
            dataType: 'json',
            data: { id: RecTaxPayIdValue },
            success: function (data) {
                TaxPayers = data;
                onTaxPayerIdFocusOut();
            }
        });
    }

    $('#TaxPayerId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetReceivers,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: request.term },
                success: function (data) {
                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === ForeignTaxpayerId)
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber }
                        else
                            return { label: item.TaxPayerId, value: item.TaxPayerId }
                    }));
                }
            })
        }
    });

    $('#TaxPayerId').focusout(onTaxPayerIdFocusOut);

    $('#TaxPayerId').keyup(hideTaxPayerIdMessages);

    function onTaxPayerIdFocusOut() {
        hideTaxPayerIdMessages();
        var IdTaxpayer = $('#TaxPayerId').val();
        var RecTaxPayId = $('#TaxPayerId');
        var response;

        var res = IdTaxpayer.split('-');
        if (res.length > 1) {
            var TaxPayerIdVal = res[0];
            var FiscalIdentityNumber = res[1];
            response = $.grep(TaxPayers, function (element) { return element.FiscalIdentityNumber == FiscalIdentityNumber })

            if (response.length > 0) {
                $('#NameCorpName').val(response[0].CorporateName);
                $('#FiscalAddress').val(response[0].FiscalResidency + '-' + response[0].FiscalResidencyDesc);
                $('#FiscalAddressCode').val(response[0].FiscalResidency);
                fResidency = response[0].FiscalResidency;
                fResidencyDesc = response[0].FiscalResidencyDesc;
                $('#PostalCode').val(response[0].PostalCode);
                $('#RecTaxRegimen').val(response[0].TaxRegimenCode + ' - ' + response[0].TaxRegimenDesc);
                $('#FiscalIdNum').val(response[0].FiscalIdentityNumber);
                fIdentityNumber = response[0].FiscalIdentityNumber;
            }
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').hide();
            $('#RecTaxPayerGroup').removeClass('esg-has-error');
            $('#TaxPayerId').val(TaxPayerIdVal);
        }
        else
            response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == IdTaxpayer; })

        if ((IdTaxpayer.length > 0 || response.length > 0) && res.length === 1) {
            if (response.length > 0) {
                $('#NameCorpName').val(response[0].CorporateName);
                $('#FiscalAddress').val(response[0].FiscalResidencyDesc);
                $('#FiscalAddressCode').val(response[0].FiscalResidency);
                fResidency = response[0].FiscalResidency;
                fResidencyDesc = response[0].FiscalResidencyDesc;
                $('#FiscalIdNum').val(response[0].FiscalIdentityNumber);
                $('#PostalCode').val(response[0].PostalCode);
                $('#RecTaxRegimen').val(response[0].TaxRegimenCode + ' - ' + response[0].TaxRegimenDesc);
                fIdentityNumber = response[0].FiscalIdentityNumber;
            }
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').hide();
            $('#RecTaxPayerGroup').removeClass('esg-has-error');
            $('#RecTaxPayerDivLookup').show();
        }
        else if (response.length === 0 && res.length === 1) {
            $('#NameCorpName').val('');
            $('#FiscalAddress').val('');
            $('#FiscalIdNum').val('');
            RecTaxPayId.parent().addClass('has-error');
            $('#RecTaxPayerIcon').show();
            $('#RecTaxPayerDivLookup').hide();
            $('#RecTaxPayerGroup').addClass('esg-has-error');
        }
        TaxPayers = [];

        //VALIDATE IF TAXPAYER ID ITS COMPANY O PERSON
        $.ajax({
            url: urlGetCFDIReceivers,
            type: 'GET',
            cache: false,
            dataType: 'json',
            data: { length: IdTaxpayer.length },
            success: function (res) {
                var cfdiPreviousValue = $('#CFDI').val();
                $('#CFDI').empty();
                $('#CFDI').append($('<option></option>').val(0).html(lblSelect));
                $.each(res, function (i, val) {
                    $('#CFDI').append($('<option></option>').val(val.Code).html(val.Description));
                });

                if ($('#PreferredEmail').val()) {
                    $('#PreferredEmail').parent().removeClass('has-error');
                    $('#EmailGroup').removeClass('esg-has-error');
                    $('#EmailIcon').hide();
                }

                if ($('#PostalCode').val()) {
                    $('#PostalCode').parent().removeClass('has-error');
                    $('#PostalCodeGroup').removeClass('esg-has-error');
                    $('#PostalCodeIcon').hide();
                }

                if (cfdiPreviousValue && res && res.find(r => r.Code == cfdiPreviousValue)) {
                    $('#CFDI').val(cfdiPreviousValue);
                }
                if ($('#CFDI').val()) {
                    $('#CFDI').parent().removeClass('has-error');
                    $('#CFDIGroup').removeClass('esg-has-error');
                    $('#CFDIIcon').hide();
                }

                if ($('#RecTaxRegimen').val()) {
                    $('#RecTaxRegimen').parent().removeClass('has-error');
                    $('#RecTaxRegimenGroup').removeClass('esg-has-error');
                    $('#RecTaxRegimenIcon').hide();
                }

                $('#formCreate').clearValidation();
            }
        });
    }

    $('#PreferredEmail').change(function () {
        var Email = $('#PreferredEmail');
        if (!Email.val()) {
            Email.parent().addClass('has-error');
            $('#EmailGroup').addClass('esg-has-error');
            $('#EmailIcon').css('display', 'block');
        }
        else {
            $('#EmailGroup').removeClass('esg-has-error');
            $('#EmailIcon').css('display', 'none');
            Email.parent().removeClass('has-error');
        }
    });

    $('#CFDI').change(function () {
        var cfdi = $('#CFDI');

        if (!cfdi.val()) {
            $('#CFDI').parent().addClass('has-error');
            $('#CFDIGroup').addClass('esg-has-error');
            $('#CFDIIcon').css('display', 'block');
        }
        else {
            $('#CFDI').parent().removeClass('has-error');
            $('#CFDIGroup').removeClass('esg-has-error');
            $('#CFDIIcon').hide();
        }
    });

    $('#btnAddReceiver').click(AddReceiverInModal);

    $('#btnEditReceiver').click(function () {
        EditReceiverInModal('TaxPayerId', 'FiscalIdNum');
    });

    $('#btnCancelSaveReceiver, #btnCloseSaveReceiver').click(CloseReceiverModal);

    $('#btnCloseSaveReceiverAfterSave').click(function () {
        if ($('#ReceiverPanel #TaxPayerId').val() == ForeignTaxpayerId) {
            $('#TaxPayerId').val($('#ReceiverPanel #TaxPayerId').val() + '-' + $('#ReceiverPanel #FiscalIdentityNumber').val());
        }
        else {
            $('#TaxPayerId').val($('#ReceiverPanel #TaxPayerId').val());
        }
        LoadReceiverInfoForInvoice();
        CloseReceiverModal();
    });

    // #endregion Receiver

    // #region Issuer

    if (IsSubstitution) {
        var IssTaxPayerIdValue = $('#IssTaxPayerId').val();
        $.ajax({
            url: urlGetIssuers,
            type: 'GET',
            cache: false,
            dataType: 'json',
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
            type: 'GET',
            cache: false,
            success: function (data) {

                if (data.IssInvoiceOrganizationId != 0) {
                    //Existe registro en la tabla se muestra boton de Actualizar
                    $('#IssSerial').empty();
                    $('#IssIssuingAddress').empty();
                    $('#IssTaxPayerId').val(data.IssInvoiceTaxpayerId);
                    $('#IssIssuingAddress').append($('<option></option>').val(data.IssInvoiceExpeditionId).html(data.IssIssuingAddress));
                    $('#IssSerial').append($('<option></option>').val(data.IssSerialNumber).html(data.IssSerialNumber));
                    $('#PaymentCondition').val(data.IssPaymentConditions);
                    $('#IssNameCorpName').val(data.IssCorporateName);

                    var InvoiceOrganizationId = data.IssInvoiceOrganizationId;
                    var InvoiceExpId = data.IssInvoiceExpeditionId;
                    var SerialNum = $('#IssSerial').val().trim();

                    $.ajax({
                        url: urlGetTaxRegimen,
                        type: 'GET',
                        cache: false,
                        dataType: 'json',
                        data: { id: InvoiceOrganizationId },
                        success: function (response) {

                            if (response.IssTaxRegimen != null) {

                                var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element });
                                IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                                IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                                $('#IssTaxRegimen').val(IssTaxRegimen[0].IssCodeValue + ' - ' + IssTaxRegimen[0].IssLongDesc);
                                $('#IssTaxRegimenIcon').css('display', 'none');
                                $('#IssTaxRegimenGroup').parent().removeClass('esg-has-error');
                                IssTaxRegimenC.parent().removeClass('has-error');
                                $('#IssTaxPayerDivLookup').show();
                            }
                            else {
                                if ($('#IssTaxPayerId').val() === '') {
                                    $('#IssTaxRegimenIcon').css('display', 'block');
                                    $('#IssTaxRegimenGroup').parent().addClass('esg-has-error');
                                    IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                                    $('#IssTaxPayerDivLookup').hide();
                                    $('#IssTaxRegimen').val('');
                                }
                            }

                            if (response.IssIssuingAdd != null) {

                                IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                                $.each(IssIssuingAddress, function (i, val) {

                                    if (InvoiceExpId != val.IssInoviceExpeditionId) {
                                        $('#IssIssuingAddress').append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                    }
                                });

                                var IssuingAddress = $.grep(response.IssIssuingAdd, function (elem) { return elem.IssInoviceExpeditionId == InvoiceExpId });

                                if (IssuingAddress[0].IssPlaceIssue != null) {
                                    $('#IssPlaceIssue').val(IssuingAddress[0].IssPlaceIssue);
                                    $('#IssPlaceIssueIcon').css('display', 'none');
                                    $('#IssPlaceIssueGroup').parent().removeClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-error');
                                }
                                else {
                                    $('#IssPlaceIssue').val('');
                                    $('#IssPlaceIssueIcon').css('display', 'block');
                                    $('#IssPlaceIssueGroup').parent().addClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-success');
                                }

                                if (IssIssuingAddress[0].IssByExpedition === 0) {
                                    InvoiceExpId = null;
                                }
                                //GET SerialNumber
                                $.ajax({
                                    url: urlGetSerialNumber,
                                    type: 'GET',
                                    cache: false,
                                    dataType: 'json',
                                    data: { id: InvoiceOrganizationId, expId: InvoiceExpId },
                                    success: function (response) {

                                        $('#IssSerial').empty();
                                        $('#lblWarnSerial').css('display', 'none');

                                        if (response.IssSerial != null) {
                                            var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                            var SerialPrefer = $.grep(IssSerial, function (e) { return e.IssSerialNumber == SerialNum })

                                            if (SerialPrefer[0].IssLastFolio == '2147483647') {

                                                $('#IssSerial').append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr('id', 1));

                                            }
                                            else {
                                                $('#IssSerial').append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr('id', 0));

                                            }

                                            $.each(IssSerial, function (i, val) {
                                                if ($('#IssSerial').val() != val.IssInvoiceReceipt) {

                                                    if (val.IssLastFolio == '2147483647') {
                                                        $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 1));

                                                    }
                                                    else {
                                                        $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 0));
                                                    }
                                                }
                                            });

                                            var selected = $('#IssSerial').find('option:selected');
                                            var optionVal = selected.attr('id');

                                            if (optionVal == '1') {
                                                $('#lblWarnSerial').css('display', 'block');
                                            }
                                            else {
                                                $('#lblWarnSerial').css('display', 'none');
                                            }

                                        }
                                        else {

                                            $('#IssSerial').empty();
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
                else {

                }

            }
        });
    }

    $('#IssTaxPayerId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetIssuers,
                type: 'GET',
                cache: false,
                dataType: 'json',
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

    $('#IssTaxPayerId').focusout(onIssTaxPayerIdFocusOut);

    function onIssTaxPayerIdFocusOut() {

        IdIssTaxPayer = $('#IssTaxPayerId').val();

        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];

        if (!response == '') {

            $('#IssNameCorpName').val('');
            $('#IssTaxRegimen').empty();
            $('#IssNameCorpName').val('');
            $('#IssSerial').empty();
            $('#IssIssuingAddress').empty();
            $('#IssPlaceIssue').val('');

            IssuerTaxPayerId.parent().removeClass('has-error');
            $('#IssTaxPayerIcon').css('display', 'none');
            $('#IssTaxPayerGroup').parent().removeClass('esg-has-error');
            $('#IssTaxPayerDivLookup').show();
            $('#IssNameCorpName').val(response.IssCorporateName);
            $('#IssIssuingAddress').parent().removeClass('esg-has-error');

            //GET TAXREGIMEN
            $.ajax({
                url: urlGetTaxRegimen,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: response.IssInvoiceOrganizationId },
                success: function (response) {

                    if (response.IssTaxRegimen != null) {

                        var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element });
                        IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                        IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                        $('#IssTaxRegimen').val(IssTaxRegimen[0].IssCodeValue + ' - ' + IssTaxRegimen[0].IssLongDesc);
                        $('#IssTaxRegimenIcon').css('display', 'none');
                        $('#IssTaxRegimenGroup').parent().removeClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-error');
                        $('#IssTaxPayerDivLookup').show();
                    }
                    else {
                        $('#IssTaxRegimenIcon').css('display', 'block');
                        $('#IssTaxRegimenGroup').parent().addClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                        $('#IssTaxPayerDivLookup').hide();
                        $('#IssTaxRegimen').val('');
                    }

                    if (response.IssIssuingAdd != null) {

                        var InvoiceExpeditionId;

                        IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                        $.each(IssIssuingAddress, function (i, val) {
                            $('#IssIssuingAddress').append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
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
                            InvoiceExpeditionId = issPlaceIssueFound.IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound != null && issPlaceIssueFound.IssPlaceIssue != null) {
                            $('#IssIssuingAddress').val(issPlaceIssueFound.IssInoviceExpeditionId);
                            $('#IssPlaceIssue').val(issPlaceIssueFound.IssPlaceIssue);
                            $('#IssPlaceIssueIcon').css('display', 'none');
                            $('#IssPlaceIssueGroup').parent().removeClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-error');
                        }
                        else {
                            $('#IssPlaceIssue').val('');
                            $('#IssPlaceIssueIcon').css('display', 'block');
                            $('#IssPlaceIssueGroup').parent().addClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-success');
                        }

                        //GET SerialNumber
                        $.ajax({
                            url: urlGetSerialNumber,
                            type: 'GET',
                            cache: false,
                            dataType: 'json',
                            data: { id: issPlaceIssueFound.IssInvoiceOrganizationId, expId: InvoiceExpeditionId },
                            success: function (response) {

                                $('#lblWarnSerial').css('display', 'none');

                                if (response.IssSerial != null) {
                                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                    $.each(IssSerial, function (i, val) {

                                        if (val.IssLastFolio == '2147483647') {
                                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 1));
                                        }
                                        else {
                                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 0));
                                        }

                                    });
                                    if (SusbsSerialNumber != null && IssSerial.length > 0 && IssSerial.find(s => s.IssSerialNumber == SusbsSerialNumber)) {
                                        $('#IssSerial').val(SusbsSerialNumber);
                                    }
                                }
                                else {
                                    $('#IssSerial').empty();
                                }
                                SusbsSerialNumber = null;

                                var selected = $('#IssSerial').find('option:selected');
                                var optionVal = selected.attr('id');

                                if (optionVal == '1') {
                                    $('#lblWarnSerial').css('display', 'block');
                                }
                                else {
                                    $('#lblWarnSerial').css('display', 'none');
                                }
                            }
                        });

                        $('#IssIssuingAddressIcon').css('display', 'none');
                        $('#IssIssuingAddressGroup').parent().removeClass('esg-has-error');
                        IssuerExpAdd.parent().removeClass('has-error');

                    }
                    else {
                        $('#IssIssuingAddress').empty();
                        $('#IssIssuingAddressIcon').css('display', 'block');
                        $('#IssIssuingAddressGroup').parent().addClass('esg-has-error');
                        $('#IssIssuingAddress').parent().addClass('esg-has-error');
                        IssuerExpAdd.parent().addClass('has-error');
                        $('#IssPlaceIssue').val('');
                        $('#IssPlaceIssueIcon').css('display', 'block');
                        $('#IssPlaceIssueGroup').parent().addClass('esg-has-error');
                        IssPlaceIssue.parent().addClass('has-error');
                    }
                }
            });

        }
        else {

            if (IdIssTaxPayer == '') {
                IssuerTaxPayerId.parent().addClass('has-error');
                $('#IssTaxPayerIcon').css('display', 'block');
                $('#IssTaxPayerGroup').parent().addClass('esg-has-error');
                $('#IssTaxRegimenIcon').css('display', 'block');
                $('#IssTaxRegimenGroup').parent().addClass('esg-has-error');
                IssTaxRegimenC.parent().addClass('has-error');
                $('#IssPlaceIssueIcon').css('display', 'block');
                $('#IssPlaceIssueGroup').parent().addClass('esg-has-error');
                IssPlaceIssue.parent().addClass('has-error');
                $('#IssIssuingAddressIcon').css('display', 'block');
                $('#IssIssuingAddressGroup').parent().addClass('esg-has-error');
                IssuerExpAdd.parent().addClass('has-error');
                $('#IssTaxRegimen').val('');
                $('#IssTaxPayerDivLookup').hide();
                $('#IssNameCorpName').val('');
                $('#IssSerial').empty();
                $('#IssIssuingAddress').val('');
                $('#IssPlaceIssue').val('');
            }
        }

    }

    $('#IssIssuingAddress').change(function () {

        var InvoiceExpeditionId = 0;
        var val = $('#IssIssuingAddress option:selected').text();
        var PlaceIssue = $.grep(IssIssuingAddress, function (element) { return element.IssIssuingAddress == val });
        var IssByExpedition = PlaceIssue[0].IssByExpedition;

        if (IssByExpedition === 0) {
            InvoiceExpeditionId = null;
        }
        else {
            InvoiceExpeditionId = PlaceIssue[0].IssInoviceExpeditionId;
        }

        //GET SerialNumber
        $.ajax({
            url: urlGetSerialNumber,
            type: 'GET',
            cache: false,
            dataType: 'json',
            data: { id: PlaceIssue[0].IssInvoiceOrganizationId, expId: InvoiceExpeditionId },
            success: function (response) {

                if (response.IssSerial != null) {
                    $('#IssSerial').empty();
                    $('#lblWarnSerial').css('display', 'none');

                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                    $.each(IssSerial, function (i, val) {

                        if (val.IssLastFolio == '2147483647') {
                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 1));

                        }
                        else {
                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 0));
                        }

                    });

                    var selected = $('#IssSerial').find('option:selected');
                    var optionVal = selected.attr('id');

                    if (optionVal == '1') {
                        $('#lblWarnSerial').css('display', 'block');
                    }
                    else {
                        $('#lblWarnSerial').css('display', 'none');
                    }

                }
                else {

                    $('#IssSerial').empty();
                }
            }
        });

        $('#IssPlaceIssue').val(PlaceIssue[0].IssPlaceIssue);

    });

    $('#IssPlaceIssue').focusout(function () {

        if (IssPlaceIssue.val() != '') {

            $('#IssPlaceIssueIcon').css('display', 'none');
            $('#IssPlaceIssueGroup').parent().removeClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-error');
        }
        else {

            $('#IssPlaceIssueIcon').css('display', 'block');
            $('#IssPlaceIssueGroup').parent().addClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-success').addClass('has-error');

        }

    });

    $('#IssSerial').change(function () {

        var selected = $(this).find('option:selected');
        var optionVal = selected.attr('id');

        if (optionVal == '0') {
            $('#lblWarnSerial').css('display', 'none');
        }
        else {
            $('#lblWarnSerial').css('display', 'block');
        }

    });

    // #endregion Issuer

    // #region Payment

    $('#PaymentCondition').keypress(function (e) {

        if (e.keyCode == 124 || e.key == '|') {
            return false;
        }
    });

    $('#IssComments').keypress(function (e) {

        if (e.keyCode == 124 || e.key == '|') {
            return false;
        }
    });

    // #endregion Payment

    // #region Charge

    $('.ChargeDesc').keypress(function (e) {

        if (e.keyCode == 124 || e.key == '|') {
            return false;
        }

    });

    $('input[id^="ccUnitDesc_"]').keyup(function (e) {
        var inputElement = $(this);
        var idElement = inputElement.prop('id');
        var splitIdElement = idElement.split('_');
        if (inputElement.val().length > 20) {
            $(`#msgUnitMaxLength_${splitIdElement[1]}`).show();
            $(`#ccUnitDescGroup_${splitIdElement[1]}`).addClass('esg-has-error');
            $(`#ccUnitDescIcon_${splitIdElement[1]}`).show();
        }
        else {
            $(`#msgUnitMaxLength_${splitIdElement[1]}`).hide();
            $(`#ccUnitDescGroup_${splitIdElement[1]}`).removeClass('esg-has-error');
            $(`#ccUnitDescIcon_${splitIdElement[1]}`).hide();
        }
    });

    // #endregion Charge

    // #region Process

    var createForm = $('#formCreate');
    $('#btnProcessC').click(function () {

        hideTaxPayerIdMessages();

        var unitMaxLengthError = false;
        var inputCharges = $('input[id^="ccUnitDesc_"]');
        for (var i = 0; i < inputCharges.length; i++) {
            if ($(inputCharges[i]).val().length > 20) {
                unitMaxLengthError = true;
            }
        }

        if (unitMaxLengthError) {
            $('.errorMessageRequiredDiv').show();
            $('.successMessageDiv').hide();
            return false;
        }

        var SerialError = $('#lblWarnSerial').css('display');

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
            if (SerialError == 'none') {
                $('.errorMessageSerialError').css('display', 'none');
                var CFDISelected = $('#CFDI option:selected').text();
                var CFDICode = $('#CFDI option:selected').val();
                var CFDIDesc = CFDISelected.substring(CFDICode.length + 1, CFDISelected.length);

                var PaymentTypeSelected = $('#PaymentType option:selected').text();
                var PaymentType = PaymentTypeSelected.split('-');
                var PaymentTypeCode = PaymentType[0].trim();
                var PaymentTypeDesc = PaymentType[1].trim();
                var PaymentMethodSelected = $('#PaymentMethod').val();
                var PaymentMethod = PaymentMethodSelected.split('-');
                var PaymentMethodCode = PaymentMethod[0].trim();
                var PaymentMethodDesc = PaymentMethod[1].trim();

                var SerialNumber = $('#IssSerial option:selected').text();

                $('.errorMessageRequiredDiv').hide();
                $('.errorMessageDiv').hide();

                createForm.validate();

                if (createForm.valid()) {
                    $('.errorMessageRequiredDiv').hide();
                    /*Read Details of Fiscal Record*/
                    var $table = $('#records_table tbody');
                    var cols = [];
                    $table.find('tr').each(function (rowIndex, r) {
                        var frd = new Object();
                        var frDetails = [];
                        $(this).find(":input[type=hidden][name='CashRe.ChargeCreditCodeId']").each(function (index, v) {
                            frDetails.push(v.value);
                        });
                        jQuery.each(frDetails, function (i, val) {
                            rowId = val;
                            $table.find('tr').find('td').each(function (colIndex, c) {
                                if (colIndex === 0) {
                                    frd.ChargeCreditCodeId = rowId;
                                    frd.ChargeCreditCode = $('#ccCode_' + rowId).val();
                                    frd.ProductServiceKey = $('#ccPrdSrv_' + rowId).text();
                                }
                                if (colIndex === 4) {
                                    frd.UnitDescription = $('#ccUnitDesc_' + rowId).val();
                                }
                                if (colIndex === 5) {
                                    frd.Description = $('#ccDesc_' + rowId).val();
                                }
                            });
                            cols.push(frd);
                        });
                    });
                    showPageLoader();
                    $('#btnProcessC').hide();
                    fIdentityNumber = $('#FiscalIdNum').val();

                    var ParamInsInvoiceHeaderList = {
                        CancelReasonKey: '',
                        CancelReasonName: CancelReasonName,
                        CFDIRelated: CFDIRelated,
                        CFDIRelatedId: CFDIRelatedId,
                        CFDIRelationTypeKey: '',
                        CFDIRelationTypeName: null,
                        CFDIUsageCode: CFDICode,
                        CFDIUsageDesc: CFDIDesc,
                        CityOfIssue: $('#IssPlaceIssue').val(),
                        Comments: $('#IssComments').val(),
                        Currency: $('#IssCurrency').val(),
                        Detail: cols,
                        EndDate: null,
                        ExchangeRate: 0.00,
                        FiscalIdentityNumber: fIdentityNumber,
                        FiscalResidency: fResidency,
                        FiscalResidencyDesc: fResidencyDesc,
                        InvoiceExpeditionId: $('#IssIssuingAddress').val(),
                        IssuerTaxPayerId: $('#IssTaxPayerId').val(),
                        PaymentAccountNumber: '',
                        PaymentCondition: $('#PaymentCondition').val(),
                        PaymentMethod: PaymentMethodCode,
                        PaymentMethodDesc: PaymentMethodDesc,
                        PaymentType: PaymentTypeCode,
                        PaymentTypeDesc: PaymentTypeDesc,
                        peopleOrgCodeId: PeopleOrgId,
                        ReceiptNumber: ReceiptNumber,
                        ReceiverEmail: $('#PreferredEmail').val(),
                        ReceiverTaxpayerId: $('#TaxPayerId').val(),
                        SerialNumber: SerialNumber,
                        StartDate: null,
                        Subtotal: $('#SubTotal').val(),
                        TaxRegimen: IssuerTaxRegimenCode,
                        TaxRegimenDesc: IssuerTaxRegimenDesc,
                        Total: $('#Total').val(),
                        TotalTransferTaxes: $('#TotalTT').val(),
                        // ACOLLI 08112021
                        ExpeditionDate: ExpeditionDate
                        // END
                    };
                    $.ajax({
                        url: urlCreateFiscalRecord,
                        dataType: 'json',
                        type: 'POST',
                        cache: false,
                        data: ParamInsInvoiceHeaderList,
                        success: function (response) {
                            if (response.id <= 0) {
                                $('#Processing').css('display', 'none');
                                $('#Overlaydiv').css('display', 'none');
                                $('#btnProcessC').css('display', 'block');
                                $('.errorMessageDiv').show();
                                $('.errorMessageResult').html(response.message);
                            }
                            else {
                                $('#Processing').css('display', 'none');
                                $('#Overlaydiv').css('display', 'none');
                                $('.successMessageDiv').show();
                                $('.errorMessageDiv').hide();
                                $('.successMessageResult').html(response.message);

                                if (IsSubstitution) {
                                    window.location.href = urlViewAll;
                                }
                                else {
                                    $.ajax({
                                        url: urlReceiptNumber,
                                        type: 'GET',
                                        cache: false,
                                        dataType: 'html',
                                        data: { receiptNumber: ReceiptNumber },
                                        success: function (response) {
                                            window.location.href = urlCashReceipts;
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
                else {
                    hidePageLoader();
                    $('#btnProcessC').css('display', 'block');
                    $('.errorMessageRequiredDiv').show();
                    $('.successMessageDiv').hide();

                    if (!$('#RecTaxRegimen').val()) {
                        $('#RecTaxRegimen').parent().addClass('has-error');
                        $('#RecTaxRegimenGroup').addClass('esg-has-error');
                        $('#RecTaxRegimenIcon').css('display', 'block');
                    }

                    if (!$('#PreferredEmail').val()) {
                        $('#PreferredEmail').parent().addClass('has-error');
                        $('#EmailGroup').addClass('esg-has-error');
                        $('#EmailIcon').css('display', 'block');
                    }

                    if (!$('#TaxPayerId').val()) {
                        $('#TaxPayerId').parent().addClass('has-error');
                        $('#RecTaxPayerGroup').addClass('esg-has-error');
                        $('#RecTaxPayerIcon').show();
                        $('#RecTaxPayerDivLookup').hide();
                    }

                    if (!$('#PostalCode').val()) {
                        $('#PostalCode').parent().addClass('has-error');
                        $('#PostalCodeGroup').addClass('esg-has-error');
                        $('#PostalCodeIcon').css('display', 'block');
                    }

                    if (!$('#CFDI').val()) {
                        $('#CFDI').parent().addClass('has-error');
                        $('#CFDIGroup').addClass('esg-has-error');
                        $('#CFDIIcon').css('display', 'block');
                    }
                }
            }
            else {
                $('.errorMessageSerialError').css('display', 'block');
                $('.errorMessageRequiredDiv').hide();
                $('.errorMessageDiv').hide();
            }
        }
    });

    // #endregion Process
});