$(document).ready(function () {
    $('#btnAddSigner').click(function () {
        window.location.href = urlAddSigner;
    });

    var createForm = $('#formCreate');
    var IsValidCurp = false;
    var IsValidThumprint = false;
    var editThumprint = false;

    $('#btnAdd').click(function () {
        createForm.validate();
        if (createForm.valid() && IsValidCurp && IsValidThumprint) {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var Model = {
                AbreviationTitleId: $('#ProfessionalTitleAbbreviation').val(),
                Curp: $('#Curp').val(),
                FirstSurname: $('#FirstSurname').val(),
                IsActive: $('#swtActive:checkbox:checked').length,
                Name: $('#Name').val(),
                SecondSurname: $('#SecondSurname').val(),
                SignerPositionId: $('#LaborPosition').val(),
                Thumbprint: $('#Thumbprint').val(),
                UserName: userName
            };

            $.ajax({
                url: urlCreateSigner,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { Model: JSON.stringify(Model) },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlSignerIndex;
                    }
                    else if (response.id === 0) {
                        window.location.href = urlUnauthorized;
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                    else {
                        $("#divError").css('display', 'block');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                }
            });
        }
        else {
            if ($('#Curp').val() === "" || !IsValidCurp) {
                $('#Curp').parent().addClass('has-error');
                $("#divCurp").addClass('esg-has-error');
                $("#divCurpErr").css('display', 'block');
            }
            else {
                $('#Curp').parent().removeClass('has-error');
                $("#divCurp").removeClass('esg-has-error');
                $("#divCurpErr").css('display', 'none');
                $("#CurpExists").css('display', 'none');
            }
            if ($('#Name').val() === "") {
                $('#Name').parent().addClass('has-error');
                $("#divName").addClass('esg-has-error');
                $("#divNameErr").css('display', 'block');
            }
            else {
                $('#Name').parent().removeClass('has-error');
                $("#divName").removeClass('esg-has-error');
                $("#divNameErr").css('display', 'none');
            }
            if ($('#FirstSurname').val() === "") {
                $('#FirstSurname').parent().addClass('has-error');
                $("#divFirstSurname").addClass('esg-has-error');
                $("#divFirstSurnameErr").css('display', 'block');
            }
            else {
                $('#FirstSurname').parent().removeClass('has-error');
                $("#divFirstSurname").removeClass('esg-has-error');
                $("#divFirstSurnameErr").css('display', 'none');
            }
            if ($('#LaborPosition').val() === "") {
                $('#LaborPosition').parent().addClass('has-error');
                $("#divLaborPosition").addClass('esg-has-error');
                $("#divLaborPositionErr").css('display', 'block');
            }
            else {
                $('#LaborPosition').parent().removeClass('has-error');
                $("#divLaborPosition").removeClass('esg-has-error');
                $("#divLaborPositionErr").css('display', 'none');
            }
            if ($('#ProfessionalTitleAbbreviation').val() === "") {
                $('#ProfessionalTitleAbbreviation').parent().addClass('has-error');
                $("#divTitileAbb").addClass('esg-has-error');
                $("#divTitileAbbErr").css('display', 'block');
            }
            else {
                $('#ProfessionalTitleAbbreviation').parent().removeClass('has-error');
                $("#divTitileAbb").removeClass('esg-has-error');
                $("#divTitileAbbErr").css('display', 'none');
            }
            if ($('#Thumbprint').val() === "" || !IsValidThumprint) {
                $('#Thumbprint').parent().addClass('has-error');
                $("#divThumbprint").addClass('esg-has-error');
                $("#divThumbprintErr").css('display', 'block');
            }
            else {
                $('#Thumbprint').parent().removeClass('has-error');
                $("#divThumbprint").removeClass('esg-has-error');
                $("#divThumbprintErr").css('display', 'none');
                $("#ThumprintValid").css('display', 'none');
            }
        }
    });

    $('#btnCancel').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.location.href = urlSignerIndex;
    });

    $('#btnEdit').click(function () {
        createForm.validate();
        if (createForm.valid() && !editThumprint) {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var Model = {
                AbreviationTitleId: $('#ProfessionalTitleAbbreviation').val(),
                FirstSurname: $('#FirstSurname').val(),
                IsActive: $('#swtEditActive:checkbox:checked').length,
                Name: $('#Name').val(),
                SecondSurname: $('#SecondSurname').val(),
                SignerPositionId: $('#LaborPosition').val(),
                Thumbprint: $('#Thumbprint').val(),
                UserName: userName,
                SignerId: signerId
            };

            $.ajax({
                url: urlUpdateSigner,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { Model: JSON.stringify(Model) },
                success: function (response) {
                    if (response.id === 1) {
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlSignerIndex;
                    }
                    else if (response.id === 0) {
                        window.location.href = urlUnauthorized;
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                    else {
                        $("#divError").css('display', 'block');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                }
            });
        }
        else {
            if ($('#Curp').val() === "") {
                $('#Curp').parent().addClass('has-error');
                $("#divCurp").addClass('esg-has-error');
                $("#divCurpErr").css('display', 'block');
            }
            else {
                $('#Curp').parent().removeClass('has-error');
                $("#divCurp").removeClass('esg-has-error');
                $("#divCurpErr").css('display', 'none');
            }
            if ($('#Name').val() === "") {
                $('#Name').parent().addClass('has-error');
                $("#divName").addClass('esg-has-error');
                $("#divNameErr").css('display', 'block');
            }
            else {
                $('#Name').parent().removeClass('has-error');
                $("#divName").removeClass('esg-has-error');
                $("#divNameErr").css('display', 'none');
            }
            if ($('#FirstSurname').val() === "") {
                $('#FirstSurname').parent().addClass('has-error');
                $("#divFirstSurname").addClass('esg-has-error');
                $("#divFirstSurnameErr").css('display', 'block');
            }
            else {
                $('#FirstSurname').parent().removeClass('has-error');
                $("#divFirstSurname").removeClass('esg-has-error');
                $("#divFirstSurnameErr").css('display', 'none');
            }
            if ($('#LaborPosition').val() === "") {
                $('#LaborPosition').parent().addClass('has-error');
                $("#divLaborPosition").addClass('esg-has-error');
                $("#divLaborPositionErr").css('display', 'block');
            }
            else {
                $('#LaborPosition').parent().removeClass('has-error');
                $("#divLaborPosition").removeClass('esg-has-error');
                $("#divLaborPositionErr").css('display', 'none');
            }
            if ($('#ProfessionalTitleAbbreviation').val() === "") {
                $('#ProfessionalTitleAbbreviation').parent().addClass('has-error');
                $("#divTitileAbb").addClass('esg-has-error');
                $("#divTitileAbbErr").css('display', 'block');
            }
            else {
                $('#ProfessionalTitleAbbreviation').parent().removeClass('has-error');
                $("#divTitileAbb").removeClass('esg-has-error');
                $("#divTitileAbbErr").css('display', 'none');
            }
            if ($('#Thumbprint').val() === "" && editThumprint)
            {
                if (!IsValidThumprint) {
                    $('#Thumbprint').parent().addClass('has-error');
                    $("#divThumbprint").addClass('esg-has-error');
                    $("#divThumbprintErr").css('display', 'block');
                }
            }
            else {
                $('#Thumbprint').parent().removeClass('has-error');
                $("#divThumbprint").removeClass('esg-has-error');
                $("#divThumbprintErr").css('display', 'none');
                $("#ThumprintValid").css('display', 'none');
            }
        }
    });

    $("[id^='btnEditSigner']").click(function () {
        var id = $(this).attr('id');
        var signerId = id.split('_');
        $.ajax({
            url: urlGetEditSigner,
            dataType: "html",
            cache: false,
            type: "GET",
            data: { signerId: Number(signerId[1]) },
            success: function (response) {
                window.location.href = urlEditSigner;
            }
        });
    });

    $('#Curp').change(function () {
        $('#Curp').parent().removeClass('has-error');
        $("#divCurp").removeClass('esg-has-error');
        $("#divCurpErr").css('display', 'none');
    });

    $('#Curp').blur(function () {
        if ($('#Curp').val() !== '') {
            $.ajax({
                url: urlValidateCurp,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { Curp: $('#Curp').val() },
                success: function (response) {
                    if (response.exists) {
                        IsValidCurp = false;
                        $('#Curp').parent().addClass('has-error');
                        $("#divCurp").addClass('esg-has-error');
                        $("#divCurpErr").css('display', 'block');
                        $("#CurpExists").css('display', 'block');
                    }
                    else {
                        IsValidCurp = true;
                        $("#CurpExists").css('display', 'none');
                    }
                }
            });
        }
    });

    $('#Name').change(function () {
        $('#Name').parent().removeClass('has-error');
        $("#divName").removeClass('esg-has-error');
        $("#divNameErr").css('display', 'none');
    });

    $('#FirstSurname').change(function () {
        $('#FirstSurname').parent().removeClass('has-error');
        $("#divFirstSurname").removeClass('esg-has-error');
        $("#divFirstSurnameErr").css('display', 'none');
    });

    $('#SecondSurname').change(function () {
        $('#SecondSurname').parent().removeClass('has-error');
        $("#divSecondSurname").removeClass('esg-has-error');
        $("#divSecondSurnameErr").css('display', 'none');
    });

    $('#LaborPosition').change(function () {
        if ($('#LaborPosition').val() !== "") {
            $('#LaborPosition').parent().removeClass('has-error');
            $("#divLaborPosition").removeClass('esg-has-error');
            $("#divLaborPositionErr").css('display', 'none');
        }
        else {
            $('#LaborPosition').parent().addClass('has-error');
            $("#divLaborPosition").addClass('esg-has-error');
            $("#divLaborPositionErr").css('display', 'block');
        }
    });

    $('#ProfessionalTitleAbbreviation').change(function () {
        if ($('#ProfessionalTitleAbbreviation').val() !== "") {
            $('#ProfessionalTitleAbbreviation').parent().removeClass('has-error');
            $("#divTitileAbb").removeClass('esg-has-error');
            $("#divTitileAbbErr").css('display', 'none');
        }
        else {
            $('#ProfessionalTitleAbbreviation').parent().addClass('has-error');
            $("#divTitileAbb").addClass('esg-has-error');
            $("#divTitileAbbErr").css('display', 'block');
        }

    });

    $('#Thumbprint').change(function () {
        $('#Thumbprint').parent().removeClass('has-error');
        $("#divThumbprint").removeClass('esg-has-error');
        $("#divThumbprintErr").css('display', 'none');
    });

    $('#Thumbprint').blur(function () {
        if ($('#Thumbprint').val() !== '') {
            editThumprint = true;
            $.ajax({
                url: urlValidateThumprint,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { thumprint: $('#Thumbprint').val() },
                success: function (response) {
                    if (!response.IsValid) {
                        $('#Thumbprint').parent().addClass('has-error');
                        $("#divThumbprint").addClass('esg-has-error');
                        $("#divThumbprintErr").css('display', 'block');
                        $("#ThumprintValid").css('display', 'block');
                        IsValidThumprint = false;
                    }
                    else {
                        $("#ThumprintValid").css('display', 'none');
                        IsValidThumprint = true;
                    }
                }
            });
        }
    });

    $('#swtEditActive').change(function () {
        var val = $("#swtEditActive").is(":checked") ? "true" : "false";
        if (val === "true") {
            $('#Name').removeAttr("readonly");
            $('#FirstSurname').removeAttr("readonly");
            $('#SecondSurname').removeAttr("readonly");
            $('#LaborPosition').removeAttr("readonly");
            $('#ProfessionalTitleAbbreviation').removeAttr("readonly");
            $('#Thumbprint').removeAttr("readonly");
        }
        else {
            $('#Name').attr('readonly', true);
            $('#FirstSurname').attr('readonly', true);
            $('#SecondSurname').attr('readonly', true);
            $('#LaborPosition').attr('readonly', true);
            $('#ProfessionalTitleAbbreviation').attr('readonly', true);
            $('#Thumbprint').attr('readonly', true);
        }
    });
});