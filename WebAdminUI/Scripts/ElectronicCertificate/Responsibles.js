function addThumbprintClassError() {
    $('#Thumbprint').parent().addClass('has-error');
    $("#divThumbprint").addClass('esg-has-error');
    $("#divThumbprintErr").css('display', 'block');
}

function removeThumbprintClassError()
{
    $('#Thumbprint').parent().removeClass('has-error');
    $("#divThumbprint").removeClass('esg-has-error');
    $("#divThumbprintErr").css('display', 'none');
    $("#ThumbprintExist").css('display', 'none');
    $("#ThumbprintNoInstalled").css('display', 'none');
    $("#ThumbprintNoPrivateKey").css('display', 'none');
}

$(document).ready(function () {
    $('#btnAddResponsible').click(function () {
        window.location.href = urlAddResponsible;
    });

    var createForm = $('#formCreate');
    var IsValidCurp = false;
    var IsValidThumprint = false;
    var editThumprint = false;

    $('#btnAdd').click(function () {
        createForm.validate();
        if (createForm.valid() && IsValidCurp) {
            var Model = {
                Curp: $('#Curp').val(),
                FirstSurname: $('#FirstSurname').val(),
                IsActive: $('#swtActive:checkbox:checked').length,
                Name: $('#Name').val(),
                SecondSurname: $('#SecondSurname').val(),
                ResponsiblePositionId: $('#Position').val(),
                Thumbprint: $('#Thumbprint').val(),
                UserName: userName
            };

            if ($('#Thumbprint').val() === '' || !editThumprint) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');
                removeThumbprintClassError();
                $.ajax({
                    url: urlCreateResponsible,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { Model: JSON.stringify(Model) },
                    success: function (response) {
                        if (response.id === 1) {
                            $("#divError").css('display', 'none');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            window.location.href = urlResponsibleIndex;
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
            else if (editThumprint && IsValidThumprint) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');
                removeThumbprintClassError();
                $.ajax({
                    url: urlCreateResponsible,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { Model: JSON.stringify(Model) },
                    success: function (response) {
                        if (response.id === 1) {
                            $("#divError").css('display', 'none');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            window.location.href = urlResponsibleIndex;
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
                addThumbprintClassError();
            }
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
            if ($('#SecondSurname').val() === "") {
                $('#SecondSurname').parent().addClass('has-error');
                $("#divSecondSurname").addClass('esg-has-error');
                $("#divSecondSurnameErr").css('display', 'block');
            }
            else {
                $('#SecondSurname').parent().removeClass('has-error');
                $("#divSecondSurname").removeClass('esg-has-error');
                $("#divSecondSurnameErr").css('display', 'none');
            }
            if ($('#Position').val() === "") {
                $('#Position').parent().addClass('has-error');
                $("#divPosition").addClass('esg-has-error');
                $("#divPositionErr").css('display', 'block');
            }
            else {
                $('#Position').parent().removeClass('has-error');
                $("#divPosition").removeClass('esg-has-error');
                $("#divPositionErr").css('display', 'none');
            }
        }
    });

    $('#btnCancel').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.location.href = urlResponsibleIndex;
    });

    $('#btnEdit').click(function () {
        createForm.validate();
        if (createForm.valid()) {
            var Model = {
                FirstSurname: $('#FirstSurname').val(),
                IsActive: $('#swtEditActive:checkbox:checked').length,
                Name: $('#Name').val(),
                SecondSurname: $('#SecondSurname').val(),
                ResponsiblePositionId: $('#Position option:selected').val(),
                Thumbprint: $('#Thumbprint').val(),
                UserName: userName,
                ResponsibleId: responsibleId
            };
            if ($('#Thumbprint').val() === '' || !editThumprint) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');
                removeThumbprintClassError();
                $.ajax({
                    url: urlUpdateResponsible,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { Model: JSON.stringify(Model) },
                    success: function (response) {
                        if (response.id === 1) {
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            window.location.href = urlResponsibleIndex;
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
            else if (editThumprint && IsValidThumprint) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');
                removeThumbprintClassError();
                $.ajax({
                    url: urlUpdateResponsible,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { Model: JSON.stringify(Model) },
                    success: function (response) {
                        if (response.id === 1) {
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            window.location.href = urlResponsibleIndex;
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
                addThumbprintClassError();
            }
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
            if ($('#SecondSurname').val() === "") {
                $('#SecondSurname').parent().addClass('has-error');
                $("#divSecondSurname").addClass('esg-has-error');
                $("#divSecondSurnameErr").css('display', 'block');
            }
            else {
                $('#SecondSurname').parent().removeClass('has-error');
                $("#divSecondSurname").removeClass('esg-has-error');
                $("#divSecondSurnameErr").css('display', 'none');
            }
            if ($('#LaborPosition').val() === "") {
                $('#LaborPosition').parent().addClass('has-error');
                $("#divLaborPosition").addClass('esg-has-error');
                $("#divLaborPositionErr").css('display', 'block');
            }
            else {
                $('#Position').parent().removeClass('has-error');
                $("#divPosition").removeClass('esg-has-error');
                $("#divPositionErr").css('display', 'none');
            }
            if ($('#Thumbprint').val() === "" && editThumprint) {
                if (!IsValidThumprint) {
                    addThumbprintClassError();
                }
            }
            else {
                removeThumbprintClassError();
            }
        }
    });

    $('.btnEdit').click(function () {
        var id = $(this).attr('id');
        var responsibleId = id.split('_');
        $.ajax({
            url: urlGetEditResponsible,
            dataType: "html",
            cache: false,
            type: "GET",
            data: { responsibleId: Number(responsibleId[1]) },
            success: function (response) {
                window.location.href = urlEditResponsible;
            }
        });
    });

    $('#Curp').change(function () {
        $('#Curp').parent().removeClass('has-error');
        $("#divCurp").removeClass('esg-has-error');
        $("#divCurpErr").css('display', 'none');
    });

    $("#Curp").keypress(function () {
        if (this.value.match(/[^a-zA-Z0-9]/g)) {
            this.value = this.value.replace(/[^a-zA-Z0-9]/g, '');
        }
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

    $("#Name").keypress(function (e) {
        if (e.which === 32 && this.value === "") {
            return false;
        }
    });

    $("#Name").blur(function (e) {
        if (e.which === 32 && this.value === "") {
            return false;
        }
    });

    $('#FirstSurname').change(function () {
        $('#FirstSurname').parent().removeClass('has-error');
        $("#divFirstSurname").removeClass('esg-has-error');
        $("#divFirstSurnameErr").css('display', 'none');
    });

    $("#FirstSurname").keypress(function (e) {
        if (e.which === 32 && this.value === "") {
            return false;
        }
    });

    $("#FirstSurname").blur(function (e) {
        if (e.which === 32 && this.value === "") {
            return false;
        }
    });

    $('#SecondSurname').change(function () {
        $('#SecondSurname').parent().removeClass('has-error');
        $("#divSecondSurname").removeClass('esg-has-error');
        $("#divSecondSurnameErr").css('display', 'none');
    });

    $("#SecondSurname").keypress(function (e) {
        if (e.which === 32 && this.value === "") {
            return false;
        }
    });

    $("#SecondSurname").blur(function (e) {
        if (e.which === 32 && this.value === "") {
            return false;
        }
    });

    $('#Position').change(function () {
        if ($('#Position').val() !== "") {
            $('#Position').parent().removeClass('has-error');
            $("#divPosition").removeClass('esg-has-error');
            $("#divPositionErr").css('display', 'none');
        }
        else {
            $('#Position').parent().addClass('has-error');
            $("#divPosition").addClass('esg-has-error');
            $("#divPositionErr").css('display', 'block');
        }
    });

    $('#Thumbprint').change(function () {
        removeThumbprintClassError();
    });

    $("#Thumbprint").keypress(function (e) {
        if (e.which === 32) {
            return false;
        }
    });

    $('#Thumbprint').blur(function () {
        if ($('#Thumbprint').val() !== '') {
            if (this.value.match(/[\s]/)) {
                this.value = this.value.replace(/ /g, '');
            }
            editThumprint = true;
            $.ajax({
                url: urlValidateThumbprint,
                dataType: "json",
                cache: false,              
                type: 'GET',
                data:
                {
                    thumbprint: $('#Thumbprint').val().trim(),
                    responsibleId: $('#hdnResponsibleId').val()
                },
                success: function (response) {
                    if (response.status == 0 || response.status == 2 || response.status == 3) {
                        if (response.status == 0) {
                            $("#ThumbprintExist").css('display', 'block');                            
                        } else if (response.status ==2) {
                            $("#ThumbprintNoInstalled").css('display', 'block');
                        } else if (response.status == 3) {                            
                            $("#ThumbprintNoPrivateKey").css('display', 'block');
                        }                      
                        IsValidThumprint = false;
                    }
                    else {                       
                        IsValidThumprint = true;
                        removeThumbprintClassError();
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
            $('#Position').removeAttr("readonly");
            $('#Thumbprint').removeAttr("readonly");
        }
        else {
            $('#Name').attr('readonly', true);
            $('#FirstSurname').attr('readonly', true);
            $('#SecondSurname').attr('readonly', true);
            $('#Position').attr('readonly', true);
            $('#Thumbprint').attr('readonly', true);
        }
    });
});