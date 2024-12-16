let matricYears;
let matricTerms;
let programs;

let campus;
let campusCodeId;
let matricYear;
let matricYearId;
let matricTerm;
let matricTermId;

$(document).ready(function () {
    $(document).on('mouseover', '.spnToolTip', function () {
        var id = $(this).attr('id');
        var index = id.split('_')[1];
        $(`#tltRVOE_${index}`).css('display', 'block');
    });

    $(document).on('mouseout', '.spnToolTip', function () {
        var id = $(this).attr('id');
        var index = id.split('_')[1];
        $(`#tltRVOE_${index}`).css('display', 'none');
    });

    campusCodeId = window.localStorage.getItem("CampusCodeId");
    matricYearId = window.localStorage.getItem("MatricYear");
    matricTermId = window.localStorage.getItem("MatricTerm");
    window.localStorage.removeItem('CampusCodeId');
    window.localStorage.removeItem('MatricYear');
    window.localStorage.removeItem('MatricTerm');
    if (campusCodeId) {
        $(`#ddlCampus option[value=${campusCodeId}]`).attr('selected', 'selected');
        campus = $('#ddlCampus').find(':selected').text();
        ddlCampusChange();
    }


    $('#ddlCampus').change(function () {
        showPageLoader();
        campus = $('#ddlCampus').find(':selected').text();
        campusCodeId = $('#ddlCampus').find(':selected').val();
        matricYearId = '';
        matricTermId = '';
        ddlCampusChange();
    });

    $('#ddlMatricYear').change(function () {
        showPageLoader();
        matricYear = $('#ddlMatricYear').find(':selected').text();
        matricYearId = $('#ddlMatricYear').find(':selected').val();
        matricTermId = '';
        ddlMatricYearChange();
    });

    $('#ddlMatricTerm').change(function () {
        showPageLoader();
        matricTerm = $('#ddlMatricTerm').find(':selected').text();
        matricTermId = $('#ddlMatricTerm').find(':selected').val();
        ddlMatricTermChange();
    });

    $('#btnClearFilters').click(function (e) {
        e.preventDefault();
        $('#ddlCampus').prop('selectedIndex', 0);
        resetDropdownLists();
    });

});

function ddlCampusChange() {
    showPageLoader();
    if (campus !== resources.lblSelect) {
        $.ajax({
            url: urlGetMatricYearList,
            dataType: 'json',
            cache: false,
            type: 'GET',
            data: { campusCodeId: campusCodeId }
        })
            .done(function (data) {
                if (data.id !== -1) {
                    matricYears = data.matricYears;
                    updateYearsList();
                    if (matricYearId) {
                        $(`#ddlMatricYear option[value=${matricYearId}]`).attr('selected', 'selected');
                        matricYear = $('#ddlMatricYear').find(':selected').text();
                        ddlMatricYearChange();
                    }
                    resetTermsList();
                    hidePageLoader();
                }
                else {
                    window.location.replace(urlUnauthorized);
                }
            })
            .fail(function () {
                window.location.replace(urlUnauthorized);
            });
    }
    else {
        resetDropdownLists();
        hidePageLoader();
    }
}

function ddlMatricYearChange() {
    showPageLoader();
    if (campus !== resources.lblSelect && matricYear !== resources.lblSelect) {
        $.ajax({
            url: urlGetMatricTermList,
            dataType: 'json',
            cache: false,
            type: 'GET',
            data: { campusCodeId: campusCodeId, matricYear: matricYear }
        })
            .done(function (data) {
                if (data.id !== -1) {
                    matricTerms = data.matricTerms;
                    updateTermsList();
                    if (matricTermId) {
                        $(`#ddlMatricTerm option[value=${matricTermId}]`).attr('selected', 'selected');
                        matricTerm = $('#ddlMatricTerm').find(':selected').text();
                        ddlMatricTermChange();
                    }
                    resetTable();
                    hidePageLoader();
                }
                else {
                    window.location.replace(urlUnauthorized);
                }
            })
            .fail(function () {
                window.location.replace(urlUnauthorized);
            });
    }
    else {
        resetTermsList();
        hidePageLoader();
    }
}

function ddlMatricTermChange() {
    showPageLoader();
    if (campus !== resources.lblSelect && matricYear !== resources.lblSelect && matricTerm !== resources.lblSelect) {
        $.ajax({
            url: urlGetPrograms,
            dataType: 'json',
            cache: false,
            type: 'GET',
            data: { campusCodeId: campusCodeId, matricYear: matricYear, matricTerm: matricTermId }
        })
            .done(function (data) {
                if (data.id !== -1) {
                    programs = data.programs;
                    renderTable();
                    $('#programsSection').css('display', 'block');
                    hidePageLoader();
                }
                else {
                    window.location.replace(urlUnauthorized);
                }
            })
            .fail(function () {
                window.location.replace(urlUnauthorized);
            });
    }
    else {
        resetTable();
        hidePageLoader();
    }
}

function resetDropdownLists() {
    resetYearsList();
    resetTermsList();
}

