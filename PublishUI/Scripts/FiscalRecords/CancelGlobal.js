// #region CancelGlobalInvoice
$(document).ready(function () {
    $("#btnReprocess").click(function (event) {
        event.preventDefault();
        showProcessing();
        $("#btnReprocess").prop("disabled", true);

        var fiscalRecordRequest = {
            InvoiceHeaderId: invoiceHeaderId,
            Status: 2, // Cancel
        };

        $.ajax({
            url: urlReprocess,
            type: "POST",
            cache: false,
            dataType: "json",
            data: { fiscalRecordRequest: fiscalRecordRequest },
            success: function () {
                window.location.reload();
            }
        });
    });

    $("#btnDelete").click(function () {
        showProcessing();

        var fiscalRecordRequest = {
            InvoiceHeaderId: invoiceHeaderId
        };

        $.ajax({
            url: urlDelete,
            type: "Post",
            cache: false,
            dataType: "json",
            data: { fiscalRecordRequest: fiscalRecordRequest },
            success: function (response) {
                if (response.id <= 0) {
                    hideProcessing();
                    $('#errorAlert').show();
                    $('#errorMessage').html(response.message);
                }
                else {
                    hideProcessing();
                    $('.successAlert').show();
                    $('.errorAlert').hide();
                    $(".successMessage").html(response.message);
                    window.location.href = urlViewAll;
                }
            }
        });
    });
});
// #endregion CancelGlobalInvoice

// #region Shared
function moveToStpNewInvoice() {
    // Navigate to New Invoice step
    showProcessing();
    $("#stpNewInvoice").empty();

    $("#stpCancelGlobalInvoice").hide();
    $('#stpNewInvoice').show();
    $('#stpNewGlobalInvoice').hide();

    if ($('#liCancelGlobalInvoice').hasClass('esg-is-active')) {
        $('#liCancelGlobalInvoice').addClass('esg-is-previous').removeClass('esg-is-active');
    }

    $('#liNewInvoice').addClass('esg-is-active').removeClass('esg-is-previous');


    if ($('#liNewGlobalInvoice').hasClass('esg-is-active')) {
        $('#liNewGlobalInvoice').addClass('esg-is-previous').removeClass('esg-is-active');
    }

    $.ajax({
        url: urlGetCanceledGlobalInvoiceDetails,
        type: "GET",
        cache: false,
        dataType: "html",
        success: function (newInvoiceTablesPartialView) {
            if (newInvoiceTablesPartialView) {
                $("#stpNewInvoice").html(newInvoiceTablesPartialView);
                hideProcessing();
            }
        }
    });
}

function moveToStpNewGlobalInvoice(cancellationIsComplete) {
    // Navigate to New Invoice step
    showProcessing();
    $("#stpGlobalNewInvoice").empty();

    $("#stpCancelGlobalInvoice").hide();
    $('#stpNewInvoice').hide();

    $('#liNewInvoice').addClass('esg-is-previous').removeClass('esg-is-active');
    $('#liNewGlobalInvoice').addClass('esg-is-active').removeClass('esg-is-previous');

    if (cancellationIsComplete) {
        $("#stpNewGlobalInvoiceComplete").show();
        loadNewGlobalInvoice();
        hideProcessing();
    }
    else {
        $('#stpNewGlobalInvoice').show();
        $.ajax({
            url: urlGetNewGlobalInvoiceDetails,
            type: "GET",
            cache: false,
            dataType: "html",
            success: function (newGlobalInvoicePartialView) {
                if (newGlobalInvoicePartialView) {
                    $("#stpNewGlobalInvoice").html(newGlobalInvoicePartialView);
                    loadNewGlobalInvoice();
                    hideProcessing();
                }
            }
        });
    }
}
// #endregion Shared

// #region NewInvoice

function onClickBtnBackCancelGlobalInvoice() {
    // Navigate to Cancel Global Invoice step

    $('#stpCancelGlobalInvoice').show();
    $('#stpNewInvoice').hide();
    $('#stpNewGlobalInvoice').hide();

    $('#liCancelGlobalInvoice').addClass('esg-is-active').removeClass('esg-is-previous');
    $('#liNewInvoice').addClass('esg-is-previous').removeClass('esg-is-active');
}

function onClickBtnViewDetail(receiptNumber) {
    showProcessing();
    $.ajax({
        url: urlGetCashReceiptDetails,
        type: "GET",
        cache: false,
        dataType: "json",
        data: { receiptNumber: receiptNumber },
        success: function (res) {
            if (res.length > 0) {
                var rows = $('.cashReceiptRow:visible');

                var peopleOrgId;
                var name;
                var tmpReceiptNumber;
                var entryDate;
                var taxAmount;
                var amount;
                rows.each(function () {
                    peopleOrgId = $(this).children('td:nth-child(1)').data('peopleOrgId');
                    name = $(this).children('td:nth-child(2)').data('name');
                    tmpReceiptNumber = $(this).children('td:nth-child(3)').data('receiptNumber');
                    entryDate = $(this).children('td:nth-child(4)').data('entryDate');
                    amount = $(this).children('td:nth-child(5)').data('amount');
                    taxAmount = $(this).children('td:nth-child(6)').data('taxAmount');

                    if (tmpReceiptNumber == receiptNumber) {
                        return false;
                    }
                });

                $('#peopleOrgIdField').html(peopleOrgId)
                $('#nameField').html(name)
                $('#receiptNumberField').html(receiptNumber)
                $('#entryDateField').html(entryDate)
                $('#amountField').html(amount)
                $('#taxAmountField').html(taxAmount)

                var rowsHtml = "";
                $.each(res, function (_, chargeCredit) {
                    rowsHtml += `
                        <tr class="esg-table-body__row">
                            <td class="esg-table-body__td text-left">${chargeCredit.ProductServiceKey}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.ChargeCreditCode}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.Quantity}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.UnityKey}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.UnityName}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.ChargeCreditDesc}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.UnitAmount}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.TotalTaxes}</td>
                            <td class="esg-table-body__td text-left">${chargeCredit.TotalUnit}</td>
                            <td class="esg-table-body__td text-right">${chargeCredit.SubjectToTax}</td>
                        </tr>
                    `;
                    $('#receiptDetails_tbody').html(rowsHtml);
                });
                $('#receiptDetailsModalOverlay').show();
                $('#receiptDetailsModal').show();
                hideProcessing();
            }
        }
    });
}

