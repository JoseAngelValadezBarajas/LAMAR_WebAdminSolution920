var campus = 0;

$(document).ready(function () {
    var operatorId = "";
    var operatorList = [];
    var index = 0;
    var institutionId = [];
    var grantedId = [];

    $('#btnAddOperator').click(function () {
        window.location.href = urlAddOperator;
    });

    $('#btnAddVisualizationPer').click(function () {
        $('#ModalAddPermissions').css('display', 'block');
        $('#AddPermissionsDiv').css('display', 'block');
    });

    $('#btnCancelModal').click(function () {
        $('#OperatorIdModal').val('');
        $('#ModalAddPermissions').css('display', 'none');
        $('#AddPermissionsDiv').css('display', 'none');
    });

    $('#btnCloseModal').click(function () {
        $('#ModalAddPermissions').css('display', 'none');
        $('#AddPermissionsDiv').css('display', 'none');
    });

    $('#btnCancel').click(function () {
        window.location.href = urlOperatorIndex;
    });

    $('#btnDeleteModal').click(function () {
        $("#Overlaydiv").css('display', 'none');
        $('#divDeleteModal').css('display', 'none');
    });

    $('#btnCloseDeleteModal').click(function () {
        $("#Overlaydiv").css('display', 'none');
        $('#divDeleteModal').css('display', 'none');
    });

    $('#btnDeleteOperator').click(function () {
        $('#divDeleteModal').css('display', 'none');
        $("#Processing").css('display', 'block');
        var operatorDeleteId = operatorId;
        $.ajax({
            url: urlDeleteOperator,
            dataType: "json",
            cache: false,
            type: "POST",
            data: { operatorId: operatorDeleteId },
            success: function (response) {
                if (response.id === 1) {
                    $("#divError").css('display', 'none');
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    window.location.href = urlOperatorIndex;
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
    });

    $('button').click(function () {
        var id = $(this).attr('id');
        var actionId = id.split('_');        
        switch (String(actionId[0])) {
            case 'btnDelete':
                $("#Overlaydiv").css('display', 'block');
                $('#divDeleteModal').css('display', 'block');
                operatorId = String(actionId[1]);
                break;
            case 'btnEdit':
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');
                $.ajax({
                    url: urlSetOperator,
                    dataType: "html",
                    cache: false,
                    type: "GET",
                    data: { operatorId: String(actionId[1]), operatorName: String(actionId[2]), peopleCodeId: String(actionId[3]) },
                    success: function (response) {
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.location.href = urlEditOperator;
                    }
                });
                break;
        }
    });

    $('#OperatorId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlSearchOperator,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { operatorId: request.term },
                success: function (data) {
                    operatorList = data.operatorsList;
                    response($.map(data.operatorsList, function (item) {
                        return { label: item.OperatorId, value: item.OperatorId };
                    }));

                    $('.ui-autocomplete').css('min-width', '100px').css('max-width', '100%').css('max-height', '100px')
                        .css('overflow-y', 'auto').css('overflow-x', 'hidden').css('z-index', '9999!important').css('background', 'white');
                }
            });
        }
    });

    $('#OperatorId').blur(function () {
        var OperatorId = $(this).val();
        if (OperatorId !== "") {
            var response = $.grep(operatorList, function (element) { return element.OperatorId === OperatorId; });
            $('#NumberId').val(response[0].PeopleCodeId);
            $('#Name').val(response[0].Name);
        }
    });

    $('select').change(Change);

    $('#btnAddInstitution').click(function () {
        campus++;
        $('#divNoCampus').css('display', 'none');
        if ($('#divSignersInst_1').css('display') === 'none') {
            $('#divSignersInst_1').show();
            $('#divNoSigners_1').css('display', 'block');
            $('#Institutions_1 option:first').attr('value', 0);
            $('#IssuingPlace_1 option:first').attr('value', 0);
        }
        else {
            var index = $('select').length + 1;
            $('#Institutions_1 option:first').attr('value', 0);
            var divSignersInst = $("#divSignersInst_1").clone();
            divSignersInst.attr("id", "divSignersInst_" + index);
            divSignersInst.find("select").attr("id", "Institutions_" + index);
            divSignersInst.find("select").change(Change);
            divSignersInst.find("#divNoSigners_1").attr("id", "divNoSigners_" + index);
            divSignersInst.find("#divNoSignersInst_1").attr("id", "divNoSignersInst_" + index);
            divSignersInst.find("#divSignersTable_1").attr("id", "divSignersTable_" + index);
            divSignersInst.find("#btnDeleteInstitution_1").attr("id", "btnDeleteInstitution_" + index);
            divSignersInst.find("button").click(DeleteInstitution);
            $('#divSignersInstClon').append(divSignersInst);
        }
    });

    $('#btnAddVisualizationPer').click(function () {
        if ($('#divPermissionsTable').css('display') === 'none' || $('#PermissionRow_1').css('display') === 'none') {
            $('#divNoPermissions').css('display', 'none');
            $('#divPermissionsTable').css('display', 'block');
            $('#permissions_table_rows_tbody').show();
            $('#PermissionRow_1').show();
        }
        else {
            index = $('#permissions_table_rows_tbody tr').length + 1;
            var tr = $('#PermissionRow_1').clone();
            tr.find("#PermissionOperatorId_1").attr("id", "PermissionOperatorId_" + index);
            tr.find("#PermissionNumberId_1").attr("id", "PermissionNumberId_" + index);
            tr.find("#PermissionName_1").attr("id", "PermissionName_" + index);
            tr.find("#PermissionNoInstitutions_1").attr("id", "PermissionNoInstitutions_" + index);
            tr.find("#btnDelete_1").attr("id", "btnDelete_" + index);
            tr.find("#PermissionOperatorId_" + index).val('');
            tr.find("#PermissionNumberId_" + index).text('');
            tr.find("#PermissionName_" + index).text('');
            tr.find("#PermissionNoInstitutions_" + index).text('');
            tr.find('input[name="PermissionOperatorId"]').keypress(AutoComplete);
            tr.find('input[name="PermissionOperatorId"]').blur(Blur);
            tr.find('#btnDeletePermission_1').attr("id", "btnDeletePermission_" + index);
            tr.find(".btnDeletePermission").click(Delete);
            tr.attr("id", "PermissionRow_" + index);
            $('#permissions_table_rows_tbody').append(tr);
            index++;
        }
    });

    $('#btnSave').click(function () {        
        if ($('#institutionTab-content').css('display') === 'block' && $('#OperatorId').val() !== "") {
            $('select').each(function () {
                if ($(this).children("option:selected").val() !== 0 && $(this).val() !== "") {
                    institutionId.push($(this).children("option:selected").val());
                }
            });

            let arr = institutionId;            
            let map = {};
            let result = false;
            for (let i = 0; i < arr.length; i++) {                
                if (map[arr[i]]) {
                    result = true;                    
                    break;
                }                
                map[arr[i]] = true;
            }
            if (!result) {
                $("#divErrorDuplicate").css('display', 'none');
                if (institutionId.length > 0) {
                    $("#Processing").css('display', 'block');
                    $("#Overlaydiv").css('display', 'block');

                    var Operators = {
                        OperatorId: $('#OperatorId').val(),
                        UserName: userName,
                        CampusCodeId: institutionId
                    };

                    $.ajax({
                        url: urlCreateOperator,
                        dataType: "json",
                        cache: false,
                        type: "POST",
                        data: { operators: Operators },
                        success: function (response) {
                            institutionId = [];
                            if (response.id === 1) {
                                $("#divError").css('display', 'none');
                                $("#divSucces").css('display', 'block');
                                $("#Processing").css('display', 'none');
                                $("#Overlaydiv").css('display', 'none');
                                window.location.href = urlOperatorIndex;
                            }
                            else if (response.id === 0) {
                                window.location.href = urlUnauthorized;
                                $("#Processing").css('display', 'none');
                                $("#Overlaydiv").css('display', 'none');
                            }
                            else {
                                $("#divError").css('display', 'block');
                                $("#divSucces").css('display', 'none');
                                $("#Processing").css('display', 'none');
                                $("#Overlaydiv").css('display', 'none');
                            }
                        }
                    });
                }
                else {
                    window.location.href = urlOperatorIndex;
                }
            }
            else {
                $("#divErrorDuplicate").css('display', 'block');
                institutionId = [];
            }
        }

        if ($('#permissionTab-content').css('display') === 'block' && $('#OperatorId').val() !== "") {
            $('input[type="text"][name="PermissionOperatorId"]').each(function () {
                grantedId.push($(this).val());
            });
            if (grantedId !== "") {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                var OperatorsPerm = {
                    OperatorId: $('#OperatorId').val(),
                    UserName: userName,
                    GrantedOperatorId: grantedId
                };

                $.ajax({
                    url: urlCreatePermission,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { operators: OperatorsPerm },
                    success: function (response) {
                        if (response.id === 1) {
                            $("#divError").css('display', 'none');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            window.location.href = urlOperatorIndex;
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
                window.location.href = urlOperatorIndex;
            }
        }
    });       

    $('input[type="text"][name="PermissionOperatorId"]').keypress(AutoComplete);

    $('input[type="text"][name="PermissionOperatorId"]').blur(Blur);

    $('.btnDeletePermission').click(Delete);

    $('.btnDeleteInstitution').click(DeleteInstitution);

    $('#institutionTab').click(function () {
        $("#divError").css('display', 'none');
        $("#divErrorDuplicate").css('display', 'none');        
        $('#institutionLI').addClass('esg-is-active');
        $('#permissionLI').removeClass('esg-is-active');
        $('#institutionTab-content').css('display', 'block');
        $('#permissionTab-content').css('display', 'none');
    });

    $('#permissionTab').click(function () {
        $("#divError").css('display', 'none');
        $("#divErrorDuplicate").css('display', 'none');
        $('#permissionTab-content').css('display', 'block');
        $('#institutionTab-content').css('display', 'none');
        $('#permissionLI').addClass('esg-is-active');
        $('#institutionLI').removeClass('esg-is-active');
    });
});

function DeleteInstitution() {
    campus--;
    var id = $(this).attr('id');
    var delId = id.split('_');        
    if (String(delId[1]) === "1") {        
        $('#Institutions_1 option:first').attr('value', 0);
        $('#Institutions_1').val('0');
        $('#divSignersInst_' + String(delId[1])).hide();        
        if (campus === 0) {
            $('#divNoCampus').css('display', 'block');            
        }        
    }
    else {
        $('#divSignersInst_' + String(delId[1])).remove();
        if (campus === 0) {
            $('#divNoCampus').css('display', 'block');
        }
    }
}

function Change() {
    var id = $(this).attr('id');
    var idTable = id.split('_');
    var institutionId = $('#' + id).find('option:selected').val();
}

function AutoComplete() {
    var id = $(this).attr('id');
    $("#" + id).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlSearchOperatorPerm,
                dataType: "json",
                cache: false,
                type: "GET",
                data: { operatorId: request.term },
                success: function (data) {
                    permissionOperatorList = data.operatorsList;
                    response($.map(data.operatorsList, function (item) {
                        return { label: item.OperatorId, value: item.OperatorId };
                    }));

                    $('.ui-autocomplete').css('min-width', '100px').css('max-width', '100%').css('max-height', '100px')
                        .css('overflow-y', 'auto').css('overflow-x', 'hidden').css('z-index', '9999!important').css('background', 'white');
                }
            });
        }
    });
}

