$(document).ready(function () {

    var IsReceiverInModal = $('#hdnIsReceiverInModal').val() == 'True';

    SetInitialStateReceiverPanel();

    $('#ReceiverPanel #FiscalResidency').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetCountires,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: request.term },
                success: function (data) {
                    if (data.length > 0) {
                        response($.map(data, function (item) {
                            return { value: item.Code, label: item.Description }
                        }))
                    }
                    else {
                        $('#ReceiverPanel #FiscalResidency').val('');
                    }
                }
            })
        },
        select: function () {
            $(this).prop('readonly', true);
            $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divFiscalResidencyIcon').hide();
            $('#ReceiverPanel #FiscalResidencyValidation').hide();
        }
    });

    $('#ReceiverPanel #FiscalResidency').focusout(function () {
        var FiscalResidency = $(this);
        if (!FiscalResidency.is('[readonly]')) {
            if (!FiscalResidency.val()) {
                $('#ReceiverPanel #divFiscalResidency').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').show();
                $('#ReceiverPanel #FiscalResidencyValidation').show();
            }
            else {
                $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').hide();
                $('#ReceiverPanel #FiscalResidencyValidation').hide();
                if (!$('#ReceiverPanel #FiscalResidency').is('[readonly]')) {
                    $('#ReceiverPanel #FiscalResidency').val('');
                    $('#ReceiverPanel #divFiscalResidency').parent().addClass('esg-has-error');
                    $('#ReceiverPanel #divFiscalResidencyIcon').show();
                    $('#ReceiverPanel #FiscalResidencyValidation').show();
                }
            }
        }
    });

    $('#ReceiverPanel #FiscalIdentityNumber').focusout(function () {
        var FiscalIdentityNumber = $(this);
        if (!FiscalIdentityNumber.is('[readonly]')) {
            if (!FiscalIdentityNumber.val()) {
                $('#ReceiverPanel #divFiscalIdentityNumber').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divFiscalIdentityNumberIcon').show();
                $('#ReceiverPanel #FiscalIdentityNumberValidation').show();
            }
            else {
                $('#ReceiverPanel #divFiscalIdentityNumber').parent().removeClass('esg-has-error');
                $('#ReceiverPanel #divFiscalIdentityNumberIcon').hide();
                $('#ReceiverPanel #FiscalIdentityNumberValidation').hide();
            }
        }
    });

    $('#ReceiverPanel #PostalCode').change(function () {
        var PostalCodeValue = $(this).val();
        if (PostalCodeValue.length === 5 && PostalCodeValue !== '00000' && $.isNumeric(PostalCodeValue)) {
            $('#btnCreateReceiver').prop('disabled', true);
            $('#btnUpdateReceiver').prop('disabled', true);
            $.ajax({
                url: urlGetPostalCode,
                type: 'GET',
                cache: false,
                dataType: 'json',
                data: { id: PostalCodeValue },
                success: function (data) {
                    if (data.length > 0) {
                        $('#ReceiverPanel #divPostalCode').parent().removeClass('esg-has-error');
                        $('#ReceiverPanel #divPostalCodeIcon').hide();
                        $('#ReceiverPanel #PostalCodeInCatalogValidation').hide();
                    }
                    else {
                        $('#ReceiverPanel #divPostalCode').parent().addClass('esg-has-error');
                        $('#ReceiverPanel #divPostalCodeIcon').show();
                        $('#ReceiverPanel #PostalCodeInCatalogValidation').show();
                    }
                    $('#btnCreateReceiver').prop('disabled', false);
                    $('#btnUpdateReceiver').prop('disabled', false);
                },
                error: function () {
                    $('#btnCreateReceiver').prop('disabled', false);
                    $('#btnUpdateReceiver').prop('disabled', false);
                }
            })
        }
        else {
            $('#ReceiverPanel #PostalCode').val('');
            $('#ReceiverPanel #divPostalCode').parent().addClass('esg-has-error');
            $('#ReceiverPanel #divPostalCodeIcon').show();
            $('#ReceiverPanel #PostalCodeInCatalogValidation').show();
        }
    });

    $('#btnClearAll').click(function () {
        ClearAllSaveReceiver();
    });

    $('#btnCreateReceiver').click(function () {
        var formSaveReceiver = $('#formSaveReceiver');
        if (!$('#ReceiverPanel #FiscalResidency').is('[readonly]')) {
            if (!$('#ReceiverPanel #FiscalResidency').val()) {
                $('#ReceiverPanel #divFiscalResidency').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').show();
                $('#ReceiverPanel #FiscalResidencyValidation').show();
                if (!$('#ReceiverPanel #FiscalIdentityNumber').is('[readonly]')) {
                    if (!$('#ReceiverPanel #FiscalIdentityNumber').val()) {
                        $('#ReceiverPanel #divFiscalIdentityNumber').parent().addClass('esg-has-error');
                        $('#ReceiverPanel #divFiscalIdentityNumberIcon').show();
                        $('#ReceiverPanel #FiscalIdentityNumberValidation').show();
                    }
                }
                return false;
            }
            else {
                $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').hide();
                $('#ReceiverPanel #FiscalIdentityNumberValidation').hide();
                $('#ReceiverPanel #FiscalResidencyValidation').hide();
            }
        }
        if (!$('#ReceiverPanel #FiscalIdentityNumber').is('[readonly]')) {
            if (!$('#ReceiverPanel #FiscalIdentityNumber').val()) {
                $('#ReceiverPanel #divFiscalIdentityNumber').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divFiscalIdentityNumberIcon').show();
                $('#ReceiverPanel #FiscalIdentityNumberValidation').show();
                return false;
            }
        }
        if (formSaveReceiver.valid() === false) {
            var TaxPayerIdValue = $('#ReceiverPanel #TaxPayerId').val();
            var CorporateNameValue = $('#ReceiverPanel #CorporateName').val();
            var TaxAddressValue = $('#ReceiverPanel #PostalCode').val();
            var TaxRegimenValue = $('#ReceiverPanel #TaxRegimenId').val();

            if (!TaxPayerIdValue) {
                $('#ReceiverPanel #divTaxpayerId').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divTaxpayerIdIcon').show();
                $('#ReceiverPanel #TaxPayerIdValidation').show();
            }
            if (!CorporateNameValue) {
                $('#ReceiverPanel #divCorporateName').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divCorporateNameIcon').show();
                $('#ReceiverPanel #CorporateNameValidation').show();
            }
            if (!TaxAddressValue) {
                $('#ReceiverPanel #divPostalCode').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divPostalCodeIcon').show()
                $('#ReceiverPanel #PostalCodeValidation').show();
            }
            if (!TaxRegimenValue || TaxRegimenValue == '0') {
                $('#ReceiverPanel #divTaxRegimen').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divTaxRegimenIcon').show()
                $('#ReceiverPanel #TaxRegimenValidation').show();
            }

            return false;
        }
        else {
            $('#ReceiverPanel #divTaxpayerId').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divTaxpayerIdIcon').hide();
            $('#ReceiverPanel #TaxPayerIdValidation').hide();
            $('#ReceiverPanel #divCorporateName').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divCorporateNameIcon').hide();
            $('#ReceiverPanel #CorporateNameValidation').hide();
            $('#ReceiverPanel #divPostalCode').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divPostalCodeIcon').hide()
            $('#ReceiverPanel #PostalCodeValidation').hide();
            $('#ReceiverPanel #divTaxRegimen').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divTaxRegimenIcon').hide()
            $('#ReceiverPanel #TaxRegimenValidation').hide();
        }
        showPageLoader();
        $.ajax({
            type: 'POST',
            cache: false,
            url: urlCreateReceiver,
            data: $('#formSaveReceiver').serialize(),
            success: function (response) {
                hidePageLoader();
                if (IsReceiverInModal) {
                    CloseReceiverModalAfterSave();
                }
                else {
                    if (response > 0) {
                        window.location.href = urlListReceivers;
                    }
                    else {
                        window.location.href = urlHome;
                        return false;
                    }
                }
            }
        });
    });

    $('#btnUpdateReceiver').click(function () {

        var formSaveReceiver = $('#formSaveReceiver');
        if (!$('#ReceiverPanel #FiscalResidency').is('[readonly]')) {
            if (!$('#ReceiverPanel #FiscalResidency').val()) {
                $('#ReceiverPanel #divFiscalResidency').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').show();
                if (!$('#ReceiverPanel #FiscalIdentityNumber').is('[readonly]')) {
                    if (!$('#ReceiverPanel #FiscalIdentityNumber').val()) {
                        $('#ReceiverPanel #divFiscalIdentityNumber').parent().addClass('esg-has-error');
                        $('#ReceiverPanel #divFiscalIdentityNumberIcon').show();
                    }
                }
                return false;
            }
            else {
                $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').hide();
            }
        }
        if (!$('#ReceiverPanel #FiscalIdentityNumber').is('[readonly]')) {
            if (!$('#ReceiverPanel #FiscalIdentityNumber').val()) {
                $('#ReceiverPanel #divFiscalIdentityNumber').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divFiscalIdentityNumberIcon').show();
                return false;
            }
        }
        if (formSaveReceiver.valid() === false) {
            var CorporateNameValue = $('#ReceiverPanel #CorporateName').val();
            var TaxAddressValue = $('#ReceiverPanel #PostalCode').val();
            var TaxRegimenValue = $('#ReceiverPanel #TaxRegimenId').val();

            if (!CorporateNameValue) {
                $('#ReceiverPanel #divCorporateName').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divCorporateNameIcon').show();
                $('#ReceiverPanel #CorporateNameValidation').show();
            }
            if (!TaxAddressValue) {
                $('#ReceiverPanel #divPostalCode').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divPostalCodeIcon').show()
                $('#ReceiverPanel #PostalCodeValidation').show();
            }
            if (!TaxRegimenValue) {
                $('#ReceiverPanel #divTaxRegimen').parent().addClass('esg-has-error');
                $('#ReceiverPanel #divTaxRegimenIcon').show()
                $('#ReceiverPanel #TaxRegimenValidation').show();
            }

            return false;
        }
        else {
            $('#ReceiverPanel #divCorporateName').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divCorporateNameIcon').hide();
            $('#ReceiverPanel #CorporateNameValidation').hide();
            $('#ReceiverPanel #divPostalCode').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divPostalCodeIcon').hide()
            $('#ReceiverPanel #PostalCodeValidation').hide();
            $('#ReceiverPanel #divTaxRegimen').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divTaxRegimenIcon').hide()
            $('#ReceiverPanel #TaxRegimenValidation').hide();
        }

        showPageLoader();
        $.ajax({
            type: 'POST',
            cache: false,
            url: urlEditReceiver,
            data: formSaveReceiver.serialize(),
            success: function (response) {
                hidePageLoader();
                if (IsReceiverInModal) {
                    CloseReceiverModalAfterSave();
                }
                else {
                    if (response.id > 0) {
                        window.location.href = urlListReceivers;
                    }
                    else {
                        window.location.href = urlHome;
                    }
                }
            }
        });
    });

    $('#ReceiverPanel #CorporateName').focusout(function () {
        var CorporateNameValue = $(this).val();

        if (!CorporateNameValue) {
            $('#ReceiverPanel #divCorporateName').parent().addClass('esg-has-error');
            $('#ReceiverPanel #divCorporateNameIcon').show();
            $('#ReceiverPanel #CorporateNameValidation').show();
        } else {
            $('#ReceiverPanel #divCorporateName').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divCorporateNameIcon').hide();
            $('#ReceiverPanel #CorporateNameValidation').hide();
        }
    })

    $('#ReceiverPanel #PostalCode').focusout(function () {
        var PostalCodeValue = $(this).val();

        if (!PostalCodeValue) {
            $('#ReceiverPanel #divPostalCode').parent().addClass('esg-has-error');
            $('#ReceiverPanel #divPostalCodeIcon').show();
            $('#ReceiverPanel #PostalCodeValidation').show();
        }
        else {
            $('#ReceiverPanel #divPostalCode').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divPostalCodeIcon').hide();
            $('#ReceiverPanel #PostalCodeValidation').hide();
        }
    })

    $('#ReceiverPanel #TaxRegimenId').focusout(function () {
        var TaxRegimenValue = $(this).val();

        if (!TaxRegimenValue) {
            $('#ReceiverPanel #divTaxRegimen').parent().addClass('esg-has-error');
            $('#ReceiverPanel #divTaxRegimenIcon').show();
            $('#ReceiverPanel #TaxRegimenValidation').show();
        }
        else {
            $('#ReceiverPanel #divTaxRegimen').parent().removeClass('esg-has-error');
            $('#ReceiverPanel #divTaxRegimenIcon').hide();
            $('#ReceiverPanel #TaxRegimenValidation').hide();
        }
    })

    $('#ReceiverPanel #TaxPayerId').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $('#ReceiverPanel #FiscalResidency').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $('#ReceiverPanel #CorporateName').keypress(function (e) {
        if (e.keyCode === 124 || e.key === '|') {
            return false;
        }
    });

    $('#ReceiverPanel #CorporateName').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $(".ui-autocomplete").css("z-index", "9999");
});