function onClickBtnCreateInvoice(receiptNumber) {
    showProcessing();
    $("#stpNewInvoice").empty();

    $.ajax({
        url: urlGetNewInvoiceToCreate,
        type: "GET",
        cache: false,
        data: { receiptNumber: receiptNumber },
        dataType: "html",
        success: function (newInvoiceCreationPartialView) {
            if (newInvoiceCreationPartialView) {
                $("#stpNewInvoice").html(newInvoiceCreationPartialView);
                loadNewInvoice();
                hideProcessing();
            }
        }
    });
}

function search() {
    var peopleOrgIdFilter = $('#peopleOrgIdFilter').val();
    var nameFilter = $('#nameFilter').val();
    var receiptNumberFilter = $('#receiptNumberFilter').val();

    if (peopleOrgIdFilter || nameFilter || receiptNumberFilter) {
        showProcessing();

        var rows = $('.cashReceiptRow');

        if (peopleOrgIdFilter) {
            if ($('input[type=radio]:checked').val() === 'RadioPerson') {
                // Search by people Id
                peopleOrgIdFilter = 'P' + peopleOrgIdFilter;
            }
            else {
                // Search by organization id
                peopleOrgIdFilter = 'O' + peopleOrgIdFilter;
            }
        }

        rows.each(function () {
            var matchPeopleOrgId = true;
            var matchName = true;
            var matchReceiptNumber = true;

            var peopleOrgId = $(this).children('td:nth-child(1)').data('peopleOrgId');
            var name = $(this).children('td:nth-child(2)').data('name');
            var receiptNumber = $(this).children('td:nth-child(3)').data('receiptNumber');

            if (peopleOrgIdFilter) {
                peopleOrgId = peopleOrgId.toUpperCase();
                peopleOrgIdFilter = peopleOrgIdFilter.toUpperCase();

                if (!peopleOrgId.startsWith(peopleOrgIdFilter)) {
                    matchPeopleOrgId = false;
                }
            }

            if (nameFilter) {
                name = name.toUpperCase();
                nameFilter = nameFilter.toUpperCase();

                if (!name.startsWith(nameFilter)) {
                    matchName = false;
                }
            }

            if (receiptNumberFilter) {
                receiptNumber = receiptNumber.toString().toUpperCase();
                receiptNumberFilter = receiptNumberFilter.toString().toUpperCase();

                if (!receiptNumber.startsWith(receiptNumberFilter)) {
                    matchReceiptNumber = false;
                }
            }

            if (matchPeopleOrgId && matchName && matchReceiptNumber) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
        hideProcessing();
    }
    else {
        $('.cashReceiptRow').show();
    }
}

function onCloseModal() {
    $('#receiptDetailsModalOverlay').hide();
    $('#receiptDetailsModal').hide();
}

function loadNewInvoice() {
    var SubsInvoiceExpeditionId = Number($('#hdnSubsInvoiceExpeditionId').val());
    var SusbsSerialNumber = $('#hdnSusbsSerialNumber').val();
    var CFDIRelated = $('#hdnCFDIRelated').val();
    var CFDIRelatedId = Number($('#hdnCFDIRelatedId').val());
    var CancelReasonName = Number($('#hdnCancelReasonName').val());
    var ReceiptNumber = Number($('#hdnReceiptNumber').val());
    var PeopleOrgId = $('#hdnPeopleOrgId').val();

    var IssuerTaxPayerId = $("#IssTaxPayerId");
    var IssuerExpAdd = $('#IssIssuingAddress');
    var IssPlaceIssue = $('#IssPlaceIssue');
    var IssTaxRegimenC = $('#IssTaxRegimen');
    var IssuerTaxRegimenCode = "";
    var IssuerTaxRegimenDesc = "";
    var fIdentityNumber = "";
    var fResidency = "";
    var fResidencyDesc = "";

    var TaxPayers = [];
    var IssTaxPayers = [];
    var IssIssuingAddress;

    var IdIssTaxPayer = 0;

    $.fn.clearValidation = function () { var v = $(this).validate(); $('[name]', this).each(function () { v.successList.push(this); v.showErrors(); }); v.resetForm(); v.reset(); };

    // #region Receiver
    $("#TaxPayerId").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetReceivers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === 'XEXX010101000')
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber }
                        else
                            return { label: item.TaxPayerId, value: item.TaxPayerId }
                    }))

                }
            })
        }
    });

    function onTaxPayerIdFocusOut() {
        var IdTaxpayer = $("#TaxPayerId").val();
        var RecTaxPayId = $("#TaxPayerId");
        var response;
        var FiscalIdentityNumber;
        var TaxPayerIdVal;

        if (IdTaxpayer.indexOf("-") >= 0) {
            var res = IdTaxpayer.split("-");
            TaxPayerIdVal = res[0];
            FiscalIdentityNumber = res[1];
            response = $.grep(TaxPayers, function (element) { return element.FiscalIdentityNumber == FiscalIdentityNumber })

            $("#NameCorpName").val(response[0].CorporateName);
            $("#FiscalAddress").val(response[0].FiscalResidencyDesc);
            $("#FiscalAddressCode").val(response[0].FiscalResidency);
            fResidency = response[0].FiscalResidency;
            fResidencyDesc = response[0].FiscalResidencyDesc;
            $("#FiscalIdNum").val(response[0].FiscalIdentityNumber);
            fIdentityNumber = response[0].FiscalIdentityNumber;
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').hide();
            $('#RecTaxPayerGroup').removeClass('esg-has-error');
            $("#TaxPayerId").val(TaxPayerIdVal);
        }
        else {
            response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == IdTaxpayer; })
        }

        if (IdTaxpayer.length > 0 || response.length > 0) {
            $("#NameCorpName").val(response[0].CorporateName);
            $("#FiscalAddress").val(response[0].FiscalResidencyDesc);
            $("#FiscalAddressCode").val(response[0].FiscalResidency);
            $("#FiscalIdNum").val(response[0].FiscalIdentityNumber);
            $("#PostalCode").val(response[0].PostalCode);
            $("#RecTaxRegimen").val(response[0].TaxRegimenCode + " - " + response[0].TaxRegimenDesc);
            fIdentityNumber = response[0].FiscalIdentityNumber;
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').hide();
            $('#RecTaxPayerGroup').removeClass('esg-has-error');
            $("#RecTaxPayerDivLookup").show();

            // Validate if taxpayer id is company or person
            $.ajax({
                url: urlGetCFDIReceivers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { length: IdTaxpayer.length },
                success: function (res) {
                    if ($('#CFDI option').length == 1) {
                        $.each(res, function (i, val) {
                            $("#CFDI").append($('<option></option>').val(val.Code).html(val.Description));
                        });
                    }

                    if ($("#PreferredEmail").val()) {
                        $("#PreferredEmail").parent().removeClass('has-error');
                        $("#EmailGroup").removeClass('esg-has-error');
                        $("#EmailIcon").hide();
                    }

                    if ($("#PostalCode").val()) {
                        $("#PostalCode").parent().removeClass('has-error');
                        $("#PostalCodeGroup").removeClass('esg-has-error');
                        $("#PostalCodeIcon").hide();
                    }

                    if ($("#CFDI").val()) {
                        $("#CFDI").parent().removeClass('has-error');
                        $("#CFDIGroup").removeClass('esg-has-error');
                        $("#CFDIIcon").hide();
                    }

                    if ($("#RecTaxRegimen").val()) {
                        $("#RecTaxRegimen").parent().removeClass('has-error');
                        $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                        $("#RecTaxRegimenIcon").hide();
                    }

                    $("#formCreate").clearValidation();
                }
            });

        }
        else {
            $("#NameCorpName").val("");
            $("#FiscalAddress").val("");
            $("#FiscalIdNum").val("");
            RecTaxPayId.parent().addClass('has-error');
            $('#RecTaxPayerIcon').show();
            $("#RecTaxPayerDivLookup").hide();
            $('#RecTaxPayerGroup').addClass('esg-has-error');
        }
    }

    $("#TaxPayerId").focusout(onTaxPayerIdFocusOut);


    $("#PreferredEmail").change(function () {
        var Email = $("#PreferredEmail");
        if (!Email.val()) {
            Email.parent().addClass('has-error');
            $("#EmailGroup").addClass('esg-has-error');
            $("#EmailIcon").show();
        }
        else {
            $("#EmailGroup").removeClass('esg-has-error');
            $("#EmailIcon").hide();
            Email.parent().removeClass('has-error');
        }
    });

    $("#CFDI").change(function () {
        var cfdi = $("#CFDI");

        if (!cfdi.val()) {
            $("#CFDI").parent().addClass('has-error');
            $("#CFDIGroup").addClass('esg-has-error');
            $("#CFDIIcon").show();
        }
        else {
            $("#CFDI").parent().removeClass('has-error');
            $("#CFDIGroup").removeClass('esg-has-error');
            $("#CFDIIcon").hide();
        }
    });
    // #endregion Receiver

    // #region Default Issuer
    function LoadIssuerSetUp() {
        $.ajax({
            url: urlSelectIssuerSetUp,
            type: "GET",
            cache: false,
            success: function (data) {
                if (data.IssInvoiceOrganizationId != 0) {
                    $("#IssSerial").empty();
                    $("#IssIssuingAddress").empty();
                    $("#IssTaxPayerId").val(data.IssInvoiceTaxpayerId);
                    $("#IssIssuingAddress").append($('<option></option>').val(data.IssInvoiceExpeditionId).html(data.IssIssuingAddress));
                    $("#IssSerial").append($('<option></option>').val(data.IssSerialNumber).html(data.IssSerialNumber));
                    $("#PaymentCondition").val(data.IssPaymentConditions);
                    $("#IssNameCorpName").val(data.IssCorporateName);

                    var InvoiceOrganizationId = data.IssInvoiceOrganizationId;
                    var InvoiceExpId = data.IssInvoiceExpeditionId;
                    var SerialNum = $("#IssSerial").val().trim();

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

                                $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                                $("#IssTaxRegimenIcon").hide();
                                $("#IssTaxRegimenGroup").parent().removeClass('esg-has-error');
                                IssTaxRegimenC.parent().removeClass('has-error');
                                $("#IssTaxPayerDivLookup").show();
                            }
                            else {
                                if ($("#IssTaxPayerId").val() === "") {
                                    $("#IssTaxRegimenIcon").show();
                                    $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
                                    IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                                    $("#IssTaxPayerDivLookup").hide();
                                    $("#IssTaxRegimen").val('');
                                }
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
                                    $("#IssPlaceIssueIcon").hide();
                                    $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-error');
                                }
                                else {
                                    $("#IssPlaceIssue").val("");
                                    $("#IssPlaceIssueIcon").show();
                                    $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-success');
                                }

                                if (IssIssuingAddress[0].IssByExpedition === 0) {
                                    InvoiceExpId = null;
                                }

                                // Get SerialNumber
                                $.ajax({
                                    url: urlGetSerialNumber,
                                    type: "GET",
                                    cache: false,
                                    dataType: "json",
                                    data: { id: InvoiceOrganizationId, expId: InvoiceExpId },
                                    success: function (response) {
                                        $("#IssSerial").empty();
                                        $("#lblWarnSerial").hide();

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
                                                $("#lblWarnSerial").show();
                                            }
                                            else {
                                                $("#lblWarnSerial").hide();
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
            }
        });
    }
    LoadIssuerSetUp();

    // #endregion Default Issuer

    // #region Issuer
    $("#IssTaxPayerId").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: urlGetIssuers,
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

    function onIssTaxPayerIdFocusOut() {
        IdIssTaxPayer = $("#IssTaxPayerId").val();
        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];

        if (!response == "") {
            $("#IssNameCorpName").val("");
            $("#IssTaxRegimen").empty();
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").empty();
            $("#IssPlaceIssue").val("");

            IssuerTaxPayerId.parent().removeClass('has-error');
            $('#IssTaxPayerIcon').hide();
            $('#IssTaxPayerGroup').parent().removeClass('esg-has-error');
            $("#IssTaxPayerDivLookup").show();
            $("#IssNameCorpName").val(response.IssCorporateName);
            $("#IssIssuingAddress").parent().removeClass('esg-has-error');

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

                        $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                        $("#IssTaxRegimenIcon").hide();
                        $("#IssTaxRegimenGroup").parent().removeClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-error');
                        $("#IssTaxPayerDivLookup").show();
                    }
                    else {
                        $("#IssTaxRegimenIcon").show();
                        $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                        $("#IssTaxPayerDivLookup").hide();
                        $("#IssTaxRegimen").val('');
                    }

                    if (response.IssIssuingAdd != null) {

                        var InvoiceExpeditionId;

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
                            InvoiceExpeditionId = issPlaceIssueFound.IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound != null && issPlaceIssueFound.IssPlaceIssue != null) {
                            $("#IssIssuingAddress").val(issPlaceIssueFound.IssInoviceExpeditionId);
                            $("#IssPlaceIssue").val(issPlaceIssueFound.IssPlaceIssue);
                            $("#IssPlaceIssueIcon").hide();
                            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-error');
                        }
                        else {
                            $("#IssPlaceIssue").val("");
                            $("#IssPlaceIssueIcon").show();
                            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-success');
                        }

                        //GET SerialNumber
                        $.ajax({
                            url: urlGetSerialNumber,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: issPlaceIssueFound.IssInvoiceOrganizationId, expId: InvoiceExpeditionId },
                            success: function (response) {

                                $("#lblWarnSerial").hide();

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
                                    $("#lblWarnSerial").show();
                                }
                                else {
                                    $("#lblWarnSerial").hide();
                                }
                            }
                        });

                        $("#IssIssuingAddressIcon").hide();
                        $("#IssIssuingAddressGroup").parent().removeClass('esg-has-error');
                        IssuerExpAdd.parent().removeClass('has-error');

                    }
                    else {
                        $("#IssIssuingAddress").empty();
                        $("#IssIssuingAddressIcon").show();
                        $("#IssIssuingAddressGroup").parent().addClass('esg-has-error');
                        $("#IssIssuingAddress").parent().addClass('esg-has-error');
                        IssuerExpAdd.parent().addClass('has-error');
                        $("#IssPlaceIssue").val("");
                        $("#IssPlaceIssueIcon").show();
                        $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                        IssPlaceIssue.parent().addClass('has-error');
                    }
                }
            });
        }
        else if (IdIssTaxPayer == "") {
            IssuerTaxPayerId.parent().addClass('has-error');
            $('#IssTaxPayerIcon').show();
            $('#IssTaxPayerGroup').parent().addClass('esg-has-error');
            $("#IssTaxRegimenIcon").show();
            $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
            IssTaxRegimenC.parent().addClass('has-error');
            $("#IssPlaceIssueIcon").show();
            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
            IssPlaceIssue.parent().addClass('has-error');
            $("#IssIssuingAddressIcon").show();
            $("#IssIssuingAddressGroup").parent().addClass('esg-has-error');
            IssuerExpAdd.parent().addClass('has-error');
            $("#IssTaxRegimen").val("");
            $("#IssTaxPayerDivLookup").hide();
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").val("");
            $("#IssPlaceIssue").val("");
        }
    }

    $("#IssTaxPayerId").focusout(onIssTaxPayerIdFocusOut);

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

        // Get SerialNumber
        $.ajax({
            url: urlGetSerialNumber,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: PlaceIssue[0].IssInvoiceOrganizationId, expId: InvoiceExpeditionId },
            success: function (response) {
                if (response.IssSerial != null) {
                    $("#IssSerial").empty();
                    $("#lblWarnSerial").hide();

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
                        $("#lblWarnSerial").show();
                    }
                    else {
                        $("#lblWarnSerial").hide();
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
            $("#IssPlaceIssueIcon").hide();
            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-error');
        }
        else {

            $("#IssPlaceIssueIcon").show();
            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-success').addClass('has-error');
        }
    });

    $("#IssSerial").change(function () {
        var selected = $(this).find('option:selected');
        var optionVal = selected.attr('id');

        if (optionVal == "0") {
            $("#lblWarnSerial").hide();
        }
        else {
            $("#lblWarnSerial").show();
        }

    });
    // #endregion Issuer

    // #region Payment
    $("#PaymentCondition").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }
    });

    $("#IssComments").keypress(function (e) {

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
    // #endregion Charge

    // #region Process
    var createForm = $('#formCreate');
    $('#btnProcessC').click(function () {
        var SerialError = $("#lblWarnSerial").css('display');

        if (SerialError == 'none') {
            $('.errorMessageSerialError').hide();
            var CFDISelected = $("#CFDI option:selected").text();
            var CFDICode = $("#CFDI option:selected").val();
            var CFDIDesc = CFDISelected.substring(CFDICode.length + 1, CFDISelected.length);

            var PaymentTypeSelected = $("#PaymentType option:selected").text();
            var PaymentType = PaymentTypeSelected.split('-');
            var PaymentTypeCode = PaymentType[0].trim();
            var PaymentTypeDesc = PaymentType[1].trim();
            var PaymentMethodSelected = $("#PaymentMethod").val();
            var PaymentMethod = PaymentMethodSelected.split('-');
            var PaymentMethodCode = PaymentMethod[0].trim();
            var PaymentMethodDesc = PaymentMethod[1].trim();

            var SerialNumber = $("#IssSerial option:selected").text();

            $('.errorMessageRequiredDiv').hide();
            $('.errorMessageDiv').hide();

            // Remove the form validation
            createForm.removeData("validator").removeData("unobtrusiveValidation");

            //Add the form validation
            $.validator.unobtrusive.parse(createForm);

            if (createForm.valid()) {
                $('.errorMessageRequiredDiv').hide();
                /*Read Details of Fiscal Record*/
                var $table = $("#records_table tbody");
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
                                frd.ChargeCreditCode = $("#ccCode_" + rowId).val();
                                frd.ProductServiceKey = $("#ccPrdSrv_" + rowId).text();
                            }
                            if (colIndex === 4) {
                                frd.UnitDescription = $("#ccUnitDesc_" + rowId).val();
                            }
                            if (colIndex === 5) {
                                frd.Description = $("#ccDesc_" + rowId).val();
                            }
                        });
                        cols.push(frd);
                    });
                });
                showProcessing();
                $('#btnProcessC').hide();
                fIdentityNumber = $("#FiscalIdNum").val();
                fResidencyDesc = $("#FiscalAddress").val();
                if (fResidencyDesc == '') {
                    fResidency = '';
                }
                else {
                    fResidency = $("#FiscalAddressCode").val();
                }
                var ParamInsInvoiceHeaderList = {
                    CancelReasonKey: "",
                    CancelReasonName: CancelReasonName,
                    CFDIRelated: CFDIRelated,
                    CFDIRelatedId: CFDIRelatedId,
                    CFDIRelationTypeKey: "",
                    CFDIRelationTypeName: null,
                    CFDIUsageCode: CFDICode,
                    CFDIUsageDesc: CFDIDesc,
                    CityOfIssue: $("#IssPlaceIssue").val(),
                    Comments: $("#IssComments").val(),
                    Currency: $("#IssCurrency").val(),
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
                    peopleOrgCodeId: PeopleOrgId,
                    ReceiptNumber: ReceiptNumber,
                    ReceiverEmail: $("#PreferredEmail").val(),
                    ReceiverTaxpayerId: $("#TaxPayerId").val(),
                    SerialNumber: SerialNumber,
                    StartDate: null,
                    Subtotal: $("#SubTotal").val(),
                    TaxRegimen: IssuerTaxRegimenCode,
                    TaxRegimenDesc: IssuerTaxRegimenDesc,
                    Total: $("#Total").val(),
                    TotalTransferTaxes: $("#TotalTT").val()
                };
                $.ajax({
                    url: urlCreateFiscalRecord,
                    dataType: "json",
                    type: "POST",
                    cache: false,
                    data: ParamInsInvoiceHeaderList,
                    success: function (response) {
                        if (response.id <= 0) {
                            hideProcessing();
                            $('#btnProcessC').show();
                            $('.errorMessageDiv').show();
                            $(".errorMessageResult").html(response.message);
                        }
                        else {
                            hideProcessing();
                            $('.successMessageDiv').show();
                            $('.errorMessageDiv').hide();
                            $(".successMessageResult").html(response.message);

                            moveToStpNewInvoice();
                        }
                    }
                });
            }
            else {
                hideProcessing();
                $('#btnProcessC').show();
                $('.errorMessageRequiredDiv').show();
                $('.successMessageDiv').hide();

                if (!$("#RecTaxRegimen").val()) {
                    $("#RecTaxRegimen").parent().addClass('has-error');
                    $("#RecTaxRegimenGroup").addClass('esg-has-error');
                    $("#RecTaxRegimenIcon").show();
                }

                if (!$("#PreferredEmail").val()) {
                    $("#PreferredEmail").parent().addClass('has-error');
                    $("#EmailGroup").addClass('esg-has-error');
                    $("#EmailIcon").show();
                }

                if (!$("#TaxPayerId").val()) {
                    $("#TaxPayerId").parent().addClass('has-error');
                    $("#RecTaxPayerGroup").addClass('esg-has-error');
                    $("#RecTaxPayerIcon").show();
                    $("#RecTaxPayerDivLookup").hide();
                }

                if (!$("#PostalCode").val()) {
                    $("#PostalCode").parent().addClass('has-error');
                    $("#PostalCodeGroup").addClass('esg-has-error');
                    $("#PostalCodeIcon").show();
                }

                if (!$("#CFDI").val()) {
                    $("#CFDI").parent().addClass('has-error');
                    $("#CFDIGroup").addClass('esg-has-error');
                    $("#CFDIIcon").show();
                }
            }
        }
        else {
            $('.errorMessageSerialError').show();
            $('.errorMessageRequiredDiv').hide();
            $('.errorMessageDiv').hide();
        }
    });
    // #endregion Process

    $("#btnBackStp2State1").click(function () {
        moveToStpNewInvoice();
    });
}
// #endregion NewInvoice

