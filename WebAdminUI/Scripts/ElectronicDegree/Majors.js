$(document).ready(function () {
    var IsValidCode = false;
    var codeOrigin = $('#Cve').val();

    $('#btnAddMajor').click(function () {
        window.location.href = urlAddMajor;
    });

    var createForm = $('#formCreateMajor');
    var editForm = $('#formEditMajor');

    $('#btnAdd').click(function () {
        createForm.validate();
        if (createForm.valid() && IsValidCode) {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var Model = {
                Code: $('#Cve').val(),
                Name: $('#MajorName').val(),
                StudyLevel: $('#EducationLevel option:selected').text(),
                UserName: userName,
                LegalBaseId: $("#LegalBaseId").val()
            };

            $.ajax({
                url: urlCreateMajor,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { Model: JSON.stringify(Model) },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlMajorIndex;
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
            $("#Processing").css('display', 'none');
            $("#Overlaydiv").css('display', 'none');
            if ($('#Cve').val() === "" || !IsValidCode) {
                $('#Cve').parent().addClass('has-error');
                $("#divCve").addClass('esg-has-error');
                $("#divCveErr").css('display', 'block');
            }
            else {
                $('#Cve').parent().removeClass('has-error');
                $("#divCve").removeClass('esg-has-error');
                $("#divCveErr").css('display', 'none');
                $("#CodeValid").css('display', 'none');
            }
            if ($('#MajorName').val() === "") {
                $('#MajorName').parent().addClass('has-error');
                $("#divMajorName").addClass('esg-has-error');
                $("#divMajorNameErr").css('display', 'block');
            }
            else {
                $('#MajorName').parent().removeClass('has-error');
                $("#divMajorName").removeClass('esg-has-error');
                $("#divMajorNameErr").css('display', 'none');
            }
            if ($('#EducationLevel').val() === "") {
                $('#EducationLevel').parent().addClass('has-error');
                $("#divEducationLevel").addClass('esg-has-error');
                $("#divEducationLevelErr").css('display', 'block');
            }
            else {
                $('#EducationLevel').parent().removeClass('has-error');
                $("#divEducationLevel").removeClass('esg-has-error');
                $("#divEducationLevelErr").css('display', 'none');
            }

            if ($('#LegalBaseId').val() === "") {
                $('#LegalBaseId').parent().addClass('has-error');
                $("#divLegalBaseId").addClass('esg-has-error');
                $("#divLegalBaseIdErr").css('display', 'block');
            }
            else {
                $('#LegalBaseId').parent().removeClass('has-error');
                $("#divLegalBaseId").removeClass('esg-has-error');
                $("#divLegalBaseIdErr").css('display', 'none');
            }
        }
    });

    $("#LegalBaseId").change(function () {
        if ($('#LegalBaseId').val() === "") {
            $('#LegalBaseId').parent().addClass('has-error');
            $("#divLegalBaseId").addClass('esg-has-error');
            $("#divLegalBaseIdErr").css('display', 'block');
        }
        else {
            $('#LegalBaseId').parent().removeClass('has-error');
            $("#divLegalBaseId").removeClass('esg-has-error');
            $("#divLegalBaseIdErr").css('display', 'none');
        }
    });

    $('#Cve').change(function () {
        if ($('#Cve').val() === "") {
            $('#Cve').parent().addClass('has-error');
            $("#divCve").addClass('esg-has-error');
            $("#divCveErr").css('display', 'block');
            $("#CodeValid").css('display', 'none');
            $("#CveInvalidLength").css('display', 'none');
        }
        else if ($('#Cve').val().length < 6 || $('#Cve').val().length > 7) {
            $('#Cve').parent().addClass('has-error');
            $("#divCve").addClass('esg-has-error');
            $("#divCveErr").css('display', 'block');
            $("#CodeValid").css('display', 'none');
            $("#CveInvalidLength").css('display', 'block');
        }
        else {
            $('#Cve').parent().removeClass('has-error');
            $("#divCve").removeClass('esg-has-error');
            $("#divCveErr").css('display', 'none');
            $("#CodeValid").css('display', 'none');
            $("#CveInvalidLength").css('display', 'none');
        }
    });

    function validateCode(code) {
        if (codeOrigin === $('#Cve').val()) {
            IsValidCode = true;
        }
        else {
            $.ajax({
                url: urlValidateCode,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { code: code },
                success: function (response) {
                    if (response.exists) {
                        $('#Cve').parent().addClass('has-error');
                        $("#divCve").addClass('esg-has-error');
                        $("#divCveErr").css('display', 'block');
                        $("#CodeValid").css('display', 'block');
                        IsValidCode = false;
                    }
                    else {
                        $('#Cve').parent().removeClass('has-error');
                        $("#divCve").removeClass('esg-has-error');
                        $("#divCveErr").css('display', 'none');
                        $("#CodeValid").css('display', 'none');
                        $("#CveInvalidLength").css('display', 'none');
                        IsValidCode = true;
                    }
                }
            });
        }

    }

    $('#Cve').blur(function () {
        if ($('#Cve').val() !== '' && $('#Cve').val().length >= 6 && $('#Cve').val().length <= 7) {
            validateCode($('#Cve').val());
        }
    });

    $('#MajorName').change(function () {
        if ($('#MajorName').val() === "") {
            $('#MajorName').parent().addClass('has-error');
            $("#divMajorName").addClass('esg-has-error');
            $("#divMajorNameErr").css('display', 'block');
        }
        else {
            $('#MajorName').parent().removeClass('has-error');
            $("#divMajorName").removeClass('esg-has-error');
            $("#divMajorNameErr").css('display', 'none');
        }
    });

    $('#EducationLevel').change(function () {
        if ($('#EducationLevel').val() === "") {
            $('#EducationLevel').parent().addClass('has-error');
            $("#divEducationLevel").addClass('esg-has-error');
            $("#divEducationLevelErr").css('display', 'block');
        }
        else {
            $('#EducationLevel').parent().removeClass('has-error');
            $("#divEducationLevel").removeClass('esg-has-error');
            $("#divEducationLevelErr").css('display', 'none');
        }
    });

    $('#btnCancel').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.location.href = urlMajorIndex;
    });

    $('button').click(function () {

        var id = $(this).attr('id');

        switch (id) {
            case "btnSave":
                return false;
        }

        var majorId = id.split('_');
        if (String(majorId[0]) !== "btnAdd") {
            $.ajax({
                url: urlGetEditMajor,
                dataType: "html",
                cache: false,
                type: "GET",
                data: { majorId: Number(majorId[1]) },
                success: function (response) {
                    if (String(majorId[0]) === "btnEdit") {
                        window.location.href = urlEditMajor;
                    }
                    else {
                        $("#Processing").css('display', 'block');
                        $("#Overlaydiv").css('display', 'block');
                        $.ajax({
                            url: urlDeleteMajor,
                            dataType: "json",
                            cache: false,
                            type: "POST",
                            data: { majorId: Number(majorId[1]) },
                            success: function (response) {
                                if (response.id === 1) {
                                    $("#divError").css('display', 'none');
                                    $("#Processing").css('display', 'none');
                                    $("#Overlaydiv").css('display', 'none');
                                    window.location.href = urlMajorIndex;
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
                }
            });
        }
    });

    $('#btnSave').click(function () {
        editForm.validate();
        validateCode($('#Cve').val());
        var legalBaseId = $("#LegalBaseId").val();
        if (editForm.valid() && legalBaseId !== "" && IsValidCode) {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var Model = {
                Code: $('#Cve').val(),
                Name: $('#MajorName').val(),
                StudyLevel: $('#EducationLevel option:selected').text(),
                UserName: userName,
                ElectronicDegreeMajorId: majorId,
                LegalBaseId: legalBaseId
            };

            $.ajax({
                url: urlUpdateMajor,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { Model: JSON.stringify(Model) },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlMajorIndex;
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
            if ($('#Cve').val() === "") {
                $('#Cve').parent().addClass('has-error');
                $("#divCve").addClass('esg-has-error');
                $("#divCveErr").css('display', 'block');
            }
            else {
                $('#Cve').parent().removeClass('has-error');
                $("#divCve").removeClass('esg-has-error');
                $("#divCveErr").css('display', 'none');
            }
            if ($('#MajorName').val() === "") {
                $('#MajorName').parent().addClass('has-error');
                $("#divMajorName").addClass('esg-has-error');
                $("#divMajorNameErr").css('display', 'block');
            }
            else {
                $('#MajorName').parent().removeClass('has-error');
                $("#divMajorName").removeClass('esg-has-error');
                $("#divMajorNameErr").css('display', 'none');
            }
            if ($('#EducationLevel').val() === "") {
                $('#EducationLevel').parent().addClass('has-error');
                $("#divEducationLevel").addClass('esg-has-error');
                $("#divEducationLevelErr").css('display', 'block');
            }
            else {
                $('#EducationLevel').parent().removeClass('has-error');
                $("#divEducationLevel").removeClass('esg-has-error');
                $("#divEducationLevelErr").css('display', 'none');
            }
            if ($('#LegalBaseId').val() === "") {
                $('#LegalBaseId').parent().addClass('has-error');
                $("#divLegalBaseId").addClass('esg-has-error');
                $("#divLegalBaseIdErr").css('display', 'block');
            }
            else {
                $('#LegalBaseId').parent().removeClass('has-error');
                $("#divLegalBaseId").removeClass('esg-has-error');
                $("#divLegalBaseIdErr").css('display', 'none');
            }
        }
        return false;
    });
});