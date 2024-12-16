$(document).ready(function () {

    var ExpeditionId;
    var OrganizationId;
    var Option;

    $("#TaxpayerId").focusout(function () {
        var TaxpayerIdValue = $(this).val();
        if (TaxpayerIdValue.length >= 12 & TaxpayerIdValue.length <= 13) {
            $("#divTaxpayerId").parent().removeClass('esg-has-error');
            $("#divTaxpayerIdIcon").hide();
            $.ajax({
                url: urlGetTaxpayer,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: TaxpayerIdValue },
                success: function (data) {
                    if (data.length > 0) {
                        $("#divTaxpayerId").parent().addClass('esg-has-error');
                        $("#divTaxpayerIdIcon").show();
                        $("#labelExist").show();
                        $("#TaxPayerIdValidation").show();
                        $("#TaxpayerId").val('');
                    }
                    else {
                        $("#divTaxpayerId").parent().removeClass('esg-has-error');
                        $("#divTaxpayerIdIcon").hide();
                        $("#TaxPayerIdValidation").hide();
                        $("#labelExist").hide();
                        $("#labelExist").hide();
                        $.ajax({
                            url: urlGetTaxRegimenCatalog,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: TaxpayerIdValue.length },
                            success: function (data) {
                                $("#TaxRegimenId").empty();
                                $.each(data, function (i, val) {
                                    $($(document.getElementById("TaxRegimenId"))).append($('<option></option>').val(val.Id).html(val.Description));
                                });
                            }
                        })
                    }
                }
            })
        }
        else {
            $("#divTaxpayerId").parent().addClass('esg-has-error');
            $("#divTaxpayerIdIcon").show();
            $("#TaxPayerIdValidation").show();
        }
    });

    $('#TaxpayerId').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $("#PostalCode").change(function () {
        var PostalCodeValue = $(this).val();

        if (PostalCodeValue.length === 5 && PostalCodeValue !== "00000" && $.isNumeric(PostalCodeValue)) {
            $("#btnCreateIssuer").prop("disabled", true);
            $.ajax({
                url: urlGetPostalCode,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: PostalCodeValue },
                success: function (data) {
                    if (data.length > 0) {
                        $("#State").val(data[0].Description);
                        $("#divPostalCode").parent().removeClass('esg-has-error');
                        $("#divPostalCodeIcon").hide();
                        $("#PostalCodeValidation").hide();
                        $("#PostalCodeCatalogValidation").hide();
                        $("#PostalCode").prop('readonly', false);
                    }
                    else {
                        $("#State").val('');
                        $("#divPostalCode").parent().addClass('esg-has-error');
                        $("#divPostalCodeIcon").show();
                        $("#PostalCodeValidation").hide();
                        $("#PostalCodeCatalogValidation").show();
                        $("#TaxpayerIdValidation").hide();
                        $("#PostalCode").prop('readonly', false);
                    }
                    $("#btnCreateIssuer").prop("disabled", false);
                },
                error: function () {
                    $("#btnCreateIssuer").prop("disabled", false);
                }
            })
        }
        else {
            $("#PostalCode").val('');
            $("#divPostalCode").parent().addClass('esg-has-error');
            $("#divPostalCodeIcon").show();
            $("#PostalCodeValidation").show();
            $("#PostalCodeCatalogValidation").hide();
            $("#TaxpayerIdValidation").hide();
        }
    });

    $("#Description").focusout(function () {
        var Description = $(this);

        if (!Description.val()) {
            $("#divDescription").parent().addClass('esg-has-error');
            $("#divDescriptionIcon").show();
            $("#DescriptionValidation").show();
        }
        else {
            $("#divDescription").parent().removeClass('esg-has-error');
            $("#divDescriptionIcon").hide();
            $("#DescriptionValidation").hide();
        }
    });

    $("#AddPostalCode").focusout(function () {
        var PostalCodeAdd = $(this);

        if (!PostalCodeAdd.val()) {
            $("#divPostalCodeAdd").parent().addClass('esg-has-error');
            $("#divPostalCodeIconAdd").show();
            $("#PostalCodeValidationAdd").show();
        } else {
            $("#divPostalCodeAdd").parent().removeClass('esg-has-error');
            $("#divPostalCodeIconAdd").hide();
            $("#PostalCodeValidationAdd").hide();
        }
    });

    $("#EditPostalCode").focusout(function () {
        var PostalCodeEdit = $(this);

        if (!PostalCodeEdit.val()) {
            $("#divPostalCodeEdit").parent().addClass('esg-has-error');
            $("#divPostalCodeIconEdit").show();
            $("#PostalCodeValidationEdit").show();
        } else {
            $("#divPostalCodeEdit").parent().removeClass('esg-has-error');
            $("#divPostalCodeIconEdit").hide();
            $("#PostalCodeValidationEdit").hide();
        }
    });

    $("#PostalCode").focusout(function () {
        var PostalCode = $(this);

        if (!PostalCode.val()) {
            $("#divPostalCode").parent().addClass('esg-has-error');
            $("#divPostalCodeIcon").show();
            $("#PostalCodeValidation").show();
        } else {
            $("#divPostalCode").parent().removeClass('esg-has-error');
            $("#divPostalCodeIcon").hide();
            $("#PostalCodeValidation").hide();
        }
    });

    $("#AddDocFolio").focusout(function () {
        var AddDocFolio = $(this);

        if (!AddDocFolio.val()) {
            $("#divFolioAddDoc").parent().addClass('esg-has-error');
            $("#divFolioIconAddDoc").show();
            $("#FolioValidationAddDoc").show();
            $("#InvalidFolioValidationAddDoc").hide();
        }
        else if (AddDocFolio.val() <= 0) {
            $("#divFolioAddDoc").parent().addClass('esg-has-error');
            $("#divFolioIconAddDoc").show();
            $("#InvalidFolioValidationAddDoc").show();
            $("#FolioValidationAddDoc").hide();
        }
        else {
            $("#divFolioAddDoc").parent().removeClass('esg-has-error');
            $("#divFolioIconAddDoc").hide();
            $("#FolioValidationAddDoc").hide();
            $("#InvalidFolioValidation").hide();
        }
    });

    $("#AddDocSerial").focusout(function () {
        var AddDocSerialNumber = $(this);

        if (!AddDocSerialNumber.val()) {
            $("#divSerialNumberAddDoc").parent().addClass('esg-has-error');
            $("#divSerialNumberIconAddDoc").show();
            $("#SerialNumberValidationAddDoc").show();
        }
        else {
            $("#divSerialNumberAddDoc").parent().removeClass('esg-has-error');
            $("#divSerialNumberIconAddDoc").hide();
            $("#SerialNumberValidationAddDoc").hide();
        }
    });

    $("#SerialNumber").focusout(function () {
        var SerialNumber = $(this);

        if (!SerialNumber.val()) {
            $("#divSerialNumber").parent().addClass('esg-has-error');
            $("#divSerialNumberIcon").show();
            $("#SerialNumberValidation").show();
        }
        else {
            $("#divSerialNumber").parent().removeClass('esg-has-error');
            $("#divSerialNumberIcon").hide();
            $("#SerialNumberValidation").hide();
        }
    });

    $("#Folio").focusout(function () {
        var Folio = $(this);

        if (!Folio.val()) {
            $("#divFolio").parent().addClass('esg-has-error');
            $("#divFolioIcon").show();
            $("#FolioValidation").show();
            $("#InvalidFolioValidation").hide();
        }
        else if (Folio.val() <= 0) {
            $("#divFolio").parent().addClass('esg-has-error');
            $("#divFolioIcon").show();
            $("#FolioValidation").hide();
            $("#InvalidFolioValidation").show();
        }
        else {
            $("#divFolio").parent().removeClass('esg-has-error');
            $("#divFolioIcon").hide();
            $("#FolioValidation").hide();
        }
    });

    $("#TaxRegimenId").focusout(function () {
        var TaxRegimen = $(this);

        if (!TaxRegimen.val()) {
            $("#divTaxRegimenId").parent().addClass('esg-has-error');
            $("#divTaxRegimenIdIcon").show();
            $("#TaxRegimenValidation").show();
        } else {
            $("#divTaxRegimenId").parent().removeClass('esg-has-error');
            $("#divTaxRegimenIdIcon").hide();
            $("#TaxRegimenValidation").hide();
        }
    })

    $("#Folio").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $('#btnClearAll').click(function () {
        $('input[type=text]').each(function () {
            $(this).val('');
            $("#divTaxpayerId").parent().removeClass('esg-has-error');
            $("#divTaxpayerIdIcon").hide();
            $("#TaxPayerIdValidation").hide();
            $("#divCorporateName").parent().removeClass('esg-has-error');
            $("#divCorporateNameIcon").hide();
            $("#CorporateNameValidation").hide();
            $("#divTaxRegimenId").parent().removeClass('esg-has-error');
            $("#divTaxRegimenIdIcon").hide();
            $("#TaxRegimenValidation").hide();
            $("#divDescription").parent().removeClass('esg-has-error');
            $("#divDescriptionIcon").hide();
            $("#DescriptionValidation").hide();
            $("#divPostalCode").parent().removeClass('esg-has-error');
            $("#divPostalCodeIcon").hide()
            $("#PostalCodeValidation").hide();
            $("#PostalCodeCatalogValidation").hide();
            $("#PostalCode").prop('readonly', false);
            $("#labelExist").hide();
        });
    });

    $('#btnClearAllDocument').click(function () {
        $('input[type=text]').each(function () {
            $(this).val('');
        });
        $("#Folio").val('');
        $("#divSerialNumber").parent().removeClass('esg-has-error');
        $("#divSerialNumberIcon").hide();
        $("#SerialNumberValidation").hide();
        $("#divFolio").parent().removeClass('esg-has-error');
        $("#divFolioIcon").hide();
        $("#FolioValidation").hide();
        $("#InvalidFolioValidation").hide();
    });

    $('#btnIssuerDocument').click(function () {

        $('.errorMessageDivFolio').css('display', 'none');
        $('.errorMessageDivSerial').css('display', 'none');
        $('.errorMessageResultFolio').css('display', 'none');
        $('.errorMessageResultSerial').css('display', 'none');
        $('.errorMessageDivFolioBig').css('display', 'none');
        $('.errorMessageResultFolioBig').css('display', 'none');

        if ($("#Folio").val() < 2147483648) {
            var $formCreate = $('#formCreate');

            var SerialNumber = $("#SerialNumber").val();
            if (!SerialNumber) {
                $("#divSerialNumber").parent().addClass('esg-has-error');
                $("#divSerialNumberIcon").show();
                $("#SerialNumberValidation").show();
                if (!$("#Folio").val()) {
                    $("#divFolio").parent().addClass('esg-has-error');
                    $("#divFolioIcon").show();
                    $("#FolioValidation").show();
                }
                return false;
            }
            if (!$("#Folio").val()) {
                $("#divFolio").parent().addClass('esg-has-error');
                $("#divFolioIcon").show();
                $("#FolioValidation").show();
            }
            else if ($("#Folio").val() <= 0) {
                $("#divFolio").parent().addClass('esg-has-error');
                $("#divFolioIcon").show();
                $("#InvalidFolioValidation").show();
            }
            if ($formCreate.valid() === false) {
                return false;
            }

            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var InvoiceExpeditionId = $("#AddInvoiceExpeditionId option:selected").val();

            if (InvoiceExpeditionId > 0) {
                var IssuerDocument = {
                    InvoiceExpeditionId: InvoiceExpeditionId,
                    InvoiceOrganizationId: $('#InvoiceOrganizationId').val(),
                    SerialNumber: SerialNumber,
                    Folio: $("#Folio").val()
                };

                $formCreate = JSON.stringify(IssuerDocument);

            }
            else {
                var IssuerDocument = {
                    InvoiceExpeditionId: null,
                    InvoiceOrganizationId: $('#InvoiceOrganizationId').val(),
                    SerialNumber: SerialNumber,
                    Folio: $("#Folio").val()
                };

                $formCreate = JSON.stringify(IssuerDocument);
            }

            $.ajax({
                type: "POST",
                cache: false,
                url: urlAddIssuerDocument,
                data: { invoiceReceiptViewModel: $formCreate },
                success: function (response) {
                    if (response > 0) {
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlEditIssuer + "/?id=" + response;
                    }
                    else {
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');

                        if (response == -2) //Folio menor
                        {
                            $('.errorMessageDivFolio').css('display', 'block');
                            $('.errorMessageResultFolio').css('display', 'block');
                        }
                        else if (response == -3) //Serial y folio
                        {
                            $('.errorMessageDivSerial').css('display', 'block');
                            $('.errorMessageResultSerial').css('display', 'block');
                        }
                    }
                }
            });
        }
        else {
            $('.errorMessageDivFolioBig').css('display', 'block');
            $('.errorMessageResultFolioBig').css('display', 'block');
        }
    });

    $("#CorporateName").focusout(function () {
        var CorporateName = $(this).val();

        if (CorporateName && CorporateName === CorporateName.toUpperCase() && !CorporateName.includes('|')) {
            $("#divCorporateName").parent().removeClass('esg-has-error');
            $("#divCorporateNameIcon").hide();
            $("#CorporateNameValidation").hide();
        }
        else {
            $("#divCorporateName").parent().addClass('esg-has-error');
            $("#divCorporateNameIcon").show();
            $("#CorporateNameValidation").show();
        }
    });

    $('#CorporateName').keyup(function () {
        var text = $(this).val();
        $(this).val(text.toUpperCase());
    });

    $("#CorporateName").keypress(function (e) {
        if (e.keyCode === 124 || e.key === "|") {
            return false;
        }
    });

    $("#Description").keypress(function (e) {
        if (e.keyCode === 124 || e.key === "|") {
            return false;
        }
    });

    $('#btnCreateIssuer').click(function () {
        var PostalCodeValue = $("#PostalCode").val();

        if (!PostalCodeValue) {
            $("#PostalCodeValidation").show();
        }

        $("#PostalCodeCatalogValidation").hide();
        var $formCreate = $('#formCreate');
        $formCreate.validate();
        if ($formCreate.valid() === false) {
            var TaxPayerIdValue = $("#TaxPayerId").val();
            var CorporateNameValue = $("#CorporateName").val();
            var TaxRegimenValue = $("#TaxRegimen").val();
            var DescriptionValue = $("#Description").val();
            var PostalCodeValue = $("#PostalCode").val();

            if (!TaxPayerIdValue) {
                $("#divTaxpayerId").parent().addClass('esg-has-error');
                $("#divTaxpayerIdIcon").show();
                $("#TaxPayerIdValidation").show();
            }
            if (!CorporateNameValue) {
                $("#divCorporateName").parent().addClass('esg-has-error');
                $("#divCorporateNameIcon").show();
                $("#CorporateNameValidation").show();
            }
            if (!TaxRegimenValue) {
                $("#divTaxRegimenId").parent().addClass('esg-has-error');
                $("#divTaxRegimenIdIcon").show()
                $("#TaxRegimenValidation").show();
            }
            if (!DescriptionValue) {
                $("#divDescription").parent().addClass('esg-has-error');
                $("#divDescriptionIcon").show();
                $("#DescriptionValidation").show();
            }
            if (!PostalCodeValue) {
                $("#divPostalCode").parent().addClass('esg-has-error');
                $("#divPostalCodeIcon").show()
                $("#PostalCodeValidation").show();
            }

            return false;
        } else {
            $("#divTaxpayerId").parent().removeClass('esg-has-error');
            $("#divTaxpayerIdIcon").hide();
            $("#TaxPayerIdValidation").hide();
            $("#divCorporateName").parent().removeClass('esg-has-error');
            $("#divCorporateNameIcon").hide();
            $("#CorporateNameValidation").hide();
            $("#divDescription").parent().removeClass('esg-has-error');
            $("#divDescriptionIcon").hide();
            $("#DescriptionValidation").hide();
            $("#divPostalCode").parent().removeClass('esg-has-error');
            $("#divPostalCodeIcon").hide()
            $("#PostalCodeValidation").hide();
        }
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        $.ajax({
            type: "POST",
            cache: false,
            url: urlCreateIssuer,
            data: $('#formCreate').serialize(),
            success: function (response) {
                if (response > 0) {
                    window.location.href = urlListIssuer;
                }
            }
        });
    });

    $('#btnUpdateIssuer').click(function () {
        var $formCreate = $('#formCreate');
        if ($formCreate.valid() === false) {
            var CorporateNameValue = $("#CorporateName").val();
            var TaxRegimenValue = $('#TaxRegimenId').val()

            if (!CorporateNameValue) {
                $("#divCorporateName").parent().addClass('esg-has-error');
                $("#divCorporateNameIcon").show();
                $("#CorporateNameValidation").show();
            }

            if (!TaxRegimenValue) {
                $("#divTaxRegimenId").parent().addClass('esg-has-error');
                $("#divTaxRegimenIdIcon").show();
                $("#TaxRegimenValidation").show();
            }

            return false;
        } else {
            $("#divCorporateName").parent().removeClass('esg-has-error');
            $("#divCorporateNameIcon").hide();
            $("#CorporateNameValidation").hide();
        }
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        $.ajax({
            type: "POST",
            cache: false,
            url: urlEditIssuer,
            data: $('#formCreate').serialize(),
            success: function (response) {
                if (response > 0) {
                    window.location.href = urlListIssuer;
                }
            }
        });
    });

    $('.btnEdit').click(function () {

        $('#EditIssuerAddress').css('display', 'block');
        $('#ModalAdd').css('display', 'block');

        $("#divPostalCodeEdit").parent().removeClass('esg-has-error');
        $("#divPostalCodeIconEdit").hide();
        $("#PostalCodeValidationEdit").hide();
        $("#PostalCodeCatalogValidationEdit").hide();

        $("#divDescriptionEdit").parent().removeClass('esg-has-error');
        $("#divDescriptionIconEdit").hide();
        $("#DescriptionValidationEdit").hide();

        OrganizationId = $(this).attr('id');
        ExpeditionId = $(this).attr('pkid');
        var Desc = $(this).attr('desc');
        var PostalCode = $(this).attr('postalCode');
        var State = $(this).attr('state');

        $('#EditDescription').val(Desc);
        $('#EditPostalCode').val(PostalCode);
        $('#State').val(State);

    });

    $('.btnDelete').click(function () {

        $('#DeleteIssuerAddress').css('display', 'block');
        $('#ModalAdd').css('display', 'block');

        OrganizationId = $(this).attr('id');
        ExpeditionId = $(this).attr('pkid');
        var Desc = $(this).attr('desc');
        var PostalCode = $(this).attr('postalCode');
        var State = $(this).attr('state');

        $('#IdDescriptionDD').text(Desc);
        $('#IdPostalCodeDD').text(PostalCode);
        $('#IdStateDD').text(State);

    });

    $('.btnCancel').click(function () {
        $('#ModalAdd').css('display', 'none');
        $('#DeleteIssuerAddress').css('display', 'none');
        $('#EditIssuerAddress').css('display', 'none');
        $('#AddIssuerAddress').css('display', 'none');
        $('#AddIssuerDocument').css('display', 'none');
    });

    $('#btnCancelDel').click(function () {
        $('#ModalAdd').css('display', 'none');
        $('#DeleteIssuerAddress').css('display', 'none');
    });

    $('#btnDelete').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');

        $.ajax({
            type: "POST",
            cache: false,
            url: urlDeleteIssuerAddress,
            data: { pkid: ExpeditionId },
            success: function (response) {
                if (response == 1) {
                    window.location.href = urlEditIssuer;
                    $("#Overlaydiv").css('display', 'none');
                    $('#DeleteIssuerAddress').css('display', 'none');
                }
                else {
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    $('#ModalAdd').css('display', 'none');
                    $('#DeleteIssuerAddress').css('display', 'none');
                }
            }
        });

    });

    $('#btnCancelUpd').click(function () {
        $('#ModalAdd').css('display', 'none');
        $('#EditIssuerAddress').css('display', 'none');
    });

    $('#btnClearAllUpd').click(function () {

        $("#divDescriptionEdit").parent().removeClass('esg-has-error');
        $("#divDescriptionIconEdit").hide();
        $("#DescriptionValidationEdit").hide();
        $("#divPostalCodeEdit").parent().removeClass('esg-has-error');
        $("#divPostalCodeIconEdit").hide();
        $("#PostalCodeValidationEdit").hide();
        $("#EditPostalCode").prop('readonly', false);
        $("#PostalCodeValidationEdit").hide();
        $("#PostalCodeCatalogValidationEdit").hide();
        $('#EditDescription').val('');
        $('#EditPostalCode').val('');
        $('#State').val('');
    });

    $("#EditPostalCode").change(function () {
        var PostalCodeValue = $(this).val();

        if (PostalCodeValue.length === 5 && PostalCodeValue !== "00000" && $.isNumeric(PostalCodeValue)) {
            $("#btnUpdate").prop("disabled", true);
            $.ajax({
                url: urlGetPostalCode,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: PostalCodeValue },
                success: function (data) {
                    if (data.length > 0) {
                        $("#State").val(data[0].Description);
                        $("#divPostalCodeEdit").parent().removeClass('esg-has-error');
                        $("#divPostalCodeIconEdit").hide();
                        $("#PostalCodeValidationEdit").hide();
                        $("#PostalCodeCatalogValidationEdit").hide();
                        $("#EditPostalCode").prop('readonly', false);
                    }
                    else {
                        $("#State").val('');
                        $("#divPostalCodeEdit").parent().addClass('esg-has-error');
                        $("#divPostalCodeIconEdit").show();
                        $("#PostalCodeValidationEdit").hide();
                        $("#PostalCodeCatalogValidationEdit").show();
                        $("#EditPostalCode").prop('readonly', false);
                    }
                    $("#btnUpdate").prop("disabled", false);
                },
                error: function () {
                    $("#btnUpdate").prop("disabled", false);
                }
            })
        }
        else {
            $("#EditPostalCode").val('');
            $("#divPostalCodeEdit").parent().addClass('esg-has-error');
            $("#divPostalCodeIconEdit").show();
            $("#PostalCodeValidationEdit").show();
            $("#PostalCodeCatalogValidationEdit").hide();
        }
    });

    $("#EditDescription").focusout(function () {
        var Description = $(this);

        if (!Description.val()) {
            $("#divDescriptionEdit").parent().addClass('esg-has-error');
            $("#divDescriptionIconEdit").show();
            $("#DescriptionValidationEdit").show();
        }
        else {
            $("#divDescriptionEdit").parent().removeClass('esg-has-error');
            $("#divDescriptionIconEdit").hide();
            $("#DescriptionValidationEdit").hide();
        }
    });

    $("#EditDescription").keypress(function (e) {
        if (e.keyCode === 124 || e.key === "|") {
            return false;
        }
    });

    $('#btnUpdate').click(function () {

        var PostalCodeValue = $("#EditPostalCode").val();

        if (PostalCodeValue.length < 5) {
            $("#EditPostalCode").val('');
            $("#divPostalCodeEdit").parent().addClass('esg-has-error');
            $("#divPostalCodeIconEdit").show();
            $("#PostalCodeValidationEdit").show();
            $("#PostalCodeCatalogValidationEdit").hide();

            if (!$("#EditDescription").val()) {
                $("#divDescriptionEdit").parent().addClass('esg-has-error');
                $("#divDescriptionIconEdit").show();
                $("#DescriptionValidationEdit").show();
            }
            return false;
        }

        if (!$("#EditDescription").val()) {
            $("#divDescriptionEdit").parent().addClass('esg-has-error');
            $("#divDescriptionIconEdit").show();
            $("#DescriptionValidationEdit").show();
            return false;
        }

        var ModelUpdate = {
            InvoiceExpeditionId: ExpeditionId,
            InvoiceOrganizationId: OrganizationId,
            Description: $("#EditDescription").val(),
            PostalCode: $("#EditPostalCode").val(),
            State: $('#State').val(),
            CanDelete: 0
        };

        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');

        $.ajax({
            type: "POST",
            cache: false,
            dataType: "json",
            url: urlEditIssuerAddress,
            data: { Model: JSON.stringify(ModelUpdate) },
            success: function (response) {
                if (response > 0) {
                    $("#Overlaydiv").css('display', 'none');
                    $('#EditIssuerAddress').css('display', 'none');
                    window.location.href = urlEditIssuer;
                }
                else {
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    $('#ModalAdd').css('display', 'none');
                    $('#EditIssuerAddress').css('display', 'none');
                    window.location.href = urlHome;
                }
            },
            error: function () {
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                $('#ModalAdd').css('display', 'none');
                $('#EditIssuerAddress').css('display', 'none');
            }
        });
    });

    $('#AddNewIssuerAddress').click(function () {

        $("#divPostalCodeAdd").parent().removeClass('esg-has-error');
        $("#divPostalCodeIconAdd").hide();
        $("#PostalCodeValidationAdd").hide();
        $("#PostalCodeCatalogValidationAdd").hide();

        $("#divDescriptionAdd").parent().removeClass('esg-has-error');
        $("#divDescriptionIconAdd").hide();
        $("#DescriptionValidationAdd").hide();

        $('#ModalAdd').css('display', 'block');
        $('#AddIssuerAddress').css('display', 'block');

        OrganizationId = $('.buttonLabel').attr('id');

    });

    $('#btnCancelAdd').click(function () {
        $('#ModalAdd').css('display', 'none');
        $('#AddIssuerAddress').css('display', 'none');
        $('#AddDescription').val('');
        $('#AddPostalCode').val('');
        $('#AddState').val('');

        $("#divDescriptionAdd").parent().removeClass('esg-has-error');
        $("#divDescriptionIconAdd").hide();
        $("#DescriptionValidationAdd").hide();
        $("#divPostalCodeAdd").parent().removeClass('esg-has-error');
        $("#divPostalCodeIconAdd").hide();
        $("#PostalCodeValidationAdd").hide();
        $("#AddPostalCode").prop('readonly', false);
    });

    $('#btnClearAllAddress').click(function () {

        $('#AddDescription').val('');
        $('#AddPostalCode').val('');
        $('#AddState').val('');

        $("#divDescriptionAdd").parent().removeClass('esg-has-error');
        $("#divDescriptionIconAdd").hide();
        $("#DescriptionValidationAdd").hide();
        $("#divPostalCodeAdd").parent().removeClass('esg-has-error');
        $("#divPostalCodeIconAdd").hide();
        $("#PostalCodeCatalogValidationAdd").hide();
        $("#PostalCodeValidationAdd").hide();
        $("#AddPostalCode").prop('readonly', false);
    });

    $('#AddPostalCode').change(function () {
        var PostalCodeValue = $(this).val();

        if (PostalCodeValue.length === 5 && PostalCodeValue !== "00000" && $.isNumeric(PostalCodeValue)) {
            $("#btnCreateIssuerAddress").prop("disabled", true);
            $.ajax({
                url: urlGetPostalCode,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: PostalCodeValue },
                success: function (data) {
                    if (data.length > 0) {
                        $("#AddState").val(data[0].Description);
                        $("#divPostalCodeAdd").parent().removeClass('esg-has-error');
                        $("#divPostalCodeIconAdd").hide();
                        $("#PostalCodeValidationAdd").hide();
                        $("#PostalCodeCatalogValidationAdd").hide();
                        $("#AddPostalCode").prop('readonly', false);
                    }
                    else {
                        $("#AddState").val('');
                        $("#divPostalCodeAdd").parent().addClass('esg-has-error');
                        $("#divPostalCodeIconAdd").show();
                        $("#PostalCodeValidationAdd").hide();
                        $("#PostalCodeCatalogValidationAdd").show();
                        $("#AddPostalCode").prop('readonly', false);
                    }
                    $("#btnCreateIssuerAddress").prop("disabled", false);
                },
                error: function () {
                    $("#btnCreateIssuerAddress").prop("disabled", false);
                }
            })
        }
        else {
            $("#AddPostalCode").val('');
            $("#divPostalCodeAdd").parent().addClass('esg-has-error');
            $("#divPostalCodeIconAdd").show();
            $("#PostalCodeValidationAdd").show();
            $("#PostalCodeCatalogValidationAdd").hide();
        }
    });

    $("#AddDescription").keypress(function (e) {
        if (e.keyCode === 124 || e.key === "|") {
            return false;
        }
    });

    $('#AddDescription').focusout(function () {

        var Description = $(this);

        if (!Description.val()) {
            $("#divDescriptionAdd").parent().addClass('esg-has-error');
            $("#divDescriptionIconAdd").show();
            $("#DescriptionValidationAdd").show();
        }
        else {
            $("#divDescriptionAdd").parent().removeClass('esg-has-error');
            $("#divDescriptionIconAdd").hide();
            $("#DescriptionValidationAdd").hide();
        }
    });

    $('#btnCreateIssuerAddress').click(function () {

        var PostalCodeValue = $("#AddPostalCode").val();

        if (PostalCodeValue.length < 5) {
            $("#AddPostalCode").val('');
            $("#divPostalCodeAdd").parent().addClass('esg-has-error');
            $("#divPostalCodeIconAdd").show();
            $("#PostalCodeValidationAdd").show();
            $("#PostalCodeCatalogValidationAdd").hide();

            if (!$("#AddDescription").val()) {
                $("#divDescriptionAdd").parent().addClass('esg-has-error');
                $("#divDescriptionIconAdd").show();
                $("#DescriptionValidationAdd").show();
            }

            return false;
        }
        if (!$("#AddDescription").val()) {
            $("#divDescriptionAdd").parent().addClass('esg-has-error');
            $("#divDescriptionIconAdd").show();
            $("#DescriptionValidationAdd").show();

            return false;
        }

        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');

        var ModelAdd = {
            InvoiceExpeditionId: ExpeditionId,
            InvoiceOrganizationId: OrganizationId,
            Description: $("#AddDescription").val(),
            PostalCode: $("#AddPostalCode").val(),
            State: $('#AddState').val(),
            CanDelete: 0
        };

        $.ajax({
            type: "POST",
            cache: false,
            url: urlAddIssuerAddress,
            data: { Model: JSON.stringify(ModelAdd) },
            success: function (response) {
                if (response > 0) {
                    $("#Overlaydiv").css('display', 'none');
                    $('#AddIssuerAddress').css('display', 'none');
                    window.location.href = urlEditIssuer;
                }
                else {
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    $('#ModalAdd').css('display', 'none');
                    $('#AddIssuerAddress').css('display', 'none');
                    window.location.href = urlEditIssuer;
                }
            }
        });
    });

    $('#AddNewIssuerDocument').click(function () {

        var InvoiceExpId = $('#AddInvoiceExpeditionId').val();

        $("#Processing").css('display', 'block');
        $('#ModalAdd').css('display', 'block');

        $('#AddDocSerial').val('');
        $('#AddDocFolio').val('');

        OrganizationId = $('.buttonIssuerDocument').attr('id');
        Option = $('.buttonIssuerDocument').attr('option');

        if (Option === "ByExpedition") {

            if (InvoiceExpId === null) {
                $.ajax({
                    type: "GET",
                    cache: false,
                    url: urlAddIssuerDocument,
                    data: { id: OrganizationId, Option: Option },
                    success: function (response) {
                        $('#AddInvoiceExpeditionId').empty();

                        $.each(response.ListInvoiceExpedition, function (i, val) {
                            $("#AddInvoiceExpeditionId").append($('<option></option>').val(val.InvoiceExpeditionId).html(val.Description));
                        });

                        $('#divByExpedition').css('display', 'block');
                        $('#AddIssuerDocument').css('display', 'block');
                        $("#Processing").css('display', 'none');

                    }
                });
            }
            else {
                $('#divByExpedition').css('display', 'block');
                $('#AddIssuerDocument').css('display', 'block');
                $("#Processing").css('display', 'none');
            }
        }
        else {
            $('#divByExpedition').css('display', 'none');
            $('#AddIssuerDocument').css('display', 'block');
            $("#Processing").css('display', 'none');
        }

    });

    $('#btnCancelAddDoc').click(function () {
        $("#divSerialNumberAddDoc").parent().removeClass('esg-has-error');
        $("#divSerialNumberIconAddDoc").hide();
        $("#SerialNumberValidationAddDoc").hide();
        $("#divFolioAddDoc").parent().removeClass('esg-has-error');
        $("#divFolioIconAddDoc").hide();
        $("#FolioValidationAddDoc").hide();
        $("#InvalidFolioValidationAddDoc").hide();
        $('.errorMessageDivFolio').css('display', 'none');
        $('.errorMessageDivSerial').css('display', 'none');
        $('.errorMessageResultFolio').css('display', 'none');
        $('.errorMessageResultSerial').css('display', 'none');
        $('.errorMessageDivFolioBig').css('display', 'none');
        $('.errorMessageResultFolioBig').css('display', 'none');
        $('#AddIssuerDocument').css('display', 'none');
        $('#ModalAdd').css('display', 'none');
        $('#AddDocSerial').val('');
        $('#AddDocFolio').val('');
    });

    $('#btnClearAddDoc').click(function () {
        $("#divSerialNumberAddDoc").parent().removeClass('esg-has-error');
        $("#divSerialNumberIconAddDoc").hide();
        $("#SerialNumberValidationAddDoc").hide();
        $("#divFolioAddDoc").parent().removeClass('esg-has-error');
        $("#divFolioIconAddDoc").hide();
        $("#FolioValidationAddDoc").hide();
        $("#InvalidFolioValidationAddDoc").hide();
        $('.errorMessageDivFolio').css('display', 'none');
        $('.errorMessageDivSerial').css('display', 'none');
        $('.errorMessageResultFolio').css('display', 'none');
        $('.errorMessageResultSerial').css('display', 'none');
        $('.errorMessageDivFolioBig').css('display', 'none');
        $('.errorMessageResultFolioBig').css('display', 'none');
        $('#AddDocSerial').val('');
        $('#AddDocFolio').val('');
    });

    $('#btnCreateAddDoc').click(function () {

        $("#divSerialNumberAddDoc").parent().removeClass('esg-has-error');
        $("#divSerialNumberIconAddDoc").hide();
        $("#SerialNumberValidationAddDoc").hide();
        $("#divFolioAddDoc").parent().removeClass('esg-has-error');
        $("#divFolioIconAddDoc").hide();
        $("#FolioValidationAddDoc").hide();
        $('.errorMessageDivFolio').css('display', 'none');
        $('.errorMessageDivSerial').css('display', 'none');
        $('.errorMessageResultFolio').css('display', 'none');
        $('.errorMessageResultSerial').css('display', 'none');
        $('.errorMessageDivFolioBig').css('display', 'none');
        $('.errorMessageResultFolioBig').css('display', 'none');

        if ($("#AddDocFolio").val() > 0 && $("#AddDocFolio").val() < 2147483648) {

            var SerialNumber = $("#AddDocSerial").val();
            if (!SerialNumber) {

                $("#divSerialNumberAddDoc").parent().addClass('esg-has-error');
                $("#divSerialNumberIconAddDoc").show();
                $("#SerialNumberValidationAddDoc").show();
                if (!$("#AddDocFolio").val()) {
                    $("#divFolioAddDoc").parent().addClass('esg-has-error');
                    $("#divFolioIconAddDoc").show();
                    $("#FolioValidationAddDoc").show();
                }
                return false;
            }
            if (!$("#AddDocFolio").val()) {

                $("#divFolioAddDoc").parent().addClass('esg-has-error');
                $("#divFolioIconAddDoc").show();
                $("#FolioValidationAddDoc").show();
            }


            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var InvoiceExpeditionId = $("#AddInvoiceExpeditionId option:selected").val();

            if (InvoiceExpeditionId > 0) {
                var IssuerDocument = {
                    InvoiceExpeditionId: InvoiceExpeditionId,
                    InvoiceOrganizationId: OrganizationId,
                    SerialNumber: SerialNumber,
                    Folio: $("#AddDocFolio").val()
                };

            }
            else {
                var IssuerDocument = {
                    InvoiceExpeditionId: null,
                    InvoiceOrganizationId: OrganizationId,
                    SerialNumber: SerialNumber,
                    Folio: $("#AddDocFolio").val()
                };
            }

            $.ajax({
                type: "POST",
                cache: false,
                url: urlAddIssuerDocument,
                data: { invoiceReceiptViewModel: JSON.stringify(IssuerDocument) },
                success: function (response) {
                    if (response > 0) {
                        $('#AddIssuerDocument').css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');

                        window.location.href = urlEditIssuer;
                    }
                    else {
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');

                        if (response == -2) //Folio menor
                        {
                            $('.errorMessageDivFolio').css('display', 'block');
                            $('.errorMessageResultFolio').css('display', 'block');
                        }
                        else if (response == -3) //Serial y folio
                        {
                            $('.errorMessageDivSerial').css('display', 'block');
                            $('.errorMessageResultSerial').css('display', 'block');
                        }
                    }
                }
            });
        }
        else {
            $("#divFolioAddDoc").parent().addClass('esg-has-error');
            $("#divFolioIconAddDoc").show();
            $("#InvalidFolioValidationAddDoc").show();
            $("#FolioValidationAddDoc").hide();
        }

    })

    $('.btnAddIssuerDocByAdd').click(function () {

        $("#Processing").css('display', 'block');
        $('#ModalAdd').css('display', 'block');

        $('#AddDocSerial').val('');
        $('#AddDocFolio').val('');

        OrganizationId = $('.btnAddIssuerDocByAdd').attr('id');
        Option = $('.btnAddIssuerDocByAdd').attr('option');

        var InvoiceExpId = $('#AddInvoiceExpeditionId').val();

        if (Option === "ByExpedition") {

            if (InvoiceExpId === null) {

                $.ajax({
                    type: "GET",
                    url: urlAddIssuerDocument,
                    cache: false,
                    data: { id: OrganizationId, Option: Option },
                    success: function (response) {
                        $('#AddInvoiceExpeditionId').empty();

                        $.each(response.ListInvoiceExpedition, function (i, val) {
                            $("#AddInvoiceExpeditionId").append($('<option></option>').val(val.InvoiceExpeditionId).html(val.Description));
                        });

                        $('#divByExpedition').css('display', 'block');
                        $('#AddIssuerDocument').css('display', 'block');
                        $("#Processing").css('display', 'none');

                    }
                });
            }
            else {
                $('#divByExpedition').css('display', 'block');
                $('#AddIssuerDocument').css('display', 'block');
                $("#Processing").css('display', 'none');
            }
        }
        else {
            $('#AddIssuerDocument').css('display', 'block');
            $("#Processing").css('display', 'none');
        }


    });

    $('.btnAddIssuerDocByTaxPay').click(function () {

        OrganizationId = $('.btnAddIssuerDocByTaxPay').attr('id');
        Option = $('.btnAddIssuerDocByTaxPay').attr('option');

        $('#ModalAdd').css('display', 'block');
        $('#AddDocSerial').val('');
        $('#AddDocFolio').val('');
        $('#divByExpedition').css('display', 'none');
        $('#AddIssuerDocument').css('display', 'block');
        $("#Processing").css('display', 'none');

    });

    $('#AddDocFolio').keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

});
