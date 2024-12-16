function EmailModal(id, idc) {
    $("#ModalAdd").css('display', 'block');
    $('.AddNewMappingDiv').css('display', 'block');
    $("ReceiverEmailValidation").hide();
    $("WarningEmailValidation").hide();
    $("#InvoiceCertificateId").val(idc);
    hideFormError("ReceiverEmail");
}

$(document).ready(function () {

    var IsPPD = $("#IsPPD").val() == 'True';
    var DeleteCreditNote = $("#VoucherType").val();
    var StatusEnum = $('#StatusEnum').val();
    var CancelPPD = $('#PaymentTypePPD').val();
    var FiscalRecordType = $('#FiscalRecordType').val();

    $("#btnReprocess").click(function () {
        showProcessing();
        $("#btnReprocess").prop("disabled", true);
        var requestState = $('#InvoiceRequestHidden').val();
        var status = -1;
        if (requestState == 2 || requestState == 3 || requestState == 4)
            status = 1;
        if (requestState == 5 || requestState == 6 || requestState == 7)
            status = 2;
        var fiscalRecordRequest = {
            InvoiceHeaderId: invoiceHeaderId,
            Status: status,//pending to create
            InvoicePaymentReceiptId: invoicePaymentReceiptId
        };
        $.ajax({
            url: urlReprocess,
            type: "Post",
            cache: false,
            dataType: "json",
            data: { fiscalRecordRequest: fiscalRecordRequest },
            success: function (response) {
                if (response.id <= 0) {
                    hideProcessing();
                }
                else {
                    hideProcessing();
                    window.location.href = urlViewAll;
                }
            }
        });

    });

    $("#btnCancel").click(function () {
        $("#divCancelReasonsErr").parent().removeClass('esg-has-error');
        $("#divCancelReasonsErr").hide();
        $("#ModalCancelFiscalRecord").show();
        $('.CancelFiscalRecordDiv').show();
        $("#formCancel .field-validation-error > span").remove();
        $("#formCancel .field-validation-error").addClass('field-validation-valid');
        $("#formCancel .field-validation-error").removeClass('field-validation-error');
    });

    $("#btnCloseCancelFiscalRecord, #btnBackFromCancel").click(function () {
        $("#ModalCancelFiscalRecord").hide();
        $('.CancelFiscalRecordDiv').hide();
    });

    $("#CancelReasons").change(function () {
        if ($('#formCancel').valid()) {
            $("#divCancelReasonsErr").parent().removeClass('esg-has-error');
            $("#divCancelReasonsErr").hide();
        }
        else {
            $("#divCancelReasonsErr").parent().addClass('esg-has-error');
            $("#divCancelReasonsErr").show();
        }
    });

    $("#btnCancelRecord").click(function () {
        var formCancel = $('#formCancel');
        if (formCancel.valid() === false) {
            hideProcessing();
            $("#divCancelReasonsErr").parent().addClass('esg-has-error');
            $("#divCancelReasonsErr").show();
            return false;
        }
        $("#ModalCancelFiscalRecord").css('display', 'none');
        $('.CancelFiscalRecordDiv').css('display', 'none');
        showProcessing();
        $("#btnCancel").prop("disabled", true);
        var cancelReasonKeyValue = $('#CancelReasons').val();
        var fiscalRecordRequest = {
            InvoiceHeaderId: invoiceHeaderId,
            Status: 2, //cancel
            CancelReasonKey: cancelReasonKeyValue
        };
        $.ajax({
            url: urlReprocess,
            type: "Post",
            cache: false,
            dataType: "json",
            data: { fiscalRecordRequest: fiscalRecordRequest },
            success: function (response) {
                if (response.id <= 0) {
                    hideProcessing();
                }
                else {
                    hideProcessing();
                    if (cancelReasonKeyValue == '04') {
                        window.location.href = urlCancelGlobal;
                    }
                    else {
                        window.location.href = urlViewAll;
                    }
                }
            }
        });
    });

    $("#btnSubstituteCancel").click(function () {
        showProcessing();
        $("#btnSubstituteCancel").prop("disabled", true);
        if (FiscalRecordType == "E") {
            var peopleOrgCodeId = $('#PeopleOrgCodeId').val();
            $.ajax({
                url: urlGetId,
                type: "GET",
                cache: false,
                dataType: "html",
                data: { id: invoiceHeaderId, peopleOrgId: peopleOrgCodeId, peopleOrgName: "" },
                success: function (response) {
                    urlCancelCreditNote = urlCancelCreditNote.replace("param-id", true);
                    window.location.href = urlCancelCreditNote;
                }
            });
        }
        else {
            if (IsPPD) {
                window.location.href = urlCreatePPD;
            }
            else if (!PeopleOrgCodeId) {
                window.location.href = urlCreateGlobalFiscalRecord;
            }
            else {
                window.location.href = urlCreateFiscalRecord;
            }
        }
    });

    $('#btnCreditNotes, .credit-note-by-people-org').click(function () {
        var code = $(this).attr("data-code-peo-org");
        var Name = $(this).attr("data-code-peo-name");
        $.ajax({
            url: urlGetId,
            type: "GET",
            cache: false,
            dataType: "html",
            data: { id: invoiceHeaderId, peopleOrgId: code, peopleOrgName: Name },
            success: function (response) {
                window.location.href = urlCreateCreditNote;
            }
        });
    });

    $("#btnCancelEmail,.btnCancel").click(function () {
        $("#ReceiverEmailValidation").hide();
        $("#WarningEmailValidation").hide();
        $('.AddNewMappingDiv').hide();
        $("#ModalAdd").hide();

    });

    $("#btnSendEmail").click(function () {
        showProcessing();
        $("#ReceiverEmailValidation").hide();
        $("#WarningEmailValidation").hide();
        var InvoiceCertificateId = $("#InvoiceCertificateId").val();
        var ReceiverEmail = $("#ReceiverEmail").val();
        if (ReceiverEmail == "") {
            showFormError("ReceiverEmail");
            hideProcessing();
        }
        else {
            var InvoiceHeaderId = $("#InvoiceHeaderId").val();
            var Folio = $("#Folio").val();
            var Serie = $("#Serie").val();
            var FiscalIdNum = $("#FiscalIdNum").val();
            $.ajax({
                type: "POST",
                cache: false,
                url: urlSendEmail,
                data: { id: InvoiceHeaderId, idc: InvoiceCertificateId, emailTo: ReceiverEmail, folio: Folio, serie: Serie, uuid: FiscalIdNum },
                success: function (response) {
                    if (response > 0) {
                        hideProcessing();
                        $('.AddNewMappingDiv').css('display', 'none');
                        $("#ModalAdd").css('display', 'none');
                        $("#ReceiverEmailValidation").hide();
                        $("#WarningEmailValidation").hide();
                    }
                    else if (response == -1) {
                        hideProcessing();
                        $("#ReceiverEmailValidation").show();
                        $("#WarningEmailValidation").hide();
                    }
                    else if (response == -2) {
                        hideProcessing();
                        $("#WarningEmailValidation").show();
                        $("#ReceiverEmailValidation").hide();
                    }
                    else {
                        hideProcessing();
                        $('.AddNewMappingDiv').css('display', 'none');
                        $("#ModalAdd").css('display', 'none');
                        $("#ReceiverEmailValidation").hide();
                        $("#WarningEmailValidation").hide();
                    }
                }
            });
        }
    });

    if (DeleteCreditNote === "Egreso" && StatusEnum === "ProviderCannotCreate" || DeleteCreditNote === "Egreso" && StatusEnum === "ProviderCannotCancel")
        $('#divDeleteCreditNote').show();

    if (CancelPPD && CancelPPD.startsWith("PPD"))
        $('#divCancelPPD').show();

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
                    $('.errorMessageDiv').show();
                    $(".errorMessageResult").html(response.message);
                    $('#divDeleteCreditNote').hide();
                }
                else {
                    hideProcessing();
                    $('.successMessageDiv').show();
                    $('.errorMessageDiv').hide();
                    $(".successMessageResult").html(response.message);
                    $('.successMessageDivCreditNote').hide();
                    $('#divDeleteCreditNote').hide();
                    window.location.href = urlViewAll;

                }
            }
        });

    });

    $("#btnViewCashReceipt").click(function () {
        var value = $('#ReceiptNumber').val();
        $.ajax({
            url: urlSetReceiptNumber,
            type: "GET",
            cache: false,
            dataType: "html",
            data: { receiptNumber: value },
            success: function (response) {
                window.location.href = urlViewCashReceipt;
            }
        });
    });
});
