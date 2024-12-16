$(document).ready(function () {
});

let xmlFile = null;
let xmlFileSaved = false;
let pdfFile = null;
let pdfFileSaved = false;
let statusSelected = '';
let ids = [];

function actionsAfterLoadTable() {
    $('#btnDownloadFiles').click((event) => {
        event.preventDefault();
        if (ids.length > 0) {
            let statusText = '';
            switch (statusSelected) {
                case 'G':
                    statusText = resources.lblGenerated;
                    break;
                case 'S':
                    statusText = resources.lblStamped;
                    break;
                case 'P':
                    statusText = resources.lblProcessedCorrectly;
                    break;
                case 'E':
                    statusText = resources.lblError;
                    break;
                case 'C':
                    statusText = resources.lblCanceled;
                    break;
                default:
                    break;
            }
            window.location.href = `${urlDownloadFiles}?status=${statusText}&ids=${JSON.stringify(ids)}`;
        }
    });
    cleanMultipleSelection();

    $('[id^=chkSelectRow_]').change((event) => {
        const status = event.target.dataset.status;
        const id = event.target.dataset.id;
        if (!statusSelected || status === statusSelected) {
            if (event.target.checked) {
                statusSelected = status;
                ids.push(parseInt(id, 10));
                $('#btnDownloadFiles').removeAttr('disabled');
                $('#FilesSelected').text(`${ids.length} ${resources.lblFilesSelected}`);
            }
            else {
                const index = ids.findIndex(s => s === parseInt(id, 10));
                ids.splice(index, 1);
                if (ids.length === 0) {
                    statusSelected = '';
                    $('#btnDownloadFiles').attr('disabled', 'disabled');
                    $('#FilesSelected').text('');
                }
                else {
                    $('#FilesSelected').text(`${ids.length} ${resources.lblFilesSelected}`);
                }
            }
        }
        else {
            $(event.target).prop('checked', false);
        }
    });

    $('[id^=btnMenu_]').click((event) => {
        event.preventDefault();
        const id = `#divMenu_${event.currentTarget.dataset.index}`;
        if ($(id).css('display') === 'none') {
            hideAllPopovers();
            $(id).show();
        }
        else {
            $(id).hide();
        }
    });

    $('[id^=btnMoreInfo_]').click((event) => {
        event.preventDefault();
        const id = `#divMoreInfo_${event.currentTarget.dataset.index}`;
        if ($(id).css('display') === 'none') {
            hideAllPopovers();
            $(id).show();
        }
        else {
            $(id).hide();
        }
    });

    $('[id^=btnStamp_]').click((event) => {
        event.preventDefault();
        const certificateOperation = {
            CertificateId: event.currentTarget.dataset.id,
            CertificateFileId: event.currentTarget.dataset.fileId,
            Status: 'S',
            GenerateXml: true
        };
        $.ajax({
            url: urlStamp,
            dataType: "json",
            cache: false,
            type: "POST",
            data: {
                certificateOperation: certificateOperation
            }
        })
            .done(function (response) {
                if (response.id === 1) {
                    $('#divGenerateError').css('display', 'none');
                    window.location.replace(window.location.href);
                }
                else {
                    $('#divGenerateError').css('display', 'block');
                    hidePageLoader();
                }
            })
            .fail(function () {
                window.location.replace(urlUnauthorized);
            });
    });

    $('[id^=btnDownload_]').click((event) => {
        event.preventDefault();
        window.location.href = `${urlDownloadXML}?id=${event.currentTarget.dataset.id}`;
    });

    $('[id^=btnUpdateStatus_]').click((event) => {
        event.preventDefault();
        cleanUpdateStatusModal();
        const id = event.currentTarget.dataset.id;
        const fileId = event.currentTarget.dataset.fileId;
        $('#PaymentFolio').val($(`#PaymentFolio_${id}`).text());
        $('#Notes').val($(`#Notes_${id}`).text());
        xmlFileSaved = $(`#btnDownloadXml_${id}`).data('has-file') === 'True';
        pdfFileSaved = $(`#btnDownloadPdf_${id}`).data('has-file') === 'True';
        if (xmlFileSaved) {
            $('#XmlName').text($(`#Folio_${id}`).text());
            $('#XmlSize').text($(`#btnDownloadXml_${id}`).data('size'));
            $('#XmlType').text('.xml');
            $('#divNoFiles').hide();
            $('#divFilesTable').show();
            $('#XmlFileRow').show();
        }
        if (pdfFileSaved) {
            $('#PdfName').text($(`#Folio_${id}`).text());
            $('#PdfSize').text($(`#btnDownloadPdf_${id}`).data('size'));
            $('#PdfType').text('.pdf');
            $('#divNoFiles').hide();
            $('#divFilesTable').show();
            $('#PdfFileRow').show();
        }
        $('#Overlaydiv').show();
        $('#divModalUpdateStatus').show();
        $('#hdnUpdateStatusId').val(id);
        $('#hdnUpdateStatusFileId').val(fileId);
    });

    $('#NewStatus').change((event) => {
        switch ($(event.target).val()) {
            case 'C':
                $('#divPaymentFolio').show();
                $('#divNotes').show();
                $('#uploadFilesSection').hide();
                $('#btnUpdateStatus').removeAttr('disabled');
                break;
            case 'E':
                $('#divPaymentFolio').show();
                $('#divNotes').show();
                $('#uploadFilesSection').hide();
                $('#btnUpdateStatus').removeAttr('disabled');
                break;
            case 'P':
                $('#divPaymentFolio').show();
                $('#divNotes').show();
                $('#uploadFilesSection').show();
                $('#divStampFileTitle').hide();
                $('#divProcessFileTitle').show();
                $('#btnLoadFile').prop('accept', '.pdf, .xml');
                if (pdfFile !== null || pdfFileSaved) {
                    $('#PdfFileRow').show();
                }
                $('#btnUpdateStatus').attr('disabled', 'disabled');
                if (xmlFile !== null || xmlFileSaved) {
                    $('#btnUpdateStatus').removeAttr('disabled');
                }
                break;
            case 'S':
                $('#divPaymentFolio').hide();
                $('#divNotes').show();
                $('#uploadFilesSection').show();
                $('#divStampFileTitle').show();
                $('#divProcessFileTitle').hide();
                $('#PdfFileRow').hide();
                $('#btnLoadFile').prop('accept', '.xml');
                $('#btnUpdateStatus').attr('disabled', 'disabled');
                if (xmlFile !== null || xmlFileSaved) {
                    $('#btnUpdateStatus').removeAttr('disabled');
                }
                break;
            default:
                $('#divPaymentFolio').hide();
                $('#divNotes').hide();
                $('#uploadFilesSection').hide();
                $('#btnUpdateStatus').attr('disabled', 'disabled');
                break;
        }
    });

    $('#btnChooseFile').click(() => {
        event.preventDefault();
        btnLoadFile.click();
    });

    $('#btnUpdateStatus').click((event) => {
        event.preventDefault();
        showPageLoaderOverModal();
        const certificateOperation = {
            CertificateId: parseInt($('#hdnUpdateStatusId').val(), 10),
            CertificateFileId: parseInt($('#hdnUpdateStatusFileId').val(), 10),
            Notes: $('#Notes').val(),
            PaymentFolio: $('#PaymentFolio').val(),
            PdfFile: null,
            Status: $('#NewStatus').val(),
            XmlFile: null
        };
        if (xmlFile && (certificateOperation.Status === 'S' || certificateOperation.Status === 'P')) {
            const xmlFileReader = new FileReader();
            xmlFileReader.onload = function () {
                if (xmlFileReader.result) {
                    certificateOperation.XmlFile = xmlFileReader.result.replace('data:text/xml;base64,', '');
                }
                if (pdfFile && certificateOperation.Status === 'P') {
                    var pdfFileReader = new FileReader();
                    pdfFileReader.onload = function () {
                        if (pdfFileReader.result) {
                            certificateOperation.PdfFile = pdfFileReader.result.replace('data:application/pdf;base64,', '');
                        }
                        updateStatus(certificateOperation);
                    };
                    pdfFileReader.readAsDataURL(pdfFile);
                }
                else {
                    updateStatus(certificateOperation);
                }
            };
            xmlFileReader.readAsDataURL(xmlFile);
        }
        else if (pdfFile && certificateOperation.Status === 'P') {
            const pdfFileReader = new FileReader();
            pdfFileReader.onload = function () {
                if (pdfFileReader.result) {
                    certificateOperation.PdfFile = pdfFileReader.result.replace('data:application/pdf;base64,', '');
                }
                updateStatus(certificateOperation);
            };
            pdfFileReader.readAsDataURL(pdfFile);
        }
        else {
            updateStatus(certificateOperation);
        }
    });

    $('#btnCloseModalUpdateStatus, #btnCancelUpdateStatus').click((event) => {
        event.preventDefault();
        cleanAndCloseUpdateStatusModal();
    });
}

