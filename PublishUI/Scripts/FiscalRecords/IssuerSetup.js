//ISSUER SETUP-----------------------------------------------------
// List for the autocomplete
var IssTaxPayers = [];
// Addresses array
var IssIssuingAddresses;
// Serial Array
var IssSerial;
// Charge Credit Code Array
var IssChargeCreditCode;
// Type of Configuration
var IssTypeConfiguration;

function ChangeAddress() {
    if (IssTypeConfiguration !== 0) {
        var IssExpeditionId = $('#DefaultIssuingAddress option:selected').val();

        if (IssExpeditionId !== "") {
            var IssInvoiceOrganizationId = $("#DefaultInvoiceOrganizationId").val();

            $("#DefaultSerial,#DefaultSerialForCreditNote").empty();
            $("#DefaultSerial,#DefaultSerialForCreditNote").append($('<option></option>').val('').html(''));

            // Get SerialNumber
            LoadSerial(IssInvoiceOrganizationId, IssExpeditionId, 0, 0);
        }
        else {
            $("#DefaultSerial,#DefaultSerialForCreditNote").empty();
            $("#DefaultSerial,#DefaultSerialForCreditNote").append($('<option></option>').val('').html(''));
        }
    }
}

function ConfigurationByRFC() {
    if ($("#dvByAddress #DefaultIssuingAddress").length > 0) {
        $("#dvByRFC").html($("#dvByAddress").html());
        $("#dvByAddress").html("");
        $("#dvByAddress").hide();
    }
    IssTypeConfiguration = 0;
    $("#dvByRFC").show();

    $("#DefaultIssuingAddress").change(function () { ChangeAddress(); });
}

function ConfigurationByAddress() {
    if ($("#dvByRFC #DefaultIssuingAddress").length > 0) {
        $("#dvByAddress").html($("#dvByRFC").html());
        $("#dvByRFC").html("");
        $("#dvByRFC").hide();
    }
    IssTypeConfiguration = 1;
    $("#dvByAddress").show();

    $("#DefaultIssuingAddress").change(function () { ChangeAddress(); });
}

