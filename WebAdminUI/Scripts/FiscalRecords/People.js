$(document).ready(function () {
    $('#tab1').click(function () {
        $('#tab2-content').removeClass("esg-is-active");
        $('#tab2').removeClass("esg-is-active");
        $('#tab1-content').addClass("esg-is-active");
        $('#tab1').addClass("esg-is-active");
        $('.esg-card--panel__footer').css('display', 'block');

    });

    $('#tab2').click(function () {
        $('#tab1-content').removeClass("esg-is-active");
        $('#tab1').removeClass("esg-is-active");
        $('#tab2-content').addClass("esg-is-active");
        $('#tab2').addClass("esg-is-active");
        $('.successMessageDiv').css('display', 'none');
        $('.esg-card--panel__footer').css('display', 'none');
        $('#YearTermSession').empty();

        $.ajax({
            url: urlGetYearTermSession,
            type: "GET",
            cache: false,
            dataType: "json",
            success: function (result) {

                if (result.YearTermSession != null) {

                    $.each(result.YearTermSession, function (i, val) {

                        if (val.Session != "")
                            $('#YearTermSession').append($('<option></option>').val(val.Year + "/" + val.Term + "/" + val.Session).html(val.Year + "/" + val.Term_Desc + "/" + val.Session_Desc));
                        else
                            $('#YearTermSession').append($('<option></option>').val(val.Year + "/" + val.Term).html(val.Year + "/" + val.Term_Desc));
                    });

                    $('#YearTermSession').css('display', 'block');

                    var YearTermSessionSelected = $("#YearTermSession option:selected").val();

                    $.ajax({
                        url: urlGetCharges,
                        type: "GET",
                        cache: false,
                        dataType: "html",
                        data: { YearTermSession: YearTermSessionSelected },
                        success: function (result) {
                            if (result != "") {
                                $("#getPeopleCharges").html(result);
                                $('#YearTermSession').css('display', 'block');
                                $('#divEmptyCharges').css('display', 'none');

                            }
                            else {
                                $('#YearTermSession').css('display', 'none');
                                $('#divEmptyCharges').css('display', 'block');
                            }
                        }
                    });
                }

                else {
                    $('#YearTermSession').css('display', 'none');
                    $('#divEmptyCharges').css('display', 'block');
                }

            }
        });

    });

    var TaxPayers = [];

    $('#TaxPayerId').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetReceivers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    if (data.length == 0) {
                        $("#TaxPayerId").val('');
                    }
                    else {
                        TaxPayers = data;
                        response($.map(data, function (item) {
                            if (item.TaxPayerId !== 'XEXX010101000' && item.TaxPayerId !== 'XAXX010101000')
                                return { label: item.TaxPayerId, value: item.TaxPayerId };
                        }));
                    }
                }
            });
        }
    });

    $('#TaxPayerId').focusout(function () {

        $('.successMessageDiv').css('display', 'none');
        $('.errorMessageDiv').css('display', 'none');

        var taxppayerIdComplete = $(this).val();
        var tempTaxpayerId = taxppayerIdComplete.split('-');
        var taxpayerId = $(this).val();
        if (tempTaxpayerId[0].length > 0 && (tempTaxpayerId[0].length == 12 || tempTaxpayerId[0].length == 13)) {
            var response;
            if (tempTaxpayerId.length > 1) {
                response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == tempTaxpayerId[0] && element.FiscalIdentityNumber == tempTaxpayerId[1]; });
            }
            else {
                response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == taxpayerId; });
            }
            if (response.length >= 0) {
                $('#PeopleNameCorpName').val(response[0].CorporateName);
                $.ajax({
                    url: urlGetCFDIReceivers,
                    type: "GET",
                    cache: false,
                    dataType: "json",
                    data: { length: tempTaxpayerId[0].length },
                    success: function (res) {
                        $('#CFDI').empty();
                        $("#CFDI").append($('<option></option>').val('').html(''));
                        $.each(res, function (i, val) {
                            $("#CFDI").append($('<option></option>').val(val.Code).html(val.Description));
                        });
                    }
                });
            }
            else {
                $('.successMessageDiv').css('display', 'none');
                $('.errorMessageDiv').css('display', 'block');
            }
        }
        else {
            $("#PeopleNameCorpName").val("");
            $('#CFDI').empty();
        }
    });

    $('#btnSave').click(function () {
        $('.successMessageDiv').css('display', 'none');
        $('#formPeople').validate();
        if ($('#formPeople').valid()) {
            var CFDISelected = $("#CFDI option:selected").text();
            var CFDICode = $("#CFDI option:selected").val();
            if (CFDISelected != "") {
                var CFDIDesc = CFDISelected.substring(CFDICode.length + 1, CFDISelected.length);
            }

            var ModelPeople = {
                PeopleCodeId: 'P' + $('#PeopleCodeId').val(),
                TaxpayerId: $('#TaxPayerId').val(),
                ReceiverEmail: $('#PeopleEmail').val(),
                CFDIUsageCode: CFDICode,
                CFDIUsageDesc: CFDIDesc
            };

            if ($('#TaxPayerId').val().length == 12 || $('#TaxPayerId').val().length == 13 || $('#TaxPayerId').val().length == 0) {
                $.ajax({
                    url: urlSaveTaxpayerIdPeople,
                    type: "POST",
                    cache: false,
                    dataType: "json",
                    data: { peopleModelView: JSON.stringify(ModelPeople) },
                    success: function (res) {

                        if (res >= 0) {
                            $('.successMessageDiv').css('display', 'block');
                            $('.errorMessageDiv').css('display', 'none');
                        }
                    }
                });
            }
            else {
                $('.successMessageDiv').css('display', 'none');
                $('.errorMessageDiv').css('display', 'block');
            }
        }
        else {
            $('.successMessageDiv').css('display', 'none');
        }
    });

    $('#btnCancel').click(function () {
        location.href = '../FiscalRecords/Menu';
    });

    $('#YearTermSession').change(function () {

        var YearTermSessionSelected = $("#YearTermSession option:selected").val();

        $.ajax({
            url: urlGetCharges,
            type: "GET",
            cache: false,
            dataType: "html",
            data: { YearTermSession: YearTermSessionSelected },
            success: function (result) {
                $("#getPeopleCharges").html(result);
            }
        });

    });

    $('body').on('click', '#CreatePPD', function () {

        var ChargeCreditNumber = $(this).attr('data-code-people');

        $.ajax({
            url: urlGetChargeCreditNumber,
            type: "GET",
            cache: false,
            dataType: "html",
            data: { ChargeCreditNumber: ChargeCreditNumber },
            success: function (response) {
                window.location.href = urlChargeCredit;
            }
        });

    });
});