function cleanUpdateStatusModal() {
    $('#NewStatus').val('');
    $('#PaymentFolio').val('');
    $('#divPaymentFolio').hide();
    $('#Notes').val('');
    $('#divNotes').hide();
    $('#uploadFilesSection').hide();
    $('#divStampFileTitle').hide();
    $('#divProcessFileTitle').hide();
    $("#btnLoadFile").val('');
    $('#divNoFiles').show();
    $('#divFilesTable').hide();
    $('#PdfFileRow').hide();
    $('#XmlFileRow').hide();
    $('#btnLoadFile').prop('accept', '');
    $('#XmlName').text('');
    $('#XmlSize').text('');
    $('#XmlType').text('');
    $('#PdfName').text('');
    $('#PdfSize').text('');
    $('#PdfType').text('');
    $('#hdnUpdateStatusId').val('');
    $('#hdnUpdateStatusFileId').val('');
    xmlFile = null;
    xmlFileSaved = false;
    pdfFile = null;
    pdfFileSaved = false;
    $('#btnUpdateStatus').attr('disabled', 'disabled');
}

function cleanAndCloseUpdateStatusModal() {
    $('#Overlaydiv').hide();
    $('#divModalUpdateStatus').hide();
    cleanUpdateStatusModal();
    hideAllPopovers();
    hidePageLoader();
}

