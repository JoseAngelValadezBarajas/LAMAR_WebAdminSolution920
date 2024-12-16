$(document).ready(function () {
    //#region Variables
    var peopleList = [];
    var IsValid = true;
    var PeopleCodeId;
    var institutionFolio;
    var institutionMajor = [];
    var Signers = [];
    var issuingDegree;
    var backgroundStudies;
    var stateCatalog;
    var stateCatalogBS;
    var socialServiceCatalog;
    var instMajorId;
    var trasncriptDegreeId;
    var rdoId;
    var AuthorizationCode;
    var FulfilledSocialService;
    var IsHideStep = false;
    var StudyLevel;
    //#endregion

    //#region MainView
    $('#btnGenerate').click(function () {
        window.location.href = urlGenerate;
    });
    //#endregion

    // #region Student
    $('#PeopleId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlPeople,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { peopleId: request.term },
                success: function (data) {
                    peopleList = data.peopleList;
                    response($.map(data.peopleList, function (item) {
                        return { label: item.PeopleCodeId, value: item.PeopleCodeId };
                    }));
                }
            });

            $('.ui-autocomplete').css('min-width', '100px').css('max-width', '100%').css('max-height', '100px')
                .css('overflow-y', 'auto').css('overflow-x', 'hidden').css('z-index', '9999!important').css('background', 'white');
        }
    });

    $('#PeopleId').blur(function () {
        PeopleCodeId = $(this).val();
        IsValid = true;
        if (PeopleCodeId !== "") {
            var response = $.grep(peopleList, function (element) { return element.PeopleCodeId === PeopleCodeId; });
            $('#Curp').val(response[0].Curp);
            $('#Name').val(response[0].Name);
            $('#FirstSurname').val(response[0].FirstSurname);
            $('#SecondSurname').val(response[0].SecondSurname);
            $('#Email').val(response[0].Email);
            if ($('#Curp').val() === "") {
                $('#Curp').parent().addClass('has-error');
                $('#CurpError').css('display', 'block');
                $("#divCurp").addClass('esg-has-error');
                $("#divCurpErr").css('display', 'block');
                IsValid = false;
            }
            else {
                $('#Curp').parent().removeClass('has-error');
                $('#CurpError').css('display', 'none');
                $("#divCurp").removeClass('esg-has-error');
                $("#divCurpErr").css('display', 'none');
                $("#CurpExists").css('display', 'none');
            }
            if ($('#Name').val() === "") {
                $('#Name').parent().addClass('has-error');
                $('#NameError').css('display', 'block');
                $("#divName").addClass('esg-has-error');
                $("#divNameErr").css('display', 'block');
                IsValid = false;
            }
            else {
                $('#Name').parent().removeClass('has-error');
                $('#NameError').css('display', 'none');
                $("#divName").removeClass('esg-has-error');
                $("#divNameErr").css('display', 'none');
            }
            if ($('#FirstSurname').val() === "") {
                $('#FirstSurname').parent().addClass('has-error');
                $('#FirstSurnameError').css('display', 'block');
                $("#divFirstSurname").addClass('esg-has-error');
                $("#divFirstSurnameErr").css('display', 'block');
                IsValid = false;
            }
            else {
                $('#FirstSurname').parent().removeClass('has-error');
                $('#FirstSurnameError').css('display', 'none');
                $("#divFirstSurname").removeClass('esg-has-error');
                $("#divFirstSurnameErr").css('display', 'none');
            }
            if ($('#Email').val() === "") {
                $('#Email').parent().addClass('has-error');
                $('#EmailError').css('display', 'block');
                $("#divEmail").addClass('esg-has-error');
                $("#divEmailErr").css('display', 'block');
                IsValid = false;
            }
            else {
                $('#Email').parent().removeClass('has-error');
                $('#EmailError').css('display', 'none');
                $("#divEmail").removeClass('esg-has-error');
                $("#divEmailErr").css('display', 'none');
            }
        }
    });

    $('#btnCancelStudent').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.location.href = urlIndex;
    });

    $('#btnValidateStudent').click(function () {
        if ($('#Curp').val() === "") {
            $('#Curp').parent().addClass('has-error');
            $('#CurpError').css('display', 'block');
            $("#divCurp").addClass('esg-has-error');
            $("#divCurpErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#Curp').parent().removeClass('has-error');
            $('#CurpError').css('display', 'none');
            $("#divCurp").removeClass('esg-has-error');
            $("#divCurpErr").css('display', 'none');
            $("#CurpExists").css('display', 'none');
        }
        if ($('#Name').val() === "") {
            $('#Name').parent().addClass('has-error');
            $('#NameError').css('display', 'block');
            $("#divName").addClass('esg-has-error');
            $("#divNameErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#Name').parent().removeClass('has-error');
            $('#NameError').css('display', 'none');
            $("#divName").removeClass('esg-has-error');
            $("#divNameErr").css('display', 'none');
        }
        if ($('#FirstSurname').val() === "") {
            $('#FirstSurname').parent().addClass('has-error');
            $('#FirstSurnameError').css('display', 'block');
            $("#divFirstSurname").addClass('esg-has-error');
            $("#divFirstSurnameErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#FirstSurname').parent().removeClass('has-error');
            $('#FirstSurnameError').css('display', 'none');
            $("#divFirstSurname").removeClass('esg-has-error');
            $("#divFirstSurnameErr").css('display', 'none');
        }
        if ($('#Email').val() === "") {
            $('#Email').parent().addClass('has-error');
            $('#EmailError').css('display', 'block');
            $("#divEmail").addClass('esg-has-error');
            $("#divEmailErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#Email').parent().removeClass('has-error');
            $('#EmailError').css('display', 'none');
            $("#divEmail").removeClass('esg-has-error');
            $("#divEmailErr").css('display', 'none');
        }
        if (IsValid) {
            if (!IsHideStep) {
                $.ajax({
                    url: urlInstitutionMajor,
                    dataType: "json",
                    cache: false,
                    type: "GET",
                    data: { peopleId: PeopleCodeId, operatorId: operatorId },
                    success: function (data) {
                        if (data.institutionMajors.length > 0) {
                            $('#tblInstitutionMajor').css('display', 'block');
                            var msgINError;
                            var iconINError;
                            var iconSignerError;
                            var iconMajorError;
                            var label;
                            institutionMajor = data.institutionMajors;
                            $.each(data.institutionMajors, function (i, item) {
                                if (item.IsOperatorOfInstitution === 0 && item.InstitutionName !== "") {
                                    msgINError = noInstitutionAssigned;
                                }
                                else {
                                    msgINError = noInstitutionMapped;
                                }
                                if (item.InstitutionName === "" || item.IsOperatorOfInstitution === 0) {
                                    iconINError = "block";
                                    label = "block";
                                }
                                else {
                                    label = "block";
                                    iconINError = "none";
                                }
                                if (item.NumberOfSigners === 0) {
                                    iconSignerError = "block";
                                }
                                else {
                                    iconSignerError = "none";
                                }
                                if (item.MajorName === "") {
                                    iconMajorError = "block";
                                }
                                else {
                                    iconMajorError = "none";
                                }
                                contentTr = "<tr class='esg-table-body__row' id=" + i + ">" +
                                    "<td class='esg-table-body__td'>" +
                                    "<div class='esg-radio' style='top:-1rem'>" +
                                    "<input id='rdo_" + i + "' type=radio name='rdoIM' />" +
                                    "<label for='rdo_" + i + "'></label>" +
                                    "</div >" +
                                    "</td>" +
                                    "<td class='esg-table-body__td'>" +
                                    "<div style='display:inline-flex'><label style='display:" + label + "'>" + item.InstitutionName + "</label>" +
                                    "<span id='spnIN_" + i + "' class='esg-icon__container spnToolTip' style='cursor:pointer; margin-left:1rem; display:" + iconINError + "'>" +
                                    "<svg class='esg-icon--error esg-icon--small'>" +
                                    "<use xlink: href='#icon-error'></use></svg>" +
                                    "</span>" +
                                    "<div id='tltIN_" + i + "' style='z-index: 1; margin-left: 1rem; margin-top: -1rem; position: relative; height:3.125rem; display:none'>" +
                                    "<div class='esg-tooltip esg-tooltip--right' role='tooltip'>" +
                                    "<div class='esg-tooltip__arrow'></div>" +
                                    "<div class='esg-tooltip__content'>" + msgINError + "</div>" +
                                    "</div>" +
                                    "</div>" +
                                    "</div></td>" +
                                    "<td class='esg-table-body__td'>" +
                                    "<div style='display:inline-flex'><label style='display:" + label + "'>" + item.NumberOfSigners + "</label>" +
                                    "<span id='spnNS_" + i + "' class='esg-icon__container spnToolTip' style='cursor:pointer; margin-left:1rem; display:" + iconSignerError + "'>" +
                                    "<svg class='esg-icon--error esg-icon--small'>" +
                                    "<use xlink: href='#icon-error'></use></svg>" +
                                    "</span>" +
                                    "<div id='tltNS_" + i + "' style='z-index: 1; margin-left: 1rem; margin-top: -1rem; position: relative; height:3.125rem; display:none'>" +
                                    "<div class='esg-tooltip esg-tooltip--right' role='tooltip'>" +
                                    "<div class='esg-tooltip__arrow'></div>" +
                                    "<div class='esg-tooltip__content'>" + noInstitutionSigner + "</div>" +
                                    "</div>" +
                                    "</div>" +
                                    "</div></td >" +
                                    "<td class='esg-table-body__td'><label>" + item.ProgramDesc + "</label></td >" +
                                    "<td class='esg-table-body__td'>" +
                                    "<div style='display:inline-flex'><label style='display:" + label + "'>" + item.MajorName + "</label>" +
                                    "<span id='spnMN_" + i + "' class='esg-icon__container spnToolTip' style='cursor:pointer; margin-left:1rem; display:" + iconMajorError + "'>" +
                                    "<svg class='esg-icon--error esg-icon--small'>" +
                                    "<use xlink: href='#icon-error'></use></svg>" +
                                    "</span>" +
                                    "<div id='tltMN_" + i + "' style='z-index: 1; margin-left: 1rem; margin-top: -1rem; position: relative; height:3.125rem; display:none'>" +
                                    "<div class='esg-tooltip esg-tooltip--right' role='tooltip'>" +
                                    "<div class='esg-tooltip__arrow'></div>" +
                                    "<div class='esg-tooltip__content'>" + noMajorProgram + "</div>" +
                                    "</div>" +
                                    "</div>" +
                                    "</div></td >" +
                                    "<td class='esg-table-body__td'><label>" + item.RvoeAgreementNumber + "(" + item.MatricYear + "/" + item.TermDesc + ")" + "</label></td >" +
                                    "</tr>";
                                $('#institutionMajor_table_rows_tbody').append(contentTr);
                            });
                        }
                    }
                });
            }

            $('#liStudent').removeClass('esg-is-active');
            $('#liStudent').addClass('esg-is-previous');
            $('#liInstitutionMajor').addClass('esg-is-active');
            $('#stpStudent').css('display', 'none');
            $('#stpInstitutionMajor').css('display', 'block');
            $('#divFirstPart').css('display', 'block');
            $('#btnStpInstMajor').css('cursor', 'pointer').css('color', '#0074db').css('text-decoration', '');
        }
    });

    $('#btnStpStudent').click(function () {
        $('#stpStudent').css('display', 'block');
        $('#stpInstitutionMajor').css('display', 'none');
        $('#divFirstPart').css('display', 'none');
        $('#divSecondPart').css('display', 'none');
        $('#stpIssuingDegree').css('display', 'none');
        $('#stpBackgroundStudies').css('display', 'none');
        $('#stpPreviewGenerate').css('display', 'none');
        $('#liInstitutionMajor').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liIssuingDegree').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liBackgroundStudies').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liPreview').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liStudent').removeClass('esg-is-previous').addClass('esg-is-active');
        $('#btnStpInstMajor').css('text-decoration', '');
        $('#btnStpIssuingDegree').css('text-decoration', '');
        $('#btnStpBackgroundStu').css('text-decoration', '');
        $('#btnStpPreview').css('text-decoration', '');
    });
    // #endregion

    // #region Institution-Major
    var localeBrowser = "es";
    if (window.navigator !== undefined) {
        if (window.navigator.languages !== undefined) {
            localeBrowser = (window.navigator.languages[0] || window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
        else {
            localeBrowser = (window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
    }
    $('#btnBackIM').click(function () {
        $('#liInstitutionMajor').removeClass('esg-is-active');
        $('#liStudent').removeClass('esg-is-previous').addClass('esg-is-active');
        $('#stpInstitutionMajor').css('display', 'none');
        $('#stpStudent').css('display', 'block');
        IsHideStep = true;
    });

    $('#btnContinueIM').click(function () {
        var contentTr = "";
        IsHideStep = false;
        $('#institutionSigners_tbody').empty();
        var rdo = $('input[type=radio]:checked').attr('id');
        rdoId = rdo.split('_');
        if (rdo !== undefined) {
            var spnIN = $('#spnIN_' + Number(rdoId[1]));
            var spnNS = $('#spnNS_' + Number(rdoId[1]));
            var spnMN = $('#spnMN_' + Number(rdoId[1]));
            if (spnIN.css('display') === 'none' && spnMN.css('display') === 'none' && spnNS.css('display') === 'none') {
                $.ajax({
                    url: urlInstitutionSigner,
                    dataType: "json",
                    cache: false,
                    type: "GET",
                    data: { institutionId: institutionMajor[Number(rdoId[1])].ElectronicDegreeInstitutionId },
                    success: function (data) {
                        if (data.institutionSigners.length > 0) {
                            institutionMajor = data.institutionMajors;
                            Signers = data.institutionSigners;
                            $.each(data.institutionSigners, function (i, item) {
                                contentTr = "<tr class='esg-table-body__row' id=" + i + ">" +
                                    "<td class='esg-table-body__td' > " + item.EdAbreviationTitle + " " + item.EdSignerName + " " + item.EdSignerFirstSurname + " " + item.EdSignerSecondSurname +"</td >" +
                                    "<td class='esg-table-body__td' > " + item.EdSignerLaborPosition + "</td >" +
                                    "</tr>";
                                $('#institutionSigners_tbody').append(contentTr);
                            });
                        }
                    }
                });
                if (institutionMajor[Number(rdoId[1])].InstitutionCode.length < 6) {
                    $('#InstitutionCode').parent().addClass('has-error');
                    $('#InstitutionCodeError').css('display', 'block');
                    $("#divInstitutionCode").addClass('esg-has-error');
                    $("#divInstitutionCodeErr").css('display', 'block');
                }
                else {
                    $('#InstitutionCode').parent().removeClass('has-error');
                    $('#InstitutionCodeError').css('display', 'none');
                    $("#divInstitutionCode").removeClass('esg-has-error');
                    $("#divInstitutionCodeErr").css('display', 'none');
                }
                if (institutionMajor[Number(rdoId[1])].MajorCode.length < 6) {
                    $('#MajorId').parent().addClass('has-error');
                    $('#MajorIdError').css('display', 'block');
                    $("#divMajorId").addClass('esg-has-error');
                    $("#divMajorIdErr").css('display', 'block');
                }
                else {
                    $('#MajorId').parent().removeClass('has-error');
                    $('#MajorIdError').css('display', 'none');
                    $("#divMajorId").removeClass('esg-has-error');
                    $("#divMajorIdErr").css('display', 'none');
                }
                $('#InstitutionCode').val(institutionMajor[Number(rdoId[1])].InstitutionCode);
                $('#InstitutionName').val(institutionMajor[Number(rdoId[1])].InstitutionName);
                $('#MajorName').val(institutionMajor[Number(rdoId[1])].MajorName);
                $('#MajorId').val(institutionMajor[Number(rdoId[1])].MajorCode);
                $('#Rvoe').val(institutionMajor[Number(rdoId[1])].RvoeAgreementNumber);
                $('#AuthorizationType').val(institutionMajor[Number(rdoId[1])].AuthorizationType);
                AuthorizationCode = institutionMajor[Number(rdoId[1])].AuthorizationCode;
                $('#EndDate').val(institutionMajor[Number(rdoId[1])].ProgramEndDate);
                $('#StartDate').val(institutionMajor[Number(rdoId[1])].ProgramStartDate);
                $('#divFirstPart').css('display', 'none');
                $('#divSecondPart').css('display', 'block');
                trasncriptDegreeId = institutionMajor[Number(rdoId[1])].TranscriptDegreeId;
                instMajorId = institutionMajor[Number(rdoId[1])].ElectronicDegreeInstMajorId;
                StudyLevel = institutionMajor[Number(rdoId[1])].StudyLevel;
                if ($('#AuthorizationType').val() === "") {
                    $('#AuthorizationType').parent().addClass('has-error');
                    $('#AuthorizationTypeError').css('display', 'block');
                    $("#divAuthorizationType").addClass('esg-has-error');
                    $("#divAuthorizationTypeErr").css('display', 'block');
                }
                if ($('#EndDate').val() === "") {
                    $('#EndDate').parent().addClass('has-error');
                    $('#EndDateError').css('display', 'block');
                    $("#divEndDate").addClass('esg-has-error');
                    $("#divEndDateErr").css('display', 'block');
                    $(".iconCalendar").css('display', 'none');
                }
                else {
                    $('#EndDate').parent().removeClass('has-error');
                    $('#EndDateError').css('display', 'none');
                    $("#divEndDate").removeClass('esg-has-error');
                    $("#divEndDateErr").css('display', 'none');
                    $(".iconCalendar").css('display', 'none');
                }
            }
        }
    });

    $('#btnBackMainIM').click(function () {
        var contentTr = "";
        $('#institutionMajor_table_rows_tbody').empty();
        $('#divFirstPart').css('display', 'block');
        $('#divSecondPart').css('display', 'none');
        IsHideStep = true;
        $.ajax({
            url: urlInstitutionMajor,
            dataType: "json",
            cache: false,
            type: "GET",
            data: { peopleId: PeopleCodeId, operatorId: operatorId },
            success: function (data) {
                if (data.institutionMajors.length > 0) {
                    $('#tblInstitutionMajor').css('display', 'block');
                    var msgINError;
                    var iconINError;
                    var iconSignerError;
                    var iconMajorError;
                    var label;
                    institutionMajor = data.institutionMajors;
                    $.each(data.institutionMajors, function (i, item) {
                        if (item.IsOperatorOfInstitution === 0 && item.InstitutionName !== "") {
                            msgINError = noInstitutionAssigned;
                        }
                        else {
                            msgINError = noInstitutionMapped;
                        }
                        if (item.InstitutionName === "" || item.IsOperatorOfInstitution === 0) {
                            iconINError = "block";
                            label = "block";
                        }
                        else {
                            label = "block";
                            iconINError = "none";
                        }
                        if (item.NumberOfSigners === 0) {
                            iconSignerError = "block";
                        }
                        else {
                            iconSignerError = "none";
                        }
                        if (item.MajorName === "") {
                            iconMajorError = "block";
                        }
                        else {
                            iconMajorError = "none";
                        }
                        contentTr = "<tr class='esg-table-body__row' id=" + i + ">" +
                            "<td class='esg-table-body__td'>" +
                            "<div class='esg-radio' style='top:-1rem'>" +
                            "<input id='rdo_" + i + "' type=radio name='rdoIM' />" +
                            "<label for='rdo_" + i + "'></label>" +
                            "</div >" +
                            "</td>" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display:inline-flex'><label style='display:" + label + "'>" + item.InstitutionName + "</label>" +
                            "<span id='spnIN_" + i + "' class='esg-icon__container spnToolTip' style='cursor:pointer; margin-left:1rem; display:" + iconINError + "'>" +
                            "<svg class='esg-icon--error esg-icon--small'>" +
                            "<use xlink: href='#icon-error'></use></svg>" +
                            "</span>" +
                            "<div id='tltIN_" + i + "' style='z-index: 1; margin-left: 1rem; margin-top: -1rem; position: relative; height:3.125rem; display:none'>" +
                            "<div class='esg-tooltip esg-tooltip--right' role='tooltip'>" +
                            "<div class='esg-tooltip__arrow'></div>" +
                            "<div class='esg-tooltip__content'>" + msgINError + "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div></td>" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display:inline-flex'><label style='display:" + label + "'>" + item.NumberOfSigners + "</label>" +
                            "<span id='spnNS_" + i + "' class='esg-icon__container spnToolTip' style='cursor:pointer; margin-left:1rem; display:" + iconSignerError + "'>" +
                            "<svg class='esg-icon--error esg-icon--small'>" +
                            "<use xlink: href='#icon-error'></use></svg>" +
                            "</span>" +
                            "<div id='tltNS_" + i + "' style='z-index: 1; margin-left: 1rem; margin-top: -1rem; position: relative; height:3.125rem; display:none'>" +
                            "<div class='esg-tooltip esg-tooltip--right' role='tooltip'>" +
                            "<div class='esg-tooltip__arrow'></div>" +
                            "<div class='esg-tooltip__content'>" + noInstitutionSigner + "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div></td >" +
                            "<td class='esg-table-body__td'><label>" + item.ProgramDesc + "</label></td >" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display:inline-flex'><label style='display:" + label + "'>" + item.MajorName + "</label>" +
                            "<span id='spnMN_" + i + "' class='esg-icon__container spnToolTip' style='cursor:pointer; margin-left:1rem; display:" + iconMajorError + "'>" +
                            "<svg class='esg-icon--error esg-icon--small'>" +
                            "<use xlink: href='#icon-error'></use></svg>" +
                            "</span>" +
                            "<div id='tltMN_" + i + "' style='z-index: 1; margin-left: 1rem; margin-top: -1rem; position: relative; height:3.125rem; display:none'>" +
                            "<div class='esg-tooltip esg-tooltip--right' role='tooltip'>" +
                            "<div class='esg-tooltip__arrow'></div>" +
                            "<div class='esg-tooltip__content'>" + noMajorProgram + "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div></td >" +
                            "<td class='esg-table-body__td'><label>" + item.RvoeAgreementNumber + "(" + item.MatricYear + "/" + item.TermDesc + ")" + "</label></td >" +
                            "</tr>";
                        $('#institutionMajor_table_rows_tbody').append(contentTr);
                    });
                }
            }
        });
    });

    $('#btnEditIM').click(function () {
        $('#divEditIM').css('display', 'block');
        $('#divBtnEditIM').css('display', 'none');
        $('#StartDate').attr('readonly', false).attr('disabled', false);
        $('#EndDate').attr('readonly', false).attr('disabled', false);
        $('#EndDate').parent().removeClass('has-error');
        $('#EndDateError').css('display', 'none');
        $("#divEndDate").removeClass('esg-has-error');
        $("#divEndDateErr").css('display', 'none');
        $(".iconCalendar").css('display', 'block');
    });

    $('#btnCancelIM').click(function () {
        $('#divEditIM').css('display', 'none');
        $('#divBtnEditIM').css('display', 'block');
        $('#StartDate').attr('readonly', true).attr('disabled', true);
        $('#EndDate').attr('readonly', true).attr('disabled', true);
    });

    $('#btnSaveIM').click(function () {
        $('#divEditIM').css('display', 'none');
        $('#divBtnEditIM').css('display', 'block');
        $('#StartDate').attr('readonly', true).attr('disabled', true);
        $('#EndDate').attr('readonly', true).attr('disabled', true);
    });

    $('#StartDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY/MM/DD',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $('#EndDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY/MM/DD',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $('#StartDate').datetimepicker().on('dp.change', function (e) {
        $('#StartDate').parent().removeClass('has-error');
        $('#StartDateError').css('display', 'none');
        $("#divStartDate").removeClass('esg-has-error');
        $("#divStartDateErr").css('display', 'none');
        $(".iconCalendar").css('display', 'block');
        var incrementDay = moment(new Date(e.date));
        incrementDay.add(0, 'days');
        $('#EndDate').data('DateTimePicker').minDate(incrementDay);
        $(this).data("DateTimePicker").hide();
    });

    $('#EndDate').datetimepicker().on('dp.change', function (e) {
        var decrementDay = moment(new Date());
        decrementDay.subtract(0, 'days');
        $('#StartDate').data('DateTimePicker').maxDate(decrementDay);
        $(this).data("DateTimePicker").hide();
    });

    $('#btnValidateIM').click(function () {
        IsValid = true;
        $('#divEditIM').css('display', 'none');
        $('#divBtnEditIM').css('display', 'block');
        if ($('#InstitutionCode').val() === "" || $('#InstitutionCode').val().length < 6) {
            $('#InstitutionCode').parent().addClass('has-error');
            $('#InstitutionCodeError').css('display', 'block');
            $("#divInstitutionCode").addClass('esg-has-error');
            $("#divInstitutionCodeErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#InstitutionCode').parent().removeClass('has-error');
            $('#InstitutionCodeError').css('display', 'none');
            $("#divInstitutionCode").removeClass('esg-has-error');
            $("#divInstitutionCodeErr").css('display', 'none');
        }
        if ($('#InstitutionName').val() === "") {
            $('#InstitutionName').parent().addClass('has-error');
            $('#InstitutionNameError').css('display', 'block');
            $("#divInstitutionName").addClass('esg-has-error');
            $("#divInstitutionNameErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#InstitutionName').parent().removeClass('has-error');
            $('#InstitutionNameError').css('display', 'none');
            $("#divInstitutionName").removeClass('esg-has-error');
            $("#divInstitutionNameErr").css('display', 'none');
        }
        if ($('#MajorName').val() === "") {
            $('#MajorName').parent().addClass('has-error');
            $('#MajorNameError').css('display', 'block');
            $("#divMajorName").addClass('esg-has-error');
            $("#divMajorNameErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#MajorName').parent().removeClass('has-error');
            $('#MajorNameError').css('display', 'none');
            $("#divMajorName").removeClass('esg-has-error');
            $("#divMajorNameErr").css('display', 'none');
        }
        if ($('#MajorId').val() === "" || $('#MajorId').val().length < 6) {
            $('#MajorId').parent().addClass('has-error');
            $('#MajorIdError').css('display', 'block');
            $("#divMajorId").addClass('esg-has-error');
            $("#divMajorIdErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#MajorId').parent().removeClass('has-error');
            $('#MajorIdError').css('display', 'none');
            $("#divMajorId").removeClass('esg-has-error');
            $("#divMajorIdErr").css('display', 'none');
        }
        if ($('#EndDate').val() === "") {
            $('#EndDate').parent().addClass('has-error');
            $('#EndDateError').css('display', 'block');
            $("#divEndDate").addClass('esg-has-error');
            $("#divEndDateErr").css('display', 'block');
            $(".iconCalendar").css('display', 'none');
            IsValid = false;
        }
        else {
            $('#EndDate').parent().removeClass('has-error');
            $('#EndDateError').css('display', 'none');
            $("#divEndDate").removeClass('esg-has-error');
            $("#divEndDateErr").css('display', 'none');
            $(".iconCalendar").css('display', 'block');
        }
        if ($('#AuthorizationType').val() === "") {
            $('#AuthorizationType').parent().addClass('has-error');
            $('#AuthorizationTypeError').css('display', 'block');
            $("#divAuthorizationType").addClass('esg-has-error');
            $("#divAuthorizationTypeErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#AuthorizationType').parent().removeClass('has-error');
            $('#AuthorizationTypeError').css('display', 'none');
            $("#divAuthorizationType").removeClass('esg-has-error');
            $("#divAuthorizationTypeErr").css('display', 'none');
        }
        if (Date.parse($('#EndDate').val()) <= Date.parse($('#StartDate').val())) {
            $('#StartDate').parent().addClass('has-error');
            $('#StartDateError').css('display', 'block');
            $("#divStartDate").addClass('esg-has-error');
            $("#divStartDateErr").css('display', 'block');
            $(".iconCalendarStartDate").css('display', 'none');
            IsValid = false;
        }

        if (IsValid) {
            $('#StartDate').attr('readonly', true).attr('disabled', true);
            $('#EndDate').attr('readonly', true).attr('disabled', true);
            $('#StartDate').parent().removeClass('has-error');
            $('#StartDateError').css('display', 'none');
            $("#divStartDate").removeClass('esg-has-error');
            $("#divStartDateErr").css('display', 'none');
            if (!IsHideStep) {
                $('#GraduationReqTypeCatalog').empty();
                $('#SocialServiceLegal').empty();
                $('#StateCatalog').empty();
                $.ajax({
                    url: urlIssuingDegree,
                    dataType: "json",
                    cache: false,
                    type: "GET",
                    data: { peopleId: PeopleCodeId, instMajorId: instMajorId, transcriptDegreeId: trasncriptDegreeId },
                    success: function (data) {
                        if (data.issuingDegree) {
                            issuingDegree = data.issuingDegree;
                            stateCatalog = issuingDegree.StateCatalog;
                            stateCatalogBS = issuingDegree.StateCatalogBS;
                            socialServiceCatalog = data.legalBaseCatalog;
                            $('#issuingDate').val(issuingDegree.IssuingDate);
                            if (issuingDegree.GraduationRequirementCode === 1) {
                                $('#ExaminationDate').val(issuingDegree.ProfExaminationDate);
                                $('#lblProfessionalDate').css('display', 'block');
                                $('#lblPGProfessionalExamDate').css('display', 'block');
                                $('#lblExemptionDate').css('display', 'none');
                                $('#lblPGExcemptionExamDate').css('display', 'none');
                            }
                            else {
                                $('#ExaminationDate').val(issuingDegree.ExemptionProfExaminationDate);
                                $('#lblProfessionalDate').css('display', 'none');
                                $('#lblExemptionDate').css('display', 'block');
                                $('#lblPGProfessionalExamDate').css('display', 'none');
                                $('#lblPGExcemptionExamDate').css('display', 'block');
                            }
                            if (issuingDegree.ShowService) {
                                $('#divChkSocialService').css('display', 'block');
                            }
                            $('#chkSocialService').prop('checked', issuingDegree.HasService);
                            $('#State').val(issuingDegree.State);
                            $.each(issuingDegree.GraduationCatalog, function (i, item) {
                                $('#GraduationReqTypeCatalog').append($("<option />").val(item.GraduationRequirementCode).text(item.GraduationRequirementDesc));
                            });
                            $.each(issuingDegree.StateCatalog, function (i, item) {
                                $('#StateCatalog').append($("<option />").val(item.StateCode).text(item.StateDesc));
                            });
                            $.each(socialServiceCatalog, function (i, item) {
                                $('#SocialServiceLegal').append($("<option />").val(item.Id).text(item.MediumDescription));
                            });
                            $('#SocialServiceLegal').val(issuingDegree.LegalBaseCode);
                            $('#StateCatalog').val(issuingDegree.StateCode);
                            $('#GraduationReqTypeCatalog').val(issuingDegree.GraduationRequirementCode);
                            if ($('#GraduationReqTypeCatalog').val() === "") {
                                $('#GraduationReqTypeCatalog').parent().addClass('has-error');
                                $('#GraduationReqTypeCatalogError').css('display', 'block');
                                $("#divGraduationReqTypeCatalog").addClass('esg-has-error');
                                $("#divGraduationReqTypeCatalogErr").css('display', 'block');
                            }
                            if ($('#State').val() === "") {
                                $('#State').parent().addClass('has-error');
                                $('#StateError').css('display', 'block');
                                $("#divState").addClass('esg-has-error');
                                $("#divStateErr").css('display', 'block');
                            }
                            if ($('#StateCatalog').val() === "" || $('#StateCatalog').val() === null) {
                                $('#StateCatalog').parent().addClass('has-error');
                                $('#StateCatalogError').css('display', 'block');
                                $("#divStateCatalog").addClass('esg-has-error');
                                $("#divStateCatalogErr").css('display', 'block');
                            }
                            if ($('#SocialServiceLegal').val() === "" || $('#SocialServiceLegal').val() === null) {
                                $('#SocialServiceLegal').parent().addClass('has-error');
                                $('#SocialServiceError').css('display', 'block');
                                $("#divSocialService").addClass('esg-has-error');
                                $("#divSocialServiceErr").css('display', 'block');
                            }

                        }
                    }
                });
            }
            $('#liInstitutionMajor').removeClass('esg-is-active');
            $('#liInstitutionMajor').addClass('esg-is-previous');
            $('#liIssuingDegree').addClass('esg-is-active');
            $('#stpInstitutionMajor').css('display', 'none');
            $('#stpIssuingDegree').css('display', 'block');
            $('#btnStpIssuingDegree').css('cursor', 'pointer').css('color', '#0074db').css('text-decoration', '');
        }
    });

    $(document).on('mouseover', '.spnToolTip', function () {
        var id = $(this).attr('id');
        var spanId = id.split('_');
        switch (String(spanId[0])) {
            case 'spnIN':
                $('#tltIN_' + String(spanId[1])).css('display', 'block');
                break;
            case 'spnNS':
                $('#tltNS_' + String(spanId[1])).css('display', 'block');
                break;
            case 'spnMN':
                $('#tltMN_' + String(spanId[1])).css('display', 'block');
                break;
        }
    });

    $(document).on('mouseout', '.spnToolTip', function () {
        var id = $(this).attr('id');
        var spanId = id.split('_');
        switch (String(spanId[0])) {
            case 'spnIN':
                $('#tltIN_' + String(spanId[1])).css('display', 'none');
                break;
            case 'spnNS':
                $('#tltNS_' + String(spanId[1])).css('display', 'none');
                break;
            case 'spnMN':
                $('#tltMN_' + String(spanId[1])).css('display', 'none');
                break;
        }
    });

    // #endregion

    //#region Issuing-Degree
    $('#btnCancelID').click(function () {
        IsHideStep = true;
        CancelSaveIssuingDegree();
        $('#liIssuingDegree').removeClass('esg-is-active');
        $('#liStudent').addClass('esg-is-previous');
        $('#liInstitutionMajor').removeClass('esg-is-previous').addClass('esg-is-active');
        $('#stpInstitutionMajor').css('display', 'block');
        $('#stpIssuingDegree').css('display', 'none');
    });

    $('#btnCancelIssuingDegree').click(function () {
        CancelSaveIssuingDegree();
    });

    $('#btnSaveIssuingDegree').click(function () {
        CancelSaveIssuingDegree();
    });

    $('#btnEditIssuingDegree').click(function () {
        $('#divEditID').css('display', 'none');
        $('#divCancelSaveID').css('display', 'block');
        $('#GraduationReqTypeCatalog').attr('readonly', false).attr('disabled', false);
        $('#ExaminationDate').attr('readonly', false).attr('disabled', false);
        $('#StateCatalog').attr('readonly', false).attr('disabled', false);
        $('#SocialServiceLegal').attr('readonly', false).attr('disabled', false);
        HideErrorMessage();
    });

    $('#ExaminationDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY-MM-DD',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $('#IssuingDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY-MM-DD',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $('#IssuingDate').click(function () {
        $('#IssuingDate').parent().removeClass('has-error');
        $('#IssuingDateError').css('display', 'none');
        $("#divIssuingDate").removeClass('esg-has-error');
        $("#divIssuingDateErr").css('display', 'none');
        $('.iconCalendarIssuingDate').css('display', 'block');
    });

    $('#State').change(function () {
        $('#State').parent().removeClass('has-error');
        $('#StateError').css('display', 'none');
        $("#divState").removeClass('esg-has-error');
        $("#divStateErr").css('display', 'none');
    });

    $('#SocialServiceLegal').change(function () {
        $('#SocialServiceLegal').parent().removeClass('has-error');
        $('#SocialServiceError').css('display', 'none');
        $("#divSocialService").removeClass('esg-has-error');
        $("#divSocialServiceErr").css('display', 'none');
    });

    $('#StateCatalog').change(function () {
        $('#State').val($('#StateCatalog option:selected').text());
    });

    $('#GraduationReqTypeCatalog').change(function () {
        if ($(this).val() === "1") {
            $('#lblProfessionalDate').css('display', 'block');
            $('#lblExemptionDate').css('display', 'none');
            $('#lblPGProfessionalExamDate').css('display', 'block');
            $('#lblPGExcemptionExamDate').css('display', 'none');
        }
        else {
            $('#lblExemptionDate').css('display', 'block');
            $('#lblProfessionalDate').css('display', 'none');
            $('#lblPGProfessionalExamDate').css('display', 'none');
            $('#lblPGExcemptionExamDate').css('display', 'block');
        }
    });

    $('#btnValidateID').click(function () {
        IsValid = true;
        var contentTr = "";
        $('#backgroundStudies_table_rows_tbody').empty();
        if ($('#IssuingDate').val() === "") {
            $('#IssuingDate').parent().addClass('has-error');
            $('#IssuingDateError').css('display', 'block');
            $("#divIssuingDate").addClass('esg-has-error');
            $("#divIssuingDateErr").css('display', 'block');
            $('.iconCalendarIssuingDate').css('display', 'none');
            IsValid = false;
        }
        else {
            $('#IssuingDate').parent().removeClass('has-error');
            $('#IssuingDateError').css('display', 'none');
            $("#divIssuingDate").removeClass('esg-has-error');
            $("#divIssuingDateErr").css('display', 'none');
            $('.iconCalendarIssuingDate').css('display', 'block');
        }
        if ($('#State').val() === "") {
            $('#State').parent().addClass('has-error');
            $('#StateError').css('display', 'block');
            $("#divState").addClass('esg-has-error');
            $("#divStateErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#State').parent().removeClass('has-error');
            $('#StateError').css('display', 'none');
            $("#divState").removeClass('esg-has-error');
            $("#divStateErr").css('display', 'none');
        }
        if ($('#StateCatalog').val() === "" || $('#StateCatalog').val() === null) {
            $('#StateCatalog').parent().addClass('has-error');
            $('#StateCatalogError').css('display', 'block');
            $("#divStateCatalog").addClass('esg-has-error');
            $("#divStateCatalogErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#StateCatalog').parent().removeClass('has-error');
            $('#StateCatalogError').css('display', 'none');
            $("#divStateCatalog").removeClass('esg-has-error');
            $("#divStateCatalogErr").css('display', 'none');
        }
        if ($('#SocialServiceLegal').val() === "" || $('#SocialServiceLegal').val() === null) {
            $('#SocialServiceLegal').parent().addClass('has-error');
            $('#SocialServiceError').css('display', 'block');
            $("#divSocialService").addClass('esg-has-error');
            $("#divSocialServiceErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#SocialServiceLegal').parent().removeClass('has-error');
            $('#SocialServiceError').css('display', 'none');
            $("#divSocialService").removeClass('esg-has-error');
            $("#divSocialServiceErr").css('display', 'none');
        }
        if ($('#GraduationReqTypeCatalog').val() === "") {
            $('#GraduationReqTypeCatalog').parent().addClass('has-error');
            $('#GraduationReqTypeCatalogError').css('display', 'block');
            $("#divGraduationReqTypeCatalog").addClass('esg-has-error');
            $("#divGraduationReqTypeCatalogErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#GraduationReqTypeCatalog').parent().removeClass('has-error');
            $('#GraduationReqTypeCatalogError').css('display', 'none');
            $("#divGraduationReqTypeCatalog").removeClass('esg-has-error');
            $("#divGraduationReqTypeCatalogErr").css('display', 'none');
        }
        if (IsValid) {
            CancelSaveIssuingDegree();
            $.ajax({
                url: urlBackgroundStudies,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { peopleId: PeopleCodeId, ElectronicDegreeInstMajorId: instMajorId },
                success: function (data) {
                    if (data.backgroundStudies) {
                        backgroundStudies = data.backgroundStudies;
                        institutionFolio = data.institutionFolio;
                        $.each(backgroundStudies, function (i, item) {
                            contentTr = "<tr class='esg-table-body__row' id=" + i + ">" +
                                "<td class='esg-table-body__td'>" +
                                "<div class='esg-radio' style='top:-1rem'>" +
                                "<input id='rdoBS_" + i + "' type=radio name='rdoBS' />" +
                                "<label for='rdoBS_" + i + "'></label>" +
                                "</div >" +
                                "</td>" +
                                "<td class='esg-table-body__td' > " + item.InstitutionNameOrigin + "</td >" +
                                "<td class='esg-table-body__td' > " + item.State + "</td >" +
                                "<td class='esg-table-body__td' > " + item.StartDate + "</td >" +
                                "<td class='esg-table-body__td' > " + item.EndDate + "</td >" +
                                "<td class='esg-table-body__td' > " + item.LicenceNumber + "</td >" +
                                "</tr>";
                            $('#backgroundStudies_table_rows_tbody').append(contentTr);
                        });
                    }
                }
            });

            $('#liIssuingDegree').removeClass('esg-is-active').addClass('esg-is-previous');
            $('#liBackgroundStudies').removeClass('esg-is-previous').addClass('esg-is-active');
            $('#stpBackgroundStudies').css('display', 'block');
            $('#stpIssuingDegree').css('display', 'none');
            $('#btnStpBackgroundStu').css('cursor', 'pointer').css('color', '#0074db').css('text-decoration', '');
        }
    });
    //#endregion

    //#region BackgroundStudies
    $('#btnBackBSTable').click(function () {
        DisableBackgrounStudies();
        $('#divBackGroundTable').css('display', 'block');
        $('#divBackGroundInfo').css('display', 'none');
    });

    $('#btnContinueBS').click(function () {
        var rdo = $('input[type=radio][name=rdoBS]:checked').attr('id');
        rdoId = rdo.split('_');
        if (rdoId !== undefined) {
            $.each(backgroundStudies[0].BackgroundStudiesCatalog, function (i, item) {
                $('#TypeBackgroundStudyCatalog').append($("<option />").val(item.BackgroundStudyTypeCode).text(item.BackgroundStudyTypeDesc));
            });
            $.each(stateCatalogBS, function (i, item) {
                $('#StateCatalogBS').append($("<option />").val(item.StateCode).text(item.StateDesc));
            });
            $('#InstitutionOrigin').val(backgroundStudies[Number(rdoId[1])].InstitutionNameOrigin);
            $('#StateBS').val(backgroundStudies[Number(rdoId[1])].State);
            $('#BGStartDate').val(backgroundStudies[Number(rdoId[1])].StartDate);
            $('#BGEndDate').val(backgroundStudies[Number(rdoId[1])].EndDate);
            $('#LicenceNumber').val(backgroundStudies[Number(rdoId[1])].LicenceNumber);
            $('#StateCatalogBS').val(backgroundStudies[Number(rdoId[1])].StateCode);
            $('#divBackGroundTable').css('display', 'none');
            $('#divBackGroundInfo').css('display', 'block');
            if ($('#BGEndDate').val() === "") {
                $('#BGEndDate').parent().addClass('has-error');
                $('#BGEndDateError').css('display', 'block');
                $("#divEndDateBS").addClass('esg-has-error');
                $("#divEndDateBSErr").css('display', 'block');
                $(".iconCalendarBS").css('display', 'none');
            }
            if ($('#StateCatalogBS').val() === "" || $('#StateCatalogBS').val() === null) {
                $('#StateCatalogBS').parent().addClass('has-error');
                $('#StateCatalogBSError').css('display', 'block');
                $("#divStateCatalogBS").addClass('esg-has-error');
                $("#divStateCatalogBSErr").css('display', 'block');
            }
        }
    });

    $('#btnBackBS').click(function () {
        $('#liIssuingDegree').addClass('esg-is-active').removeClass('esg-is-previous');
        $('#liBackgroundStudies').removeClass('esg-is-active');
        $('#stpBackgroundStudies').css('display', 'none');
        $('#stpIssuingDegree').css('display', 'block');
    });

    $('#btnEditBS').click(function () {
        EnableBackgroundStudies();
    });

    $('#btnCancelBS').click(function () {
        DisableBackgrounStudies();
    });

    $('#btnSaveBS').click(function () {
        DisableBackgrounStudies();
    });

    $('#BGStartDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY-MM-DD',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $('#BGEndDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY-MM-DD',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });

    $('#BGStartDate').datetimepicker().on('dp.change', function (e) {
        $('#BSStartDate').parent().removeClass('has-error');
        $('#BSStartDateError').css('display', 'none');
        $("#divBSStartDate").removeClass('esg-has-error');
        $("#divBSStartDateErr").css('display', 'none');
        $(".iconCalendarStartDate").css('display', 'block');
        var incrementDay = moment(new Date(e.date));
        incrementDay.add(0, 'days');
        $('#BGEndDate').data('DateTimePicker').minDate(incrementDay);
        $(this).data("DateTimePicker").hide();
    });

    $('#BGEndDate').datetimepicker().on('dp.change', function (e) {
        var decrementDay = moment(new Date());
        decrementDay.subtract(0, 'days');
        $('#BGStartDate').data('DateTimePicker').maxDate(decrementDay);
        $(this).data("DateTimePicker").hide();
    });

    $('#BGEndDate').click(function () {
        $('#BGEndDate').parent().removeClass('has-error');
        $('#BGEndDateError').css('display', 'none');
        $("#divEndDateBS").removeClass('esg-has-error');
        $("#divEndDateBSErr").css('display', 'none');
        $(".iconCalendarBS").css('display', 'block');
    });

    $('#InstitutionOrigin').change(function () {
        $('#InstitutionOrigin').parent().removeClass('has-error');
        $('#InstitutionOriginError').css('display', 'none');
        $("#divInstitutionOrigin").removeClass('esg-has-error');
        $("#divInstitutionOriginErr").css('display', 'none');
    });


    $('#StateCatalogBS').change(function () {
        $('#StateBS').val($('#StateCatalogBS option:selected').text());
    });

    $('#LicenceNumber').blur(function () {
        if ($('#LicenceNumber').val().length > 5 || $('#LicenceNumber').val().length === 0) {
            $('#LicenceNumber').parent().removeClass('has-hint');
            $('#LicenceNumberError').css('display', 'none');
            $("#divLicenceNumber").removeClass('esg-has-hint');
            $("#divLicenceNumberErr").css('display', 'none');
        }
        else {
            $('#LicenceNumber').parent().addClass('has-hint');
            $('#LicenceNumberError').css('display', 'block');
            $("#divLicenceNumber").addClass('esg-has-hint');
            $("#divLicenceNumberErr").css('display', 'block');
        }
    });

    $('#LicenceNumber').keypress(function () {
        if ($('#LicenceNumber').val().length > 4) {
            $('#LicenceNumber').parent().removeClass('has-hint');
            $('#LicenceNumberError').css('display', 'none');
            $("#divLicenceNumber").removeClass('esg-has-hint');
            $("#divLicenceNumberErr").css('display', 'none');
        }
        else {
            $('#LicenceNumber').parent().addClass('has-hint');
            $('#LicenceNumberError').css('display', 'block');
            $("#divLicenceNumber").addClass('esg-has-hint');
            $("#divLicenceNumberErr").css('display', 'block');
        }
    });

    $('#btnValidateBS').click(function () {
        IsValid = true;
        if ($('#InstitutionOrigin').val() === "") {
            $('#InstitutionOrigin').parent().addClass('has-error');
            $('#InstitutionOriginError').css('display', 'block');
            $("#divInstitutionOrigin").addClass('esg-has-error');
            $("#divInstitutionOriginErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#InstitutionOrigin').parent().removeClass('has-error');
            $('#InstitutionOriginError').css('display', 'none');
            $("#divInstitutionOrigin").removeClass('esg-has-error');
            $("#divInstitutionOriginErr").css('display', 'none');
        }
        if ($('#TypeBackgroundStudyCatalog').val() === "") {
            $('#TypeBackgroundStudyCatalog').parent().addClass('has-error');
            $('#TypeBackgroundStudyError').css('display', 'block');
            $("#divTypeOfBackgroundStudy").addClass('esg-has-error');
            $("#divTypeBackgroundStudyCatalogErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#TypeBackgroundStudyCatalog').parent().removeClass('has-error');
            $('#TypeBackgroundStudyError').css('display', 'none');
            $("#divTypeOfBackgroundStudy").removeClass('esg-has-error');
            $("#divTypeBackgroundStudyCatalogErr").css('display', 'none');
        }
        if ($('#StateCatalogBS').val() === "" || $('#StateCatalogBS').val() === null) {
            $('#StateCatalogBS').parent().addClass('has-error');
            $('#StateCatalogBSError').css('display', 'block');
            $("#divStateCatalogBS").addClass('esg-has-error');
            $("#divStateCatalogBSErr").css('display', 'block');
            IsValid = false;
        }
        else {
            $('#StateCatalogBS').parent().removeClass('has-error');
            $('#StateCatalogBSError').css('display', 'none');
            $("#divStateCatalogBS").removeClass('esg-has-error');
            $("#divStateCatalogBSErr").css('display', 'none');
        }
        if ($('#BGEndDate').val() === "") {
            $('#BGEndDate').parent().addClass('has-error');
            $('#BGEndDateError').css('display', 'block');
            $("#divEndDateBS").addClass('esg-has-error');
            $("#divEndDateBSErr").css('display', 'block');
            $(".iconCalendarBS").css('display', 'none');
            IsValid = false;
        }
        else {
            $('#BGEndDate').parent().removeClass('has-error');
            $('#BGEndDateError').css('display', 'none');
            $("#divEndDateBS").removeClass('esg-has-error');
            $("#divEndDateBSErr").css('display', 'none');
            $(".iconCalendarBS").css('display', 'block');
        }
        if ($('#LicenceNumber').val() !== "" && $('#LicenceNumber').val().length < 5) {
            IsValid = false;
        }
        if (Date.parse($('#BGEndDate').val()) <= Date.parse($('#BGStartDate').val())) {
            $('#BSStartDate').parent().addClass('has-error');
            $('#BSStartDateError').css('display', 'block');
            $("#divBSStartDate").addClass('esg-has-error');
            $("#divBSStartDateErr").css('display', 'block');
            $(".iconCalendarStartDate").css('display', 'none');
            IsValid = false;
        }

        if (IsValid) {
            DisableBackgrounStudies();
            $('#stpBackgroundStudies').css('display', 'none');
            $('#stpPreviewGenerate').css('display', 'block');
            $('#btnStpPreview').css('cursor', 'pointer').css('color', '#0074db').css('text-decoration', '');
            $('#liPreview').addClass('esg-is-active');
            $('#liBackgroundStudies').removeClass('esg-is-active').addClass('esg-is-previous');
            $('#PGFirstSigner').text('');
            $('#PGSecondSigner').text('');
            $('#PGThirdSigner').text('');
            $('#PGDegreeFolio').val(institutionFolio);
            if ($('#PGDegreeFolio').val().length > 40 || $('#PGDegreeFolio').val().length < 1) {
                $('#PGDegreeFolio').parent().addClass('has-error');
                $('#PGDegreeFolioError').css('display', 'block');
                $("#divDegreeFolio").addClass('esg-has-error');
                $("#divDegreeFolioErr").css('display', 'block');
            }
            $.each(Signers, function (i, item) {
                if (i < 3) {
                    switch (i) {
                        case 0:
                            $('#PGFirstSigner').text(item.EdSignerName);
                            break;
                        case 1:
                            $('#PGSecondSigner').text(item.EdSignerName);
                            break;
                        case 2:
                            $('#PGThirdSigner').text(item.EdSignerName);
                            break;
                    }
                }
            });
            if ($('#chkSocialService').is(':checked')) {
                $('#PGCompletedSocialService').text(lblYes);
                FulfilledSocialService = true;
            }
            else {
                $('#PGCompletedSocialService').text(lblNo);
                FulfilledSocialService = false;
            }
            SetInformationPreview();
        }
    });
    //#endregion

    //#region Preview-Generate
    $('#btnBackPG').click(function () {
        $('#stpBackgroundStudies').css('display', 'block');
        $('#stpPreviewGenerate').css('display', 'none');
        $('#liPreview').removeClass('esg-is-active');
        $('#liBackgroundStudies').removeClass('esg-is-previous').addClass('esg-is-active');
    });

    $('#btnEditPG').click(function () {
        $('#PGDegreeFolio').attr('readonly', false).attr('disabled', false);
        $('#PGDegreeFolio').parent().removeClass('has-error');
        $('#PGDegreeFolioError').css('display', 'none');
        $("#divDegreeFolio").removeClass('esg-has-error');
        $("#divDegreeFolioErr").css('display', 'none');
    });

    $('#PGDegreeFolio').blur(function () {
        if ($('#PGDegreeFolio').val().length < 1) {
            $('#PGDegreeFolio').parent().addClass('has-error');
            $('#PGDegreeFolioError').css('display', 'block');
            $("#divDegreeFolio").addClass('esg-has-error');
            $("#divDegreeFolioErr").css('display', 'block');
        }
        else {
            $('#PGDegreeFolio').parent().removeClass('has-error');
            $('#PGDegreeFolioError').css('display', 'none');
            $("#divDegreeFolio").removeClass('esg-has-error');
            $("#divDegreeFolioErr").css('display', 'none');
        }
    });

    $('#PGDegreeFolio').keypress(function () {
        if ($('#PGDegreeFolio').val().length < 1) {
            $('#PGDegreeFolio').parent().addClass('has-error');
            $('#PGDegreeFolioError').css('display', 'block');
            $("#divDegreeFolio").addClass('esg-has-error');
            $("#divDegreeFolioErr").css('display', 'block');
        }
        else {
            $('#PGDegreeFolio').parent().removeClass('has-error');
            $('#PGDegreeFolioError').css('display', 'none');
            $("#divDegreeFolio").removeClass('esg-has-error');
            $("#divDegreeFolioErr").css('display', 'none');
        }
    });

    $('#btnCreate').click(function () {
        if ($('#PGDegreeFolio').val().length >= 1) {
            $("#divGenerateError").css('display', 'none');
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');
            var Model = {
                PeopleCodeId: PeopleCodeId,
                Folio: $('#PGDegreeFolio').val(),
                EducationLevel: StudyLevel,
                InstitutionCode: $('#InstitutionCode').val(),
                InstitutionName: $('#InstitutionName').val(),
                MajorCode: $('#MajorId').val(),
                Major: $('#MajorName').val(),
                MajorStartDate: $('#StartDate').val(),
                MajorEndDate: $('#EndDate').val(),
                AuthorizationTypeCode: AuthorizationCode,
                AuthorizationType: $('#AuthorizationType').val(),
                RvoeAgreementNumber: $('#Rvoe').val(),
                Curp: $('#Curp').val(),
                Name: $('#Name').val(),
                FirstSurname: $('#FirstSurname').val(),
                SecondSurname: $('#SecondSurname').val(),
                Email: $('#Email').val(),
                ExpeditionDate: $('#IssuingDate').val(),
                GraduationRequirementCode: $('#GraduationReqTypeCatalog option:selected').val(),
                GraduationRequirement: $('#GraduationReqTypeCatalog option:selected').text(),
                ExaminationExemptionDate: $('#ExaminationDate').val(),
                FulfilledSocialService: FulfilledSocialService,
                LegalBaseCode: $('#SocialServiceLegal option:selected').val(),
                LegalBase: $('#SocialServiceLegal option:selected').text(),
                FederalEntityCode: $('#StateCatalog option:selected').val(),
                FederalEntity: $('#StateCatalog option:selected').text(),
                OriginInstitution: $('#InstitutionOrigin').val(),
                BackgroundStudyTypeCode: $('#TypeBackgroundStudyCatalog option:selected').val(),
                BackgroundStudyType: $('#TypeBackgroundStudyCatalog option:selected').text(),
                OriginInstFederalEntityCode: $('#StateCatalogBS option:selected').val(),
                OriginInstFederalEntity: $('#StateCatalogBS option:selected').text(),
                BackgroundStudyStartDate: $('#BGStartDate').val(),
                BackgroundStudyEndDate: $('#BGEndDate').val(),
                LicenseNumber: $('#LicenceNumber').val(),
                CreateUserName: operatorId,
                Signer: Signers
            };
            $.ajax({
                url: urlCreate,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { ElectronicDegreeInfo: Model },
                success: function (data) {
                    if (data.id > 0) {
                        $("#divGenerateError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlIndex;
                    }
                    else {
                        $("#divGenerateError").css('display', 'block');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                }
            });
        }
    });
    //#endregion
});

//#region functions
function CancelSaveIssuingDegree() {
    $('#divEditID').css('display', 'block');
    $('#divCancelSaveID').css('display', 'none');
    $('#GraduationReqTypeCatalog').attr('readonly', true).attr('disabled', true);
    $('#ExaminationDate').attr('readonly', true).attr('disabled', true);
    $('#State').attr('readonly', true).attr('disabled', true);
    $('#StateCatalog').attr('readonly', true).attr('disabled', true);
    $('#SocialServiceLegal').attr('readonly', true).attr('disabled', true);
}

function HideErrorMessage() {
    $('#State').parent().removeClass('has-error');
    $('#StateError').css('display', 'none');
    $("#divState").removeClass('esg-has-error');
    $("#divStateErr").css('display', 'none');
    $('#StateCatalog').parent().removeClass('has-error');
    $('#StateCatalogError').css('display', 'none');
    $("#divStateCatalog").removeClass('esg-has-error');
    $("#divStateCatalogErr").css('display', 'none');
    $('#SocialServiceLegal').parent().removeClass('has-error');
    $('#SocialServiceError').css('display', 'none');
    $("#divSocialService").removeClass('esg-has-error');
    $("#divSocialServiceErr").css('display', 'none');
    $('#GraduationReqTypeCatalog').parent().removeClass('has-error');
    $('#GraduationReqTypeCatalogError').css('display', 'none');
    $("#divGraduationReqTypeCatalog").removeClass('esg-has-error');
    $("#divGraduationReqTypeCatalogErr").css('display', 'none');
}

function EnableBackgroundStudies() {
    $('#divEditBS').css('display', 'none');
    $('#divCancelSaveBS').css('display', 'block');
    $('#InstitutionOrigin').attr('readonly', false).attr('disabled', false);
    $('#TypeBackgroundStudyCatalog').attr('readonly', false).attr('disabled', false);
    $('#StateCatalogBS').attr('readonly', false).attr('disabled', false);
    $('#BGStartDate').attr('readonly', false).attr('disabled', false);
    $('#BGEndDate').attr('readonly', false).attr('disabled', false);
    $('#LicenceNumber').attr('readonly', false).attr('disabled', false);
    $('#LicenceNumber').parent().addClass('has-hint');
    $('#LicenceNumberError').css('display', 'block');
    $("#divLicenceNumber").addClass('esg-has-hint');
    $("#divLicenceNumberErr").css('display', 'block');
    $('#StateBS').parent().removeClass('has-error');
    $('#StateBSError').css('display', 'none');
    $("#divStateBS").removeClass('esg-has-error');
    $("#divStateBSErr").css('display', 'none');
    $('#StateCatalogBS').parent().removeClass('has-error');
    $('#StateCatalogBSError').css('display', 'none');
    $("#divStateCatalogBS").removeClass('esg-has-error');
    $("#divStateCatalogBSErr").css('display', 'none');
    $('#BGEndDate').parent().removeClass('has-error');
    $('#BGEndDateError').css('display', 'none');
    $("#divEndDateBS").removeClass('esg-has-error');
    $("#divEndDateBSErr").css('display', 'none');
    $(".iconCalendarBS").css('display', 'block');
    $('#LicenceNumber').parent().removeClass('has-hint');
    $('#LicenceNumberError').css('display', 'none');
    $("#divLicenceNumber").removeClass('esg-has-hint');
    $("#divLicenceNumberErr").css('display', 'none');
}

function DisableBackgrounStudies() {
    $('#divEditBS').css('display', 'block');
    $('#divCancelSaveBS').css('display', 'none');
    $('#InstitutionOrigin').attr('readonly', true).attr('disabled', true);
    $('#TypeBackgroundStudyCatalog').attr('readonly', true).attr('disabled', true);
    $('#StateCatalogBS').attr('readonly', true).attr('disabled', true);
    $('#BGStartDate').attr('readonly', true).attr('disabled', true);
    $('#BGEndDate').attr('readonly', true).attr('disabled', true);
    $('#LicenceNumber').attr('readonly', true).attr('disabled', true);
    $('#StateBS').parent().removeClass('has-error');
    $('#StateBSError').css('display', 'none');
    $("#divStateBS").removeClass('esg-has-error');
    $("#divStateBSErr").css('display', 'none');
    $('#StateCatalogBS').parent().removeClass('has-error');
    $('#StateCatalogBSError').css('display', 'none');
    $("#divStateCatalogBS").removeClass('esg-has-error');
    $("#divStateCatalogBSErr").css('display', 'none');
    $('#BGEndDate').parent().removeClass('has-error');
    $('#BGEndDateError').css('display', 'none');
    $("#divEndDateBS").removeClass('esg-has-error');
    $("#divEndDateBSErr").css('display', 'none');
    $(".iconCalendarBS").css('display', 'block');
    $('#LicenceNumber').parent().removeClass('has-hint');
    $('#LicenceNumberError').css('display', 'none');
    $("#divLicenceNumber").removeClass('esg-has-hint');
    $("#divLicenceNumberErr").css('display', 'none');
}

function SetInformationPreview() {
    $('#PGSCurp').text($('#Curp').val());
    $('#PGSName').text($('#Name').val());
    $('#PGSFirstSurname').text($('#FirstSurname').val());
    $('#PGSSecondSurname').text($('#SecondSurname').val());
    $('#PGSEmail').text($('#Email').val());
    $('#PGInstitutionCode').text($('#InstitutionCode').val());
    $('#PGInstitutionName').text($('#InstitutionName').val());
    $('#PGMajorId').text($('#MajorId').val());
    $('#PGMajorName').text($('#MajorName').val());
    $('#PGMStartDate').text($('#StartDate').val());
    $('#PGMEndDate').text($('#EndDate').val());
    $('#PGAuthorizationType').text($('#AuthorizationType').val());
    $('#PGRove').text($('#Rvoe').val());
    $('#PGGraduationReqType').text($('#GraduationReqTypeCatalog option:selected').text());
    $('#PGProfessionalExamDate').text($('#ExaminationDate').val());
    $('#PGExemptionExamDate').text($('#ExaminationDate').val());
    $('#PSocialService').text($('#SocialServiceLegal option:selected').text());
    $('#PGIDState').text($('#State').val());
    $('#PGIssuingDate').text($('#IssuingDate').val());
    $('#PGInstitutionOrigin').text($('#InstitutionOrigin').val());
    $('#PGTypeBackgroundStudy').text($('#TypeBackgroundStudyCatalog option:selected').text());
    $('#PGBSState').text($('#StateBS').val());
    $('#PGLicenceNumber').text($('#LicenceNumber').val());
}
//#endregion