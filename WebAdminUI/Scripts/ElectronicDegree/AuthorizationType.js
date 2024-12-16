var selectedAuthorizationType = '';
var selectedRvoes = [];
var selectedRvoesDescription = [];

function enableDropdown(id) {

    if (selectedAuthorizationType === '') {
        $("#divMappedRvoes_" + id).css('display', 'none');
        $("#divRvoe_" + id).css('display', 'block');

        selectedAuthorizationType = id;
    } else {

        $("#divMappedRvoes_" + id).css('display', 'block');
        $("#divRvoe_" + id).css('display', 'none');

        selectedAuthorizationType = '';
        selectedRvoes = [];
        selectedRvoesDescription = [];
    }
}

function openDropDownOptions(id) {
    var ul = $("#ul_" + id);
    if (ul.css('display') === 'none') {
        ul.css('display', 'block');
    } else {
        ul.css('display', 'none');
    }
}

function applyChanges(id) {
    selectedRvoes = [];
    selectedRvoesDescription = [];

    var checkedStates = $(".rvoesFor" + id + ":checked");

    checkedStates.each(function () {
        selectedRvoes.push($(this).val());
    });

    checkedStates.each(function () {
        var label = $(this).next();
        selectedRvoesDescription.push(label.text());
    });

    $("#divMappedRvoes_" + id).css('display', 'block');
    $("#divRvoe_" + id).css('display', 'none');

    $("#divMappedRvoes_" + id).text(selectedRvoesDescription.join(","));

    var model = {
        userName: userName,
        authorizationType: selectedAuthorizationType,
        rvoes: selectedRvoes
    };

    $.ajax({
        url: urlSaveAuthorizationTypeMapping,
        dataType: "json",
        cache: false,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.id === 1) {
                $("#divError").css('display', 'none');
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                window.location.href = urlAuthorizationTypeMapping;
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