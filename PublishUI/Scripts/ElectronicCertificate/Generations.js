let PreviewData = {};
let courses = [];
let coursesViewModel;
let PrevFolio = '';
let coursesToAdd = [];
let periodTypes;
let codeFederalEntities;

$(document).ready(function () {
    // #region Variables
    let localeBrowser = "es";
    if (window.navigator !== undefined) {
        if (window.navigator.languages !== undefined) {
            localeBrowser = (window.navigator.languages[0] || window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
        else {
            localeBrowser = (window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
    }
    let StudentStepIsValid = true;
    let PeopleList = '';
    let GenderIdentityEditable = true;
    let PrevCurp = '';
    let PrevBirthDate = '';
    let PrevGenderIdentity = '';
    let StudiesProgramIsValid = true;
    let SelectText = $('#hdnSelectText').val();
    let PrevEntityMapping = '';
    let PrevMajorPeriodType = '';
    let IssuingCertificateIsValid = false;
    let CoursesStepIsValid = false;
    // #endregion Variables

    // #region MainView
    $('#btnGenerate').click(function () {
        window.location.href = urlGenerate;
    });

    $('#divBtnGenerate, #spanBtnGenerate, #btnGenerate').hover(
        () => $('#lblGenerate').css('display', 'flex'),
        () => $('#lblGenerate').css('display', 'none')
    );
    //#endregion MainView

    // #region Student
    $('#PeopleIdSearch').autocomplete({
        source: function (request, response) {
            if (request.term && request.term.length >= 5) {
                $.ajax({
                    url: urlPeople,
                    dataType: 'json',
                    cache: false,
                    type: 'GET',
                    data: { peopleId: request.term }
                }).done(data => {
                    if (data) {
                        PeopleList = data.peopleList;
                        response($.map(PeopleList, (item) => ({ label: item.PeopleCodeId, value: item.PeopleCodeId })));
                    }
                    else {
                        PeopleList = undefined;
                        response(PeopleList);
                    }
                });

                $('.ui-autocomplete').css('min-width', '100px').css('max-width', '100%').css('max-height', '100px')
                    .css('overflow-y', 'auto').css('overflow-x', 'hidden').css('z-index', '9999!important').css('background', 'white');
            }
        }
    });

    $('#btnCleanPeopleId').click(function () {
        $('#PeopleIdSearch').val('');
    });

    $('#PeopleIdSearch').blur(function () {
        if (PeopleList && PeopleList.length > 0) {
            let peopleCodeId = $(this).val();
            if (peopleCodeId !== '') {
                const response = PeopleList.find(e => e.PeopleCodeId === peopleCodeId);
                if (response) {
                    $('#Curp').val(response.Curp);
                    $('#Name').val(response.Name);
                    $('#FirstSurname').val(response.FirstSurname);
                    $('#SecondSurname').val(response.SecondSurname);
                    GenderIdentityEditable = true;
                    if (response.GenderIdentity) {
                        $('#hdnGenderCode').val(response.GenderIdentity);
                        if (response.GenderIdentity === 'F') {
                            GenderIdentityEditable = false;
                            $('#GenderIdentity').val($('#GenderIdentity > option:nth-child(2)').text());
                        }
                        if (response.GenderIdentity === 'M') {
                            GenderIdentityEditable = false;
                            $('#GenderIdentity').val($('#GenderIdentity > option:nth-child(3)').text());
                        }
                    }

                    if (GenderIdentityEditable) {
                        $('#GenderIdentity').val($('#ulGenderIdentity > option:first-child').text());
                    }

                    $('#BirthDate').val(response.BirthDate);
                    $('#PeopleId').val(peopleCodeId);
                    StudentStepIsValid = validateStudent();
                }
            }
        }
    });

    $('#btnCancelStudent').click(function () {
        showPageLoader();
        window.location.href = urlIndex;
    });

    $('#btnValidateStudent').click(function () {
        StudentStepIsValid = validateStudent();
        if (StudentStepIsValid) {
            showPageLoader();
            $.ajax({
                url: urlStudiesPrograms,
                dataType: 'json',
                cache: false,
                type: 'GET',
                data: { peopleId: $('#PeopleId').val() }
            }).done(data => {
                $('#stpStudent').css('display', 'none');
                $('#liStudent').removeClass('esg-is-active').addClass('esg-is-previous');
                $('#liStudiesPrograms').addClass('esg-is-active');
                $('#btnStpStudiesPrograms').css('cursor', 'pointer').css('color', '#0074db');

                if (data && data.length > 0) {
                    let tableBodyContent;
                    let dataAttributes;
                    $.each(data, (iSP, value) => {
                        dataAttributes = '';
                        dataAttributes += `data-has-operator-campus="${value.HasOperatorCampus}" `;
                        dataAttributes += `data-has-rvoe-information="${value.HasRvoeInformation}" `;
                        dataAttributes += `data-has-responsible-campus="${value.HasResponsibleCampus}" `;
                        dataAttributes += `data-has-courses-mapping="${value.HasCoursesMapping}" `;
                        dataAttributes += `data-has-institution-code="${value.HasInstitutionCode}" `;
                        dataAttributes += `data-has-campus-code="${value.HasCampusCode}" `;
                        dataAttributes += `data-has-signing-code="${value.HasSigningInstitution}" `;

                        tableBodyContent += '<tr class="esg-table-body__row">';
                        tableBodyContent += '<td class="esg-table-body__td">';
                        tableBodyContent += `<input id="rdbStudiesProgram_${iSP}" `;
                        tableBodyContent += dataAttributes;
                        tableBodyContent += `type="radio" name="radioSP" value="${value.Id}" style="opacity: 1; position: relative" onchange="selectStudiesProgram(event)" />`;
                        tableBodyContent += '</td>';
                        tableBodyContent += `<td class="esg-table-body__td"><label>${value.Program}</label></td>`;
                        tableBodyContent += `<td class="esg-table-body__td"><label>${value.Term}</label></td>`;
                        tableBodyContent += `<td class="esg-table-body__td"><label>${value.Campus}</label></td>`;
                        tableBodyContent += '<td class="esg-table-body__td">';
                        if (value.HasOperatorCampus && value.HasRvoeInformation && value.HasResponsibleCampus
                            && value.HasCoursesMapping && value.HasInstitutionCode && value.HasOperatorCampus
                            && value.HasCampusCode && value.HasSigningInstitution) {
                            tableBodyContent += '<svg class="esg-icon esg-icon--small esg-icon--success-dark"><use xlink:href="#icon-check"></use></svg></span>';
                        }
                        else {
                            tableBodyContent += '<a ';
                            tableBodyContent += dataAttributes;
                            tableBodyContent += 'onclick="showConflicts(event);"><span class="esg-icon__container no-pointer-events"><svg class="esg-icon esg-icon--small esg-icon--error-dark no-pointer-events"><use xlink:href="#icon-error"></use></svg></span></a>';
                        }
                        tableBodyContent += '</tr>';
                    });
                    $('#studiesPrograms_table_rows_tbody').html(tableBodyContent);
                    $('#divStudiesProgramsInstructions').css('display', 'block');
                    $('#divStudiesPrograms').css('display', 'block');
                }
                else {
                    $('#divNoStudiesPrograms').css('display', 'block');
                }

                $('#divFirstPart').css('display', 'block');
                $('#divSecondPart').css('display', 'none');
                $('#stpStudiesPrograms').css('display', 'block');
                $('#btnContinueStudiesPrograms').attr('disabled', true);

                hidePageLoader();
            }).fail(() => hidePageLoader());
        }
    });

    $('#btnEditStudent').click(function () {
        $('#divEditStudent').css('display', 'block');
        $('#divBtnEditStudent').css('display', 'none');

        enableField('Curp');
        hideError('Curp');
        enableField('BirthDate');
        hideError('BirthDate');
        $(".iconCalendarBirthDate").css('display', 'block');
        if (GenderIdentityEditable) {
            enableField('GenderIdentity');
            hideError('GenderIdentity');
        }

        $('#PeopleIdSearch').attr('disabled', true);
        $('#divStudentFooter').css('display', 'none');

        PrevCurp = $('#Curp').val();
        PrevBirthDate = $('#BirthDate').val();
        PrevGenderIdentity = $('#GenderIdentity').val();
    });

    $('#btnCancelEditStudent').click(function () {
        $('#divEditStudent').css('display', 'none');
        $('#divBtnEditStudent').css('display', 'block');

        disableField('Curp');
        disableField('GenderIdentity');
        disableField('BirthDate');

        $('#PeopleIdSearch').attr('disabled', false);
        $('#divStudentFooter').css('display', 'block');

        $('#Curp').val(PrevCurp);
        $('#BirthDate').val(PrevBirthDate);
        $('#GenderIdentity').val(PrevGenderIdentity);
        PrevCurp = '';
        PrevBirthDate = '';
        PrevGenderIdentity = '';
    });

    $('#btnSaveStudent').click(function () {
        $('#divEditStudent').css('display', 'none');
        $('#divBtnEditStudent').css('display', 'block');

        disableField('Curp');
        disableField('GenderIdentity');
        disableField('BirthDate');

        $('#PeopleIdSearch').attr('disabled', false);
        $('#divStudentFooter').css('display', 'block');

        PrevCurp = '';
        PrevBirthDate = '';
        PrevGenderIdentity = '';
    });

    $('#BirthDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY/MM/DD',
        locale: localeBrowser,
        useCurrent: false,
        date: null
    }).on('dp.change', function (e) {
        $('#BirthDate').parent().removeClass('has-error');
        $('#BirthDateError').css('display', 'none');
        $("#divBirthDate").removeClass('esg-has-error');
        $("#divBirthDateErr").css('display', 'none');
        $(".iconCalendar").css('display', 'block');
        $(this).data("DateTimePicker").hide();
    });

    $('#btnStpStudent, #btnBackStudent').click(function () {
        $('#stpStudent').css('display', 'block');
        $('#stpStudiesPrograms').css('display', 'none');
        $('#stpCourses').css('display', 'none');
        $('#stpIssuingCertificate').css('display', 'none');
        $('#stpPreviewGenerate').css('display', 'none');

        $('#liStudent').removeClass('esg-is-previous').addClass('esg-is-active');
        $('#liStudiesPrograms').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liCourses').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liIssuingCertificate').removeClass('esg-is-active').removeClass('esg-is-previous');
        $('#liPreviewGenerate').removeClass('esg-is-active').removeClass('esg-is-previous');
    });
    // #endregion Student

    // #region Studies Programs
    $('#btnContinueStudiesPrograms').click(function () {
        showPageLoader();
        $.ajax({
            url: urlStudiesProgramDetail,
            dataType: 'json',
            cache: false,
            type: 'GET',
            data: { id: $('#hdnStudiesProgramId').val() }
        }).done(data => {
            if (data && data.studiesProgramDetail) {
                const studiesProgramDetail = data.studiesProgramDetail;
                periodTypes = data.periodTypes;
                codeFederalEntities = data.codeFederalEntities;
                $('#hdnInstitutionCampusId').val(studiesProgramDetail.InstitutionCampusId);
                $('#InstitutionName').val(studiesProgramDetail.InstitutionName);
                $('#InstitutionSepId').val(studiesProgramDetail.InstitutionSepId);
                $('#SigningInstitutionId').val(studiesProgramDetail.SigningInstitutionId);
                $('#CampusName').val(studiesProgramDetail.CampusName);
                $('#CampusSepId').val(studiesProgramDetail.CampusSepId);

                let federalEntityCatalogOptions = `<option value="">${SelectText}</option>`;
                let federalEntityValidValue = false;
                if (codeFederalEntities) {
                    $.each(codeFederalEntities, (_i, value) => {
                        if (studiesProgramDetail.FederalEntityCatalogMappingCode && value.CodeValueKey === studiesProgramDetail.FederalEntityCatalogMappingCode) {
                            federalEntityValidValue = true;
                        }
                        federalEntityCatalogOptions += `<option data-short-desc="${value.ShortDescription}" value="${value.CodeValueKey}">${value.Description}</option>`
                    });
                }
                $('#FederalEntityCatalogMapping').html(federalEntityCatalogOptions);
                if (federalEntityValidValue) {
                    $('#FederalEntityCatalogMapping').val(studiesProgramDetail.FederalEntityCatalogMappingCode);
                }
                else {
                    $('#FederalEntityCatalogMapping').val('');
                }

                let jobTitleOptions = '<option value=""></option>';
                if (studiesProgramDetail.StudiesProgramResponsible) {
                    $('#StudiesProgramResponsible_Curp').val(studiesProgramDetail.StudiesProgramResponsible.Curp);
                    $('#StudiesProgramResponsible_Name').val(studiesProgramDetail.StudiesProgramResponsible.Name);
                    $('#StudiesProgramResponsible_FirstSurname').val(studiesProgramDetail.StudiesProgramResponsible.FirstSurname);
                    $('#StudiesProgramResponsible_SecondSurname').val(studiesProgramDetail.StudiesProgramResponsible.SecondSurname);
                    if (studiesProgramDetail.StudiesProgramResponsible.JobTitleShortDesc) {
                        jobTitleOptions += `<option value="${studiesProgramDetail.StudiesProgramResponsible.JobTitleShortDesc}">${studiesProgramDetail.StudiesProgramResponsible.JobTitle}</option>`;
                        $('#StudiesProgramResponsible_JobTitle').html(jobTitleOptions);
                        $('#StudiesProgramResponsible_JobTitle').val(studiesProgramDetail.StudiesProgramResponsible.JobTitleShortDesc);
                    }
                    else {
                        $('#StudiesProgramResponsible_JobTitle').html(jobTitleOptions);
                        $('#StudiesProgramResponsible_JobTitle').val('');
                    }
                }
                else {
                    $('#StudiesProgramResponsible_Curp').val('');
                    $('#StudiesProgramResponsible_Name').val('');
                    $('#StudiesProgramResponsible_FirstSurname').val('');
                    $('#StudiesProgramResponsible_SecondSurname').val('');
                    $('#StudiesProgramResponsible_JobTitle').html(jobTitleOptions);
                    $('#StudiesProgramResponsible_JobTitle').val('');
                }

                let studyLevelOptions = '<option value=""></option>';
                let periodTypeOptions = `<option value="">${SelectText}</option>`;
                let periodTypeValidValue = false;
                if (periodTypes) {
                    $.each(periodTypes, (_i, value) => {
                        if (studiesProgramDetail.StudiesProgramMajor.PeriodTypeId && value.Id === studiesProgramDetail.StudiesProgramMajor.PeriodTypeId) {
                            periodTypeValidValue = true;
                        }
                        periodTypeOptions += `<option data-short-desc="${value.ShortDescription}" value="${value.Id}">${value.Description}</option>`
                    });
                }
                if (studiesProgramDetail.StudiesProgramMajor) {
                    $('#hdnStudiesProgramMajor_Code').val(studiesProgramDetail.StudiesProgramMajor.Code);
                    $('#StudiesProgramMajor_Name').val(studiesProgramDetail.StudiesProgramMajor.Name);
                    $('#StudiesProgramMajor_Id').val(studiesProgramDetail.StudiesProgramMajor.Id);
                    $('#StudiesProgramMajor_PlanCode').val(studiesProgramDetail.StudiesProgramMajor.PlanCode);
                    $('#StudiesProgramMajor_PeriodType').html(periodTypeOptions);
                    if (periodTypeValidValue) {
                        $('#StudiesProgramMajor_PeriodType').val(studiesProgramDetail.StudiesProgramMajor.PeriodTypeId);
                    }
                    else {
                        $('#StudiesProgramMajor_PeriodType').val('');
                    }
                    if (studiesProgramDetail.StudiesProgramMajor.StudyLevelShortDesc) {
                        studyLevelOptions += `<option value="${studiesProgramDetail.StudiesProgramMajor.StudyLevelShortDesc}">${studiesProgramDetail.StudiesProgramMajor.StudyLevel}</option>`;
                        $('#StudiesProgramMajor_StudyLevel').html(studyLevelOptions);
                        $('#StudiesProgramMajor_StudyLevel').val(studiesProgramDetail.StudiesProgramMajor.StudyLevelShortDesc);
                    }
                    else {
                        $('#StudiesProgramMajor_StudyLevel').html(studyLevelOptions);
                        $('#StudiesProgramMajor_StudyLevel').val('');
                    }
                    $('#StudiesProgramMajor_MinimumGrade').val(studiesProgramDetail.StudiesProgramMajor.MinimumGrade);
                    $('#StudiesProgramMajor_MaximumGrade').val(studiesProgramDetail.StudiesProgramMajor.MaximumGrade);
                    $('#StudiesProgramMajor_MinimumPassingGrade').val(studiesProgramDetail.StudiesProgramMajor.MinimumPassingGrade);
                }
                else {
                    $('#hdnStudiesProgramMajor_Code').val('');
                    $('#StudiesProgramMajor_Name').val('');
                    $('#StudiesProgramMajor_Id').val('');
                    $('#StudiesProgramMajor_PlanCode').val('');
                    $('#StudiesProgramMajor_PeriodType').html(periodTypeOptions);
                    $('#StudiesProgramMajor_PeriodType').val('');
                    $('#StudiesProgramMajor_StudyLevel').html(studyLevelOptions);
                    $('#StudiesProgramMajor_StudyLevel').val('');
                    $('#StudiesProgramMajor_MinimumGrade').val('');
                    $('#StudiesProgramMajor_MaximumGrade').val('');
                    $('#StudiesProgramMajor_MinimumPassingGrade').val('');
                }

                if (studiesProgramDetail.StudiesProgramRvoe) {
                    $('#StudiesProgramRvoe_Number').val(studiesProgramDetail.StudiesProgramRvoe.Number);
                    $('#StudiesProgramRvoe_IssuingDate').val(studiesProgramDetail.StudiesProgramRvoe.IssuingDate);
                }
                else {
                    $('#StudiesProgramRvoe_Number').val('');
                    $('#StudiesProgramRvoe_IssuingDate').val('');
                }

                $('#divFirstPart').css('display', 'none');
                $('#divSecondPart').css('display', 'block');

                setRvoeId(studiesProgramDetail.Id);
            }
            else {
                $('#divFirstPart').css('display', 'none');
                $('#divSecondPart').css('display', 'block');
                cleanStudiesProgramFields();
            }

            hidePageLoader();
        }).fail(() => hidePageLoader());
    });

    $('#btnBackStudiesPrograms').click(function () {
        $('#divFirstPart').css('display', 'block');
        $('#divSecondPart').css('display', 'none');

        cleanStudiesProgramFields();
    });

    $('#btnValidateStudiesProgram').click(function () {
        StudiesProgramIsValid = validateStudiesProgram();
        if (StudiesProgramIsValid) {
            showPageLoader();
            $('#stpStudiesPrograms').css('display', 'none');
            $('#liStudiesPrograms').removeClass('esg-is-active').addClass('esg-is-previous');
            $('#liCourses').addClass('esg-is-active');
            $('#btnStpCourses').css('cursor', 'pointer').css('color', '#0074db');
            showPageLoader();
            courses = [];
            $.ajax({
                url: urlCourses,
                dataType: 'json',
                cache: false,
                type: 'GET',
                data:
                {
                    peopleId: $('#PeopleId').val(),
                    rvoeId: $('#hdnRvoeId').val()
                }
            }).done(data => {
                if (data && data.coursesViewModel) {
                    coursesViewModel = data.coursesViewModel;
                    console.log(coursesViewModel);
                    let average = Number(coursesViewModel.Average).toFixed(3);
                    let averageString = average.toString();
                    averageString = averageString.slice(0, (averageString.indexOf(".")) + 3);

                    $('#TotalCourses').val(coursesViewModel.TotalCourses);
                    $('#TotalCredits').val((coursesViewModel.TotalCredits).toFixed(2));
                    $('#AsignedCourses').val(averageString);
                    $('#Average').val(coursesViewModel.Average);
                    $('#ObtainedCredits').val((coursesViewModel.ObtainedCredits).toFixed(2));

                    $('#courses_table_rows_tbody').children().remove();
                    $.each(coursesViewModel.CoursesDetailViewModel, function (i, item) {
                        let dataAttributes = '';
                        let courseHTML = '';
                        let contentTr = '';
                        courseHTML = `
                            <label>${item.CourseName}</label>
                            <div id="spn_${item.TranscriptDetailId}" class="esg-icon__container spnToolTip" style="cursor:pointer; margin-left:1rem; display:block; line-break:anywhere">
                                <svg class="esg-icon esg-icon--small">
                                    <use xlink: href="#icon-error"></use>
                                </svg>
                                <div id="tlt_${item.TranscriptDetailId}" style="display:none; text-align: left">
                                    <div class="esg-popover esg-popover--bottom" role="tooltip" style="width: 100%; min-width:20rem; left:-9.5rem; margin-top:1.7rem;">
                                        <div class="esg-popover__arrow"></div>
                                            <div class="esg-popover__content">
                                                <div class="esg-container-fluid">
                                                    <div class="esg-row">
                                                        <div class="esg-col-xs-8 esg-grid-overlay">
                                                            <strong>${resources.lblSection}</strong>
                                                        </div>
                                                        <div class="esg-col-xs-4 esg-grid-overlay">
                                                            ${item.Section}
                                                        </div>
                                                    </div>
                                                    <div class="esg-row">
                                                        <div class="esg-col-xs-8 esg-grid-overlay">
                                                            <strong>${resources.lblType1}</strong>
                                                        </div>
                                                        <div class="esg-col-xs-4 esg-grid-overlay">
                                                            ${item.EventType}
                                                        </div>
                                                    </div>
                                                    <div class="esg-row">
                                                        <div class="esg-col-xs-8 esg-grid-overlay">
                                                            <strong>${resources.lblCreditType}</strong>
                                                        </div>
                                                        <div class="esg-col-xs-4 esg-grid-overlay">
                                                            ${item.CreditType}
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `;
                        let checked = item.IsInclude == true ? "checked=checked" : "";
                        let average = (item.FinalGrade).toFixed(3);
                        let averageString = average.toString();
                        averageString = averageString.slice(0, (averageString.indexOf(".")) + 3);
                        const roundFinalGrade = Number(item.FinalGrade) != 10 && Number(item.FinalGrade) != 5 ? Number(averageString) : item.FinalGrade.toFixed(0);
                        dataAttributes = '';
                        dataAttributes += `data-transcriptdetailid=${item.TranscriptDetailId} `;
                        dataAttributes += `data-graderemarkid=${Number(item.GradeRemarkId)} `;
                        dataAttributes += `data-subjecttypeid=${Number(item.SubjectTypeId)} `;
                        dataAttributes += `data-includedincertificate=${item.IsInclude} `;
                        dataAttributes += `data-credit=${Number(item.Credit)} `;
                        dataAttributes += `data-finalgrade=${roundFinalGrade} `;
                        dataAttributes += `data-coursecycle="${item.CourseCycle}" `;
                        dataAttributes += `data-eventname="${item.CourseName}" `;
                        dataAttributes += `data-sepid="${item.SepId}" `;
                        dataAttributes += `data-transcriptdetailcertificateid=${item.TranscriptDetailCertificateId} `;
                        dataAttributes += `data-coursecycleid=${Number(item.CourseCycleId)} `;
                        contentTr = "<tr class='esg-table-body__row' id=" + i + ">" +
                            "<td class='esg-table-body__td'>" +
                            "<div class='esg-checkbox'>" +
                            "<input id='chxInclude_" + item.TranscriptDetailId + "'" + dataAttributes + checked + " disabled='disabled'" +
                            "name='checkCourses' style='opacity:1;position:relative' type ='checkbox' onchange='selectCourse(event)'/> " +
                            "</div>" +
                            "</td>" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display: flex; align-items: center; height:2.75rem '>" +
                            "<label> " + item.CourseCycle + "</label > " +
                            "</div></td >" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display: flex; align-items: center; height:2.75rem '>" +
                            "<label> " + item.SepId + "</label > " +
                            "</div></td >" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display: flex; align-items: center; height:2.75rem '>" +
                            courseHTML +
                            "</div></td >" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display: flex; align-items: center; height:2.75rem '>" +
                            "<label> " + roundFinalGrade + "</label > " +
                            "</div></td >" +
                            "<td class='esg-table-body__td'>" +
                            "<select class='esg-form__input' id='RemarksPlace" + item.TranscriptDetailId + "' name='RemarksPlace'" + dataAttributes +
                            " disabled='disabled'" +
                            "onchange = 'selectRemark(event)' required value=" + item.GradeRemarkId + " /> " +
                            "</td >" +
                            "<td class='esg-table-body__td'>" +
                            "<div id='divSubjectsPlace_" + item.TranscriptDetailId + "'>" +
                            "<select class='esg-form__input' id='SubjectsPlace_" + item.TranscriptDetailId + "' name='SubjectsPlace'" + dataAttributes +
                            " disabled='disabled'" +
                            "onchange='selectSubject(event)' required value=" + item.SubjectTypeId + "/>" +
                            "</div>" +
                            "</td >" +
                            "<td class='esg-table-body__td'>" +
                            "<div style='display: flex; align-items: center; height:2.75rem '>" +
                            "<label> " + (item.Credit).toFixed(2) + "</label > " +
                            "</div></td >" +
                            "</tr>";
                        $('#courses_table_rows_tbody').append(contentTr);

                        const gradeIndex =
                            coursesViewModel.GradeRemarks.findIndex(x => x.Id === Number(item.GradeRemarkId));
                        const subjectIndex =
                            coursesViewModel.Subjects.findIndex(x => x.Id === Number(item.SubjectTypeId));

                        courses.push({
                            TranscriptDetailId: item.TranscriptDetailId,
                            GradeRemarkId: Number(item.GradeRemarkId),
                            SubjectTypeId: Number(item.SubjectTypeId),
                            IsInclude: item.IsInclude,
                            Credit: Number(item.Credit),
                            FinalGrade: roundFinalGrade,
                            CourseCycle: item.CourseCycle,
                            EventName: item.CourseName,
                            SepId: item.SepId,
                            TranscriptDetailCertificateId: item.TranscriptDetailCertificateId,
                            CourseCode: item.CourseCode,
                            GradeRemark: coursesViewModel.GradeRemarks[gradeIndex].Desc,
                            SubjectType: coursesViewModel.Subjects[subjectIndex].Desc,
                            IncludeInTemporalTable: true,
                            CourseCycleId: Number(item.CourseCycleId)
                        });

                        setRemarks(coursesViewModel.GradeRemarks, "RemarksPlace" + item.TranscriptDetailId, Number(item.GradeRemarkId));
                        setSubjects(coursesViewModel.Subjects, "SubjectsPlace_" + item.TranscriptDetailId, Number(item.SubjectTypeId));
                    });
                }

                hidePageLoader();
                $('#stpCourses').css('display', 'block');
            });
        }
    });

    $('#btnEditStudiesProgram').click(function () {
        $('#divEditStudiesProgram').css('display', 'block');
        $('#divBtnEditStudiesProgram').css('display', 'none');

        enableField('FederalEntityCatalogMapping');
        hideError('FederalEntityCatalogMapping');
        enableField('StudiesProgramMajor_PeriodType');
        hideError('StudiesProgramMajor_PeriodType');

        $('#divStudiesProgramFooter').css('display', 'none');

        PrevEntityMapping = $('#FederalEntityCatalogMapping').val();
        PrevMajorPeriodType = $('#StudiesProgramMajor_PeriodType').val();
    });

    $('#btnCancelEditStudiesProgram').click(function () {
        $('#divEditStudiesProgram').css('display', 'none');
        $('#divBtnEditStudiesProgram').css('display', 'block');

        disableField('FederalEntityCatalogMapping');
        disableField('StudiesProgramMajor_PeriodType');

        $('#divStudiesProgramFooter').css('display', 'block');

        $('#FederalEntityCatalogMapping').val(PrevEntityMapping);
        $('#StudiesProgramMajor_PeriodType').val(PrevMajorPeriodType);
        PrevEntityMapping = '';
        PrevMajorPeriodType = '';
    });

    $('#btnSaveStudiesProgram').click(function () {
        $('#divEditStudiesProgram').css('display', 'none');
        $('#divBtnEditStudiesProgram').css('display', 'block');

        disableField('FederalEntityCatalogMapping');
        disableField('StudiesProgramMajor_PeriodType');

        $('#divStudiesProgramFooter').css('display', 'block');

        PrevEntityMapping = '';
        PrevMajorPeriodType = '';
    });

    $('#btnCloseModalStudiesProgramConflicts').click(function () {
        $('#Overlaydiv').css('display', 'none');
        $('#divModalStudiesProgramConflicts').css('display', 'none');
    });

    $('#btnStpStudiesPrograms, #btnBackStudyPrograms').click(function () {
        if (StudentStepIsValid && $('#PeopleId').val()) {
            $('#stpStudent').css('display', 'none');
            $('#stpStudiesPrograms').css('display', 'block');
            $('#stpCourses').css('display', 'none');
            $('#stpIssuingCertificate').css('display', 'none');
            $('#stpPreviewGenerate').css('display', 'none');

            $('#liStudiesPrograms').removeClass('esg-is-previous').addClass('esg-is-active');
            $('#liCourses').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liIssuingCertificate').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liPreviewGenerate').removeClass('esg-is-active').removeClass('esg-is-previous');

            coursesViewModel = {};
            $('#TotalCourses').val('');
            $('#TotalCredits').val('');
            $('#AsignedCourses').val('');
            $('#Average').val('');
            $('#ObtainedCredits').val('');
            $("#divErrorCycle").css('display', 'none');
            $("#divError").css('display', 'none');
        }
    });
    // #endregion Studies Programs

    // #region Courses
    $(document).on('mouseover', '.spnToolTip', function () {
        var id = $(this).attr('id');
        var index = id.split('_')[1];
        $(`#tlt_${index}`).css('display', 'block');
    });

    $(document).on('mouseout', '.spnToolTip', function () {
        var id = $(this).attr('id');
        var index = id.split('_')[1];
        $(`#tlt_${index}`).css('display', 'none');
    });

    $('#btnEditCourses').click(function () {
        $.each(courses, function (i, item) {
            $("#chxInclude_" + item.TranscriptDetailId).removeAttr('disabled');
            $("#SubjectsPlace_" + item.TranscriptDetailId).removeAttr('disabled');
            $("#RemarksPlace" + item.TranscriptDetailId).removeAttr('disabled');
            $("#inputSepId_" + item.TranscriptDetailId).removeAttr('disabled');
            $("#divInputSep_" + item.TranscriptDetailId).removeClass('esg-has-feedback esg-has-error');
        });

        // Hide buttons
        $('#divEditCourses').css('display', 'block');
        $('#divBtnEditCourses').css('display', 'none');
        $('#divCoursesFooter').css('display', 'none');
    });

    $('#btnStpCourses, #btnBackCourses').click(function () {
        if (StudentStepIsValid && StudiesProgramIsValid) {
            $('#stpCourses').css('display', 'block');
            $('#stpStudiesPrograms').css('display', 'none');
            $('#stpStudent').css('display', 'none');
            $('#stpIssuingCertificate').css('display', 'none');
            $('#stpPreviewGenerate').css('display', 'none');

            $('#liCourses').removeClass('esg-is-previous').addClass('esg-is-active');
            $('#liStudiesPrograms').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liStudent').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liIssuingCertificate').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liPreviewGenerate').removeClass('esg-is-active').removeClass('esg-is-previous');

            $('#courses_table_rows_tbody').val("");

            $("#divErrorCycle").css('display', 'none');
            $("#divError").css('display', 'none');
        }
    });

    $('#btnCancelCourses').click(function () {
        $('#divEditCourses').css('display', 'none');
        $('#divBtnEditCourses').css('display', 'block');
        $('#divCoursesFooter').css('display', 'block');
        disableCoursesFields();
    });

    $('#btnSaveCourses').click(function () {
        showPageLoader();
        CoursesStepIsValid = true;

        let courseCycleValid = true;
        $.each(courses, function (i, item) {
            if (item.SubjectTypeId === 0) {
                $("#divSubjectsPlace_" + item.TranscriptDetailId).addClass('esg-has-feedback esg-has-error');
                CoursesStepIsValid = false;
            }
            else if (item.SubjectTypeId !== 0) {
                $("#divSubjectsPlace_" + item.TranscriptDetailId).removeClass('esg-has-feedback esg-has-error');
            }
            if (item.IsInclude && (item.SepId === undefined || item.SepId.trim() === "" || item.SepId === null)) {
                $("#divInputSep_" + item.TranscriptDetailId).addClass('esg-has-feedback esg-has-error');
                CoursesStepIsValid = false;
            }
            else if (item.SepId !== undefined) {
                $("#divInputSep_" + item.TranscriptDetailId).removeClass('esg-has-feedback esg-has-error');
            }
            if (item.IsInclude && (item.CourseCycle === undefined || item.CourseCycle === null || item.CourseCycle === "")) {
                courseCycleValid = false;
                CoursesStepIsValid = false;
            }
        });

        if (!courseCycleValid) {
            $("#divErrorCycle").css('display', 'block');
        }
        else {
            $("#divErrorCycle").css('display', 'none');
        }

        if (CoursesStepIsValid) {
            $.ajax({
                url: urlSaveCourses,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { academicPlanCourseDetails: courses },
                success: function (response) {
                    if (response.id === 1) {
                        $('#divEditCourses').css('display', 'none');
                        $('#divBtnEditCourses').css('display', 'block');
                        $('#divCoursesFooter').css('display', 'block');
                        disableCoursesFields();
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
        hidePageLoader();
    });

    $('#btnContinueCourses').click(function () {
        showPageLoader();
        CoursesStepIsValid = true;
        let includedCourses = 0;
        let courseCycleValid = true;
        $.each(courses, function (i, item) {
            if (item.SubjectTypeId === 0) {
                $("#divSubjectsPlace_" + item.TranscriptDetailId).addClass('esg-has-feedback esg-has-error');
                CoursesStepIsValid = false;
            }
            else if (item.SubjectTypeId !== 0) {
                $("#divSubjectsPlace_" + item.TranscriptDetailId).removeClass('esg-has-feedback esg-has-error');
            }
            if (item.IsInclude && (item.SepId === undefined || item.SepId.trim() === "" || item.SepId === null)) {
                $("#divInputSep_" + item.TranscriptDetailId).addClass('esg-has-feedback esg-has-error');
                CoursesStepIsValid = false;
            }
            else if (item.SepId !== undefined) {
                $("#divInputSep_" + item.TranscriptDetailId).removeClass('esg-has-feedback esg-has-error');
            }
            if (item.IsInclude) {
                includedCourses++;
            }
            if (item.IsInclude && (item.CourseCycle === undefined || item.CourseCycle === null || item.CourseCycle === "")) {
                courseCycleValid = false;
                CoursesStepIsValid = false;
            }

            const grade =
                coursesViewModel.GradeRemarks.find(x => x.Id === Number(item.GradeRemarkId));
            item.GradeRemarkShortDesc = grade.Code;

            const subject =
                coursesViewModel.Subjects.find(x => x.Id === Number(item.SubjectTypeId));
            item.SubjectTypeShortDesc = subject.Code;
        });

        if (!courseCycleValid) {
            $("#divErrorCycle").css('display', 'block');
        }
        else {
            $("#divErrorCycle").css('display', 'none');
        }

        if (includedCourses === 0) {
            CoursesStepIsValid = false;
            $("#divError").css('display', 'block');
            $("#Processing").css('display', 'none');
            $("#Overlaydiv").css('display', 'none');
        }

        if (CoursesStepIsValid) {
            $('#stpCourses').css('display', 'none');
            $('#stpIssuingCertificate').css('display', 'block');
            $('#liIssuingCertificate').addClass('esg-is-active');
            $('#btnStpIssuingCertificate').css('cursor', 'pointer').css('color', '#0074db');
            $('#liCourses').removeClass('esg-is-active').addClass('esg-is-previous');
            $('#btnStpCourses').removeClass('esg-is-active').addClass('esg-is-previous');
            updateIssuingCertificateData();
            CoursesStepIsValid = true;
        }
        hidePageLoader();
    });

    $('#btnAddCourse').click(function () {
        coursesToAdd = [];
        var newUrl = urlAddCourse;
        newUrl = newUrl.replace("param-peopleId", $('#PeopleId').val());
        newUrl = newUrl.replace("param-rvoeId", $('#hdnRvoeId').val());
        $("#ajaxpanel_AddCourse").load(encodeURI(newUrl));
    });

    $(document).on("click", ".checkboxCourse", function () {
        if ($(this).is(':checked')) {
            const courseIndex =
                courses.findIndex(x => x.TranscriptDetailId === Number($(this).attr("data-transcriptdetailid")));
            if (courseIndex == -1) {
                coursesToAdd.push({
                    TranscriptDetailId: Number($(this).attr("data-transcriptdetailid")),
                    CourseCode: $(this).attr("data-coursecode"),
                    CourseName: $(this).attr("data-coursename"),
                    FinalGrade: Number($(this).attr("data-finalgrade")),
                    Credit: Number($(this).attr("data-credit")),
                    Section: $(this).attr("data-section"),
                    EventType: $(this).attr("data-eventtype"),
                    CreditType: $(this).attr("data-credittype"),
                    GradeRemarkId: Number($(this).attr("data-graderemarkid")),
                    SubjectTypeId: Number($(this).attr("data-subjecttypeid")),
                    CourseCycle: $(this).attr("data-coursecycle"),
                    IsInclude: true,
                    CourseCycleId: Number($(this).attr("data-coursecycleid"))
                });
            }
        } else {
            let courseIndex = coursesToAdd.findIndex(x => x.TranscriptDetailId ===
                Number($(this).attr("data-transcriptdetailid")));
            if (courseIndex > -1) {
                coursesToAdd.splice(courseIndex, 1);
            }
        }
    });

    $(document).on("click", "#ddlAddCourse", function () {
        if ($('#ullCourses').css('display') === 'none')
            $('#ullCourses').css('display', 'block');
        else
            $('#ullCourses').css('display', 'none');
    });

    $(document).on("click", "#btnClose", function () {
        $('#divAddCourseModal').hide();
    });

    $(document).on("click", "#btnCancelAddCourse", function () {
        $('#divAddCourseModal').hide();
    });

    $(document).on("click", "#btnApplyAddCourse", function () {
        $.each(coursesToAdd, function (i, item) {
            var index = courses.length;
            courseHTML = `
                <label>${item.CourseName}</label>
                <div id="spn_${item.TranscriptDetailId}" class="esg-icon__container spnToolTip" style="cursor:pointer; margin-left:1rem; display:block; line-break:anywhere">
                    <svg class="esg-icon esg-icon--small">
                        <use xlink: href="#icon-error"></use>
                    </svg>
                    <div id="tlt_${item.TranscriptDetailId}" style="display:none; text-align: left">
                        <div class="esg-popover esg-popover--bottom" role="tooltip" style="width: 100%; min-width:20rem; left:-9.5rem; margin-top:1.7rem;">
                            <div class="esg-popover__arrow"></div>
                                <div class="esg-popover__content">
                                    <div class="esg-container-fluid">
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblSection}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${item.Section}
                                            </div>
                                        </div>
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblType1}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${item.EventType}
                                            </div>
                                        </div>
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblCreditType}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${item.CreditType}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            let checked = "checked=checked";
            dataAttributes = '';
            dataAttributes += `data-transcriptdetailid=${item.TranscriptDetailId} `;
            dataAttributes += `data-graderemarkid=${Number(item.GradeRemarkId)} `;
            dataAttributes += `data-subjecttypeid=${Number(item.SubjectTypeId)} `;
            dataAttributes += `data-includedincertificate=${item.IsInclude} `;
            dataAttributes += `data-credit=${Number(item.Credit)} `;
            dataAttributes += `data-finalgrade=${Number(item.FinalGrade)} `;
            dataAttributes += `data-coursecycle="${item.CourseCycle}" `;
            dataAttributes += `data-eventname="${item.CourseName}" `;
            dataAttributes += `data-sepid="${item.SepId}" `;
            dataAttributes += `data-transcriptdetailcertificateid=${item.TranscriptDetailCertificateId} `;
            dataAttributes += `data-coursecycleid=${Number(item.CourseCycleId)} `;
            let average = (item.FinalGrade).toFixed(3);
            let averageString = average.toString();
            averageString = averageString.slice(0, (averageString.indexOf(".")) + 3);
            const finalGrade = Number(item.FinalGrade) != 10 && Number(item.FinalGrade) != 5 ? Number(averageString) : item.FinalGrade.toFixed(0);
            var contentTr = "<tr class='esg-table-body__row' id=" + item.TranscriptDetailId + ">" +
                "<td class='esg-table-body__td'>" +
                "<div class='esg-checkbox'>" +
                "<input id='chxInclude_" + item.TranscriptDetailId + "' " + checked +
                " disabled='disabled'" + dataAttributes +
                "name='checkCourses' style='opacity:1;position:relative' type ='checkbox' onchange='selectCourse(event)'/> " +
                "</div>" +
                "</td>" +
                "<td class='esg-table-body__td'>" +
                "<div style='display: flex; align-items: center; height:2.75rem '>" +
                "<label> " + item.CourseCycle + "</label > " +
                "</div></td >" +
                "<td class='esg-table-body__td'>" +
                "<div style='display: flex; align-items: center; height:2.75rem '>" +
                "<div id='divInputSep_" + item.TranscriptDetailId + "'>" +
                "<input class='esg-form__input' type='text'" + dataAttributes +
                "id='inputSepId_" + item.TranscriptDetailId + "' disabled onchange='selectSepId(event)'/>" +
                "</div>" +
                "</div></td >" +
                "<td class='esg-table-body__td'>" +
                "<div style='display: flex; align-items: center; height:2.75rem '>" +
                courseHTML +
                "</div></td >" +
                "<td class='esg-table-body__td'>" +
                "<div style='display: flex; align-items: center; height:2.75rem '>" +
                "<label> " + finalGrade + "</label > " +
                "</div></td >" +
                "<td class='esg-table-body__td'>" +
                "<select class='esg-form__input' id='RemarksPlace" + item.TranscriptDetailId + "' name='RemarksPlace'" +
                dataAttributes +
                " disabled='disabled'" +
                "onchange = 'selectRemark(event)' required value = " + item.GradeRemarkId + " /> " +
                "</td >" +
                "<td class='esg-table-body__td'>" +
                "<div id='divSubjectsPlace_" + item.TranscriptDetailId + "'>" +
                "<select class='esg-form__input' id='SubjectsPlace_" + item.TranscriptDetailId + "' name='SubjectsPlace'" +
                dataAttributes +
                " disabled='disabled'" +
                "onchange='selectSubject(event)' required value=" + item.SubjectTypeId +
                "/>" +
                "</div>" +
                "</td >" +
                "<td class='esg-table-body__td'>" +
                "<div style='display: flex; align-items: center; height:2.75rem '>" +
                "<label> " + (item.Credit).toFixed(2) + "</label > " +
                "</div></td >" +
                "</tr>";
            $('#courses_table_rows_tbody').append(contentTr);

            const gradeIndex =
                coursesViewModel.GradeRemarks.findIndex(x => x.Id === Number(item.GradeRemarkId));
            const subjectIndex =
                coursesViewModel.Subjects.findIndex(x => x.Id === Number(item.SubjectTypeId));

            courses.push({
                TranscriptDetailId: item.TranscriptDetailId,
                GradeRemarkId: Number(item.GradeRemarkId),
                SubjectTypeId: Number(item.SubjectTypeId),
                IsInclude: item.IsInclude,
                Credit: Number(item.Credit),
                FinalGrade: item.FinalGrade != 10 && item.FinalGrade != 5 ? finalGrade : item.FinalGrade.toFixed(0),
                CourseCycle: item.CourseCycle,
                EventName: item.CourseName,
                SepId: item.SepId,
                TranscriptDetailCertificateId: item.TranscriptDetailCertificateId,
                CourseCode: item.CourseCode,
                GradeRemark: coursesViewModel.GradeRemarks[gradeIndex].Desc,
                SubjectType: coursesViewModel.Subjects[subjectIndex].Desc,
                IncludeInTemporalTable: false,
                CourseCycleId: Number(item.CourseCycleId)
            });

            setRemarks(coursesViewModel.GradeRemarks, "RemarksPlace" + item.TranscriptDetailId, Number(item.GradeRemarkId));
            setSubjects(coursesViewModel.Subjects, "SubjectsPlace_" + item.TranscriptDetailId, Number(item.SubjectTypeId));

            calculateCredits();

            $("#divError").css('display', 'none');
        });

        $('#divAddCourseModal').hide();
    });

    // #endregion Courses

    // #region Issuing Certificate
    $('#btnStpIssuingCertificate, #btnBackIssuingCertificate').click(function () {
        if (StudentStepIsValid && StudiesProgramIsValid && CoursesStepIsValid) {
            $('#stpIssuingCertificate').css('display', 'block');
            $('#stpStudiesPrograms').css('display', 'none');
            $('#stpStudent').css('display', 'none');
            $('#stpCourses').css('display', 'none');
            $('#stpPreviewGenerate').css('display', 'none');

            $('#liIssuingCertificate').removeClass('esg-is-previous').addClass('esg-is-active');
            $('#liStudiesPrograms').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liStudent').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liCourses').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liPreviewGenerate').removeClass('esg-is-active').removeClass('esg-is-previous');
        }
    });

    $('#btnValidateIssuingCertificate').click(function () {
        IssuingCertificateIsValid = validateIssuingCertificate();
        if (IssuingCertificateIsValid) {
            const totalTypeCode = 79;
            let isTotal = $('#CertificationType').val() == totalTypeCode;
            let totalCoursesMatch = $('#TotalCourses').val() === $('#AsignedCourses').val();
            let totalCreditsMatch = $('#TotalCredits').val() === $('#ObtainedCredits').val();

            if (isTotal) {
                if (totalCoursesMatch && totalCreditsMatch) {
                    showPreviewData();
                }
                else {
                    showIssuingCertificateWarning(totalCoursesMatch, totalCreditsMatch);
                }
            }
            else {
                showPreviewData();
            }
        }
    });

    $('#btnStpPreviewGenerate').click(function () {
        if (StudentStepIsValid && StudiesProgramIsValid && CoursesStepIsValid && IssuingCertificateIsValid) {
            $('#stpPreviewGenerate').css('display', 'block');
            $('#stpStudiesPrograms').css('display', 'none');
            $('#stpStudent').css('display', 'none');
            $('#stpCourses').css('display', 'none');
            $('#stpIssuingCertificate').css('display', 'none');

            $('#liPreviewGenerate').removeClass('esg-is-previous').addClass('esg-is-active');
            $('#liStudiesPrograms').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liStudent').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liCourses').removeClass('esg-is-active').removeClass('esg-is-previous');
            $('#liIssuingCertificate').removeClass('esg-is-active').removeClass('esg-is-previous');
        }
    });

    $('#CertificationType, #IssuingDate, #IssuingPlace').blur(function () {
        validateIssuingCertificate(true);
    });

    $('#CertificationType, #IssuingDate, #IssuingPlace').change(function () {
        validateIssuingCertificate(true);
    });

    $('#btnCancelWarningIssuingCertificate').click(function () {
        $('#divIssuingCertificateWarning').css('display', 'none');
    });

    $('#btnAcceptWarningIssuingCertificate').click(function () {
        showPreviewData();
    });

    // #endregion Issuing Certificate

    // #region Preview-Generate
    $('#btnEditFolio').click(function (e) {
        e.preventDefault();
        $('#divEditFolio').css('display', 'none');
        $('#divSaveCancelFolio').css('display', 'block');
        $('#inputFolio').removeAttr('disabled');
    });

    $('#btnCancelEditFolio').click(function (e) {
        e.preventDefault();
        $('#inputFolio').val(PrevFolio);
        $('#divSaveCancelFolio').css('display', 'none');
        $('#divEditFolio').css('display', 'block');
        $('#inputFolio').attr('disabled', 'disabled');
        validateFolio();
    });

    $('#btnSaveFolio').click(function (e) {
        e.preventDefault();
        let folio = $('#inputFolio').val();
        let folioIsValid = folio && folio.length >= 1 && folio.length <= 40;
        if (folioIsValid) {
            $('#divSaveCancelFolio').css('display', 'none');
            $('#divEditFolio').css('display', 'block');
            $('#inputFolio').attr('disabled', 'disabled');
            PrevFolio = folio;
        }
    });

    $('#inputFolio').change(validateFolio).keyup(validateFolio).blur(validateFolio);

    $('#btnGenerateCertificate').click(function () {
        let isValidFolio = validateFolio();

        if (isValidFolio) {
            showPageLoader();
            $.ajax({
                url: urlGenerateCertificate,
                dataType: "json",
                cache: false,
                type: "POST",
                data: PreviewData
            })
                .done(function (response) {
                    if (response.id == 1) {
                        $('#divGenerateError').css('display', 'none');
                        window.location.replace(urlIndex);
                    }
                    else {
                        $('#divGenerateError').css('display', 'block');
                        hidePageLoader();
                    }
                })
                .fail(function () {
                    window.location.replace(urlUnauthorized);
                });;
        }
        else {
            $(window).scrollTop(0);
        }
    });
    // #endregion Preview-Generate
});

//#region Functions

// #region MainView
// #endregion MainView

// #region Student
function validateStudent() {
    let isValid = true;
    isValid = validateIntupField('PeopleId') && isValid;
    isValid = validateIntupField('Curp') && isValid;
    isValid = validateIntupField('Name') && isValid;
    isValid = validateIntupField('FirstSurname') && isValid;
    isValid = validateIntupField('GenderIdentity') && isValid;
    let isValidBirthDate = validateIntupField('BirthDate');
    isValid = isValidBirthDate && isValid;

    if (isValidBirthDate) {
        $('.iconCalendarBirthDate').css('display', 'none');
    }
    else {
        $('.iconCalendarBirthDate').css('display', 'block');
    }
    return isValid;
}
// #endregion Student

// #region Studies Programs
function cleanStudiesProgramFields() {
    $('#InstitutionName').val('');
    hideError('InstitutionName');
    $('#InstitutionSepId').val('');
    hideError('InstitutionSepId');
    $('#CampusName').val('');
    hideError('CampusName');
    $('#CampusSepId').val('');
    hideError('CampusSepId');
    $('#FederalEntityCatalogMapping').val('');
    hideError('FederalEntityCatalogMapping');

    $('#StudiesProgramResponsible_Curp').val('');
    hideError('StudiesProgramResponsible_Curp');
    $('#StudiesProgramResponsible_Name').val('');
    hideError('StudiesProgramResponsible_Name');
    $('#StudiesProgramResponsible_FirstSurname').val('');
    hideError('StudiesProgramResponsible_FirstSurname');
    $('#StudiesProgramResponsible_SecondSurname').val('');
    hideError('StudiesProgramResponsible_SecondSurname');
    $('#StudiesProgramResponsible_JobTitle').val('');
    hideError('StudiesProgramResponsible_JobTitle');

    $('#StudiesProgramMajor_Name').val('');
    hideError('StudiesProgramMajor_Name');
    $('#StudiesProgramMajor_Id').val('');
    hideError('StudiesProgramMajor_Id');
    $('#StudiesProgramMajor_PlanCode').val('');
    hideError('StudiesProgramMajor_PlanCode');
    $('#StudiesProgramMajor_PeriodType').val('');
    hideError('StudiesProgramMajor_PeriodType');
    $('#StudiesProgramMajor_StudyLevel').val('');
    hideError('StudiesProgramMajor_StudyLevel');
    $('#StudiesProgramMajor_MinimumGrade').val('');
    hideError('StudiesProgramMajor_MinimumGrade');
    $('#StudiesProgramMajor_MaximumGrade').val('');
    hideError('StudiesProgramMajor_MaximumGrade');
    $('#StudiesProgramMajor_MinimumPassingGrade').val('');
    hideError('StudiesProgramMajor_MinimumPassingGrade');

    $('#StudiesProgramRvoe_Number').val('');
    hideError('StudiesProgramRvoe_Number');
    $('#StudiesProgramRvoe_IssuingDate').val('');
    hideError('StudiesProgramRvoe_IssuingDate');
}

function selectStudiesProgram(event) {
    if (event.target.dataset.hasOperatorCampus === 'true' && event.target.dataset.hasRvoeInformation === 'true'
        && event.target.dataset.hasResponsibleCampus === 'true' && event.target.dataset.hasCoursesMapping === 'true'
        && event.target.dataset.hasInstitutionCode === 'true' && event.target.dataset.hasCampusCode === 'true'
        && event.target.dataset.hasSigningCode === 'true') {
        $('#hdnStudiesProgramId').val(event.target.value);
        $('#btnContinueStudiesPrograms').attr('disabled', false);
    }
    else {
        $('#hdnStudiesProgramId').val('0');
        $('#btnContinueStudiesPrograms').attr('disabled', true);
    }
}

function showConflicts(event) {
    if (event.target.dataset.hasOperatorCampus !== 'true') {
        $('#divOperatorCampusConflict').css('display', 'block');
    }
    else {
        $('#divOperatorCampusConflict').css('display', 'none');
    }
    if (event.target.dataset.hasRvoeInformation !== 'true') {
        $('#divHasRvoeInformationConflict').css('display', 'block');
    }
    else {
        $('#divHasRvoeInformationConflict').css('display', 'none');
    }
    if (event.target.dataset.hasResponsibleCampus !== 'true') {
        $('#divResponsibleCampusConflict').css('display', 'block');
    }
    else {
        $('#divResponsibleCampusConflict').css('display', 'none');
    }
    if (event.target.dataset.hasCoursesMapping !== 'true') {
        $('#divCoursesMappingConflict').css('display', 'block');
    }
    else {
        $('#divCoursesMappingConflict').css('display', 'none');
    }
    if (event.target.dataset.hasInstitutionCode !== 'true') {
        $('#divInstitutionCodeConflict').css('display', 'block');
    }
    else {
        $('#divInstitutionCodeConflict').css('display', 'none');
    }
    if (event.target.dataset.hasCampusCode !== 'true') {
        $('#divCampusCodeConflict').css('display', 'block');
    }
    else {
        $('#divCampusCodeConflict').css('display', 'none');
    }
    if (event.target.dataset.hasSigningCode !== 'true') {
        $('#divSigingCodeConflict').css('display', 'block');
    }
    else {
        $('#divSigingCodeConflict').css('display', 'none');
    }
    $('#Overlaydiv').css('display', 'block');
    $('#divModalStudiesProgramConflicts').css('display', 'block');
}

function validateStudiesProgram() {
    let isValid = true;
    isValid = validateIntupField('InstitutionSepId') && isValid;
    isValid = validateIntupField('CampusSepId') && isValid;
    isValid = validateIntupField('FederalEntityCatalogMapping') && isValid;
    isValid = validateIntupField('StudiesProgramResponsible_Curp') && isValid;
    isValid = validateIntupField('StudiesProgramResponsible_Name') && isValid;
    isValid = validateIntupField('StudiesProgramResponsible_FirstSurname') && isValid;
    isValid = validateIntupField('StudiesProgramResponsible_JobTitle') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_Id') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_PlanCode') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_PeriodType') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_StudyLevel') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_MinimumGrade') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_MaximumGrade') && isValid;
    isValid = validateIntupField('StudiesProgramMajor_MinimumPassingGrade') && isValid;
    isValid = validateIntupField('StudiesProgramRvoe_Number') && isValid;
    isValid = validateIntupField('StudiesProgramRvoe_IssuingDate') && isValid;
    return isValid;
}
// #endregion Studies Programs

// #region Courses
function setRvoeId(rvoeId) {
    $('#hdnRvoeId').val(rvoeId);
}

function setRemarks(remarks, placeholder, selected) {
    if (remarks && remarks.length > 0) {
        let remarksPlaceHtml = '';
        remarks.map((remark) => {
            if (remark.Id == selected)
                remarksPlaceHtml += `<option value=${remark.Id} selected="selected">${remark.Desc}</option>\n`;
            else
                remarksPlaceHtml += `<option value=${remark.Id}>${remark.Desc}</option>\n`;
        });

        $("#" + placeholder + "").html(`
            <option>${resources.lblSelect}</option>
            ${remarksPlaceHtml}
        `);
    }
}

function setSubjects(subjects, placeholder, selected) {
    if (subjects && subjects.length > 0) {
        let subjectsPlaceHtml = '';
        subjects.map((subject) => {
            if (subject.Id == selected)
                subjectsPlaceHtml += `<option value=${subject.Id} selected="selected">${subject.Desc}</option>\n`;
            else
                subjectsPlaceHtml += `<option value=${subject.Id}>${subject.Desc}</option>\n`;
        });

        $("#" + placeholder + "").html(`
            <option>${resources.lblSelect}</option>
            ${subjectsPlaceHtml}
        `);
    }
}

function selectCourse(event) {
    const courseIndex =
        courses.findIndex(x => x.TranscriptDetailId ===
            Number(event.target.dataset.transcriptdetailid));
    if (courseIndex > -1) {
        if (event.currentTarget.checked) {
            courses[courseIndex].IsInclude = true;
            $("#divError").css('display', 'none');
        }
        else {
            courses[courseIndex].IsInclude = false;
        }
    }
    calculateCredits();
}

function selectRemark(event) {
    const gradeIndex =
        coursesViewModel.GradeRemarks.findIndex(x => x.Id === Number($(event.target).val()));
    const courseIndex =
        courses.findIndex(x => x.TranscriptDetailId ===
            Number(event.target.dataset.transcriptdetailid));
    if (courseIndex > -1 && gradeIndex > -1) {
        courses[courseIndex].GradeRemarkId = Number($(event.target).val());
        courses[courseIndex].GradeRemark = coursesViewModel.GradeRemarks[gradeIndex].Desc;
    }
}

function selectSubject(event) {
    const subjectIndex =
        coursesViewModel.Subjects.findIndex(x => x.Id === Number($(event.target).val()));
    const courseIndex =
        courses.findIndex(x => x.TranscriptDetailId ===
            Number(event.target.dataset.transcriptdetailid));
    if (courseIndex > -1 && subjectIndex > -1) {
        courses[courseIndex].SubjectTypeId = Number($(event.target).val());
        courses[courseIndex].SubjectType = coursesViewModel.Subjects[subjectIndex].Desc;
        $("#divSubjectsPlace_" + courses[courseIndex].TranscriptDetailId).removeClass('esg-has-feedback esg-has-error');
    }
    else {
        courses[courseIndex].SubjectTypeId = 0;
        courses[courseIndex].SubjectType = "";
        $("#divSubjectsPlace_" + courses[courseIndex].TranscriptDetailId).addClass('esg-has-feedback esg-has-error');
    }
    calculateCredits();
}

function selectSepId(event) {
    const courseIndex =
        courses.findIndex(x => x.TranscriptDetailId ===
            Number(event.target.dataset.transcriptdetailid));
    if (courseIndex > -1) {
        courses[courseIndex].SepId = $(event.target).val();
        $("#divInputSep_" + Number(event.target.dataset.transcriptdetailid)).removeClass('esg-has-feedback esg-has-error');
    }
}

function calculateCredits() {
    let assignedCourses = 0;
    let totalAssigned = 0;
    let average = 0.0;
    let obtainedCredits = 0.0;
    let totalFinalGrades = 0.0;

    $('#AsignedCourses').val("");
    $('#ObtainedCredits').val("");
    $('#Average').val("");

    $.each(courses, function (i, item) {
        //Id = 4 is complementary. Should not count for average
        if (item.IsInclude)
            totalAssigned++;
        if (item.IsInclude && item.SubjectTypeId !== 4) {
            assignedCourses++;
            obtainedCredits = obtainedCredits + Number(item.Credit);
            totalFinalGrades = Number(totalFinalGrades) + Number(item.FinalGrade);
        }
    });

    if (assignedCourses > 0)
        average = totalFinalGrades / assignedCourses;

    average = (average).toFixed(3);
    let averageString = average.toString();
    averageString = averageString.slice(0, (averageString.indexOf(".")) + 3);
    average = Number(averageString);

    $('#AsignedCourses').val(totalAssigned);
    $('#ObtainedCredits').val((obtainedCredits).toFixed(2));
    $('#Average').val(average);
}

function openDropDownOptions(id) {
    var ul = $("#ul_" + id);
    if (ul.css('display') === 'none') {
        ul.css('display', 'block');
    } else {
        ul.css('display', 'none');
    }
}

function disableCoursesFields() {
    $.each(courses, function (i, item) {
        $("#chxInclude_" + item.TranscriptDetailId).attr('disabled', 'disabled');
        $("#SubjectsPlace_" + item.TranscriptDetailId).attr('disabled', 'disabled');
        $("#RemarksPlace" + item.TranscriptDetailId).attr('disabled', 'disabled');
        $("#inputSepId_" + item.TranscriptDetailId).attr('disabled', 'disabled');
    });
}

function calculateCycles() {
    let cycles = new Array();
    $.each(courses, function (i, item) {
        if (item.IsInclude) {
            if (cycles.length == 0) {
                cycles.push(item.CourseCycleId)
            }
            else {
                if (!cycles.includes(item.CourseCycleId)) {
                    cycles.push(item.CourseCycleId)
                }
            }
        }
    });

    PreviewData.TotalCycle = cycles.length;
}
// #endregion Courses

// #region Issuing Certificate
function updateIssuingCertificateData() {
    $.ajax({
        url: urlGetIssuingCertificate,
        dataType: 'json',
        cache: false,
        type: 'GET',
        data: {}
    })
        .done(function (response) {
            const certificationTypes = response.certificationTypes;
            const federalEntitiesCatalog = response.federalEntitiesCatalog;
            let certificationTypesHtml = '';
            let issuingPlaceHtml = '';

            if (certificationTypes && certificationTypes.length > 0) {
                certificationTypes.map((certificationType) => {
                    certificationTypesHtml += `<option value=${certificationType.ShortDescription}>${certificationType.Description}</option>\n`
                });
            }

            if (federalEntitiesCatalog && federalEntitiesCatalog.length > 0) {
                federalEntitiesCatalog.map((federalEntity) => {
                    issuingPlaceHtml += `<option value=${federalEntity.CodeValueKey}>${federalEntity.Description}</option>\n`
                });
            }

            $('#CertificationType').html(`
                    <option>${resources.lblSelect}</option>
                    ${certificationTypesHtml}
                `);

            $('#IssuingPlace').html(`
                    <option>${resources.lblSelect}</option>
                    ${issuingPlaceHtml}
                `);
        })
        .fail(function () {
            window.location.replace(urlUnauthorized);
        });

    let prevIssuingDate = $('#IssuingDate').val();
    let issuingDate;
    if (prevIssuingDate.length > 0 && moment(prevIssuingDate, 'YYYY-MM-DD', true).isValid()) {
        issuingDate = prevIssuingDate;
    }
    else {
        issuingDate = moment().format('YYYY-MM-DD');
    }
    $('#IssuingDate').val(issuingDate);
}

function validateIssuingCertificate(isListener) {
    let isValid = true;
    let issuingDate = $('#IssuingDate').val();
    let issuingPlace = $('#IssuingPlace').val();
    let certificationType = $('#CertificationType').find(':selected').text();

    let validIssuingDate = moment(issuingDate, 'YYYY-MM-DD', true).isValid();
    let validCertificationType = certificationType != resources.lblSelect;
    let validIssuingPlace = issuingPlace >= 1;

    if (validIssuingDate) {
        hideError('IssuingDate');
        $('#divIssuingDateErrorFormat').css('display', 'none');
        $('#divIssuingDateErrorEmpty').css('display', 'none');
    }
    else if (!isListener) {
        showError('IssuingDate');
        if (issuingDate.length === 0) {
            $('#divIssuingDateErrorEmpty').css('display', 'block');
            $('#divIssuingDateErrorFormat').css('display', 'none');
        }
        else {
            $('#divIssuingDateErrorFormat').css('display', 'block');
            $('#divIssuingDateErrorEmpty').css('display', 'none');
        }
        isValid = false;
    }

    if (validCertificationType) {
        hideError('CertificationType');
    }
    else if (!isListener) {
        showError('CertificationType');
        isValid = false;
    }

    if (validIssuingPlace) {
        hideError('IssuingPlace');
    }
    else if (!isListener) {
        showError('IssuingPlace');
        isValid = false;
    }

    return isValid;
}
// #endregion Issuing Certificate

//#region Preview-Generate
function showPreviewData() {
    $('#divIssuingCertificateWarning').css('display', 'none');
    showPageLoader();
    let institutionCampusId = Number($('#hdnInstitutionCampusId').val());
    let peopleId = $('#PeopleId').val();

    $.ajax({
        url: urlGetFolio,
        dataType: 'json',
        cache: false,
        type: 'GET',
        data: { peopleId: peopleId, institutionCampusId: institutionCampusId }
    })
        .done(function (data) {
            PrevFolio = data.folio;
            $('#inputFolio').val(PrevFolio);
            validateFolio();
            $('#liIssuingCertificate').removeClass('esg-is-active').addClass('esg-is-previous');
            $('#liPreviewGenerate').addClass('esg-is-active');
            $('#btnStpPreviewGenerate').css('cursor', 'pointer').css('color', '#0074db');
            $('#stpIssuingCertificate').css('display', 'none');
            $('#stpPreviewGenerate').css('display', 'block');
            hidePageLoader();
        })
        .fail(function () {
            window.location.replace(urlUnauthorized);
        });

    updatePreviewData();
}

function showIssuingCertificateWarning(totalCoursesMatch, totalCreditsMatch) {
    if (!totalCoursesMatch) {
        $('#warningLessTotalCourses').css('display', 'block');
    }
    else {
        $('#warningLessTotalCourses').css('display', 'none');
    }

    if (!totalCreditsMatch) {
        $('#warningLessTotalCredits').css('display', 'block');
    }
    else {
        $('#warningLessTotalCredits').css('display', 'none');
    }

    $('#divIssuingCertificateWarning').css('display', 'block');
}

function validateFolio() {
    let folio = $('#inputFolio').val();
    let isValidFolio = folio && folio.length >= 1 && folio.length <= 40;
    if (isValidFolio) {
        $('#folioFormGroup').removeClass('esg-has-feedback esg-has-error');
        $('#divFolioErrorIcon').html('');
        $('#divFolioErrorEmpty').css('display', 'none');
        $('#divFolioErrorCharacters').css('display', 'none');
        PreviewData.folio = $('#inputFolio').val();
    }
    else {
        $('#folioFormGroup').addClass('esg-has-feedback esg-has-error');

        $('#divFolioErrorIcon').html(`
            <div class="esg-form__feedback-icon esg-icon__container">
                <svg class="esg-icon esg-icon--error">
                    <use xlink:href="#icon-error"></use>
                </svg>
            </div>
        `);

        if (folio == undefined || folio.length == 0) {
            $('#divFolioErrorEmpty').css('display', 'block');
            $('#divFolioErrorCharacters').css('display', 'none');
        }
        else {
            $('#divFolioErrorCharacters').css('display', 'block');
            $('#divFolioErrorEmpty').css('display', 'none');
        }
    }

    return isValidFolio;
}

function updatePreviewData() {
    // #region Student information
    PreviewData.peopleId = $('#PeopleId').val();
    $('#previewStudentPeopleId').html(PreviewData.peopleId);

    PreviewData.curp = $('#Curp').val();
    $('#previewStudentCurp').html(PreviewData.curp);

    PreviewData.name = $('#Name').val();
    $('#previewStudentName').html(PreviewData.name);

    PreviewData.firstSurname = $('#FirstSurname').val();
    $('#previewStudentFirstSurname').html(PreviewData.firstSurname);

    PreviewData.secondSurname = $('#SecondSurname').val();
    $('#previewStudentSecondSurname').html(PreviewData.secondSurname);

    PreviewData.gender = $('#hdnGenderCode').val();
    $('#previewStudentGenderIdentity').html($('#GenderIdentity').find(':selected').text());

    PreviewData.birthDate = $('#BirthDate').val();
    $('#previewStudentBirthDate').html(PreviewData.birthDate);
    // #endregion Student information

    // #region Institution information
    PreviewData.institutionSepId = $('#InstitutionSepId').val();
    $('#previewInstitutionNameId').html(PreviewData.institutionSepId);

    PreviewData.signingInstitutionId = $('#SigningInstitutionId').val();
    $('#previewSigningInstitutionId').html(PreviewData.signingInstitutionId);

    PreviewData.institutionName = $('#InstitutionName').val();
    $('#previewInstitutionName').html(PreviewData.institutionName);

    PreviewData.campusSepCode = $('#CampusSepId').val();
    $('#previewInstitutionCampusId').html(PreviewData.campusSepCode);

    PreviewData.campusName = $('#CampusName').val();
    $('#previewInstitutionCampus').html($('#CampusName').val());

    PreviewData.federalEntityCode = $('#FederalEntityCatalogMapping').find(':selected').val();
    PreviewData.federalEntity = $('#FederalEntityCatalogMapping').find(':selected').text();
    PreviewData.FederalEntityShortDesc =
        codeFederalEntities.find(x => x.CodeValueKey === Number(PreviewData.federalEntityCode)).ShortDescription;

    $('#previewInstitutionFederalEntityId').html(PreviewData.FederalEntityShortDesc);
    $('#previewInstitutionFederalEntity').html(PreviewData.federalEntity);
    // #endregion Institution information

    // #region Responsible information
    PreviewData.responsibleCurp = $('#StudiesProgramResponsible_Curp').val();
    $('#previewResponsibleCurp').html(PreviewData.responsibleCurp);

    PreviewData.responsibleName = $('#StudiesProgramResponsible_Name').val();
    $('#previewResponsibleName').html(PreviewData.responsibleName);

    PreviewData.responsibleFirstSurname = $('#StudiesProgramResponsible_FirstSurname').val();
    $('#previewResponsibleFirstSurname').html(PreviewData.responsibleFirstSurname);

    PreviewData.responsibleSecondSurname = $('#StudiesProgramResponsible_SecondSurname').val();
    $('#previewResponsibleSecondSurname').html(PreviewData.responsibleSecondSurname);

    PreviewData.responsibleJobTitleId = $('#StudiesProgramResponsible_JobTitle').find(':selected').val();
    $('#previewResponsibleJobTitleId').html(PreviewData.responsibleJobTitleId);

    PreviewData.responsibleJobTitle = $('#StudiesProgramResponsible_JobTitle').find(':selected').text();
    $('#previewResponsibleJobTitle').html(PreviewData.responsibleJobTitle);
    // #endregion Responsible information

    // #region Major information
    PreviewData.majorCode = $('#hdnStudiesProgramMajor_Code').val();

    PreviewData.majorId = Number($('#StudiesProgramMajor_Id').val());
    $('#previewMajorId').html(String(PreviewData.majorId));

    PreviewData.majorName = $('#StudiesProgramMajor_Name').val();
    $('#previewMajorName').html(PreviewData.majorName);

    PreviewData.periodTypeId = $('#StudiesProgramMajor_PeriodType').find(':selected').val();

    PreviewData.periodType = $('#StudiesProgramMajor_PeriodType').find(':selected').text();
    PreviewData.periodTypeShortDesc =
        periodTypes.find(x => x.Id === Number(PreviewData.periodTypeId)).ShortDescription;
    $('#previewMajorPeriodTypeId').html(PreviewData.periodTypeShortDesc);
    $('#previewMajorPeriodType').html(PreviewData.periodType);

    PreviewData.planCode = $('#StudiesProgramMajor_PlanCode').val();
    $('#previewMajorPlanCode').html(PreviewData.planCode);

    PreviewData.studyLevelId = $('#StudiesProgramMajor_StudyLevel').val();
    $('#previewMajorStudyLevelId').html(PreviewData.studyLevelId);

    PreviewData.studyLevel = $('#StudiesProgramMajor_StudyLevel').text();
    $('#previewMajorStudyLevel').html(PreviewData.studyLevel);

    PreviewData.minimumGrade = Number($('#StudiesProgramMajor_MinimumGrade').val());
    $('#previewMajorMinimumGrade').html(String(PreviewData.minimumGrade));

    PreviewData.maximumGrade = Number($('#StudiesProgramMajor_MaximumGrade').val());
    $('#previewMajorMaximumGrade').html(String(PreviewData.maximumGrade));

    PreviewData.minimumPassingGrade = Number($('#StudiesProgramMajor_MinimumPassingGrade').val());
    $('#previewMinPassingGrade').html(String(PreviewData.minimumPassingGrade));
    // #endregion Major information

    // #region RVOE information
    PreviewData.rvoeAgreementNumber = $('#StudiesProgramRvoe_Number').val();
    $('#previewRvoeNumber').html(PreviewData.rvoeAgreementNumber);

    PreviewData.expeditionDate = $('#StudiesProgramRvoe_IssuingDate').val();
    $('#previewRvoeIssuingDate').html(PreviewData.expeditionDate);
    // #endregion RVOE information

    // #region Courses information
    PreviewData.totalCourse = Number($('#TotalCourses').val());
    $('#previewCoursesTotal').html(String(PreviewData.totalCourse));

    PreviewData.courseAssigned = Number($('#AsignedCourses').val());
    $('#previewCoursesAssigned').html(String(PreviewData.courseAssigned));

    PreviewData.gpa = Number($('#Average').val());
    $('#previewCoursesAverage').html(String(PreviewData.gpa));

    PreviewData.totalCredit = Number($('#TotalCredits').val());
    $('#previewCoursesTotalCredits').html(String(PreviewData.totalCredit));

    PreviewData.creditEarned = Number($('#ObtainedCredits').val());
    $('#previewCoursesObtainedCredits').html(String(PreviewData.creditEarned));

    PreviewData.courses = [];
    let coursesTableRowsHtml = '';
    courses.map((course) => {
        if (course.IsInclude) {
            PreviewData.courses.push({
                courseCode: course.CourseCode,
                credits: course.Credit,
                eventCycle: course.CourseCycle,
                eventName: course.EventName,
                finalGrade: Number(course.FinalGrade) != 10 && Number(course.FinalGrade) != 5 ? course.FinalGrade : Number(course.FinalGrade).toFixed(0),
                gradeRemark: course.GradeRemark,
                gradeRemarkId: course.GradeRemarkId,
                sepId: course.SepId,
                subjectType: course.SubjectType,
                subjectTypeId: course.SubjectTypeId,
                transcriptDetailId: course.TranscriptDetailId,
                gradeRemarkShortDesc: course.GradeRemarkShortDesc,
                subjectTypeShortDesc: course.SubjectTypeShortDesc,
                CourseCycleId: course.CourseCycleId
            });
            coursesTableRowsHtml += `
            <tr class="esg-table-body__row" id="course_${course.SepId}">
                <td class="esg-table-body__td">
                    ${course.CourseCycle}
                </td>
                <td class="esg-table-body__td">
                    ${course.SepId}
                </td>
                <td class="esg-table-body__td">
                    ${course.EventName}
                </td>
                <td class="esg-table-body__td">
                    ${course.FinalGrade}
                </td>
                <td class="esg-table-body__td">
                    ${course.GradeRemark}
                </td>
                <td class="esg-table-body__td">
                    ${course.SubjectType}
                </td>
                <td class="esg-table-body__td">
                    ${course.Credit.toFixed(2)}
                </td>
            </tr>
        `;
        }
    });
    $('#coursesPreview_table_rows_tbody').html(coursesTableRowsHtml);

    calculateCycles();
    $('#previewCyclesTotal').html(String(PreviewData.TotalCycle));

    // #endregion Courses information

    // #region Issuing Degree information
    PreviewData.certificationTypeId = $('#CertificationType').find(':selected').val();
    $('#previewIssuingDegreeCertificationTypeId').html(PreviewData.certificationTypeId);

    PreviewData.certificationType = $('#CertificationType').find(':selected').text();
    $('#previewIssuingDegreeCertificationType').html(PreviewData.certificationType);

    PreviewData.issuingDate = $('#IssuingDate').val();
    $('#previewIssuingDegreeIssuingDate').html(PreviewData.issuingDate);

    PreviewData.issuingFederalEntityCode = $('#IssuingPlace').find(':selected').val();
    PreviewData.IssuingFederalShortDesc =
        codeFederalEntities.find(x => x.CodeValueKey === Number(PreviewData.issuingFederalEntityCode)).ShortDescription;
    $('#previewIssuingDegreeIssuingPlaceId').html(PreviewData.IssuingFederalShortDesc);

    PreviewData.issuingFederalEntity = $('#IssuingPlace').find(':selected').text();
    $('#previewIssuingDegreeIssuingPlace').html(PreviewData.issuingFederalEntity);

    // #endregion Issuing Degree information
}
//#endregion Preview-Generate

//#endregion Functions