function cleanMultipleSelection() {
    $.each($('[id^=chkSelectRow_]'), (_i, value) => {
        value.checked = false;
    });
    ids = [];
    statusSelected = '';
    $('#btnDownloadFiles').attr('disabled', 'disabled');
    $('#FilesSelected').text('');
}

function onECFileChange(files) {
    var file = files[0];
    if (file.type.includes('xml')) {
        $('#XmlName').text(file.name);
        $('#XmlSize').text((file.size / 1024).toFixed(2));
        $('#XmlType').text('.xml');
        $('#divNoFiles').hide();
        $('#divFilesTable').show();
        $('#XmlFileRow').show();
        $('#btnUpdateStatus').attr('disabled', 'disabled');
        if (file !== null && file.size > 0) {
            xmlFile = file;
            $('#btnUpdateStatus').removeAttr('disabled');
        }
    }
    else {
        $('#PdfName').text(file.name);
        $('#PdfSize').text((file.size / 1024).toFixed(2));
        $('#PdfType').text('.pdf');
        $('#divNoFiles').hide();
        $('#divFilesTable').show();
        $('#PdfFileRow').show();
        pdfFile = file;
    }
}

function updateStatus(certificateOperation) {
    $.ajax({
        url: urlUpdateStatus,
        dataType: "json",
        cache: false,
        type: "POST",
        data: {
            certificateOperation: certificateOperation
        }
    }).done(data => {
        if (data.id === 1) {
            if (certificateOperation.Status !== 'S') {
                $(`#PaymentFolio_${certificateOperation.CertificateId}`).text(certificateOperation.PaymentFolio);
                $(`#divPaymentFolio_${certificateOperation.CertificateId}`).show();
            }
            $(`#Notes_${certificateOperation.CertificateId}`).text(certificateOperation.Notes);
            $(`#StatusLabel_${certificateOperation.CertificateId}`)
                .removeClass('esg-label--pending')
                .removeClass('esg-label--draft')
                .removeClass('esg-label--success')
                .removeClass('esg-label--error');
            switch (certificateOperation.Status) {
                case 'C':
                    $(`#StatusLabel_${certificateOperation.CertificateId}`)
                        .html($('#NewStatus :selected').text());
                    break;
                case 'E':
                    $(`#StatusLabel_${certificateOperation.CertificateId}`)
                        .addClass('esg-label--error')
                        .html($('#NewStatus :selected').text());
                    break;
                case 'P':
                    $(`#StatusLabel_${certificateOperation.CertificateId}`)
                        .addClass('esg-label--success')
                        .html($('#NewStatus :selected').text());
                    break;
                case 'S':
                    $(`#StatusLabel_${certificateOperation.CertificateId}`)
                        .addClass('esg-label--draft')
                        .html($('#NewStatus :selected').text());
                    break;
                default:
                    $(`#StatusLabel_${certificateOperation.CertificateId}`)
                        .html('');
                    break;
            }
            $(`#btnMoreInfo_${certificateOperation.CertificateId}`).show();
            $(`#chkSelectRow_${certificateOperation.CertificateId}`).attr('data-status', certificateOperation.Status);
            if (certificateOperation.Status === 'P' && (xmlFile || xmlFileSaved || pdfFile || pdfFileSaved)) {
                if (xmlFile || xmlFileSaved) {
                    $(`#btnDownloadXml_${certificateOperation.CertificateId}`).show();
                    $(`#btnDownloadXml_${certificateOperation.CertificateId}`).data('size', $('#XmlSize').text());
                    $(`#btnDownloadXml_${certificateOperation.CertificateId}`).data('has-file', 'True');
                }
                if (pdfFile || pdfFileSaved) {
                    $(`#btnDownloadPdf_${certificateOperation.CertificateId}`).show();
                    $(`#btnDownloadPdf_${certificateOperation.CertificateId}`).data('size', $('#PdfSize').text());
                    $(`#btnDownloadPdf_${certificateOperation.CertificateId}`).data('has-file', 'True');
                }
                $(`#divSepFiles_${certificateOperation.CertificateId}`).show();
            }
            else {
                $(`#btnDownloadXml_${certificateOperation.CertificateId}`).hide();
                $(`#btnDownloadPdf_${certificateOperation.CertificateId}`).hide();
                $(`#divSepFiles_${certificateOperation.CertificateId}`).hide();
            }
            cleanAndCloseUpdateStatusModal();
            cleanMultipleSelection();
        }
        else if (data.id === 0) {
            redirectUnauthorized();
        }
        else {
            redirectException();
        }
    }).fail(() => redirectException());
}