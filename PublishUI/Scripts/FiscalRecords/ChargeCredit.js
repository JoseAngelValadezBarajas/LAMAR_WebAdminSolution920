$(document).ready(function () {

    $("#btnSearchChargeCredits").click(function () {
        var value = $("#ChargeCreditCode").val();
        if (value) {
            showPageLoader();
            $.ajax({
                url: urlGetChargeCredits,
                type: "GET",
                cache: false,
                dataType: "html",
                data: { id: value },
                success: function (data) {
                    hideFormError("ChargeCreditCode");
                    $("#msgErrorChargeCreditCode").hide();
                    $("#ChargeCreditCodeValidation").hide();
                    $("#ChargeCreditCode").prop('readonly', true);
                    $("#btnSearchChargeCredits").prop('disabled', true);
                    $("#ChargeCreditsDivResult").html(data);
                    hidePageLoader();
                }
            });
        }
        else {
            showFormError("ChargeCreditCode");
            $("#ChargeCreditCodeValidation").show();
        }
    });

    $("#ChargeCreditCode").focusout(function () {
        var chargeCreditCodeValue = $(this).val();

        if (chargeCreditCodeValue) {
            hideFormError("ChargeCreditCode");
            $("#msgErrorChargeCreditCode").hide();
            $("#ChargeCreditCodeValidation").hide();
        }
        else {
            showFormError("ChargeCreditCode");
            $("#ChargeCreditCodeValidation").show();
        }
    });

    $("#ProductServiceDesc").focusout(function () {
        var ProductServiceDescValue = $(this).val();
        if (ProductServiceDescValue) {
            hideFormError("ProductServiceDesc");
            $("#ProductServiceDescValidation").hide();
            if (!$("#ProductServiceDesc").is('[readonly]')) {
                $("#ProductServiceDesc").val('');
                showFormError("ProductServiceDesc");
                $("#ProductServiceDescLookup").hide();
                $("#ProductServiceDescValidation").show();
            }
        }
        else {
            showFormError("ProductServiceDesc");
            $("#ProductServiceDescLookup").hide();
            $("#ProductServiceDescValidation").show();
        }
    });

    $("#UnityDesc").focusout(function () {
        var UnityDescValue = $(this).val();
        if (UnityDescValue) {
            hideFormError("UnityDesc");
            $("#UnityDescValidation").hide();
            if (!$("#UnityDesc").is('[readonly]')) {
                $("#UnityDesc").val('');
                showFormError("UnityDesc");
                $("#UnityDescLookup").hide();
                $("#UnityDescValidation").show();
            }
        }
        else {
            showFormError("UnityDesc");
            $("#UnityDescLookup").hide();
            $("#UnityDescValidation").show();
        }
    });

    $('#ChargeCreditCode').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $('#ProductServiceDesc').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $('#UnityDesc').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $("#ProductServiceDesc").autocomplete({
        source: function (request, response) {
            var len = request.term.length;
            if (len >= 3) {
                $.ajax({
                    url: urlGetProductService,
                    type: "GET",
                    cache: false,
                    dataType: "json",
                    data: { id: request.term },
                    success: function (data) {
                        if (data.length > 0) {
                            response($.map(data, function (item) {
                                return { value: item.Code, label: item.Description }
                            }))
                        }
                        else {
                            $("#ProductServiceDesc").val('');
                        }
                    }
                })
            }
        },
        select: function (event, ui) {
            $("#ProductServiceKey").val(ui.item.value);
            event.preventDefault();
            $(this).val(ui.item.label);
            $(this).prop('readonly', true);
            hideFormError("ProductServiceDesc");
            $("#ProductServiceDescLookup").show();
            $("#ProductServiceDescValidation").hide();
        }
    });

    $("#UnityDesc").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetUnityKey,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    if (data.length > 0) {
                        response($.map(data, function (item) {
                            return { value: item.Code, label: item.Description }
                        }))
                    }
                    else {
                        $("#UnityDesc").val('');
                    }
                }
            })
        },
        select: function (event, ui) {
            $("#UnityKey").val(ui.item.value);
            event.preventDefault();
            $(this).val(ui.item.label);
            $(this).prop('readonly', true);
            hideFormError("UnityDesc");
            $("#UnityDescLookup").show();
            $("#UnityDescValidation").hide();
        }
    });

    $('#btnAdd').click(function () {
        showPageLoader();

        var isValid = true;

        // Validations
        var ChargeCreditCodeValue = $('#ChargeCreditCode').val();
        if (!ChargeCreditCodeValue) {
            showFormError("ChargeCreditCode");
            $("#ChargeCreditCodeValidation").show();
            isValid = false;
        }
        else if ($('#ChargeCreditsDivResult input[type=checkbox]:checked:not(#chkSelectAllTaxProfile)').length <= 0) {
            $("#msgErrorChargeCreditCode").show();
            isValid = false;
        }
        $("#ProductServiceDescLookup").hide();
        var ProductServiceDescValue = $('#ProductServiceDesc').val();
        if (!ProductServiceDescValue) {
            showFormError("ProductServiceDesc");
            $("#ProductServiceDescValidation").show();
            isValid = false;
        }
        $("#UnityDescLookup").hide();
        var UnityDescValue = $('#UnityDesc').val();
        if (!UnityDescValue) {
            showFormError("UnityDesc");
            $("#UnityDescValidation").show();
            isValid = false;
        }

        if (isValid) {
            // Data
            var specialTax = null;
            if ($('#TaxProfile').val()) {
                var specialCreditTaxText = $('#TaxProfile').val().split('-');
                specialTax = {
                    TaxCode: specialCreditTaxText[0].trim(),
                    TaxDescription: specialCreditTaxText[1].trim(),
                    FactorType: specialCreditTaxText[2].trim(),
                    TaxRate: specialCreditTaxText.length > 3 ? specialCreditTaxText[3].trim() : null,
                };
            }

            var chargeCreditCodesSelected = $('#ChargeCreditsDivResult input[type=checkbox]:checked:not(#chkSelectAllTaxProfile)');
            var formDataArray = [];
            for (var i = 0; i < chargeCreditCodesSelected.length; i++) {
                var splitSelectedId = chargeCreditCodesSelected[i].id.split('_');
                var formData = {
                    ChargeCreditCodeId: $(`#hdnChargeCreditCodeId_${splitSelectedId[1]}`).val(),
                    ProductServiceKey: $("#ProductServiceKey").val(),
                    UnityKey: $("#UnityKey").val()
                };
                if (specialTax && !$(`#hdnTaxProfile_${splitSelectedId[1]}`).val()) {
                    formData.TaxCode = specialTax.TaxCode;
                    formData.TaxDescription = specialTax.TaxDescription;
                    formData.FactorType = specialTax.FactorType;
                    formData.TaxRate = specialTax.TaxRate;
                }
                formDataArray.push(formData);
            }

            // Save
            $.ajax({
                url: urlCreate,
                dataType: "json",
                type: "POST",
                cache: false,
                data: { chargeCreditMappings: formDataArray },
                success: function (response) {
                    if (response) {
                        CloseAddNewMappingModal();
                        hidePageLoader();
                        window.location.href = urlIndex;
                    }
                    else {
                        CloseAddNewMappingModal();
                        hidePageLoader();
                        window.location.href = urlHome;
                    }
                }
            });
        }
        else {
            hidePageLoader();
            return false;
        }
    });

    $("#btnAddNewMapping").click(function () {
        $("#AddNewMappingModal").show();
        $('.AddNewMappingDiv').show();
        $("#ChargeCreditsDivResult").html('');
    });

    $('.btnCancel').click(CloseAddNewMappingModal);

    $('#btnClearAll').click(ClearAllAddNewMappingModal);

    $(".ui-autocomplete").css("z-index", "9999");
});

