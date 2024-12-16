function Search() {
    if (!($('#StartDatePicker').val() || $('#EndDatePicker').val()) && !$('#PeopleOrgIdFilter').val()
        && !$('#FolioFilter').val() && !$('#ReceiverFilter').val()
        && Number($('#ddlStatus').val()) === NullStatus && $('#ddlRecordType').val() === lblAll) {
        $("#divAlertNoCriteria").removeClass("no-display");
        $('#records_table_rows_tbody').empty();
        $('#hdnModelCount').val('0');
        ShowHideRows("#divDetails", 1);
        $('#divDetails').hide();
        $('#noResultsAlert').hide();


        $("#StartDateMissing").hide();
        $("#StartDateGroup").removeClass('esg-has-error');

        $("#EndDateMissing").hide();
        $("#EndDateGroup").removeClass('esg-has-error');
    }
    else if ($('#StartDatePicker').val() && $('#EndDatePicker').val() && !isDateRangeValid()) {
        $("#divAlertNoCriteria").addClass("no-display");
        $('#records_table_rows_tbody').empty();
        $('#noResultsAlert').hide();
        $('#hdnModelCount').val('0');
        ShowHideRows("#divDetails", 1);
        $('#divDetails').hide();

        $('#StartDateError').show();
        $("#StartDateGroup").addClass('esg-has-error');
        $("#StartDateMissing").hide();
        $("#EndDateMissing").hide();
    }
    else if ($('#StartDatePicker').val() && !$('#EndDatePicker').val()) {
        $("#divAlertNoCriteria").addClass("no-display");
        $('#records_table_rows_tbody').empty();
        $('#noResultsAlert').hide();
        $('#hdnModelCount').val('0');
        ShowHideRows("#divDetails", 1);
        $('#divDetails').hide();

        $("#StartDateMissing").hide();
        $("#StartDateGroup").removeClass('esg-has-error');
        $("#EndDateMissing").show();
        $("#EndDateGroup").addClass('esg-has-error');
    }
    else if (!$('#StartDatePicker').val() && $('#EndDatePicker').val()) {
        $("#divAlertNoCriteria").addClass("no-display");
        $('#records_table_rows_tbody').empty();
        $('#noResultsAlert').hide();
        $('#hdnModelCount').val('0');
        ShowHideRows("#divDetails", 1);
        $('#divDetails').hide();
        $('#noResultsAlert').hide();

        $("#StartDateMissing").show();
        $("#StartDateGroup").addClass('esg-has-error');
        $("#EndDateMissing").hide();
        $("#EndDateGroup").removeClass('esg-has-error');
    }
    else {
        showProcessing();
        var Checked = $('input[type=radio]:checked').val();
        var peopleOrgId = $("#PeopleOrgIdFilter").val();

        if (peopleOrgId !== "") {
            if (Checked === 'RadioPerson') //Search by people id
            {
                peopleOrgId = 'P' + peopleOrgId;
            }
            else // Search by organization id
            {
                peopleOrgId = 'O' + peopleOrgId;
            }
        }

        $("#divAlertNoCriteria").addClass("no-display");

        $("#StartDateMissing").hide();
        $("#StartDateGroup").removeClass('esg-has-error');
        $("#EndDateMissing").hide();
        $("#EndDateGroup").removeClass('esg-has-error');

        var startDate = $("#StartDatePicker").val();
        var endDate = $("#EndDatePicker").val();
        var folio = $("#FolioFilter").val();
        var receiver = $("#ReceiverFilter").val();
        var status = Number($("#ddlStatus").val());
        var fiscalRecordType = $("#ddlRecordType").val() === lblAll ? null : $("#ddlRecordType").val();


        $.ajax({
            url: urlFilteredFiscalRecords,
            type: "POST",
            cache: false,
            dataType: "json",
            data: {
                StartDate: startDate,
                EndDate: endDate,
                PeopleOrgCodeId: peopleOrgId,
                Folio: folio,
                Keyword: receiver,
                Status: status,
                FiscalRecordType: fiscalRecordType
            },
            success: function (data) {
                $('#records_table_rows_tbody').empty();
                var viewLabel = document.getElementById('viewLabel').value;
                var tableHTLM = '';
                var j = 0;
                if (data && data.length > 0) {
                    $('#noResultsAlert').hide();
                    $('#divDetails').show();
                }
                else {
                    $('#noResultsAlert').show();
                    $('#divDetails').hide();
                }
                $.each(data, function (i, item) {
                    tableHTLM += '<tr class="esg-table-body__row" id="' + j + '">'
                        + '<td class="esg-table-body__td" style="word-wrap:break-word;">' + item.PeopleOrgCodeId
                        + '</td><td class="esg-table-body__td" style="word-wrap:break-word;">' + item.expeditionDateTime
                        + '</td><td class="esg-table-body__td" style="word-wrap:break-word;">' + item.serialNumber
                        + '</td><td class="esg-table-body__td" style="word-wrap:break-word;">' + item.folio
                        + '</td><td class="esg-table-body__td" style="word-wrap:break-word;">' + item.receiverTaxPayerId
                        + '</td><td class="esg-table-body__td" style="word-wrap:break-word;">' + item.fiscalRecordType
                        + '</td><td class="esg-table-body__td" style="word-wrap:break-word;">';
                    if (item.CancelReasonName == SubstitutionReasonName && item.FiscalRecordStatusEnum != CanceledFiscalRecordStatus) {
                        tableHTLM += `
                            <div style="display: flex; align-items: center;">
                                <span class="esg-icon__container" style="cursor:pointer; margin-right: 5px;"
                                        title="${CancelProgressTitle}">
                                    <svg class="esg-icon esg-icon--warning esg-icon--small">
                                        <use xlink: href="#icon-warning"></use>
                                    </svg>
                                </span>
                                <span>${item.requestState}</span>
                            </div>
                        `;
                    }
                    else if ((item.IsCancellationInProgress
                        || item.FiscalRecordStatusEnum == ProviderCannotCancelStatus
                        || item.FiscalRecordStatusEnum == ProviderIsCancelingStatus
                        || item.FiscalRecordStatusEnum == RequestedCancellationStatus) && item.CancelReasonName == NominativeOperationReasonName) {
                        tableHTLM += `
                            <div style="display: flex; align-items: center;">
                                <span class="esg-icon__container" style="cursor:pointer; margin-right: 5px;"
                                        title="${CancelProgressTitle}">
                                    <svg class="esg-icon esg-icon--warning esg-icon--small">
                                        <use xlink: href="#icon-warning"></use>
                                    </svg>
                                </span>
                                <div>
                                    <span>${item.requestState}</span><br />
                                    <span><a href="CancelGlobal/?id=${item.InvoiceHeaderId}">${lblContinueCancellation}<a/></span>
                                </div>
                            </div>
                        `;
                    }
                    else {
                        tableHTLM += item.requestState;
                    }
                    tableHTLM += '</td><td class="esg-table-body__td" style="word-wrap:break-word;">'
                        + '<a href="Edit/?id=' + item.InvoiceHeaderId + '">' + viewLabel + '<a/>'
                        + '</td></tr>';
                    j++;
                });
                $('#hdnModelCount').val(j);
                $('#records_table_rows').append(tableHTLM);
                ShowHideRows("#divDetails", 1);
                $('#filtersPanel').click();
                hideProcessing();
            }
        });
    }
}