function ClearAllSaveReceiver() {
    $('#ReceiverPanel input[type=text]').each(function () {
        $(this).val('');
    });
    $('#ReceiverPanel #TaxRegimenId').val('');
    $('#ReceiverPanel #divTaxpayerId').parent().removeClass('esg-has-error');
    $('#ReceiverPanel #divTaxpayerIdIcon').hide();
    $('#ReceiverPanel #TaxPayerIdValidation').hide();
    $('#ReceiverPanel #divCorporateName').parent().removeClass('esg-has-error');
    $('#ReceiverPanel #divCorporateNameIcon').hide();
    $('#ReceiverPanel #CorporateNameValidation').hide();
    $('#ReceiverPanel #divPostalCode').parent().removeClass('esg-has-error');
    $('#ReceiverPanel #divPostalCodeIcon').hide()
    $('#ReceiverPanel #PostalCodeValidation').hide();
    $('#ReceiverPanel #PostalCodeInCatalogValidation').hide();
    $('#ReceiverPanel #divTaxRegimen').parent().removeClass('esg-has-error');
    $('#ReceiverPanel #divTaxRegimenIcon').hide();
    $('#ReceiverPanel #TaxRegimenValidation').hide();
    $('#ReceiverPanel #labelExist').hide();
    $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
    $('#ReceiverPanel #divFiscalResidencyIcon').hide();
    $('#ReceiverPanel #divFiscalIdentityNumber').parent().removeClass('esg-has-error');
    $('#ReceiverPanel #divFiscalIdentityNumberIcon').hide();
    $('#ReceiverPanel #FiscalResidency').prop('readonly', true);
    $('#ReceiverPanel #FiscalIdentityNumber').prop('readonly', true);
    $('#ReceiverPanel #TaxPayerIdValidation').hide();
    $('#ReceiverPanel #FiscalResidencyValidation').hide();
    $('#ReceiverPanel #FiscalIdentityNumberValidation').hide();
}