function CloseAddNewMappingModal() {
    $("#AddNewMappingModal").hide();
    $('.AddNewMappingDiv').hide();
    ClearAllAddNewMappingModal();
}

function ClearAllAddNewMappingModal() {
    $("#ProductServiceDescLookup").show();
    $("#UnityDescLookup").show();
    $('input[type=text]').each(function () {
        $(this).val('');
    });
    $("#TaxProfile").val('');
    $("#ChargeCreditsDivResult").html('');

    hideFormError("ChargeCreditCode");
    hideFormError("ProductServiceDesc");
    hideFormError("UnityDesc");

    $("#msgErrorChargeCreditCode").hide();
    $("#ChargeCreditCodeValidation").hide();
    $("#ProductServiceDescValidation").hide();
    $("#UnityDescValidation").hide();

    $("#ChargeCreditCode").prop('readonly', false);
    $("#btnSearchChargeCredits").prop('disabled', false);
    $("#ProductServiceDesc").prop('readonly', false);
    $("#UnityDesc").prop('readonly', false);
}

function onSelectAllTaxProfiles() {
    $("#msgErrorChargeCreditCode").hide();
    var selectedAllValue = $("#chkSelectAllTaxProfile").prop("checked");
    $('#ChargeCreditsDivResult input[type=checkbox]').each(function () {
        $(this).prop("checked", selectedAllValue);
    });
}

function onSelectTaxProfile() {
    $("#msgErrorChargeCreditCode").hide();
    var count = parseInt($("#hdnChargeCreditResultsCount").val());
    var selectedCount = $('#ChargeCreditsDivResult input[type=checkbox]:checked:not(#chkSelectAllTaxProfile)').length;
    $("#chkSelectAllTaxProfile").prop("checked", count > 0 && count === selectedCount);
}

function DeleteChargeCreditMapping(id) {
    showPageLoader();
    $.ajax({
        type: "POST",
        cache: false,
        url: urlDelete,
        data: { id: id },
        success: function (response) {
            hidePageLoader();
            if (response > 0) {
                window.location.href = urlIndex;
            }
            else {
                window.location.href = urlHome;
                return false;
            }
        }
    });
}