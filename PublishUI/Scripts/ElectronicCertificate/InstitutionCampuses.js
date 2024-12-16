$(document).ready(function () {
    var createForm = $('#formCreate');
    var FolioFormat;
    selectCheckboxes();
    var IndexFormatFolio;

    $('#btnCancel').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.location.href = urlInstitutionCampusesIndex;
    });

    $('#btnCancelFolioFormat').click(function () {
        $("#divModalFolioFormat").css('display', 'none');
    });

    $('#CloseModalFolioFormat').click(function () {
        $("#divModalFolioFormat").css('display', 'none');
    });

    $('[id^=btnFolioFormat_]').click(function () {
        $("#divModalFolioFormat").css('display', 'block');
        IndexFormatFolio = $(this)[0].dataset.index;
        FolioFormat = $("#FolioFormat_" + IndexFormatFolio).text();
        $('#FolioFormat').val(FolioFormat);
        unSelectCheckboxes();
        selectCheckboxes();
    });

    $('[id^=CampusSepCode_]').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });

    $('[id^=InsSep_]').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });

    $('[id^=SignInstId_]').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });

    $('#btnApplyFolioFormat').click(function () {
        FolioFormat = $('#FolioFormat').val();
        $('#FolioFormat_' + IndexFormatFolio).text(FolioFormat);
        $('#btnFolioFormat_' + IndexFormatFolio).data('id', FolioFormat);
        $('#FolioFormat').val('');
        unSelectCheckboxes();
        FolioFormat = '';
        $("#divModalFolioFormat").css('display', 'none');
    });

    $('#groupHeadingOne').click(function () {
        if ($("#expandIcon").attr('class') === 'esg-icon esg-icon--down') {
            $("#collapseGroupOne").css('display', 'none');
        }
        else {
            $("#collapseGroupOne").css('display', 'block');
        }
    });

    $('#chkFolioNum').click(function () {
        if ($('#chkFolioNum').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#InstitutionCampus.Folio#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#InstitutionCampus.Folio#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkInstitutionName').click(function () {
        if ($('#chkInstitutionName').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ORGANIZATION.ORG_NAME_1#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ORGANIZATION.ORG_NAME_1#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkFirstName').click(function () {
        if ($('#chkFirstName').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.FIRST_NAME#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.FIRST_NAME#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkFirstSurname').click(function () {
        if ($('#chkFirstSurname').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.LAST_NAME#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.LAST_NAME#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkSecondSurname').click(function () {
        if ($('#chkSecondSurname').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.Last_Name_Prefix#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.Last_Name_Prefix#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkPeopleId').click(function () {
        if ($('#chkPeopleId').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.PEOPLE_ID#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.PEOPLE_ID#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkCurp').click(function () {
        if ($('#chkCurp').prop("checked") === false) {
            var FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#PEOPLE.GOVERNMENT_ID#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#PEOPLE.GOVERNMENT_ID#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
        }
    });

    $('#chkInstitutionCode').click(function () {
        if ($('#chkInstitutionCode').prop("checked") === false) {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat.replace('#ORGANIZATION.ORG_CODE_ID#', '');
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() === "") {
                $('#FolioFormat').parent().addClass('has-error');
                $("#divCertificateFolio").addClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'block');
            }
        }
        else {
            FolioFormat = $('#FolioFormat').val();
            FolioFormat = FolioFormat + '#ORGANIZATION.ORG_CODE_ID#';
            $('#FolioFormat').val(FolioFormat);
            if ($('#FolioFormat').val() != "") {
                $('#FolioFormat').parent().removeClass('has-error');
                $("#divCertificateFolio").removeClass('esg-has-error');
                $("#divCertificateFolioErr").css('display', 'none');
            }
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

    $('#btnSave').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        var $table = $("#records_table tbody");
        var cols = [];
        var i = 0;
        var indexInstitution = $('input#hdnIndexInstitution').val();
        var icd = new Object();
        var validateLenght = false;
        for (var i = 0; i < indexInstitution; i++) {
            var icd = new Object();
            icd.InstitutionCodeId = $('#InsName_' + i).data('id');
            icd.CampusCodeId = $('#CampusCode_' + i).data('id');
            if ($("#InsSep_" + i).val().length < 5
                && ($("#CampusSepCode_" + i).val() != ''
                    || $("#ResponsibleId_" + i).val() > 0
                    || $("#FederalEntityId_" + i).val() > 0
                    || $("#InsSep_" + i).val() != '')) {
                $('#ValidateCharacters_' + i).css('display', 'block')
                validateLenght = true;
            }
            else {
                $('#ValidateCharacters_' + i).css('display', 'none')
            }
            icd.InstitutionSepId = $('#InsSep_' + i).val();
            icd.SigningInstitutionId = $('#SignInstId_' + i).val();
            icd.CampusSepCode = $("#CampusSepCode_" + i).val();
            icd.ResponsibleId = $("#ResponsibleId_" + i).val();
            icd.FederalEntityId = $("#FederalEntityId_" + i).val();
            icd.FolioFormat = $("#FolioFormat_" + i).text();
            icd.InstitutionCampusId = $("#InsCampusId_" + i).val();
            cols.push(icd);
        };
        if (!validateLenght) {
            $.ajax({
                url: urlSaveInstitutionCampuses,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { institutionCampuses: cols },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlInstitutionCampusesIndex;
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
        }
    });

    function unSelectCheckboxes() {
        $('#chkFolioNum').prop("checked", false);
        $('#chkInstitutionName').prop("checked", false);
        $('#chkFirstName').prop("checked", false);
        $('#chkFirstSurname').prop("checked", false);
        $('#chkSecondSurname').prop("checked", false);
        $('#chkPeopleId').prop("checked", false);
        $('#chkCurp').prop("checked", false);
        $('#chkInstitutionCode').prop("checked", false);
    }

    function selectCheckboxes() {
        if ($('#FolioFormat').val() !== undefined && $('#FolioFormat').val() !== '') {
            if ($('#FolioFormat').val().includes('#InstitutionCampus.Folio#'))
                $('#chkFolioNum').prop("checked", true);

            if ($('#FolioFormat').val().includes('#ORGANIZATION.ORG_NAME_1#'))
                $('#chkInstitutionName').prop("checked", true);

            if ($('#FolioFormat').val().includes('#PEOPLE.FIRST_NAME#'))
                $('#chkFirstName').prop("checked", true);

            if ($('#FolioFormat').val().includes('#PEOPLE.LAST_NAME#'))
                $('#chkFirstSurname').prop("checked", true);

            if ($('#FolioFormat').val().includes('#PEOPLE.Last_Name_Prefix#'))
                $('#chkSecondSurname').prop("checked", true);

            if ($('#FolioFormat').val().includes('#PEOPLE.PEOPLE_ID#'))
                $('#chkPeopleId').prop("checked", true);

            if ($('#FolioFormat').val().includes('#PEOPLE.GOVERNMENT_ID#'))
                $('#chkCurp').prop("checked", true);

            if ($('#FolioFormat').val().includes('#ORGANIZATION.ORG_CODE_ID#'))
                $('#chkInstitutionCode').prop("checked", true);
        }
    }


});