function updateYearsList() {
    let yearOptionsHtml = '';

    if (matricYears && matricYears.length > 0) {
        matricYears.map((matricYear) => {
            yearOptionsHtml += `<option value=${matricYear.CodeValueKey}>${matricYear.Description}</option>\n`
        });
    }

    $('#ddlMatricYear').html(`
        <option>${resources.lblSelect}</option>
        ${yearOptionsHtml}
    `);
}

function resetYearsList() {
    $('#ddlMatricYear').html(`<option>${resources.lblSelect}</option>`);
    $('#ddlMatricYear').prop('selectedIndex', 0);
    resetTable();
}

function updateTermsList() {
    let termOptionsHtml = '';

    if (matricTerms && matricTerms.length > 0) {
        matricTerms.map((matricTerm) => {
            termOptionsHtml += `<option value=${matricTerm.CodeValueKey}>${matricTerm.Description}</option>\n`
        });
    }

    $('#ddlMatricTerm').html(`
        <option>${resources.lblSelect}</option>
        ${termOptionsHtml}
    `);
}

function resetTermsList() {
    $('#ddlMatricTerm').html(`<option>${resources.lblSelect}</option>`);
    $('#ddlMatricTerm').prop('selectedIndex', 0);
    resetTable();
}

function renderTable() {
    let errorIconHtml = `
        <span class="esg-icon__container">
            <svg class="esg-icon--error esg-icon--small">
                <use xlink: href="#icon-error"></use>
            </svg>
        </span>
    `;

    let programsRowsHtml = '';
    if (programs && programs.length > 0) {
        $('#prgResults').html(`${programs.length} ${resources.lblResults}`);

        programs.map((program, iProgram) => {
            let RVOECellHtml = '';
            if (program.RVOE) {
                RVOECellHtml = `
                <div style="display:inline-flex">
                    <span style="line-break:anywhere">${program.RVOE.RvoeAgreement}</span>
                    <div id="spnRVOE_${iProgram}" class="esg-icon__container spnToolTip" style="cursor:pointer; margin-left:1rem; display:block; line-break:anywhere; margin-top:0.2rem">
                        <svg class="esg-icon esg-icon--small">
                            <use xlink: href="#icon-error"></use>
                        </svg>
                        <div id="tltRVOE_${iProgram}" style="display:none; text-align: left">
                            <div class="esg-popover esg-popover--bottom" role="tooltip" style="width: 100%; min-width:22rem; left:-9.5rem; margin-top:1.7rem;">
                                <div class="esg-popover__arrow"></div>
                                <div class="esg-popover__content">
                                    <div class="esg-container-fluid">
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblIssuingDate}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${program.RVOE.IssuingDate ? program.RVOE.IssuingDate : errorIconHtml}
                                            </div>
                                        </div>
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblMinimumGrade}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${program.RVOE.MinimumGrade ? program.RVOE.MinimumGrade : errorIconHtml}
                                            </div>
                                        </div>
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblMaximumGrade}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${program.RVOE.MaximumGrade ? program.RVOE.MaximumGrade : errorIconHtml}
                                            </div>
                                        </div>
                                        <div class="esg-row">
                                            <div class="esg-col-xs-8 esg-grid-overlay">
                                                <strong>${resources.lblMinPassingGrade}</strong>
                                            </div>
                                            <div class="esg-col-xs-4 esg-grid-overlay">
                                                ${program.RVOE.MinimumPassingGrade ? program.RVOE.MinimumPassingGrade.toFixed(2) : errorIconHtml}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                `
            }

            let courseMappingCellHtml = '';
            if (program.CourseMappingPercent !== undefined) {
                courseMappingCellHtml = `
                    <span>${program.CourseMappingPercent}%</span>
                    <span id="spnRVOE_${iProgram}" class="esg-icon__container">
                        <svg class="esg-icon--${program.CourseMappingPercent == 100 ? "success" : "error"} esg-icon--small">
                            <use xlink: href="#icon-${program.CourseMappingPercent == 100 ? "check" : "swap"}"></use>
                        </svg>
                    </span>
                    <a class="esg-breadcrumb__link" id="btnEdit" onClick="editBotton('${program.RVOE.Id}')">${resources.lblEdit}</a>
                `
            }

            programsRowsHtml += `
                <tr class="esg-table-body__row">
                    <td class="esg-table-body__td">
                        ${program.Name}
                    </td>
                    <td class="esg-table-body__td">
                        ${program.MajorId ? program.MajorId : errorIconHtml}
                    </td>
                    <td class="esg-table-body__td">
                        ${program.RVOE ? RVOECellHtml : errorIconHtml}
                    </td>
                    <td class="esg-table-body__td">
                        ${program.CourseMappingPercent >= 0 ? courseMappingCellHtml : errorIconHtml}
                    </td>
                </tr>
            `
        });
    }

    $('#programs_table_rows_tbody').html(programsRowsHtml);
}

function resetTable() {
    programs = undefined;
    $('#programs_table_rows_tbody').html('');
    $('#programsSection').css('display', 'none');
}

function editBotton(rvoeId) {
    $("#Processing").css('display', 'block');
    $("#Overlaydiv").css('display', 'block');
    window.location.href = urlEdit + "?id=" + rvoeId;
}