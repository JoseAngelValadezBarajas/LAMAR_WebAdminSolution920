$(document).ready(function () {
    var createForm = $('#formCreate');

    var TaxPayers = [];
    var IssTaxPayers = [];
    var IssIssuingAddress;

    var IdIssTaxPayer = 0;

    //RECEIVER-----------------------------------------

    $('#RecTaxpayerId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlReceiversGet,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: request.term },
                success: function (data) {
                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === 'XEXX010101000')
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber, id: item.InvoiceTaxpayerId }
                        else
                            return { label: item.TaxPayerId + '-' + item.CorporateName, value: item.TaxPayerId }
                    }))

                }
            })
        }
    });

    $('#RecTaxpayerId').focusout(function () {
        var IdTaxpayer = $(this).val();
        var response;
        var TaxPayerIdVal;

        if (IdTaxpayer.indexOf('-') >= 0) {
            var res = IdTaxpayer.split('-');
            TaxPayerIdVal = res[0];
            var fiscalIdentityNumber = res[1];
            response = $.grep(TaxPayers, function (element) { return element.FiscalIdentityNumber === fiscalIdentityNumber });
        }
        else
            response = $.grep(TaxPayers, function (element) { return element.TaxPayerId === IdTaxpayer; });

        if (IdTaxpayer.length > 0 || response.length > 0) {
            $('#RecNameCorpName').val(response[0].CorporateName);
            $('#RecFiscalAddress').val(response[0].FiscalResidencyDesc);
            $('#RecFiscalIdNum').val(response[0].FiscalIdentityNumber);
            hideFormError('RecTaxAddress');
            $('#RecTaxAddress').val(response[0].PostalCode);
            hideFormError('RecTaxRegimen');
            $('#RecTaxRegimen').val(response[0].TaxRegimenCode + ' - ' + response[0].TaxRegimenDesc);
            hideFormError('RecTaxpayerId');
            $('#RecTaxPayerDivLookup').show();
            $('#RecTaxpayerId').val(TaxPayerIdVal || response[0].TaxPayerId);

            createForm.clearValidation();
        }
        else {
            $('#RecNameCorpName').val('');
            $('#RecFiscalAddress').val('');
            $('#RecFiscalIdNum').val('');
            showFormError('RecTaxAddress');
            $('#RecTaxAddress').val('');
            showFormError('RecTaxRegimen');
            $('#RecTaxRegimen').val('');
            showFormError('RecTaxpayerId');
            $('#RecTaxPayerDivLookup').hide();
        }
    });

    $('#ReceiverEmail').change(function () {
        if ($(this).val()) {
            hideFormError($(this).attr('id'));
        }
        else {
            showFormError($(this).attr('id'));
        }
    });

    //ISSUER-------------------------------------------
    $.ajax({
        url: urlIssuersGet,
        type: 'GET',
        cache: false,
        dataType: 'json',
        data: { id: $('#IssTaxPayerId').val() },
        success: function (data) {
            IssTaxPayers = data;
            onIssTaxPayerIdFocusOut();
        }
    });

    function onIssTaxPayerIdFocusOut() {
        IdIssTaxPayer = $('#IssTaxPayerId').val();

        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId === IdIssTaxPayer; })[0];

        if (response) {

            $('#IssNameCorpName').val(response.IssCorporateName);
            $('#IssTaxRegimen').empty();
            $('#IssIssuingAddress').empty();
            $('#IssPlaceIssue').val('');
            $('#IssSerial').empty();

            $('#IssTaxPayerIcon').hide();
            $('#IssTaxPayerGroup').removeClass('esg-has-error');
            $('#IssTaxPayerDivLookup').show();
            $('#IssIssuingAddress').parent().removeClass('esg-has-error');

            //GET TAXREGIMEN
            $.ajax({
                url: urlIssuersGetTaxRegimen,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: response.IssInvoiceOrganizationId },
                success: function (response) {

                    if (response.IssTaxRegimen !== null) {

                        var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element });
                        IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                        IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                        $('#IssTaxRegimenIcon').hide();
                        $('#IssTaxRegimenGroup').removeClass('esg-has-error');
                        $('#IssTaxPayerDivLookup').show();
                        $('#IssTaxRegimen').val(IssTaxRegimen[0].IssCodeValue + ' - ' + IssTaxRegimen[0].IssLongDesc);
                        $('#IssTaxRegimenHidden').val(IssTaxRegimen[0].IssCodeValue);
                        $('#IssTaxRegimenDescHidden').val(IssTaxRegimen[0].IssLongDesc);
                    }
                    else {
                        $('#IssTaxRegimenIcon').show();
                        $('#IssTaxRegimenGroup').addClass('esg-has-error');
                        $('#IssTaxPayerDivLookup').hide();
                        $('#IssTaxRegimen').val('');
                        $('#IssTaxRegimenHidden').val('');
                        $('#IssTaxRegimenDescHidden').val('');
                    }

                    if (response.IssIssuingAdd !== null) {

                        var InvoiceExpeditionId;

                        IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                        $.each(IssIssuingAddress, function (i, val) {
                            $('#IssIssuingAddress').append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                        });

                        var issPlaceIssueFound;
                        if (PPDInvoiceExpeditionId != null && PPDInvoiceExpeditionId > 0) {
                            issPlaceIssueFound = IssIssuingAddress.find(a => a.IssInoviceExpeditionId == PPDInvoiceExpeditionId);
                            if (issPlaceIssueFound == null) {
                                issPlaceIssueFound = IssIssuingAddress[0];
                            }
                        }
                        else {
                            issPlaceIssueFound = IssIssuingAddress[0];
                        }
                        PPDInvoiceExpeditionId = null;

                        if (issPlaceIssueFound.IssByExpedition === 0) {
                            InvoiceExpeditionId = null;
                        }
                        else {
                            InvoiceExpeditionId = issPlaceIssueFound.IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound != null && issPlaceIssueFound.IssPlaceIssue != null) {
                            $('#IssIssuingAddress').val(issPlaceIssueFound.IssInoviceExpeditionId);
                            $('#IssPlaceIssue').val(issPlaceIssueFound.IssPlaceIssue);
                            $('#IssPlaceIssueIcon').hide();
                            $('#IssPlaceIssueGroup').removeClass('esg-has-error');
                        }
                        else {
                            $('#IssPlaceIssue').val('');
                            $('#IssPlaceIssueIcon').show();
                            $('#IssPlaceIssueGroup').addClass('esg-has-error');
                        }

                        //GET SerialNumber
                        $.ajax({
                            url: urlIssuersGetSerialNumber,
                            type: 'GET',
                            cache: false,
                            dataType: 'json',
                            data: { id: issPlaceIssueFound.IssInvoiceOrganizationId, expId: InvoiceExpeditionId },
                            success: function (response) {

                                $('#lblWarnSerial').hide();

                                if (response.IssSerial !== null) {
                                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                    $.each(IssSerial, function (i, val) {

                                        if (val.IssLastFolio === '2147483647') {
                                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 1));
                                        }
                                        else {
                                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 0));
                                        }

                                    });
                                    if (PPDSerialNumber != null && IssSerial.length > 0 && IssSerial.find(s => s.IssSerialNumber == PPDSerialNumber)) {
                                        $('#IssSerial').val(PPDSerialNumber);
                                    }
                                }
                                else {
                                    $('#IssSerial').empty();
                                }
                                PPDSerialNumber = null;

                                var selected = $('#IssSerial').find('option:selected');
                                var optionVal = selected.attr('id');

                                if (optionVal === '1') {
                                    $('#lblWarnSerial').show();
                                }
                                else {
                                    $('#lblWarnSerial').hide();
                                }
                            }
                        });

                        hideFormError('IssIssuingAddress');
                    }
                    else {
                        $('#IssIssuingAddress').empty();
                        showFormError('IssIssuingAddress');
                        $('#IssPlaceIssue').val('');
                        showFormError('IssPlaceIssue');
                    }
                }
            });

            createForm.clearValidation();
        }
        else if (IdIssTaxPayer === '') {
            $('#IssTaxPayerIcon').show();
            $('#IssTaxPayerGroup').addClass('esg-has-error');
            $('#IssTaxRegimenIcon').show();
            $('#IssTaxRegimenGroup').addClass('esg-has-error');
            $('#IssPlaceIssueIcon').show();
            $('#IssPlaceIssueGroup').addClass('esg-has-error');
            $('#IssIssuingAddressIcon').show();
            $('#IssIssuingAddressGroup').addClass('esg-has-error');
            $('#IssTaxRegimen').val('');
            $('#IssTaxPayerDivLookup').hide();
            $('#IssNameCorpName').val('');
            $('#IssSerial').val('');
            $('#IssSerial').empty();
            $('#IssIssuingAddress').val('');
            $('#IssIssuingAddress').empty('');
            $('#IssPlaceIssue').val('');
        }
    }

    $('#IssTaxPayerId').autocomplete({

        source: function (request, response) {
            $.ajax({
                url: urlIssuersGet,
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
            });
        }
    });

    $('#IssTaxPayerId').focusout(onIssTaxPayerIdFocusOut);

    $('#IssIssuingAddress').change(function () {

        var InoviceExpeditionId = 0;
        var val = $('#IssIssuingAddress option:selected').text();
        var PlaceIssue = $.grep(IssIssuingAddress, function (element) { return element.IssIssuingAddress === val });
        var IssByExpedition = PlaceIssue[0].IssByExpedition;

        if (IssByExpedition === 0) {
            InoviceExpeditionId = null;
        }
        else {
            InoviceExpeditionId = PlaceIssue[0].IssInoviceExpeditionId;
        }
        $('#IssSerial option').remove();

        //GET SerialNumber
        $.ajax({
            url: urlIssuersGetSerialNumber,
            type: 'GET',
            cache: false,
            dataType: 'json',
            data: { id: PlaceIssue[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
            success: function (response) {

                if (response.IssSerial !== null) {
                    $('#IssSerial').empty();
                    $('#lblWarnSerial').hide();

                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                    $.each(IssSerial, function (i, val) {

                        if (val.IssLastFolio === '2147483647') {
                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 1));

                        }
                        else {
                            $('#IssSerial').append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr('id', 0));
                        }

                    });

                    var selected = $('#IssSerial').find('option:selected');
                    var optionVal = selected.attr('id');

                    if (optionVal === '1') {
                        $('#lblWarnSerial').show();
                    }
                    else {
                        $('#lblWarnSerial').hide();
                    }

                }
                else {

                    $('#IssSerial').empty();
                }
            }
        });

        $('#IssPlaceIssue').val(PlaceIssue[0].IssPlaceIssue);

    });

    $('#IssSerial').change(function () {

        var selected = $(this).find('option:selected');
        var optionVal = selected.attr('id');

        if (optionVal === '0') {
            $('#lblWarnSerial').hide();
        }
        else {
            $('#lblWarnSerial').show();
        }

    });

    $('#Comments').keypress(function (e) {
        if (e.keyCode === 124 || e.key === '|') {
            return false;
        }
    });

    $('#InstallmentNumber').focusout(function () {
        if ($('#InstallmentNumber').val() !== '' && parseInt($('#InstallmentNumber').val()) > 0) {
            $('#InstallmentNumber').parent().removeClass('esg-has-error');
            $('#lblInstallmentNumber').hide();
            $('#lblInstallmentNumberMinValue').hide();
        }
        else {
            $('#InstallmentNumber').parent().addClass('esg-has-error');

            if ($('#InstallmentNumber').val() === '')
                $('#lblInstallmentNumber').show();
            if (parseInt($('#InstallmentNumber').val()) <= 0)
                $('#lblInstallmentNumberMinValue').show();
        }
    });

    // COMPLEMENT-------------------------------------
    $('#OriginAccountIssuerTaxPayerId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlReceiversGet,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: request.term },
                success: function (data) {
                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === 'XEXX010101000')
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber, id: item.InvoiceTaxpayerId }
                        else
                            return { label: item.TaxPayerId + '-' + item.CorporateName, value: item.TaxPayerId }
                    }))

                }
            })
        },
        select: function (event, ui) {

            var selected = $.grep(TaxPayers, function (v, i) {
                return v.TaxPayerId == ui.item.value;
            })[0];

            $('#OriginAccountIssuerTaxPayerIdHidden').val(selected.InvoiceTaxpayerId);
        }
    });

    $('#TargetAccountIssuerTaxPayerId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlReceiversGet,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: request.term },
                success: function (data) {
                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === 'XEXX010101000')
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber }
                        else
                            return { label: item.TaxPayerId + '-' + item.CorporateName, value: item.TaxPayerId }
                    }))

                }
            })
        },
        select: function (event, ui) {

            var selected = $.grep(TaxPayers, function (v, i) {
                return v.TaxPayerId == ui.item.value;
            })[0];

            $('#TargetAccountIssuerTaxPayerIdHidden').val(selected.InvoiceTaxpayerId);
        }
    });

    $('#PaymentTypeComplement').change(function () {
        if ($(this).val()) {
            hideFormError($(this).attr('id'));
        }
        else {
            showFormError($(this).attr('id'));
        }
    });

    // PROCESS----------------------------------------
    $('#btnProcessC').click(function () {

        var SerialError = $("#lblWarnSerial").css('display');

        if (SerialError == 'none') {
            $('.errorMessageSerialError').css('display', 'none');
            $('.errorMessageRequiredDiv').hide();
            $('.errorMessageDiv').hide();
            $('.successMessageDiv').hide();

            createForm.validate();

            if (createForm.valid()) {
                showPageLoader();
                $('#btnProcessC').hide();

                var fiscalResidencyTemp = $("#RecFiscalAddress").val().split('-');

                var CFDISelected = $("#CFDIUsageDesc").val();
                var CFDIUsage = CFDISelected.split('-');
                var CFDICode = CFDIUsage[0].trim();
                var CFDIDesc = CFDIUsage[1].trim();
                var ParamInsInvoiceHeaderList = {
                    ReceiverTaxpayerId: $("#RecTaxpayerId").val(),
                    ReceiverEmail: $("#ReceiverEmail").val(),
                    FiscalIdentityNumber: $("#RecFiscalIdNum").val(),
                    FiscalResidency: fiscalResidencyTemp[0] ? fiscalResidencyTemp[0] : null,
                    FiscalResidencyDesc: fiscalResidencyTemp[1],
                    CFDIUsageCode: CFDICode,
                    CFDIUsageDesc: CFDIDesc,
                    IssuerTaxPayerId: $("#IssTaxPayerId").val(),
                    TaxRegimen: $("#IssTaxRegimenHidden").val(),
                    TaxRegimenDesc: $("#IssTaxRegimenDescHidden").val(),
                    InvoiceExpeditionId: $("#IssIssuingAddress").val(),
                    CityOfIssue: $("#IssPlaceIssue").val(),
                    SerialNumber: $("#IssSerial").val(),
                    Currency: $("#Currency").val(),
                    Comments: $("#Comments").val(),
                    CurrencyComplement: $("#CurrencyComplement").val(),
                    BankName: $("#ForeignerBankName").val(),
                    BeneficiaryAccount: $("#TargetAccount").val(),
                    InstallmentNumber: $("#InstallmentNumber").val(),
                    IssuerTaxPayerIdBeneficiaryAccount: $("#TargetAccountIssuerTaxPayerIdHidden").val(),
                    IssuerTaxPayerIdSourceAccount: $("#OriginAccountIssuerTaxPayerIdHidden").val(),
                    PaymentCertificate: $("#PaymentCertificate").val(),
                    PaymentChain: $("#PaymentOriginalChain").val(),
                    PaymentChainType: $("#PaymentOriginalChainType").val(),
                    PaymentDate: $("#PaymentDate").val(),
                    PaymentStamp: $("#PaymentSignature").val(),
                    PaymentTypeComplement: $("#PaymentTypeComplement").val(),
                    SourceAccount: $("#OriginAccount").val(),
                    TransactionNumber: $("#TransactionNumber").val()
                };

                $.ajax({
                    url: urlCreatePaymentReceipt,
                    dataType: "json",
                    type: "POST",
                    cache: false,
                    data: ParamInsInvoiceHeaderList,
                    success: function (response) {
                        hidePageLoader();
                        if (response.id <= 0) {
                            $('#btnProcessC').show();
                            $('.errorMessageDiv').show();
                            $(".errorMessageResult").html(response.message);
                        }
                        else {
                            $('.errorMessageDiv').hide();
                            $('.successMessageDiv').show();
                            $(".successMessageResult").html(response.message);

                            window.location.href = urlReturn;
                        }
                    }
                });
            }
            else {
                hidePageLoader();
                $('#btnProcessC').show();
                $('.errorMessageRequiredDiv').show();

                if (!$("#RecTaxpayerId").val()) {
                    showFormError('RecTaxpayerId');
                }
                if (!$("#RecTaxAddress").val()) {
                    showFormError('RecTaxAddress');
                }
                if (!$("#RecTaxRegimen").val()) {
                    showFormError('RecTaxRegimen');
                }
                if (!$("#ReceiverEmail").val()) {
                    showFormError('ReceiverEmail');
                }
                if (!$("#IssTaxPayerId").val()) {
                    showFormError('IssTaxPayerId');
                }
                if (!$("#IssTaxRegimen").val()) {
                    showFormError('IssTaxRegimen');
                }
                if (!$("#IssIssuingAddress").val()) {
                    showFormError('IssIssuingAddress');
                }
                if (!$("#IssPlaceIssue").val()) {
                    showFormError('IssPlaceIssue');
                }
                if (!$("#PaymentTypeComplement").val()) {
                    showFormError('PaymentTypeComplement');
                }
            }
        }
        else {
            $('.errorMessageSerialError').show();
            $('.errorMessageRequiredDiv').hide();
            $('.errorMessageDiv').hide();
        }
    });
});