function GetTaxRegimenList(loadReceiverInfo) {
    var previousVal = $("#ReceiverPanel #TaxRegimenId").val();
    $.ajax({
        url: urlGetTaxRegimenCatalog,
        type: 'GET',
        cache: false,
        dataType: 'json',
        data: { id: $("#ReceiverPanel #TaxPayerId").val().length || 13 },
        success: function (data) {
            $('#ReceiverPanel #TaxRegimenId').empty();
            $.each(data, function (i, val) {
                $('#ReceiverPanel #TaxRegimenId').append($('<option></option>').val(val.Id == 0 ? '' : val.Id).html(val.Description));
            });
            if (previousVal && data.find(e => e.Id == previousVal)) {
                $('#ReceiverPanel #TaxRegimenId').val(previousVal);
            }
            if (loadReceiverInfo) {
                GetReceiverInformation();
            }
        }
    });
}

function GetReceiverInformation() {
    var TaxPayerIdValue = $("#ReceiverPanel #TaxPayerId").val();
    var FiscalIdentityNumberValue = $("#ReceiverPanel #FiscalIdentityNumber").val();
    $.ajax({
        url: urlGetReceiver,
        type: 'GET',
        cache: false,
        dataType: 'json',
        data: { id: TaxPayerIdValue },
        success: function (data) {
            if (data.length > 0) {
                var selected = $.grep(data, function (v, i) {
                    if (v.TaxPayerId === ForeignTaxpayerId)
                        return v.TaxPayerId + '-' + v.FiscalIdentityNumber == TaxPayerIdValue + '-' + FiscalIdentityNumberValue;
                    else
                        return v.TaxPayerId == TaxPayerIdValue;
                })[0];

                $('#ReceiverPanel #InvoiceTaxpayerId').val(selected.InvoiceTaxpayerId);
                $('#ReceiverPanel #InvoiceForeignTaxpayerId').val(selected.InvoiceForeignTaxpayerId);
                $("#ReceiverPanel #CorporateName").val(selected.CorporateName);
                $("#ReceiverPanel #PostalCode").val(selected.PostalCode);
                $("#ReceiverPanel #TaxRegimenId").val(selected.TaxRegimenId);
                $("#ReceiverPanel #FiscalResidency").val(selected.FiscalResidency);
                $("#ReceiverPanel #FiscalIdentityNumber").val(selected.FiscalIdentityNumber);
            }
        }
    });
}

