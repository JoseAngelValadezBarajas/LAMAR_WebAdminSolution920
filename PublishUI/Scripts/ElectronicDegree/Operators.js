$(document).ready(function () {    
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

    $('button').click(function () {
        var id = $(this).attr('id');
        var actionId = id.split('_');
        switch (String(actionId[0])) {
            case 'btnDelete':
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');
                $.ajax({
                    url: urlDeleteOperator,
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    data: { operatorId: String(actionId[1]) },
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
        if ($('#divSignersInst_1').css('display') === 'none') {
            $('#divSignersInst_1').show();
            $('#divNoSigners_1').css('display', 'block');
            $('#Institutions_1 option:first').attr('value', 0);
        }
        else {
            var index = $('select').length + 1;
            $('#Institutions_1 option:first').attr('value', 0);
            var divSignersInst = $("#divSignersInst_1").clone();
            divSignersInst.attr("id", "divSignersInst_" + index);
            divSignersInst.find("select").attr("id", "Institutions_" + index);
            divSignersInst.find("table").attr("id", "institutionSigners_table_" + index);
            divSignersInst.find("table > tbody").attr("id", "signersInstitution_table_rows_tbody_" + index);
            divSignersInst.find("table > thead").attr("id", "signersInstitution_table_rows_thead_" + index);
            divSignersInst.find("table > tbody").empty();
            divSignersInst.find("table > thead").hide();
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
            if (institutionId.length > 0) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                var Operators = {
                    OperatorId: $('#OperatorId').val(),
                    UserName: userName,
                    InstitutionId: institutionId
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
        $('#institutionLI').addClass('esg-is-active');
        $('#permissionLI').removeClass('esg-is-active');
        $('#institutionTab-content').css('display', 'block');
        $('#permissionTab-content').css('display', 'none');
    });

    $('#permissionTab').click(function () {
        $('#permissionTab-content').css('display', 'block');
        $('#institutionTab-content').css('display', 'none');
        $('#permissionLI').addClass('esg-is-active');
        $('#institutionLI').removeClass('esg-is-active');
    });
});

function DeleteInstitution() {
    var id = $(this).attr('id');
    var delId = id.split('_');    
    if (String(delId[1]) === "1") {  
        $('#Institutions_1 option:first').attr('value', 0);
        $('#Institutions_1').val('0');
        $('#divSignersInst_' + String(delId[1])).hide();
        $('#signersInstitution_table_rows_tbody_' + delId[1]).empty();
        $('#signersInstitution_table_rows_thead_' + delId[1]).hide();
    }
    else {
        $('#divSignersInst_' + String(delId[1])).remove();
    }
}

function Change() {
    var id = $(this).attr('id');
    var idTable = id.split('_');
    var institutionId = $('#' + id).find('option:selected').val();
    var contentTable = "";
    var contentTr = "";
    $('#signersInstitution_table_rows_tbody_' + idTable[1]).empty();
    if (institutionId !== "") {
        $.ajax({
            url: urlGetInstSigner,
            dataType: "json",
            cache: false,
            type: "GET",
            async: true,
            data: { institutionId: institutionId },
            success: function (data) {
                if (data.singerInstitutionViewModel.InstitutionSignerLists.length > 0) {
                    $.each(data.singerInstitutionViewModel.InstitutionSignerLists, function (i, item) {
                        contentTr = "<tr class='esg-table-body__row' id=" + i + "><td class='esg-table-body__td'>" + item.EdSignerName + "</td><td class='esg-table-body__td'>" + item.EdAbreviationTitle + "</td></tr>";
                        $('#signersInstitution_table_rows_tbody_' + idTable[1]).append(contentTr);
                    });
                    contentTable = $('#signersInstitution_table_rows_tbody_' + idTable[1]);
                    $('#institutionSigners_table_' + idTable[1]).append(contentTable);
                    $('#signersInstitution_table_rows_thead_' + idTable[1]).show();
                    $('#divNoSigners_' + idTable[1]).css('display', 'none');
                    $('#divNoSignersInst_' + idTable[1]).css('display', 'none');
                    $('#divSignersTable_' + idTable[1]).css('display', 'block');
                }
                else {
                    $('#divNoSignersInst_' + idTable[1]).css('display', 'block');
                    $('#divNoSigners_' + idTable[1]).css('display', 'none');
                    $('#divSignersTable_' + idTable[1]).css('display', 'none');
                    $('#signersInstitution_table_rows_thead_' + idTable[1]).hide();
                }

            }
        });
    }
    else {
        $('#signersInstitution_table_rows_thead_' + idTable[1]).hide();
        $('#divNoSigners_' + idTable[1]).css('display', 'block');
    }
}

function AutoComplete() {
    var id = $(this).attr('id');
    $("#" + id).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlSearchOperator,
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
        $('#PermissionNumberId_' + String(permissionId[1])).text(response[0].PeopleCodeId);
        $('#PermissionName_' + String(permissionId[1])).text(response[0].Name);
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
        if ($('#permissions_table_rows_tbody tr').length === 1 && $('#PermissionRow_1').css('display') === 'none'){
            $('#permissions_table_rows_tbody').hide();
            $('#divPermissionsTable').css('display', 'none');
            $('#divNoPermissions').css('display', 'block');
        }
    }
    
}