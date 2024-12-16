$(document).ready(function () {

    $('#btnClearAll').click(function () {
        $("#divChargeCreditCodeLookup").show();
        $("#divProductServiceDescLookup").show();
        $("#divUnityDescLookup").show();
        $('input[type=text]').each(function () {
            $(this).val('');
        });
        $("#divChargeCreditCode").parent().removeClass('esg-has-error');
        $("#divChargeCreditCodeIcon").hide();
        $("#divProductServiceDesc").parent().removeClass('esg-has-error');
        $("#divProductServiceDescIcon").hide();
        $("#divUnityDesc").parent().removeClass('esg-has-error');
        $("#divUnityDescIcon").hide();
        $("#ChargeCreditCode").prop('readonly', false);
        $("#ProductServiceDesc").prop('readonly', false);
        $("#UnityDesc").prop('readonly', false);
        $("#ChargeCreditCodeValidation").hide();
        $("#ProductServiceDescValidation").hide();
        $("#UnityDescValidation").hide();

        $("#PaymentMethodCode").val('');
        $("#PaymentMethodCode").parent().removeClass('has-error');
        $("#PaymentMethodCodeGroup").removeClass('esg-has-error');
        $("#PaymentMethodIdIcon").hide();
        $("#formCreate").clearValidation();
    });

    $('#btnAdd').click(function () {

        showPageLoader();
        if ($("#PaymentMethodId").val() === null) {
            $("#divChargeCreditCode").parent().addClass('esg-has-error');
            $("#PaymentMethodIdIcon").show();
            hidePageLoader();
            return false;
        }

        var $formCreate = $('#formCreate');
        if ($formCreate.valid() === false) {
            $("#PaymentMethodCode").parent().addClass('has-error');
            $("#PaymentMethodCodeGroup").addClass('esg-has-error');
            $("#PaymentMethodIdIcon").show();

            hidePageLoader();
            return false;
        }

        $.ajax({
            type: "POST",
            cache: false,
            url: urlReceiptPaymentCreate,
            data: $('#formCreate').serialize(),
            success: function (response) {
                if (response > 0) {
                    $('input[type=text]').each(function () {
                        $(this).val('');
                    });

                    hidePageLoader();
                    $('.AddNewMappingDiv').hide();
                    $("#ModalAdd").hide();
                    window.location.href = urlReceiptPaymentList;
                }
                else {
                    $('.AddNewMappingDiv').hide();
                    $("#ModalAdd").hide();
                    hidePageLoader();
                    window.location.href = urlHome;
                }
            }
        });
    });

    $("#AddNewMapping").click(function () {
        $("#ModalAdd").show();
        $('.AddNewMappingDiv').show();
    });

    $(".ui-autocomplete").css("z-index", "9999");

    $("#btnCancel, .btnCancel").click(function () {
        $('.AddNewMappingDiv').hide();
        $("#ModalAdd").hide();

        $("#PaymentMethodCode").val('');
        $("#PaymentMethodCode").parent().removeClass('has-error');
        $("#PaymentMethodCodeGroup").removeClass('esg-has-error');
        $("#PaymentMethodIdIcon").hide();
        $("#formCreate").clearValidation();
    });

    $('#PaymentMethodCode').change(function () {
        $("#PaymentMethodCode").parent().removeClass('has-error');
        $("#PaymentMethodCodeGroup").removeClass('esg-has-error');
        $("#PaymentMethodIdIcon").hide();
    });
});

function DeletePaymentTypeMapping(id) {
    showPageLoader();
    $.ajax({
        type: "POST",
        cache: false,
        url: urlReceiptPaymentDelete,
        data: { id: id },
        success: function (response) {
            hidePageLoader();
            if (response > 0) {
                window.location.href = urlReceiptPaymentList;
            }
            else {
                window.location.href = urlHome;
                return false;
            }
        }
    });
}