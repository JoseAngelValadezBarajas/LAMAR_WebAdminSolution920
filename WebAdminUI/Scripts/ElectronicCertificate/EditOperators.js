var campus = 0;
var institutions = 0;

$(document).ready(function () {
    var operatorList = [];
    var index = 0;
    var institutionId = [];
    var grantedId = [];
    
    $('#btnCancel').click(function () {
        window.location.href = urlOperatorIndex;
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
            if (response !== undefined) {
                $('#NumberId').val(response[0].PeopleCodeId);
                $('#Name').val(response[0].Name);
            }            
        }
    });

    $('select').change(Change);

    $('#btnAddInstitutionEdit').click(function () {
        campus++;
        $('#divNoCampus').css('display', 'none');
        if ($('#divSignersInstAdded_1').css('display') === 'none') {
            $('#divSignersInstAdded_1').show();
            $('#divNoSignersAdded_1').css('display', 'block');
            $('#InstitutionsAdded_1 option:first').attr('value', 0);
        }
        else {
            var index = $('select').length + 1;
            $('#InstitutionsAdded_1 option:first').attr('value', 0);
            var divSignersInst = $("#divSignersInstAdded_1").clone();
            divSignersInst.attr("id", "divSignersInstAdded_" + index);
            divSignersInst.find("select").attr("id", "InstitutionsAdded_" + index);
            divSignersInst.find("select").change(Change);
            divSignersInst.find("#divNoSignersAdded_1").attr("id", "divNoSignersAdded_" + index).css('display', 'block');
            divSignersInst.find("#divNoSignersInstAdded_1").attr("id", "divNoSignersInstAdded_" + index);
            divSignersInst.find("#divSignersTableAdded_1").attr("id", "divSignersTableAdded_" + index);
            divSignersInst.find("#btnDeleteInstitutionAdded_1").attr("id", "btnDeleteInstitutionAdded_" + index);
            divSignersInst.find("button").click(DeleteInstitution);
            $('#divSignersInstClon').append(divSignersInst);
        }
    });

    $('#btnAddVisualizationPer').click(function () {
        if ($('#divPermissionsTable').css('display') === 'none') {
            $('#divNoPermissions').css('display', 'none');
            $('#divPermissionsTable').css('display', 'block');
            $('#permissions_table_rows_tbody').show();
            $('#PermissionRow_1').show();
            $('#PermissionOperatorId_1').attr('readonly', false);
            $('#PermissionOperatorId_1').attr('disabled', false);
            var numberId = $('<label>').attr("id", "lblNumberIdAdded_1");
            var name = $('<label>').attr("id", "lblNameAdded_1");
            var institutions = $('<label>').attr("id", "lblNoInstitutions_1");
            $("#PermissionNumberId_1").append(numberId);
            $("#PermissionName_1").append(name);
            $("#PermissionNoInstitutions_1").append(institutions);
        }
        else {
            index = $('#permissions_table_rows_tbody tr').length + 1;
            var tr = $('#PermissionRowAdded_1').clone();
            var numberIdAdd = $('<label>').attr("id", "lblNumberIdAdded_" + index);
            var nameAdd = $('<label>').attr("id", "lblNameAdded_" + index);
            var institutionsAdd = $('<label>').attr("id", "lblNoInstitutions_" + index);
            tr.find("#PermissionOperatorIdAdded_1").attr("id", "PermissionOperatorId_" + index).attr('readonly', false).attr('autocomplete', 'on');
            tr.find("#PermissionNumberIdAdded_1").attr("id", "PermissionNumberId_" + index);
            tr.find("#PermissionNameAdded_1").attr("id", "PermissionName_" + index);
            tr.find("#PermissionNoInstitutionsAdded_1").attr("id", "PermissionNoInstitutions_" + index);
            tr.find(".btnDelete").attr("id", "btnDeleteAdded_" + index);
            tr.find("#PermissionOperatorId_" + index).val('');
            tr.find("#PermissionNumberId_" + index).text('');
            tr.find("#PermissionName_" + index).text('');
            tr.find("#PermissionNoInstitutions_" + index).text('');
            tr.find('input[name="PermissionOperatorId"]').keypress(AutoComplete);
            tr.find('input[name="PermissionOperatorId"]').blur(Blur);
            tr.find(".btnDelete").click(Delete);
            tr.attr("id", "PermissionRow_" + index);
            tr.find("#PermissionNumberId_" + index).append(numberIdAdd);
            tr.find("#PermissionName_" + index).append(nameAdd);
            tr.find("#PermissionNoInstitutions_" + index).append(institutionsAdd);
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
                        url: urlUpdateOperator,
                        dataType: "json",
                        cache: false,
                        type: "POST",
                        data: { operators: Operators },
                        success: function (response) {
                            institutionId = [];
                            if (response.id === 1) {
                                $("#divSucces").css('display', 'block');
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
                                $("#divErrorDuplicate").css('display', 'block');
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
                if ($(this).val() !== "" && !$(this).is('[readonly]')) {
                    grantedId.push($(this).val());
                }
            });
            if (grantedId.length > 0) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                var OperatorsPerm = {
                    OperatorId: $('#OperatorId').val(),
                    UserName: userName,
                    GrantedOperatorId: grantedId
                };

                $.ajax({
                    url: urlUpdatePermission,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { operators: OperatorsPerm },
                    success: function (response) {
                        if (response.id === 1) {
                            $("#divSucces").css('display', 'block');
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
                            $("#divErrorDuplicate").css('display', 'block');
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
    });

    $('input[type="text"][name="PermissionOperatorId"]').keypress(AutoComplete);

    $('input[type="text"][name="PermissionOperatorId"]').blur(Blur);

    $('.btnDelete').click(Delete);

    $('.btnDeleteInstitution').click(DeleteInstitution);

    $('#institutionTab').click(function () {
        $('#institutionLI').addClass('esg-is-active');
        $('#permissionLI').removeClass('esg-is-active');
        $('#institutionTab-content').css('display', 'block');
        $('#permissionTab-content').css('display', 'none');
        $("#divSucces").css('display', 'none');
        $("#divError").css('display', 'none');
    });

    $('#permissionTab').click(function () {
        $('#permissionTab-content').css('display', 'block');
        $('#institutionTab-content').css('display', 'none');
        $('#permissionLI').addClass('esg-is-active');
        $('#institutionLI').removeClass('esg-is-active');
        $("#divSucces").css('display', 'none');
        $("#divError").css('display', 'none');
    });

    campus = $('select').length - 1;
    institutions = $('select').length - 1;
});

function DeleteInstitution() {    
    var id = $(this).attr('id');
    var delId = id.split('_');    
    if (String(delId[0]) === "btnDeleteInstitution") {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        $.ajax({
            url: urlDeleteInstitution,
            dataType: "json",
            cache: false,
            type: "POST",
            data: { institutionId: String(delId[2]), operatorId: String(delId[3]) },
            success: function (response) {
                if (response.id === 1) {
                    institutions--;
                    if (String(delId[1]) === "1") {
                        $('#divSignersInst_' + String(delId[1])).hide();                        
                    }
                    else {
                        $('#divSignersInst_' + String(delId[1])).remove();
                    }
                    if (institutions === 0) {
                        $('#divNoCampus').css('display', 'block');
                    }
                    $("#divError").css('display', 'none');
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
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
        campus--;
        if (String(delId[1]) === "1") {
            $('#InstitutionsAdded_1 option:first').attr('value', 0);
            $('#InstitutionsAdded_1').val('0');
            $('#divSignersInstAdded_' + String(delId[1])).hide();            
            if (campus === 0) {
                campus = 0;
                $('#divNoCampus').css('display', 'block');
            }
        }
        else {
            $('#divSignersInstAdded_' + String(delId[1])).remove();
            if (campus === 0) {
                campus = 0;
                $('#divNoCampus').css('display', 'block');
            }
        }
    }
}

function Change() {
    var id = $(this).attr('id');
    var idTable = id.split('_');    
}

function AutoComplete() {
    permissionOperatorList = [];
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
            $('#lblNumberIdAdded_' + String(permissionId[1])).text(response[0].PeopleCodeId);
            $('#lblNameAdded_' + String(permissionId[1])).text(response[0].Name);
            $('#lblNoInstitutions_' + String(permissionId[1])).text(response[0].Institutions);
        }        
    }
}

function Delete() {
    var id = $(this).attr('id');
    var permissionId = id.split('_');
    if (permissionId.length > 2) {
        if (String(permissionId[0]) === "btnDeleteAdded") {
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
        else {
            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');
            $.ajax({
                url: urlDeletePermission,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { operatorId: String(permissionId[2]), grantedOperatorId: String(permissionId[3]) },
                success: function (response) {
                    if (response.id === 1) {
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
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
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
    }
    else {
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
}