function loadInvoiceFilters() {
    $.ajax({
        url: urlGetInvoiceFilters,
        type: "GET",
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data.StatusList) {
                $("#ddlStatus").empty();
                $("#ddlStatus").append($('<option></option>').val(data.AllStatusOption.Id).html(data.AllStatusOption.Description));
                var sortedStatuses = data.StatusList.sort((a, b) => a.Description.localeCompare(b.Description));
                $.each(sortedStatuses, function (i, status) {
                    $("#ddlStatus").append($('<option></option>').val(status.Id).html(status.Description));
                });
            }

            if (data.RecordTypeList) {
                $("#ddlRecordType").empty();
                $.each(data.RecordTypeList, function (i, recordType) {
                    $("#ddlRecordType").append($('<option></option>').val(recordType.Code).html(recordType.Description));
                });
            }
        }
    });
}

function isDateRangeValid() {
    var startDate = $("#StartDatePicker").val();
    var endDate = $("#EndDatePicker").val();

    if (startDate && endDate) {
        var startMomentDate = moment(startDate, "DD/MM/YYYY");
        var endMomentDate = moment(endDate, "DD/MM/YYYY");

        return endMomentDate.isSameOrAfter(startMomentDate);
    }
    return false;
}

$(document).ready(function () {
    var localeBrowser = "es";
    if (window.navigator !== undefined) {
        if (window.navigator.languages !== undefined) {
            localeBrowser = (window.navigator.languages[0] || window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
        else {
            localeBrowser = (window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
    }

    loadInvoiceFilters();

    $('#StartDatePicker,#EndDatePicker,#PeopleOrgIdFilter,#FolioFilter,#ReceiverFilter').keypress(function (e) {
        if (e.keyCode === 13)
            Search();
    });

    $("#search").click(function () {
        Search();
    });

    $("#StartDatePicker").datetimepicker({
        viewMode: 'days',
        format: 'DD/MM/YYYY',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date(),
        maxDate: $.now()
    });

    $("#EndDatePicker").datetimepicker({
        viewMode: 'days',
        format: 'DD/MM/YYYY',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date(),
        maxDate: $.now()
    });

    $("#clearStartDate").click(function () {
        var endDate = $("#EndDatePicker").val();
        var bothSame = !endDate;

        StartDatePicker.value = "";

        if ($('#StartDateError').is(":visible")) {
            $('#StartDateError').hide();
            $("#StartDateGroup").removeClass('esg-has-error');
        }

        if (($('#StartDateMissing').is(":visible") || $('#EndDateMissing').is(":visible")) && bothSame) {
            $('#StartDateMissing').hide()
            $("#StartDateGroup").removeClass('esg-has-error');

            $('#EndDateMissing').hide()
            $("#EndDateGroup").removeClass('esg-has-error');
        }
    });

    $("#clearEndDate").click(function () {
        var startDate = $("#StartDatePicker").val();
        var bothSame = !startDate;

        EndDatePicker.value = "";

        if ($('#StartDateError').is(":visible")) {
            $('#StartDateError').hide();
            $("#StartDateGroup").removeClass('esg-has-error');
        }

        if (($('#StartDateMissing').is(":visible") || $('#EndDateMissing').is(":visible")) && bothSame) {
            $('#StartDateMissing').hide()
            $("#StartDateGroup").removeClass('esg-has-error');

            $('#EndDateMissing').hide()
            $("#EndDateGroup").removeClass('esg-has-error');
        }
    });

    $('#StartDatePicker,#EndDatePicker').datetimepicker().on('dp.hide', function (e) {
        var startDate = $("#StartDatePicker").val();
        var endDate = $("#EndDatePicker").val();
        var bothSame = Boolean(startDate) === Boolean(endDate)

        if (startDate && endDate && !isDateRangeValid()) {
            $('#StartDateError').show()
            $("#StartDateGroup").addClass('esg-has-error');
        }

        if (isDateRangeValid()) {
            $('#StartDateError').hide()
            $("#StartDateGroup").removeClass('esg-has-error');
        }

        if ($('#StartDateError').is(":visible") && !startDate) {
            $('#StartDateError').hide()
            $("#StartDateGroup").removeClass('esg-has-error');
        }

        if (($('#StartDateMissing').is(":visible") || $('#EndDateMissing').is(":visible")) && bothSame) {
            $('#StartDateMissing').hide()
            $("#StartDateGroup").removeClass('esg-has-error');

            $('#EndDateMissing').hide()
            $("#EndDateGroup").removeClass('esg-has-error');
        }
    });

    $("#btnClear").click(function () {
        StartDatePicker.value = "";
        $('#StartDateMissing').hide()
        $('#StartDateError').hide()
        $("#StartDateGroup").removeClass('esg-has-error');

        EndDatePicker.value = "";
        $('#EndDateMissing').hide()
        $("#EndDateGroup").removeClass('esg-has-error');

        $("#divAlertNoCriteria").addClass("no-display");
        $('#records_table_rows_tbody').empty();
        $('#noResultsAlert').hide();
        $('#hdnModelCount').val('0');
        ShowHideRows("#divDetails", 1);
        $('#divDetails').hide();

        $('#PeopleOrgIdFilter').val('');
        $('#FolioFilter').val('');
        $("#ddlStatus").val($("#ddlStatus option:first").val());
        $('#ReceiverFilter').val('');
        $("#ddlRecordType").val($("#ddlRecordType option:first").val());
    });
});