$(document).ready(function () {
    var createForm = $('#formCreate');
    var updateForm = $('#formUpdate');
    var FolioFormat;
    selectCheckboxes();

    $('#btnAddInstitution').click(function () {
        window.location.href = urlAddInstitution;
    });

    $('#btnCancel').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.location.href = urlInstitutionIndex;
    });

    $('#Code').blur(function () {
        if ($('#Code').val() !== '') {
            $.ajax({
                url: urlValidateCode,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { Code: $('#Code').val() },
                success: function (response) {
                    if (response.exists) {
                        IsValidCurp = false;
                        $('#Code').parent().addClass('has-error');
                        $("#divCode").addClass('esg-has-error');
                        $("#divCodeErr").css('display', 'block');
                        $("#CodeExists").css('display', 'block');
                    }
                    else {
                        IsValidCurp = true;
                        $("#CodeExists").css('display', 'none');
                    }
                }
            });
        }
    });

    $('#Code').change(function () {
        if ($('#Code').val() === "") {
            $('#Code').parent().addClass('has-error');
            $("#divCode").addClass('esg-has-error');
            $("#divCodeErr").css('display', 'block');
        }
        else {
            $('#Code').parent().removeClass('has-error');
            $("#divCode").removeClass('esg-has-error');
            $("#divCodeErr").css('display', 'none');
        }
    });

    $('#Name').change(function () {
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
    });

    $('#Name').blur(function () {
        if ($('#Name').val() !== '') {
            $.ajax({
                url: urlValidateName,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { Name: $('#Name').val() },
                success: function (response) {
                    if (response.exists) {
                        IsValidCurp = false;
                        $('#Name').parent().addClass('has-error');
                        $("#divName").addClass('esg-has-error');
                        $("#divNameErr").css('display', 'block');
                        $("#NameExists").css('display', 'block');
                    }
                    else {
                        IsValidCurp = true;
                        $("#NameExists").css('display', 'none');
                    }
                }
            });
        }
    });

    $('#FolioFormat').change(function () {
        if ($('#FolioFormat').val() === "") {
            $('#FolioFormat').parent().addClass('has-error');
            $("#divDegreeFolio").addClass('esg-has-error');
            $("#divDegreeFolioErr").css('display', 'block');
        }
        else {
            $('#FolioFormat').parent().removeClass('has-error');
            $("#divDegreeFolio").removeClass('esg-has-error');
            $("#divDegreeFolioErr").css('display', 'none');
        }
    });

    $('#btnAdd').click(function () {
        createForm.validate();
        if (createForm.valid()) {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var Model = {
                Code: $('#Code').val(),
                EquivalentId: $('#Equivalents').val(),
                Name: $('#Name').val(),
                FolioFormat: $('#FolioFormat').val(),
                CreateUserName: userName,
                RevisionUserName: userName
            };

            $.ajax({
                url: urlCreateInstitution,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { institutionDAModel: Model },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlInstitutionIndex;
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
            if ($('#Code').val() === "") {
                $('#Code').parent().addClass('has-error');
                $("#divCode").addClass('esg-has-error');
                $("#divCodeErr").css('display', 'block');
            }
            else {
                $('#Code').parent().removeClass('has-error');
                $("#divCode").removeClass('esg-has-error');
                $("#divCodeErr").css('display', 'none');
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
        }
    });

    $('#chkSerialNum').click(function () {
        if ($('#chkSerialNum').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ElectronicDegreeInstitution.Folio#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ElectronicDegreeInstitution.Folio#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkInstitutionName').click(function () {
        if ($('#chkInstitutionName').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ElectronicDegreeInstitution.Name#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ElectronicDegreeInstitution.Name#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkEDInstCode').click(function () {
        if ($('#chkEDInstCode').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ElectronicDegreeInstitution.Code#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ElectronicDegreeInstitution.Code#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkFirstName').click(function () {
        if ($('#chkFirstName').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.FIRST_NAME#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.FIRST_NAME#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkFirstSurname').click(function () {
        if ($('#chkFirstSurname').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.LAST_NAME#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.LAST_NAME#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkSecondSurname').click(function () {
        if ($('#chkSecondSurname').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.Last_Name_Prefix#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.Last_Name_Prefix#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkPeopleId').click(function () {
        if ($('#chkPeopleId').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.PEOPLE_ID#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.PEOPLE_ID#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkCurp').click(function () {
        if ($('#chkCurp').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.GOVERNMENT_ID#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.GOVERNMENT_ID#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkMajor').click(function () {
        if ($('#chkMajor').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ElectronicDegreeMajor.Name#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ElectronicDegreeMajor.Name#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#chkInstitutionCode').click(function () {
        if ($('#chkInstitutionCode').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ElectronicDegreeMajor.Code#', '');
            $('#FolioFormat').val(FolioFormat);
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ElectronicDegreeMajor.Code#';
            $('#FolioFormat').val(FolioFormat);
        }
    });

    $('#groupHeadingOne').click(function () {
        if ($("#expandIcon").attr('class') === 'esg-icon esg-icon--down') {
            $("#collapseGroupOne").css('display', 'none');
        }
        else {
            $("#collapseGroupOne").css('display', 'block');
        }
    });

    $('button').click(function () {
        var id = $(this).attr('id').split('_');
        if (id[0] === 'btnEdit') {
            var institutionId = id[1];
            urlEditInstitution = urlEditInstitution.replace("param-id", institutionId);
            window.location.href = urlEditInstitution;
        }
    });

    $('#btnSave').click(function () {
        updateForm.validate();
        if (updateForm.valid()) {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            var Model = {
                Code: $('#Code').val(),
                EquivalentId: $('#Equivalents').val(),
                Name: $('#Name').val(),
                FolioFormat: $('#FolioFormat').val(),
                RevisionUserName: userName,
                Id: $('#Id').val()
            };

            $.ajax({
                url: urlInstitutionUpdate,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { institutionDAModel: Model },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlInstitutionIndex;
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
            if ($('#Code').val() === "") {
                $('#Code').parent().addClass('has-error');
                $("#divCode").addClass('esg-has-error');
                $("#divCodeErr").css('display', 'block');
            }
            else {
                $('#Code').parent().removeClass('has-error');
                $("#divCode").removeClass('esg-has-error');
                $("#divCodeErr").css('display', 'none');
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
        }
    });

    function selectCheckboxes() {
        if ($('#FolioFormat').val() !== undefined && $('#FolioFormat').val() !== '') {
            if ($('#FolioFormat').val().includes('#ElectronicDegreeInstitution.Folio#'))
                $('#chkSerialNum').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#ElectronicDegreeInstitution.Name#'))
                $('#chkInstitutionName').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#ElectronicDegreeInstitution.Code#'))
                $('#chkEDInstCode').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#PEOPLE.FIRST_NAME#'))
                $('#chkFirstName').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#PEOPLE.LAST_NAME#'))
                $('#chkFirstSurname').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#PEOPLE.Last_Name_Prefix#'))
                $('#chkSecondSurname').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#PEOPLE.PEOPLE_ID#'))
                $('#chkPeopleId').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#PEOPLE.GOVERNMENT_ID#'))
                $('#chkCurp').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#ElectronicDegreeMajor.Name#'))
                $('#chkMajor').attr('checked', 'checked');

            if ($('#FolioFormat').val().includes('#ElectronicDegreeMajor.Code#'))
                $('#chkInstitutionCode').attr('checked', 'checked');
        }
    }
});