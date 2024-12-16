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
                            $('#YearTermSession').append($('<option></option>').val(val.Year + "/" +  val.Term + "/" + val.Session).html(val.Year + "/" + val.Term_Desc + "/" + val.Session_Desc));
                        else
                            $('#YearTermSession').append($('<option></option>').val(val.Year + "/" +  val.Term).html(val.Year + "/" + val.Term_Desc));
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
                                $("#getOrganizationCharges").html(result);
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

                else
                {
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
                url: urlGetReceiver,
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
                            if (item.TaxPayerId === 'XEXX010101000')
                                return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber }
                            else
                                return { label: item.TaxPayerId, value: item.TaxPayerId }
                        }))
                    }
                }
            })
        }
    });

    $('#TaxPayerId').focusout(function () {

        $('.successMessageDiv').css('display', 'none');
        $('.errorMessageDiv').css('display', 'none');

        var taxpayerId = $(this).val();
        if (taxpayerId.length > 0 && (taxpayerId.length == 12 || taxpayerId.length == 13)) {
            var response;
            response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == taxpayerId; })
            if (response.length >= 0) {
                $('#CorporateName').val(response[0].CorporateName)
                $.ajax({
                    url: urlGetCFDIReceiver,
                    type: "GET",
                    cache: false,
                    dataType: "json",
                    data: { length: taxpayerId.length },
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
            $("#CorporateName").val("");
            $('#CFDI').empty();
        }
    });

    $('#btnSave').click(function () {
        $('.successMessageDiv').css('display', 'none');
        var formOrganization = $('#formOrganization');
        formOrganization.validate();
        if (formOrganization.valid()) {
            var CFDISelected = $("#CFDI option:selected").text();
            var CFDICode = $("#CFDI option:selected").val();
            if (CFDISelected != "") {
                var CFDIDesc = CFDISelected.substring(CFDICode.length + 1, CFDISelected.length);
            }

            var organizationModel = {
                OrganizationCodeId: 'O' + $('#OrganizationCodeIdHidden').val(),
                TaxpayerId: $('#TaxPayerId').val(),
                Email: $('#OrganizationEmail').val(),
                CFDIUsageCode: CFDICode,
                CFDIUsageDesc: CFDIDesc
            };

            if ($('#TaxPayerId').val().length == 12 || $('#TaxPayerId').val().length == 13 || $('#TaxPayerId').val().length == 0) {
                $.ajax({
                    url: urlSaveTaxpayerIdOrg,
                    type: "POST",
                    cache: false,
                    dataType: "json",
                    data: { organizationViewModel: JSON.stringify(organizationModel) },
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
        window.location.href = urlMenu;
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
                $("#getOrganizationCharges").html(result);
            }
        });

    });

    $('body').on('click', '#CreatePPD', function () {

        var ChargeCreditNumber = $(this).attr('data-code-organization');

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