var selectedBackgroundStudyType = '';
var selectedLevels = [];
var selectedLevelsDescription = [];

function enableDropdown(id) {
    if (selectedBackgroundStudyType === '') {
        $("#divMappedLevels_" + id).css('display', 'none');
        $("#divLevel_" + id).css('display', 'block');

        selectedBackgroundStudyType = id;
    } else {
        $("#divMappedLevels_" + id).css('display', 'block');
        $("#divLevel_" + id).css('display', 'none');

        selectedBackgroundStudyType = '';
        selectedLevels = [];
        selectedLevelsDescription = [];
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
    selectedLevels = [];
    selectedLevelsDescription = [];

    var checkedStates = $(".levelsFor" + id + ":checked");

    checkedStates.each(function () {
        selectedLevels.push($(this).val());
    });

    checkedStates.each(function () {
        var label = $(this).next();
        selectedLevelsDescription.push(label.text());
    });

    $("#divMappedLevels_" + id).css('display', 'block');
    $("#divLevel_" + id).css('display', 'none');

    $("#divMappedLevels_" + id).text(selectedLevelsDescription.join(","));

    var model = {
        userName: userName,
        backgroundStudyType: selectedBackgroundStudyType,
        levels: selectedLevels
    };

    $.ajax({
        url: urlSaveBackgroundStudyTypeMapping,
        dataType: "json",
        cache: false,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.id === 1) {
                $("#divError").css('display', 'none');
                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                window.location.href = urlBackgroundStudyTypesMapping;
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