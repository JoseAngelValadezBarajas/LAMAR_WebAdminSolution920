var selectedFederalEntity = '';
var selectedStates = [];
var selectedStatesDescription = [];
var dropOpen = [];

function enableDropdown(id) {
    if (selectedFederalEntity === '') {
        $("#divMappedStates_" + id).css('display', 'none');
        $("#divState_" + id).css('display', 'block');

        selectedFederalEntity = id;
    } else {
        $("#divMappedStates_" + id).css('display', 'block');
        $("#divState_" + id).css('display', 'none');

        selectedFederalEntity = '';
        selectedStates = [];
        selectedStatesDescription = [];
    }
}

function openDropDownOptions(id) {
    var ul = $("#ul_" + id);
    if (dropOpen.length > 0) {
        if (dropOpen.findIndex(x => x === id) == -1) {
            dropOpen.push(id);
        }
    }
    else {
        dropOpen.push(id);
    }
    if (ul.css('display') === 'none') {
        ul.css('display', 'block');
    } else {
        ul.css('display', 'none');
    }
}

function applyChanges(id) {
    selectedStates = [];
    selectedStatesDescription = [];

    if (dropOpen.length > 0 && dropOpen.findIndex(x => x === id) > -1) {
        var checkedStates = $(".statesFor" + id + ":checked");

        checkedStates.each(function () {
            selectedStates.push($(this).val());
        });

        checkedStates.each(function () {
            var label = $(this).next();
            selectedStatesDescription.push(label.text());
        });

        $("#divMappedStates_" + id).css('display', 'block');
        $("#divState_" + id).css('display', 'none');

        $("#divMappedStates_" + id).text(selectedStatesDescription.join(","));

        var model = {
            userName: userName,
            federalEntity: selectedFederalEntity,
            states: selectedStates
        };

        $.ajax({
            url: urlSaveFederalEntityMapping,
            dataType: "json",
            cache: false,
            type: "POST",
            data: model,
            success: function (response) {
                if (response.id === 1) {
                    $("#divError").css('display', 'none');
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    window.location.href = urlFederalEntitiesMapping;
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