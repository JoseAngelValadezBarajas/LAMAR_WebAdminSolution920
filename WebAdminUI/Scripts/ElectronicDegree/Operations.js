$(document).ready(function () {
    var ElectronicDegreeId;
    var DeleteId;
    loadTable();

    $(document).on("click", "button", function () {
        if ($(this).attr('id') !== undefined) {
            var id = $(this).attr('id').split('_');
            switch (id[0]) {
                case 'btnMore':
                    if ($("#divMenu" + id[1]).css('display') === 'none') {
                        $("#divMenu" + id[1]).show('slow');
                    } else {
                        $("#divMenu" + id[1]).hide('slow');
                    }
                    break;

                case 'btnView':
                    url = url.replace("param-id", Number(id[1]));
                    $("#ajaxpanel_ViewElectronicDegreeInfo").load(url);
                    break;

                case 'btnDelete':
                    DeleteId = $(this).attr('id').split('_');
                    $("#divConfirmationDialog").css('display', 'block');
                    break;
            }
        }
    });

    $(document).on("click", 'a[href="#ViewData"]', function () {
        var id = $(this).attr('id').split('_');
        url = url.replace("param-id", Number(id[1]));
        $("#ajaxpanel_ViewElectronicDegreeInfo").load(url);
    });

    $(document).on("click", 'a[href="#Clear"]', function () {
        $('#Folio').val('');
        $('#Student').val('');
        $('#EducationLevel').val('');
        $('#Major').val('');
    });

    $('#btnSearch').click(function () {
        var newUrl = urlGetResults;
        if ($('#Folio').val() !== '')
            newUrl = newUrl.replace("param-folio", $('#Folio').val());

        if ($('#Student').val() !== '')
            newUrl = newUrl.replace("param-student", $('#Student').val());

        if ($('#EducationLevel').val() !== '')
            newUrl = newUrl.replace("param-degreeType", $("#EducationLevel option:selected").text());

        if ($('#Major').val() !== '')
            newUrl = newUrl.replace("param-major", $("#Major option:selected").text());

        $("#ajaxpanel_GeneratedTable").load(encodeURI(newUrl));
    });

    $(document).on("click", "#btnClose", function () {
        var url = window.location.href.replace("#ViewData", "");
        window.location.href = url;
    });

    $(document).on("click", 'a[name="Delete"]', function () {
        DeleteId = $(this).attr('id').split('_');
        $("#divConfirmationDialog").css('display', 'block');
    });

    $(document).on("click", '#btnAcceptDelete', function () {
        DeleteED(Number(DeleteId[1]));
    });

    $(document).on("click", 'a[name="Cancel"]', function () {
        var id = $(this).attr('id').split('_');
        ElectronicDegreeId = Number(id[2]);
        $("#Overlaydiv").css('display', 'block');
        $('#spnFolio').text(String(id[1]));
        $("#divCancelation").css('display', 'block');
    });

    $('#CloseCancelModal').click(function () {
        $("#divCancelation").css('display', 'none');
        $("#Overlaydiv").css('display', 'none');
    });

    $('#CancelModal').click(function () {
        $("#divCancelation").css('display', 'none');
        $("#Overlaydiv").css('display', 'none');
    });

    $('#CloseConfirmationDialog').click(function () {
        $("#divConfirmationDialog").css('display', 'none');
        $("#Overlaydiv").css('display', 'none');
    });

    $('#btnCancelDelete').click(function () {
        $("#divConfirmationDialog").css('display', 'none');
        $("#Overlaydiv").css('display', 'none');
    });

    $('#SendCancelation').click(function () {
        if ($('#ReasonCancelCatalog').val() !== '') {
            $('#ReasonCancelCatalog').parent().removeClass('has-error');
            $('#ReasonError').css('display', 'none');
            $("#divReason").removeClass('esg-has-error');
            $("#divReasonErr").css('display', 'none');
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');
            $("#divCancelation").css('display', 'none');
            var document = {
                ElectronicDegreeId: ElectronicDegreeId,
                OperatorName: userName,
                CancelationCode: $('#ReasonCancelCatalog option:selected').val(),
                CancelationDescription: $('#ReasonCancelCatalog option:selected').text()
            };
            $.ajax({
                url: urlCancel,
                dataType: "json",
                cache: false,
                type: "POST",
                data: document,
                success: function (response) {
                    if (response.id === 1) {
                        $("#divCancelation").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.reload(true);
                    }
                    else if (response.id === 0) {
                        window.location.href = urlUnauthorized;
                        $("#divCancelation").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                    else {
                        $("#divCancelation").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                }
            });
        }
        else {
            $('#ReasonCancelCatalog').parent().addClass('has-error');
            $('#ReasonError').css('display', 'block');
            $("#divReason").addClass('esg-has-error');
            $("#divReasonErr").css('display', 'block');
        }
    });

    $('#ReasonCancelCatalog').change(function () {
        if ($(this).val() !== '') {
            $('#ReasonCancelCatalog').parent().removeClass('has-error');
            $('#ReasonError').css('display', 'none');
            $("#divReason").removeClass('esg-has-error');
            $("#divReasonErr").css('display', 'none');
        }
        else {
            $('#ReasonCancelCatalog').parent().addClass('has-error');
            $('#ReasonError').css('display', 'block');
            $("#divReason").addClass('esg-has-error');
            $("#divReasonErr").css('display', 'block');
        }
    });
});

function SendED(id) {
    var model = {
        electronicDegreeId: id
    };
    $("#Processing").css('display', 'block');
    $("#Overlaydiv").css('display', 'block');
    $.ajax({
        url: urlStampAndSend,
        dataType: "json",
        cache: false,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.id === 1) {
                $("#divError").css('display', 'none');
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                console.log(response);
                if (response.status === 0) {
                    window.location.reload(true);
                }

                $("#divResults").css("display", "block");
                $("#status" + response.status).css("display", "block");

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

function UpdateStatusED(id) {
    var model = {
        electronicDegreeId: id
    };
    $("#Processing").css('display', 'block');
    $("#Overlaydiv").css('display', 'block');
    $.ajax({
        url: urlUpdateStatus,
        dataType: "json",
        cache: false,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.id === 1) {
                $("#divError").css('display', 'none');
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                console.log(response);
                if (response.status === 0) {
                    window.location.reload(true);
                } else {
                    $("#divResults").css("display", "block");
                    $("#status" + response.status).css("display", "block");
                }
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

function DeleteED(id) {
    $("#Processing").css('display', 'block');
    $("#Overlaydiv").css('display', 'block');
    $.ajax({
        url: urlDelete,
        dataType: "json",
        cache: false,
        type: "POST",
        data: { electronicDegreeId: id },
        success: function (response) {
            if (response.id === 1) {
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                window.location.reload(true);
            }
            else if (response.id === 0) {
                window.location.href = urlUnauthorized;
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
            }
            else {
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
            }
        }
    });
}

function loadTable() {
    var newUrl = urlGetResults;
    newUrl = newUrl.replace("param-folio", '*');
    newUrl = newUrl.replace("param-student", '*');
    newUrl = newUrl.replace("param-degreeType", '*');
    newUrl = newUrl.replace("param-major", '*');

    $("#ajaxpanel_GeneratedTable").load(encodeURI(newUrl));
}