var permissionOperatorList = [];
function Blur() {
    var id = $(this).attr('id');
    var permissionOperatorId = $(this).val();
    var permissionId = id.split('_');
    if (permissionOperatorId !== "") {
        var response = $.grep(permissionOperatorList, function (element) { return element.OperatorId === permissionOperatorId; });
        if (response !== undefined) {
            $('#PermissionNumberId_' + String(permissionId[1])).text(response[0].PeopleCodeId);
            $('#PermissionName_' + String(permissionId[1])).text(response[0].Name);
            $('#PermissionNoInstitutions_' + String(permissionId[1])).text(response[0].Institutions);
        }        
    }
}

function Delete() {
    var id = $(this).attr('id');
    var permissionId = id.split('_');
    if (String(permissionId[1]) === "1") {
        $('#PermissionRow_' + String(permissionId[1])).hide();
        $("#PermissionOperatorId_" + String(permissionId[1])).val('');
        $("#PermissionNumberId_" + String(permissionId[1])).text('');
        $("#PermissionName_" + String(permissionId[1])).text('');
        $("#PermissionNoInstitutions_" + String(permissionId[1])).text('');
        if ($('#permissions_table_rows_tbody tr').length === 1) {
            $('#permissions_table_rows_tbody').hide();
            $('#divPermissionsTable').css('display', 'none');
            $('#divNoPermissions').css('display', 'block');
        }
    }
    else {
        $('#PermissionRow_' + String(permissionId[1])).remove();
        if ($('#permissions_table_rows_tbody tr').length === 1 && $('#PermissionRow_1').css('display') === 'none') {
            $('#permissions_table_rows_tbody').hide();
            $('#divPermissionsTable').css('display', 'none');
            $('#divNoPermissions').css('display', 'block');
        }
    }

}