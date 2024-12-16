function CloseReceiverModal() {
    $('#ModalSaveReceiver').hide();
    $('.SaveReceiverDiv').hide();
    $('#btnCreateReceiver').hide();
    $('#btnUpdateReceiver').hide();
    ClearAllSaveReceiver();
}

function CloseReceiverModalAfterSave() {
    $('#btnCloseSaveReceiverAfterSave').click();
}

function AddReceiverInModal() {
    hideTaxPayerIdMessages();
    $('#ReceiverPanel #TaxPayerId').val('');
    SetInitialStateReceiverPanel();
    $("#ReceiverPanel #TaxRegimenId").val('');
    GetTaxRegimenList(false);
    $('#ModalSaveReceiver').show();
    $('.SaveReceiverDiv').show();
    $('#btnCreateReceiver').show();
}

function EditReceiverInModal(taxpayerIdElementId, fiscalIdNumberElementId) {
    hideTaxPayerIdMessages();
    var taxPayerIdValue = $(`#${taxpayerIdElementId}`).val()
    if (taxPayerIdValue) {
        var regex = new RegExp("[A-Z, Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9, A-Z]", "i");
        if (regex.test(taxPayerIdValue)) {
            $('#ReceiverPanel #TaxPayerId').val(taxPayerIdValue);
            SetInitialStateReceiverPanel();
            $("#ReceiverPanel #FiscalIdentityNumber").val($(`#${fiscalIdNumberElementId}`).val());
            GetTaxRegimenList(true);
            $('#ModalSaveReceiver').show();
            $('.SaveReceiverDiv').show();
            $('#btnUpdateReceiver').show();
        }
        else {
            showInvalidTaxPayerIdMessage();
        }
    }
    else if (!$('#TaxPayerId-error').is(":visible")) {
        showEmptyTaxPayerIdMessage();
    }
}

function hideTaxPayerIdMessages() {
    $('#TaxPayerId').parent().removeClass('has-error');
    $('#RecTaxPayerGroup').removeClass('esg-has-error');
    $('#RecTaxPayerIcon').hide();
    $('#RecTaxPayerDivLookup').show();
    $('#msgTaxpayerIdEmpty').hide();
    $('#msgTaxpayerIdInvalid').hide();
}

function showEmptyTaxPayerIdMessage() {
    $('#TaxPayerId').parent().addClass('has-error');
    $('#RecTaxPayerGroup').addClass('esg-has-error');
    $('#RecTaxPayerIcon').show();
    $('#RecTaxPayerDivLookup').hide();
    $('#msgTaxpayerIdEmpty').show();
}

function showInvalidTaxPayerIdMessage() {
    $('#TaxPayerId').parent().addClass('has-error');
    $('#RecTaxPayerGroup').addClass('esg-has-error');
    $('#RecTaxPayerIcon').show();
    $('#RecTaxPayerDivLookup').hide();
    $('#msgTaxpayerIdInvalid').show();
}