function SetInitialStateReceiverPanel() {
    if ($('#ReceiverPanel #TaxPayerId').val()) {
        $("#CreateReceiverTitle").hide();
        $("#EditReceiverTitle").show();
        $('#ReceiverPanel #TaxPayerId').prop('readonly', true);
        if ($('#ReceiverPanel #TaxPayerId').val() === ForeignTaxpayerId) {
            $('#ReceiverPanel #FiscalResidency').prop('readonly', false);
            $('#ReceiverPanel #FiscalIdentityNumber').prop('readonly', false);
        }
        else {
            $('#ReceiverPanel #FiscalResidency').prop('readonly', true);
            $('#ReceiverPanel #FiscalIdentityNumber').prop('readonly', true);
        }

        $('#ReceiverPanel #TaxPayerId').unbind('focusout');
    }
    else {
        $('#ReceiverPanel #TaxPayerId').prop('readonly', false);
        $('#ReceiverPanel #FiscalResidency').prop('readonly', true);
        $('#ReceiverPanel #FiscalIdentityNumber').prop('readonly', true);
        $("#CreateReceiverTitle").show();
        $("#EditReceiverTitle").hide();

        $('#ReceiverPanel #TaxPayerId').focusout(function () {
            var TaxPayerIdValue = $(this).val();
            if (TaxPayerIdValue === ForeignTaxpayerId) {
                $('#ReceiverPanel #FiscalResidency').prop('readonly', false);
                $('#ReceiverPanel #FiscalIdentityNumber').prop('readonly', false);
                $.ajax({
                    url: urlGetReceiver,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    data: { id: TaxPayerIdValue },
                    success: function (data) {
                        if (data.length > 0) {
                            $('#ReceiverPanel #InvoiceTaxpayerId').val(data[0].InvoiceTaxpayerId);
                        }

                        $('#ReceiverPanel #divTaxpayerId').parent().removeClass('esg-has-error');
                        $('#ReceiverPanel #divTaxpayerIdIcon').hide();
                        $('#ReceiverPanel #TaxPayerIdValidation').hide();
                        $('#ReceiverPanel #labelExist').hide();

                        GetTaxRegimenList(false);
                    }
                });
                $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
                $('#ReceiverPanel #divFiscalResidencyIcon').hide();
                $('#ReceiverPanel #divFiscalIdentityNumber').parent().removeClass('esg-has-error');
                $('#ReceiverPanel #divFiscalIdentityNumberIcon').hide();
            }
            else {
                if (TaxPayerIdValue.length >= 12 && TaxPayerIdValue.length <= 13) {
                    var length = TaxPayerIdValue.length;
                    $('#ReceiverPanel #FiscalResidency').val('');
                    $('#ReceiverPanel #FiscalIdentityNumber').val('');
                    $('#ReceiverPanel #FiscalResidency').prop('readonly', true);
                    $('#ReceiverPanel #FiscalIdentityNumber').prop('readonly', true);
                    $('#ReceiverPanel #divTaxpayerId').parent().removeClass('esg-has-error');
                    $('#ReceiverPanel #divTaxpayerIdIcon').hide();
                    $('#ReceiverPanel #divFiscalResidency').parent().removeClass('esg-has-error');
                    $('#ReceiverPanel #divFiscalResidencyIcon').hide();
                    $('#ReceiverPanel #divFiscalIdentityNumber').parent().removeClass('esg-has-error');
                    $('#ReceiverPanel #divFiscalIdentityNumberIcon').hide();
                    $('#ReceiverPanel #FiscalResidencyValidation').hide();
                    $('#ReceiverPanel #FiscalIdentityNumberValidation').hide();
                    $.ajax({
                        url: urlGetReceiver,
                        type: 'GET',
                        cache: false,
                        dataType: 'json',
                        data: { id: TaxPayerIdValue },
                        success: function (data) {
                            if (data.length > 0) {
                                $('#ReceiverPanel #divTaxpayerId').parent().addClass('esg-has-error');
                                $('#ReceiverPanel #divTaxpayerIdIcon').show();
                                $('#ReceiverPanel #labelExist').show();
                                $('#ReceiverPanel #TaxPayerIdValidation').show();
                                $('#ReceiverPanel #TaxPayerId').val('');
                            }
                            else {
                                $('#ReceiverPanel #divTaxpayerId').parent().removeClass('esg-has-error');
                                $('#ReceiverPanel #divTaxpayerIdIcon').hide();
                                $('#ReceiverPanel #TaxPayerIdValidation').hide();
                                $('#ReceiverPanel #labelExist').hide();
                            }
                            GetTaxRegimenList(false);
                        }
                    })
                }
                else {
                    $('#ReceiverPanel #divTaxpayerId').parent().addClass('esg-has-error');
                    $('#ReceiverPanel #divTaxpayerIdIcon').show();
                    $('#ReceiverPanel #TaxPayerIdValidation').show();
                }
            }
        });
    }
}