// #region NewGlobalInvoice
function loadNewGlobalInvoice() {
    var SubsInvoiceExpeditionId = Number($('#hdnSubsInvoiceExpeditionId').val());
    var SusbsSerialNumber = $('#hdnSusbsSerialNumber').val();
    var CFDIRelated = $('#hdnCFDIRelated').val();
    var CFDIRelatedId = Number($('#hdnCFDIRelatedId').val());
    var CancelReasonName = Number($('#hdnCancelReasonName').val());

    var IssuerTaxPayerId = $("#IssTaxPayerId");
    var IssuerExpAdd = $('#IssIssuingAddress');
    var IssPlaceIssue = $('#IssPlaceIssue');
    var IssTaxRegimenC = $('#IssTaxRegimen');
    var IssuerTaxRegimenCode = '';
    var IssuerTaxRegimenDesc = '';

    var IssTaxPayers = [];
    var IssIssuingAddress;

    var IdIssTaxPayer = 0;

    $('#Year option:eq(1)').prop('selected', true);

    $("#btnBackNewInvoice").click(function () {
        $("#stpNewGlobalInvoiceComplete").hide();
        moveToStpNewInvoice();
    });

    $("#btnFinish").click(function () {
        window.location.href = urlViewAll;
    });

    $("#PreferredEmail").change(function () {
        var Email = $("#PreferredEmail");
        if (!Email.val()) {
            Email.parent().addClass('has-error');
            $("#EmailGroup").addClass('esg-has-error');
            $("#EmailIcon").show();
        }
        else {

            $("#EmailGroup").removeClass('esg-has-error');
            $("#EmailIcon").hide();
            Email.parent().removeClass('has-error');
        }
    });

    // #region DefaultIssuer
    function LoadIssuerSetUp() {
        $.ajax({
            url: urlSelectIssuerSetUp,
            type: "GET",
            cache: false,
            success: function (data) {

                if (data.IssInvoiceOrganizationId !== 0) {
                    //Existe registro en la tabla se muestra boton de Actualizar
                    $("#IssSerial").empty();
                    $("#IssIssuingAddress").empty();
                    $("#IssTaxPayerId").val(data.IssInvoiceTaxpayerId);
                    $("#IssIssuingAddress").append($('<option></option>').val(data.IssInvoiceExpeditionId).html(data.IssIssuingAddress));
                    $("#IssSerial").append($('<option></option>').val(data.IssSerialNumber).html(data.IssSerialNumber));
                    $("#IssNameCorpName").val(data.IssCorporateName);

                    var InvoiceOrganizationId = data.IssInvoiceOrganizationId;
                    var InvoiceExpId = data.IssInvoiceExpeditionId;
                    var SerialNum = $("#IssSerial").val().trim();

                    $.ajax({
                        url: urlGetTaxRegimen,
                        type: "GET",
                        cache: false,
                        dataType: "json",
                        data: { id: InvoiceOrganizationId },
                        success: function (response) {
                            if (response.IssTaxRegimen != null) {

                                var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element; });
                                IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                                IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                                $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                                $("#IssTaxRegimenIcon").hide();
                                $("#IssTaxRegimenGroup").removeClass('esg-has-error');
                                IssTaxRegimenC.removeClass('has-error');
                                $("#IssTaxPayerDivLookup").show();
                            }
                            else {
                                $("#IssTaxRegimenIcon").show();
                                $("#IssTaxRegimenGroup").addClass('esg-has-error');
                                IssTaxRegimenC.removeClass('has-success').addClass('has-error');
                                $("#IssTaxPayerDivLookup").hide();
                                $("#IssTaxRegimen").val('');
                            }

                            if (response.IssIssuingAdd != null) {

                                IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element; });

                                $.each(IssIssuingAddress, function (i, val) {

                                    if (InvoiceExpId != val.IssInoviceExpeditionId) {
                                        $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                    }
                                });

                                var IssuingAddress = $.grep(response.IssIssuingAdd, function (elem) { return elem.IssInoviceExpeditionId == InvoiceExpId; });

                                if (IssuingAddress[0].IssPlaceIssue != null) {
                                    $("#IssPlaceIssue").val(IssuingAddress[0].IssPlaceIssue);
                                    $("#IssPlaceIssueIcon").hide();
                                    $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-error');

                                    $("#RecTaxAddressGroup").parent().removeClass('esg-has-error');
                                    $("#RecTaxAddressIcon").hide();
                                }
                                else {
                                    $("#IssPlaceIssue").val("");
                                    $("#IssPlaceIssueIcon").show();
                                    $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-success');

                                    $("#RecTaxAddressIcon").show();
                                    $("#RecTaxAddressGroup").parent().addClass('esg-has-error');
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
                                        $("#lblWarnSerial").hide();

                                        if (response.IssSerial != null) {
                                            var IssSerial = $.grep(response.IssSerial, function (element) { return element; });

                                            var SerialPrefer = $.grep(IssSerial, function (e) { return e.IssSerialNumber == SerialNum; });

                                            if (SerialPrefer[0].IssLastFolio == '2147483647') {

                                                $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 1));

                                            }
                                            else {
                                                $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 0));

                                            }

                                            $.each(IssSerial, function (i, val) {
                                                if ($("#IssSerial").val() != val.IssInvoiceReceipt) {
                                                    if (val.IssLastFolio == '2147483647') {
                                                        $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 1));

                                                    }
                                                    else {
                                                        $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 0));
                                                    }
                                                }
                                            });

                                            var selected = $("#IssSerial").find('option:selected');
                                            var optionVal = selected.attr('id');

                                            if (optionVal == "1") {
                                                $("#lblWarnSerial").show();
                                            }
                                            else {
                                                $("#lblWarnSerial").hide();
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

            }
        });
    }

    LoadIssuerSetUp();
    // #endregion DefaultIssuer

    // #region Issuer
    $("#IssTaxPayerId").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetIssuers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    IssTaxPayers = data;
                    response($.map(data, function (item) {
                        return { value: item.IssTaxpayerId, id: item.IssTaxpayerId };
                    }));
                }
            });
        }
    });

    $("#IssTaxPayerId").focusout(onIssTaxPayerIdFocusOut);

    function onIssTaxPayerIdFocusOut() {
        IdIssTaxPayer = $("#IssTaxPayerId").val();

        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];

        if (!response == "") {

            $("#IssNameCorpName").val("");
            $("#IssTaxRegimen").empty();
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").empty();
            $("#IssPlaceIssue").val("");

            IssuerTaxPayerId.parent().removeClass('has-error');
            $('#IssTaxPayerIcon').hide();
            $('#IssTaxPayerGroup').parent().removeClass('esg-has-error');
            $("#IssTaxPayerDivLookup").show();
            $("#IssNameCorpName").val(response.IssCorporateName);
            $("#IssIssuingAddress").parent().removeClass('esg-has-error');

            // Get tax regimen
            $.ajax({
                url: urlGetTaxRegimen,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: response.IssInvoiceOrganizationId },
                success: function (response) {

                    if (response.IssTaxRegimen != null) {

                        var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element; });
                        IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                        IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                        $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                        $("#IssTaxRegimenIcon").hide();
                        $("#IssTaxRegimenGroup").parent().removeClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-error');
                    }
                    else {
                        $("#IssTaxRegimenIcon").show();
                        $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                        $("#IssTaxPayerDivLookup").hide();
                        $("#IssTaxRegimen").val('');
                    }

                    if (response.IssIssuingAdd != null) {

                        var InoviceExpeditionId;

                        IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element; });

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
                            InoviceExpeditionId = null;
                        }
                        else {
                            InoviceExpeditionId = issPlaceIssueFound.IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound.IssPlaceIssue != null) {
                            $("#IssIssuingAddress").val(issPlaceIssueFound.IssInoviceExpeditionId);
                            $("#IssPlaceIssue").val(issPlaceIssueFound.IssPlaceIssue);
                            $("#IssPlaceIssueIcon").hide();
                            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-error');
                        }
                        else {
                            $("#IssPlaceIssue").val("");
                            $("#IssPlaceIssueIcon").show();
                            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-success');
                        }

                        // Get serial number
                        $.ajax({
                            url: urlGetSerialNumber,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                            success: function (response) {

                                $("#lblWarnSerial").hide();

                                if (response.IssSerial != null) {
                                    var IssSerial = $.grep(response.IssSerial, function (element) { return element; });

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
                                    $("#lblWarnSerial").show();
                                }
                                else {
                                    $("#lblWarnSerial").hide();
                                }

                            }
                        });

                        $("#IssIssuingAddressIcon").hide();
                        $("#IssIssuingAddressGroup").parent().removeClass('esg-has-error');
                        IssuerExpAdd.parent().removeClass('has-error');

                    }
                    else {
                        $("#IssIssuingAddress").empty();
                        $("#IssIssuingAddressIcon").show();
                        $("#IssIssuingAddressGroup").parent().addClass('esg-has-error');
                        $("#IssIssuingAddress").parent().addClass('esg-has-error');
                        IssuerExpAdd.parent().addClass('has-error');
                        $("#IssPlaceIssue").val("");
                        $("#IssPlaceIssueIcon").show();
                        $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                        IssPlaceIssue.parent().addClass('has-error');
                    }
                }
            });

        }
        else if (IdIssTaxPayer == "") {
            IssuerTaxPayerId.parent().addClass('has-error');
            $('#IssTaxPayerIcon').show();
            $('#IssTaxPayerGroup').parent().addClass('esg-has-error');
            $("#IssTaxRegimenIcon").show();
            $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
            IssTaxRegimenC.parent().addClass('has-error');
            $("#IssPlaceIssueIcon").show();
            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
            IssPlaceIssue.parent().addClass('has-error');
            $("#IssIssuingAddressIcon").show();
            $("#IssIssuingAddressGroup").parent().addClass('esg-has-error');
            IssuerExpAdd.parent().addClass('has-error');
            $("#IssTaxRegimen").val("");
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").val("");
            $("#IssPlaceIssue").val("");
            $("#IssTaxPayerDivLookup").hide();
        }
    }

    $('#IssIssuingAddress').change(function () {

        var InoviceExpeditionId = 0;
        var val = $('#IssIssuingAddress option:selected').text();
        var PlaceIssue = $.grep(IssIssuingAddress, function (element) { return element.IssIssuingAddress == val; });
        var IssByExpedition = PlaceIssue[0].IssByExpedition;

        if (IssByExpedition === 0) {
            InoviceExpeditionId = null;
        }
        else {
            InoviceExpeditionId = PlaceIssue[0].IssInoviceExpeditionId;
        }

        // Get serial number
        $.ajax({
            url: urlGetSerialNumber,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: PlaceIssue[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
            success: function (response) {

                if (response.IssSerial != null) {
                    $("#IssSerial").empty();
                    $("#lblWarnSerial").hide();

                    var IssSerial = $.grep(response.IssSerial, function (element) { return element; });

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
                        $("#lblWarnSerial").show();
                    }
                    else {
                        $("#lblWarnSerial").hide();
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

            $("#IssPlaceIssueIcon").hide();
            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-error');
        }
        else {

            $("#IssPlaceIssueIcon").show();
            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-success').addClass('has-error');

        }

    });

    $("#IssSerial").change(function () {

        var selected = $(this).find('option:selected');
        var optionVal = selected.attr('id');

        if (optionVal == "0") {
            $("#lblWarnSerial").hide();
        }
        else {
            $("#lblWarnSerial").show();
        }

    });

    // #endregion Issuer


    // #region Payment

    $("#PaymentCondition").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }
    });

    $("#IssComments").keypress(function (e) {

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
    // #endregion Charge

    // #region Fiscal Records
    $("#Frequency").change(function () {
        var frequencyValue = $("#Frequency option:selected").val();
        if (frequencyValue) {
            $("#Frequency").removeClass('esg-has-error');
            $("#FrequencyGroup").removeClass('esg-has-error');
            $("#FrequencyIcon").hide();
            $("#FrequencyRequired").hide();
        }
    });

    $("#Month").change(function () {
        var monthValue = $("#Month option:selected").val();
        if (monthValue) {
            $("#Month").removeClass('esg-has-error');
            $("#MonthGroup").removeClass('esg-has-error');
            $("#MonthIcon").hide();
            $("#MonthRequired").hide();

        }
    });

    $("#Year").change(function () {
        var monthValue = $("#Year option:selected").val();
        if (monthValue) {
            $("#Year").removeClass('esg-has-error');
            $("#YearGroup").removeClass('esg-has-error');
            $("#YearIcon").hide();
            $("#YearRequired").hide();
        }
    });
    // #endregion Fiscal Records

    // #region Create
    var createForm = $('#formCreate');

    $("#btnProcessGlobal").click(function () {
        var SerialError = $("#lblWarnSerial").css('display');

        if (SerialError == 'none') {
            $('.errorMessageSerialError').hide();

            var PaymentTypeSelected = $("#PaymentType option:selected").text();
            var PaymentType = PaymentTypeSelected.split('-');
            var PaymentTypeCode = PaymentType[0].trim();
            var PaymentTypeDesc = PaymentType[1].trim();
            var PaymentMethodSelected = $("#PaymentMethod").val();
            var PaymentMethod = PaymentMethodSelected.split('-');
            var PaymentMethodCode = PaymentMethod[0].trim();
            var PaymentMethodDesc = PaymentMethod[1].trim();

            var FrequencySelected = $("#Frequency option:selected").text();
            var FrequencyCode = $("#Frequency option:selected").val();
            var FrequencyDesc = FrequencySelected;

            var MonthSelected = $("#Month option:selected").text();
            var MonthCode = $("#Month option:selected").val();
            var MonthDesc = MonthSelected;

            var YearCode = $("#Year option:selected").val();

            var SerialNumber = $("#IssSerial option:selected").text();

            $('.errorMessageDiv').hide();
            $('.errorMessageRequiredDiv').hide();

            createForm.validate();

            if (createForm.valid() && formIsValid()) {

                // Read details of fiscal record
                var $table = $("#records_table tbody");
                var cols = [];
                $table.find('tr').each(function (rowIndex, r) {
                    var frd = new Object();
                    var frDetails = [];
                    $(this).find(":input[type=hidden][name='CashRe.ChargeCreditCode']").each(function (index, v) {
                        frDetails.push(v.value);
                    });
                    jQuery.each(frDetails, function (i, val) {
                        var rowId = val;
                        $table.find('tr').find('td').each(function (colIndex, c) {
                            if (colIndex === 0) {
                                frd.chargeCreditCode = $("#ccCode_" + rowId).val();
                                frd.ProductServiceCode = $("#ccPrdSrv_" + rowId).text();
                            }
                        });
                        cols.push(frd);
                    });
                });

                $("#btnProcessGlobal").hide();
                showProcessing();

                var ParamInsInvoiceHeaderList = {
                    CancelReasonKey: "",
                    CancelReasonName: CancelReasonName,
                    CFDIRelated: CFDIRelated,
                    CFDIRelatedId: CFDIRelatedId,
                    CFDIRelationTypeKey: "",
                    CFDIRelationTypeName: null,
                    CFDIUsageCode: 'S01',
                    CFDIUsageDesc: 'Sin efectos fiscales',
                    CityOfIssue: $("#IssPlaceIssue").val(),
                    Comments: $("#IssComments").val(),
                    Currency: $("#IssCurrency").val(),
                    Detail: cols,
                    EndDate: endDate,
                    ExchangeRate: 0.00,
                    FiscalIdentityNumber: '',
                    FiscalResidency: '',
                    FiscalResidencyDesc: '',
                    FrequencyCode: FrequencyCode,
                    FrequencyDesc: FrequencyDesc,
                    InvoiceExpeditionId: $("#IssIssuingAddress").val(),
                    InvoiceHeaderId: invoiceHeaderId,
                    IssuerTaxPayerId: $("#IssTaxPayerId").val(),
                    IsCreationForReason04: true,
                    MonthCode: MonthCode,
                    MonthDesc: MonthDesc,
                    PaymentAccountNumber: "",
                    PaymentCondition: '',
                    PaymentMethod: PaymentMethodCode,
                    PaymentMethodDesc: PaymentMethodDesc,
                    PaymentType: PaymentTypeCode,
                    PaymentTypeDesc: PaymentTypeDesc,
                    PeopleOrgCodeId: '',
                    ReceiptNumber: 0,
                    ReceiverEmail: $("#PreferredEmail").val(),
                    ReceiverTaxpayerId: $("#RecTaxPayerId").val(),
                    RecTaxRegimen: '616',
                    RecTaxRegimenDesc: 'Sin obligaciones fiscales',
                    SerialNumber: SerialNumber,
                    StartDate: startDate,
                    Subtotal: $("#SubTotal").val(),
                    TaxRegimen: IssuerTaxRegimenCode,
                    TaxRegimenDesc: IssuerTaxRegimenDesc,
                    Total: $("#Total").val(),
                    TotalTransferTaxes: $("#TotalTT").val(),
                    Year: YearCode
                };

                $.ajax({
                    url: urlProcessGlobal,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: ParamInsInvoiceHeaderList,
                    success: function (response) {
                        if (response.id == -1) {
                            hideProcessing();
                            $("#btnProcessGlobal").show();
                            $("#PreferredEmail").addClass('has-error');
                            $("#EmailGroup").addClass('esg-has-error');
                            $("#EmailIcon").show();
                            $('.errorMessageRequiredDiv').show();
                        }
                        else if (response.id <= -2 || response.id == 0) {
                            hideProcessing();
                            $("#btnProcessGlobal").show();
                            $('.errorMessageDiv').show();
                            $(".errorMessageResult").html(response.message);
                        }
                        else {
                            hideProcessing();
                            $('.successMessageDiv').show();
                            $('.errorMessageDiv').hide();
                            $(".successMessageResult").html(response.message);
                            window.location.href = urlViewAll;
                        }
                    }
                });

            }
            else {
                hideProcessing();
                $('.errorMessageRequiredDiv').show();
                $('.successMessageDiv').hide();
                $("#btnProcessGlobal").show();

                // UI validations for frequency

                var Email = $("#PreferredEmail");
                if (!Email.val()) {
                    Email.parent().addClass('has-error');
                    $("#EmailGroup").addClass('esg-has-error');
                    $("#EmailIcon").show();
                }
                else {
                    $("#EmailGroup").removeClass('esg-has-error');
                    $("#EmailIcon").hide();
                    Email.parent().removeClass('has-error');
                }

                // UI validations for frequency
                if (FrequencyCode) {
                    $("#Frequency").removeClass('has-error');
                    $("#FrequencyIcon").hide();
                    $("#FrequencyRequired").hide();
                    $("#FrequencyGroup").removeClass('esg-has-error');
                }
                else {
                    $("#Frequency").val("");
                    $("#FrequencyIcon").show();
                    $("#FrequencyRequired").show();
                    $("#FrequencyGroup").addClass('esg-has-error');
                    $("#Frequency").removeClass('has-success');
                }

                // UI validations for month
                if (MonthCode) {
                    $("#Month").removeClass('has-error');
                    $("#MonthIcon").hide();
                    $("#MonthRequired").hide();
                    $("#MonthGroup").removeClass('esg-has-error');
                }
                else {
                    $("#Month").val("");
                    $("#MonthIcon").show();
                    $("#MonthRequired").show();
                    $("#MonthGroup").addClass('esg-has-error');
                    $("#Month").removeClass('has-success');
                }

                // UI validations for year
                if (YearCode) {
                    $("#Year").removeClass('has-error');
                    $("#YearIcon").hide();
                    $("#YearRequired").hide();
                    $("#YearGroup").removeClass('esg-has-error');
                }
                else {
                    $("#Year").val("");
                    $("#YearIcon").show();
                    $("#YearRequired").show();
                    $("#YearGroup").addClass('esg-has-error');
                    $("#Year").removeClass('has-success');
                }

                // UI validations for RecTaxAddress
                var RecTaxAddress = $("#RecTaxAddress").val();
                var IssPlaceOfIssue = $("#IssPlaceIssue").val();

                if (RecTaxAddress == IssPlaceOfIssue) {
                    $("#RecTaxAddress").removeClass('has-error');
                    $("#RecTaxAddressIcon").hide();
                    $("#RecTaxAddressIncorrect").hide();
                    $("#RecTaxAddressGroup").removeClass('esg-has-error');
                }
                else {
                    $("#RecTaxAddressIcon").show();
                    $("#RecTaxAddressIncorrect").show();
                    $("#RecTaxAddressGroup").addClass('esg-has-error');
                    $("#RecTaxAddress").removeClass('has-success');
                }

                // UI validations for RecTaxRegimen
                var RecTaxRegimen = $("#RecTaxRegimen").val();

                if (RecTaxRegimen == '616 - Sin obligaciones fiscales') {
                    $("#RecTaxRegimen").removeClass('has-error');
                    $("#RecTaxRegimenIcon").hide();
                    $("#RecTaxRegimenIncorrect").hide();
                    $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                }
                else {
                    $("#RecTaxRegimenIcon").show();
                    $("#RecTaxRegimenIncorrect").show();
                    $("#RecTaxRegimenGroup").addClass('esg-has-error');
                    $("#RecTaxRegimen").removeClass('has-success');
                }

                // UI validations for RecCfdiUsage
                var RecCfdiUsage = $("#RecCfdiUsage").val();

                if (RecCfdiUsage == 'S01 - Sin efectos fiscales') {
                    $("#RecCfdiUsage").removeClass('has-error');
                    $("#RecCfdiUsageIcon").hide();
                    $("#RecCfdiUsageIncorrect").hide();
                    $("#RecCfdiUsageGroup").removeClass('esg-has-error');
                }
                else {
                    $("#RecCfdiUsageIcon").show();
                    $("#RecCfdiUsageIncorrect").show();
                    $("#RecCfdiUsageGroup").addClass('esg-has-error');
                    $("#RecCfdiUsage").removeClass('has-success');
                }

                // UI validations for RecName
                var RecName = $("#RecName").val();

                if (RecName == 'PUBLICO EN GENERAL' || RecName == 'PÚBLICO EN GENERAL') {
                    $("#RecName").removeClass('has-error');
                    $("#RecNameIcon").hide();
                    $("#RecNameIncorrect").hide();
                    $("#RecNameGroup").removeClass('esg-has-error');
                }
                else {
                    $("#RecNameIcon").show();
                    $("#RecNameIncorrect").show();
                    $("#RecNameGroup").addClass('esg-has-error');
                    $("#RecName").removeClass('has-success');
                }
            }
        }
        else {
            $('.errorMessageSerialError').show();
            $('.errorMessageRequiredDiv').hide();
            $('.errorMessageDiv').hide();
        }

    });

    function formIsValid() {
        var PaymentTypeCode = $("#PaymentType option:selected").val();
        var FrequencyCode = $("#Frequency option:selected").val();
        var MonthCode = $("#Month option:selected").val();
        var SerialNumber = $("#IssSerial option:selected").text();

        var isValid = PaymentTypeCode && FrequencyCode && MonthCode && SerialNumber;

        var RecTaxAddress = $("#RecTaxAddress").val();
        var RecTaxRegimen = $("#RecTaxRegimen").val();
        var RecCfdiUsage = $("#RecCfdiUsage").val();
        var RecName = $("#RecName").val();
        var Email = $("#PreferredEmail").val();

        var IssPlaceOfIssue = $("#IssPlaceIssue").val();

        isValid = isValid
            && RecTaxAddress == IssPlaceOfIssue
            && RecTaxRegimen == '616 - Sin obligaciones fiscales'
            && RecCfdiUsage == 'S01 - Sin efectos fiscales'
            && (RecName == 'PUBLICO EN GENERAL' || RecName == 'PÚBLICO EN GENERAL')
            && Email;

        return isValid;
    }

    // #endregion Create
}
// #endregion NewGlobalInvoice