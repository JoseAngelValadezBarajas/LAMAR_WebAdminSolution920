$(document).ready(function () {
    $('body').on('click', '.btnCancel', function () {
        $('#validityDetailsDiv').css('display', 'none');
        window.location.href = urlListTaxProfiles;
    });

    $('body').on('click', '.btnSubmit', function () {

        var $table = $("#ValidityDetailsTable tbody");
        $("#Overlaydiv").css('display', 'none');

        var cols = [];
        $table.find('tr').each(function (rowIndex, r) {
            var mappingObject = new Object();
            mappingObject.ChargeCreditDescription = '';
            var taxProfileDetails = [];
            $(this).find(":input[type=hidden][name='item.TaxProfileDetailId']").each(function (index, v) {
                taxProfileDetails.push(v.value);
            });

            jQuery.each(taxProfileDetails, function (i, val) {
                rowId = val;
                $table.find('tr').find('td').each(function (colIndex, c) {
                    if (colIndex === 0) {
                        mappingObject.Percentage = c.textContent.trim();
                        mappingObject.FiscalRecordTaxMappingId = $("#fiscalRecordTaxMappingId_" + rowId).val();
                        mappingObject.TaxProfileDetailId = rowId;
                    }
                    if (colIndex === 3) {
                        mappingObject.TaxCode = $('#taxesDropdown_' + rowId + ' option:selected').val();
                        mappingObject.TaxDescription = $('#taxesDropdown_' + rowId + ' option:selected').text();
                    }
                    if (colIndex === 4) {
                        mappingObject.FactorType = $('#ratesDropdown_' + rowId + ' option:selected').val();
                        mappingObject.TaxRate = $('#ratesDropdown_' + rowId + ' option:selected').text().trim();
                        mappingObject.TaxRatesCatalog = new Object();
                        mappingObject.TaxesCatalog = new Object();
                    }
                });
                cols.push(mappingObject);
            });
        });

        $.ajax({
            contentType: 'application/json;',
            dataType: 'json',
            type: "POST",
            cache: false,
            url: urlCreateTaxProfile,
            data: JSON.stringify(cols),
            success: function (response) {
                if (response.id <= 0) {
                    $('.errorMessageDiv').show();
                    $('.successMessageDiv').hide();
                    $(".errorMessageResult").html(response.message);
                    $("#Overlaydiv").show();
                }
                else if (response.id === 1) {
                    $('.successMessageDiv').show();
                    $('.errorMessageDiv').hide();
                    $(".successMessageResult").html(response.message);
                    $("#Overlaydiv").show();
                    window.location.href = urlListTaxProfiles;
                }
            }
        });
    });
});

function getTaxRate(dropdownItem) {

    var selectedText = $(dropdownItem).find("option:selected").text();

    var taxText = selectedText.split(" - ");

    $.ajax({
        url: urlGetTaxRateCatalog,
        type: "GET",
        cache: false,
        dataType: "json",
        data: { tax: taxText[1] },
        success: function (data) {

            var dropId = dropdownItem.id;
            var rateDropdown = "ratesDropdown_" + dropId.substring(14, dropId.length);

            $($(document.getElementById(rateDropdown))).empty();
            $.each(data, function (i, val) {
                $($(document.getElementById(rateDropdown))).append($('<option></option>').val(val.Code).html(val.Description));
            });
        }
    });
}

function getProfileDetails(validityDropdown) {
    var validityId = $(validityDropdown).val();

    $.ajax({
        url: urlValidityDetails,
        type: "GET",
        cache: false,
        data: { id: validityId },
        dataType: "html",
        success: function (data) {
            $("#validityDetailsDiv").html(data);
            $("#Overlaydiv").css('display', 'block');
            $('#validityDetailsDiv').css('display', 'block');
        }
    });
}
