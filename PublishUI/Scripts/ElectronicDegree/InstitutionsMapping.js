$(document).ready(function () {
    var majorIdList = [];
    var majorDeleteIdList = [];
    var rvoeList = [];

    $('button').click(function () {
        var id = $(this).attr('id').split('_');
        if (id[0] == 'btnEdit') {
            var institutionId = id[1];
            urlEditInstitution = urlEditInstitution.replace("param-id", institutionId);
            window.location.href = urlEditInstitution;
        }
        if (id[0] == 'btnAddMapping') {
            var institutionId = id[1];
            urlEditMapping = urlEditMapping.replace("param-id", institutionId);
            window.location.href = urlEditMapping;
        }
        if (id[0] == 'btnDelete') {
            var institutionId = id[1];
            var electronicDegreeInstMajorId = id[2];
            $.ajax({
                url: urlDeleteInstMajorMap,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { Id: electronicDegreeInstMajorId },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        urlMajorsMappingList = urlMajorsMappingList.replace("param-id", institutionId);
                        window.location.href = urlMajorsMappingList;
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
    });

    $('#btnCancel').click(function () {
        var params = new URLSearchParams(location.search);
        var institutionId = params.get('institutionId');
        urlMajorsMappingList = urlMajorsMappingList.replace("param-id", institutionId);
        window.location.href = urlMajorsMappingList;
    });

    $('#btnClose').click(function () {
        var params = new URLSearchParams(location.search);
        var institutionId = params.get('institutionId');
        urlMajorsMappingList = urlMajorsMappingList.replace("param-id", institutionId);
        window.location.href = urlMajorsMappingList;
    });

    $('#btnCancelSave').click(function () {
        window.location.href = urlMajorsMapping;
    });

    $('#btnContinue').click(function () {
        var params = new URLSearchParams(location.search);
        var institutionId = params.get('institutionId');
        $('input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                if ($(this).attr('name') != "True") {
                    var id = $(this).attr('id');
                    var majorId = id.split('_');
                    majorIdList.push(Number(majorId[1]));
                }
            }
            else if ($(this).is(":not(:checked)")) {
                if ($(this).attr('name') == "True") {
                    var id = $(this).attr('id');
                    var majorId = id.split('_');
                    majorDeleteIdList.push(Number(majorId[1]));
                }
            }
        });
        var Model = {
            InstitutionId: institutionId,
            MajorIdList: majorIdList,
            MajorDeleteIdList: majorDeleteIdList
        };

        $.ajax({
            url: urlMapInstitutionMajor,
            dataType: "json",
            cache: false,
            type: "POST",
            data: { MajorMappingModel: Model },
            success: function (response) {
                if (response.id === 1) {
                    $("#divError").css('display', 'none');
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    urlMajorsMappingList = urlMajorsMappingList.replace("param-id", institutionId);
                    window.location.href = urlMajorsMappingList;
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
        majorIdList = [];
        majorDeleteIdList = [];
    });

    $('#btnSave').click(function () {
        $("input[id^='ddlRvoe_']").each(function () {
            var id = $(this).attr('id');
            var majors = id.split('_');
            if ($(this).is(":checked")) {
                var rvoeItem = {
                    ElectronicDegreeInstMajorId: majors[1],
                    RvoeId: majors[2],
                    Checked: true
                }
            }
            else {
                var rvoeItem = {
                    ElectronicDegreeInstMajorId: majors[1],
                    RvoeId: majors[2],
                    Checked: false
                }
            }
            rvoeList.push(rvoeItem);
        });

        $.ajax({
            url: urlMapInstitutionMajorRvoe,
            dataType: "json",
            cache: false,
            type: "POST",
            data: { RvoeList: rvoeList },
            success: function (response) {
                if (response.id === 1) {
                    $("#divError").css('display', 'none');
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    window.location.href = urlMajorsMapping;
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
        rvoeList = [];
    });
});

function openDropDownOptions(id) {
    var ul = $("#ul_" + id);
    if (ul.css('display') === 'none') {
        ul.css('display', 'block');
    } else {
        ul.css('display', 'none');
    }
}