function LoadAddresses(IssInvoiceOrganizationId, IssInvoiceExpeditionId, IssInvoiceReceiptId, IssCreditNoteReceiptId) {
    $.ajax({
        url: urlGetTaxRegimen,
        type: "GET",
        cache: false,
        dataType: "json",
        data: { id: IssInvoiceOrganizationId },
        success: function (dataAddress) {
            if (dataAddress.IssIssuingAdd !== null) {
                IssIssuingAddresses = dataAddress.IssIssuingAdd;

                // Fill dropdownlist with addresses
                $.each(IssIssuingAddresses, function (i, val) {
                    $("#DefaultIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                });

                if (IssInvoiceExpeditionId !== null && IssInvoiceExpeditionId > 0) {
                    // Select default value in dropdownlist for the address
                    $("#DefaultIssuingAddress option[value=" + IssInvoiceExpeditionId + "]").attr('selected', 'selected');
                }

                // Review the first address that indicates if the configuration is by RFC or by Expedition Address
                // Show the dropdownlist for addresses according with the configuration and if required fill dropdownlists for serial number
                if (IssIssuingAddresses[0].IssByExpedition !== 0) {
                    ConfigurationByAddress();
                    if (IssInvoiceExpeditionId > 0) {
                        // Get SerialNumber
                        LoadSerial(IssInvoiceOrganizationId, IssInvoiceExpeditionId, IssInvoiceReceiptId, IssCreditNoteReceiptId);
                    }
                    else {
                        $("#DefaultSerial,#DefaultSerialForCreditNote").empty();
                        $("#DefaultSerial,#DefaultSerialForCreditNote").append($('<option></option>').val('').html(''));
                    }
                }
                else {
                    ConfigurationByRFC();
                    // Get SerialNumber
                    LoadSerial(IssInvoiceOrganizationId, null, IssInvoiceReceiptId, IssCreditNoteReceiptId);
                }
            }
        }
    });
}

function LoadSerial(IssInvoiceOrganizationId, IssExpeditionId, IssInvoiceReceiptId, IssCreditNoteReceiptId) {
    $.ajax({
        url: urlGetSerialNumber,
        type: "GET",
        cache: false,
        dataType: "json",
        data: { id: IssInvoiceOrganizationId, expId: IssExpeditionId },
        success: function (dataSerial) {
            if (dataSerial.IssSerial !== null) {
                IssSerial = dataSerial.IssSerial;

                // Fill dropdownlist with serial for invoices
                $.each(IssSerial, function (i, val) {
                    $("#DefaultSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber));
                });

                if (IssInvoiceReceiptId !== null && IssInvoiceReceiptId > 0 && $.grep(IssSerial, function (element) { return element.IssInvoiceReceipt === IssInvoiceReceiptId; })[0] !== null) {
                    // Select default value in dropdownlist for the serie of the invoice
                    $("#DefaultSerial option[value=" + IssInvoiceReceiptId + "]").attr('selected', 'selected');
                }

                // Fill dropdownlist with serial for credit notes
                $.each(IssSerial, function (i, val) {
                    $("#DefaultSerialForCreditNote").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber));
                });

                if (IssCreditNoteReceiptId !== null && IssCreditNoteReceiptId > 0 && $.grep(IssSerial, function (element) { return element.IssInvoiceReceipt === IssCreditNoteReceiptId; })[0] !== null) {
                    // Select default value in dropdownlist for the serie of the credit note
                    $("#DefaultSerialForCreditNote option[value=" + IssCreditNoteReceiptId + "]").attr('selected', 'selected');
                }
            }
        }
    });
}

function LoadChargeCreditCode(IssChargeCreditCodeId, IssChargeCreditCodeTaxId) {
    $.ajax({
        url: urlGetChargeCreditCode,
        type: "GET",
        cache: false,
        dataType: "json",
        data: {},
        success: function (dataChargeCreditCode) {
            if (dataChargeCreditCode !== null) {
                IssChargeCreditCode = dataChargeCreditCode;

                // Fill dropdownlist with charge credit code (amount)
                $.each(IssChargeCreditCode, function (i, val) {
                    $("#DefaultChargeCreditCode").append($('<option></option>').val(val.ChargeCreditCodeId).html(val.DistributionOrder + " - " + val.ChargeCreditCode + " - " + val.ChargeCreditDesc));
                });

                if (IssChargeCreditCodeId !== null && IssChargeCreditCodeId > 0 && $.grep(IssChargeCreditCode, function (element) { return element.ChargeCreditCodeId === IssChargeCreditCodeId; })[0] !== null) {
                    // Select default value in dropdownlist for charge credit code (amount)
                    $("#DefaultChargeCreditCode option[value=" + IssChargeCreditCodeId + "]").attr('selected', 'selected');
                }

                // Fill dropdownlist with charge credit code (taxes)
                $.each(IssChargeCreditCode, function (i, val) {
                    $("#DefaultChargeCreditCodeTax").append($('<option></option>').val(val.ChargeCreditCodeId).html(val.DistributionOrder + " - " + val.ChargeCreditCode + " - " + val.ChargeCreditDesc));
                });

                if (IssChargeCreditCodeTaxId !== null && IssChargeCreditCodeTaxId > 0 && $.grep(IssChargeCreditCode, function (element) { return element.ChargeCreditCodeId === IssChargeCreditCodeTaxId; })[0] !== null) {
                    // Select default value in dropdownlist for charge credit code (amount)
                    $("#DefaultChargeCreditCodeTax option[value=" + IssChargeCreditCodeTaxId + "]").attr('selected', 'selected');
                }
            }
        }
    });
}

function LoadIssuerSetUp() {
    $.ajax({
        url: urlSelectIssuerSetUp,
        type: "GET",
        cache: false,
        success: function (dataDefault) {
            // Empty values in hidden fields
            $("#DefaultId").val("");
            $("#DefaultInvoiceOrganizationId").val("");

            // Empty values in text fields
            $("#DefaulTaxpayerId").val("");
            $("#DefaultPaymentCondition").val("");

            // Empty dropdownlists and add one blank option
            $("#DefaultIssuingAddress,#DefaultSerial,#DefaultSerialForCreditNote,#DefaultChargeCreditCode,#DefaultChargeCreditCodeTax").empty();
            $("#DefaultIssuingAddress,#DefaultSerial,#DefaultSerialForCreditNote,#DefaultChargeCreditCode,#DefaultChargeCreditCodeTax").append($('<option></option>').val('').html(''));

            if (dataDefault.IssDefaultId > 0) {
                // Set Id for all default values, exist a register with the default values
                $("#DefaultId").val(dataDefault.IssDefaultId);
                // Set button title to Update
                $("#btnSave").val($("#txtUpdate").val());
            }

            if (dataDefault.IssInvoiceOrganizationId !== null && dataDefault.IssInvoiceOrganizationId > 0) {
                // Set Id of the Organization
                $("#DefaultInvoiceOrganizationId").val(dataDefault.IssInvoiceOrganizationId);

                // Set the Tax Payer Id associated to Organization Id
                $("#DefaulTaxpayerId").val(dataDefault.IssInvoiceTaxpayerId);

                // Get Addresses for the Tax Payer Id
                LoadAddresses(dataDefault.IssInvoiceOrganizationId, dataDefault.IssInvoiceExpeditionId, dataDefault.IssInvoiceReceiptId, dataDefault.IssCreditNoteReceiptId);
            }

            if (dataDefault.IssPaymentConditions !== null && dataDefault.IssPaymentConditions !== "") {
                // Set default value for Payment Conditions
                $("#DefaultPaymentCondition").val(dataDefault.IssPaymentConditions);
            }

            LoadChargeCreditCode(dataDefault.IssChargeCreditCodeId, dataDefault.IssChargeCreditCodeTaxId);
        }
    });
}

$(document).ready(function () {
    LoadIssuerSetUp();

    $("#DefaulTaxpayerId").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGet,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    IssTaxPayers = data;
                    response($.map(data, function (item) {
                        return { value: item.IssTaxpayerId, id: item.IssInvoiceOrganizationId }
                    }))

                }
            })
        }
    });

    $("#DefaulTaxpayerId").focusout(function () {

        var IdIssTaxPayer = $(this).val();

        if (IdIssTaxPayer == "") {
            // Empty hidden value of the organization is set blank
            $("#DefaultInvoiceOrganizationId").val("");

            // Empty dropdownlists and add one blank option
            $("#DefaultIssuingAddress,#DefaultSerial,#DefaultSerialForCreditNote").empty();
            $("#DefaultIssuingAddress,#DefaultSerial,#DefaultSerialForCreditNote").append($('<option></option>').val('').html(''));

            // By default set addresses dropdownlist by RFC
            ConfigurationByRFC();
        }
        else {
            var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];

            if (response != "" && response != null) {
                // Empty hidden value of the organization is set according to the response
                $("#DefaultInvoiceOrganizationId").val(response.IssInvoiceOrganizationId);

                // Empty dropdownlists and add one blank option
                $("#DefaultIssuingAddress,#DefaultSerial,#DefaultSerialForCreditNote").empty();
                $("#DefaultIssuingAddress,#DefaultSerial,#DefaultSerialForCreditNote").append($('<option></option>').val('').html(''));

                // Get Addresses
                LoadAddresses(response.IssInvoiceOrganizationId, 0, 0, 0);
            }
        }
    });

    $("#DefaultPaymentCondition").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }
    });

    $("#btnSave").click(function () {
        var DefaultId = $("#DefaultId").val();
        var TaxpayerId = $("#DefaulTaxpayerId").val();
        var PaymentCondition = $("#DefaultPaymentCondition").val();

        if (TaxpayerId != "" || PaymentCondition != "" || DefaultId != "" || DefaultId != 0) {
            showProcessing();
            $.ajax({
                type: "POST",
                cache: false,
                url: urlCreateIssuerSetUp,
                data: $('#formCreateSetUp').serialize(),
                success: function (response) {
                    if (response > 0) {
                        hideProcessing();
                        $("#alertSuccess").removeClass("no-display");
                        setTimeout(function () {
                            window.location.href = urlSettings;
                        }, 1000);
                    }
                    else {
                        hideProcessing();
                        $("#alertError").removeClass("no-display");
                    }
                }
            });
        }
    });

    $("#btnCancel").click(function () {
        window.location.href = urlSettings;
    });
});