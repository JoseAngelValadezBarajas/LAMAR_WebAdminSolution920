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
    // #endregion Variables

    newSearch();

    $('#lnkNewSearch').click(() => newSearch());

    $('#btnBasicSearch').click((event) => {
        event.preventDefault();
        $("#ajaxpanel_GeneratedTable").load(encodeURI(generateUrlTable(false, $('#Keywords').val())), () => commonActionsAfterLoadTable());
    });

    $('#Keywords').keyup((event) => {
        if (event.key === "Enter") {
            $("#ajaxpanel_GeneratedTable").load(encodeURI(generateUrlTable(false, $('#Keywords').val())), () => commonActionsAfterLoadTable());
        }
    });

    $('#lnkAdvancedSearch').click(() => {
        $('#Overlaydiv').show();
        $('#divModalAdvancedSearch').show();
    });

    $('#IssuingDate').datetimepicker({
        viewMode: 'days',
        format: 'YYYY/MM/DD',
        locale: localeBrowser,
        useCurrent: false,
        date: null
    });

    $('#btnCloseModalAdvancedSearch').click((event) => {
        event.preventDefault();
        $('#Overlaydiv').hide();
        $('#divModalAdvancedSearch').hide();
    });

    $('#btnAdvancedSearch').click((event) => {
        event.preventDefault();
        $("#ajaxpanel_GeneratedTable").load(encodeURI(generateUrlTable(true, '',
            $('#Folio').val(), $('#Student').val(), $('#Campus').val(),
            $('#Program').val(), $('#CertificationType').val(),
            $('#IssuingDate').val(), $('#Status').val())), () => commonActionsAfterLoadTable());
        $('#Overlaydiv').hide();
        $('#divModalAdvancedSearch').hide();
    });

    $('#btnClearAdvancedSearch').click((event) => {
        event.preventDefault();
        $('#Folio').val('');
        $('#Student').val('');
        $('#Campus').val('');
        $('#Program').val('');
        $('#CertificationType').val('');
        $('#IssuingDate').val('');
        $('#Status').val('');
    });
});

function generateUrlTable(advanced, keywords, folio, student, campus, program, certificationType, issuingDate, status) {
    if (!keywords) {
        keywords = '';
    }
    if (!folio) {
        folio = '';
    }
    if (!student) {
        student = '';
    }
    if (!campus) {
        campus = '';
    }
    if (!program) {
        program = '';
    }
    if (!certificationType) {
        certificationType = '';
    }
    if (!issuingDate) {
        issuingDate = '';
    }
    if (!status) {
        status = '';
    }
    return `${urlTable}?Advanced=${advanced}&Keywords=${keywords}&Folio=${folio}&Student=${student}&CampusCodeId=${campus}&MajorId=${program}&CertificationTypeId=${certificationType}&IssuingDate=${issuingDate}&Status=${status}`;
}

function generateUrlViewData(id) {
    return `${urlViewData}?id=${id}`;
}

function newSearch() {
    $('#Keywords').val('');
    $('#Folio').val('');
    $('#Student').val('');
    $('#Campus').val('');
    $('#Program').val('');
    $('#CertificationType').val('');
    $('#IssuingDate').val('');
    $('#Status').val('');
    $("#ajaxpanel_GeneratedTable").load(encodeURI(generateUrlTable(false, '', '', '', '', '', '', '', '')), () => commonActionsAfterLoadTable());
}

function cleanConfirmationModal() {
    $('#Overlaydiv').hide();
    $('#divDeleteConfirmationModal').hide();
    $('#hdnDeleteId').val('');
    $('#chkDeleteConfirmation').prop('checked', false);
    $('#btnAcceptDeleteConfirmationModal').attr('disabled', 'disabled');
    hideAllPopovers();
    hidePageLoader();
}

function commonActionsAfterLoadTable() {
    applyPagination();

    $('[id^=btnViewData_]').click((event) => {
        event.preventDefault();
        $("#ajaxpanel_ViewData").load(encodeURI(generateUrlViewData(event.target.dataset.id)), () => {
            $('#Overlaydiv').show();
            $('#divModalViewData').show();
            $('#btnCloseModalViewData').click(() => {
                $('#Overlaydiv').hide();
                $('#divModalViewData').hide();
                hideAllPopovers();
            });
        });
    });

    $('[id^=btnDelete_]').click((event) => {
        event.preventDefault();
        $('#Overlaydiv').show();
        $('#divDeleteConfirmationModal').show();
        $('#hdnDeleteId').val(event.currentTarget.dataset.id);
    });

    $('#chkDeleteConfirmation').change((event) => {
        if (event.target.checked) {
            $('#btnAcceptDeleteConfirmationModal').removeAttr('disabled');
        }
        else {
            $('#btnAcceptDeleteConfirmationModal').attr('disabled', 'disabled');
        }
    });

    $('#btnAcceptDeleteConfirmationModal').click((event) => {
        event.preventDefault();
        showPageLoader();
        $.ajax({
            url: urlDelete,
            dataType: 'json',
            cache: false,
            type: 'POST',
            data:
            {
                id: $('#hdnDeleteId').val()
            }
        }).done(data => {
            if (data.id === 1) {
                $(`#ecRow_${$('#hdnDeleteId').val()}`).remove();
                let count = parseInt($('#hdnModelCount').val(), 10);
                count--;
                if (count <= 0) {
                    $('#divECTable').hide();
                    $('#divECEmptyTable').show();
                }
                else {
                    $('#hdnModelCount').val(count);
                    ShowHideRows('#divECTable', 1);
                }
                cleanConfirmationModal();
                cleanMultipleSelection();
            }
            else if (data.id === 0) {
                redirectUnauthorized();
            }
            else {
                redirectException();
            }
        }).fail(() => redirectException());
    });

    $('#btnCloseDeleteConfirmationModal, #btnCancelDeleteConfirmationModal').click((event) => {
        event.preventDefault();
        cleanConfirmationModal();
    });

    actionsAfterLoadTable();
}