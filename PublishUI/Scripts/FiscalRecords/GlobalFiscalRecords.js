var paymentTypesBackup;
var dateFormat = 'DD/MM/YYYY';
$(document).ready(function () {
    var IsSubstitution = $("#IsSubstitution").val() == 'True';
    var localeBrowser = "es";
    var ExpeditionDate = "";
    var validateDate = moment(new Date()).endOf('month').add(3, "days").format('YYYY-MM-DD');
    var expDate = new Date();

    if (window.navigator !== undefined) {
        if (window.navigator.languages !== undefined) {
            localeBrowser = (window.navigator.languages[0] || window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
        else {
            localeBrowser = (window.navigator.userLanguage || window.navigator.language).substring(0, 2);
        }
    }

    // EmisFact ACOLLI 04112021
    $("#IssueAlert").css('display', 'none');
    $("#IssueDate").datetimepicker({
        viewMode: 'days',
        format: 'DD/MM/YYYY',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });
    var defDay = moment(new Date());
    //$('#IssueDate').data('DateTimePicker').minDate(defDay);

    $("#IssueTime").datetimepicker({
        // viewMode: 'days',
        format: 'HH:mm',
        locale: localeBrowser,
        useCurrent: false,
        date: new Date()
    });
    // END EmisFact

    // EmisFact ACOLLI 04112021

    var strExpDate = String(moment(new Date()).format('YYYY-MM-DD'));
    // String(moment(new Date()).endOf('month').add(3,"days").format('YYYY-MM-DD'));
    var strExpTime = String(moment(new Date()).format('HH:mm'));
    ExpeditionDate = `${strExpDate} ${strExpTime}`;
    // new Date(`${strExpDate} ${strExpTime}`);
    // console.log((moment(ExpeditionDate) > moment(validateDate)) ? "La fecha y Hora de generación esta fuera de rango" : "pasa");

    $('#IssueDate').datetimepicker().on('dp.change', function (e) {
        var selectedDate = moment(new Date(e.date));
        // expDate = selectedDate.endOf('month').format('YYYY-MM-DD');
        // expDate = moment(expDate).add(3,"days");
        strExpDate = moment(new Date(e.date)).format('YYYY-MM-DD');
        ExpeditionDate = `${strExpDate} ${strExpTime}`;
        // console.log("mes corriente: ", validateDate, "fecha emision", ExpeditionDate);

        if (moment(ExpeditionDate) > moment(validateDate)) {
            // console.log($("#IssueAlert").is(":visible"));
            if (!$("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'block');
            }
        }
        else {
            // console.log($("#IssueAlert").is(":visible"));
            if ($("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'none');
            }
        }
        // console.log((moment(ExpeditionDate) > moment(validateDate)) ? "La fecha y Hora de generación esta fuera de rango" : "pasa");
        $(this).data("DateTimePicker").hide();
    });

    $("#IssueTime").datetimepicker().on('dp.change', function (e) {
        // console.log(moment(new Date(e.date)).format("HH:mm"));
        strExpTime = moment(new Date(e.date)).format("HH:mm");
        // console.log("concat data: ", strExpDate + " " + strExpTime);
        ExpeditionDate = `${strExpDate} ${strExpTime}`;
        // console.log("mes corriente: ", validateDate, "fecha emision", ExpeditionDate);
        if (moment(ExpeditionDate) > moment(validateDate)) {
            // console.log($("#IssueAlert").is(":visible"));
            if (!$("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'block');
            }
        }
        else {
            // console.log($("#IssueAlert").is(":visible"));
            if ($("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'none');
            }
        }
        // console.log((moment(ExpeditionDate) > moment(validateDate)) ? "La fecha y Hora de generación esta fuera de rango" : "pasa");
    });
    // END EmisFact ACOLLI 04112021

    paymentTypesBackup = $('#PaymentType').html();

    if (IsSubstitution) {
        $('#StartDate').attr('readonly', true);
        $('#EndDate').attr('readonly', true);
        $('#noFiscalRecordsDiv').show();
        ShowHideRows("#divCharges", 1);
    }
    else {
        $("#StartDate").datetimepicker({
            viewMode: 'days',
            format: dateFormat,
            locale: localeBrowser,
            useCurrent: false,
            date: null,
            maxDate: $.now()
        });

        $("#EndDate").datetimepicker({
            viewMode: 'days',
            format: dateFormat,
            locale: localeBrowser,
            useCurrent: false,
            date: null,
            maxDate: $.now()
        });

        $('#StartDate').datetimepicker().on('dp.change', function (e) {
            $(this).data("DateTimePicker").hide();
        });

        $('#StartDate').datetimepicker().on('dp.hide', function (e) {
            if (e.date) {
                if (!$("#StartDate").val()) {
                    $("#StartDate").val(e.date.format(dateFormat));
                }
                updateFilters();
            }

            var startDate = $("#StartDate").val();
            var endDate = $("#EndDate").val();
            if (startDate && $('#StartDateRequired').is(":visible") || (isDateRangeValid() || !endDate)) {
                $('#StartDateIcon').hide();
                $("#StartDateGroup").removeClass('esg-has-error');
            }

            if (startDate != "") {
                $('#StartDateRequired').hide();
            }

            if (startDate && endDate && !isDateRangeValid()) {
                $('#StartDateError').show()
                $('#StartDateIcon').show();
                $("#StartDateGroup").addClass('esg-has-error');
            }

            if (isDateRangeValid()) {
                $('#StartDateError').hide()
            }

            if ($('#StartDateError').is(":visible") && !startDate) {
                $('#StartDateError').hide()
                $('#StartDateIcon').hide();
                $("#StartDateGroup").removeClass('esg-has-error');
            }
        });

        $('#EndDate').datetimepicker().on('dp.change', function (e) {
            $(this).data("DateTimePicker").hide();
        });

        $('#EndDate').datetimepicker().on('dp.hide', function (e) {
            if (e.date) {
                if (!$("#EndDate").val()) {
                    $("#EndDate").val(e.date.format(dateFormat));
                }
                updateFilters();
            }

            var startDate = $("#StartDate").val();
            var endDate = $("#EndDate").val();
            if (endDate != "") {
                $('#EndDateRequired').hide();
                $('#EndDateIcon').hide();
                $("#EndDateGroup").removeClass('esg-has-error');
            }
            else {
                $('#StartDateIcon').hide();
                $("#StartDateGroup").removeClass('esg-has-error');
                $('#StartDateRequired').hide();
                $('#StartDateError').hide()
            }

            if (startDate && endDate && !isDateRangeValid()) {
                $('#StartDateError').show()
                $('#StartDateIcon').show();
                $("#StartDateGroup").addClass('esg-has-error');
            }

            if (isDateRangeValid()) {
                $('#StartDateError').hide()
                $('#StartDateIcon').hide();
                $("#StartDateGroup").removeClass('esg-has-error');
            }
        });
    }

    function updateFilters() {
        var startDate = $("#StartDate").val();
        var endDate = $("#EndDate").val();

        if (startDate != "" && endDate != "") {

            $("#Processing").css('display', 'block');
            $("#Overlaydiv").css('display', 'block');

            $.ajax({
                url: urlGetGlobalAvailableFilters,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { startDate: startDate, endDate: endDate },
                success: function (res) {
                    $("#ddlPaymentType").empty();
                    $("#ddlAcademicYear").empty();
                    $("#ddlAcademicTerm").empty();
                    $("#ddlAcademicSession").empty();

                    if (res.PaymentTypes) {
                        $("#ddlPaymentType").append($('<option></option>').val('').html(lblSelect));
                        $("#ddlAcademicYear").append($('<option></option>').val('').html(lblSelect));
                        $("#ddlAcademicTerm").append($('<option></option>').val('').html(lblSelect));
                        $("#ddlAcademicSession").append($('<option></option>').val('').html(lblSelect));

                        if (res.PaymentTypes.length > 0) {
                            $.each(res.PaymentTypes, function (i, paymentType) {
                                $("#ddlPaymentType").append($('<option></option>').val(paymentType.Id).html(paymentType.Description));
                            });
                        }

                        if (res.AcademicPeriods.length > 0) {
                            var periodYearsAdded = [];
                            $.each(res.AcademicPeriods, function (i, academicPeriod) {
                                if (!periodYearsAdded.find(py => py.Year === academicPeriod.Year)) {
                                    periodYearsAdded.push(academicPeriod);
                                    $("#ddlAcademicYear").append($('<option></option>').val(academicPeriod.Year).html(academicPeriod.Year));
                                }
                            });

                            var periodTermsAdded = [];
                            $.each(res.AcademicPeriods, function (i, academicPeriod) {
                                if (academicPeriod.Term && !periodTermsAdded.find(pt => pt.Term === academicPeriod.Term)) {
                                    periodTermsAdded.push(academicPeriod);
                                }
                            });
                            periodTermsAdded.sort((a, b) => Number(a.TermSortOrder) - Number(b.TermSortOrder));

                            var periodSessionsAdded = [];
                            $.each(res.AcademicPeriods, function (i, academicPeriod) {
                                if (academicPeriod.Session && !periodSessionsAdded.find(ps => ps.Session === academicPeriod.Session)) {
                                    periodSessionsAdded.push(academicPeriod);
                                }
                            });
                            periodSessionsAdded.sort((a, b) => Number(a.SessionSortOrder) - Number(b.SessionSortOrder));

                            $.each(periodTermsAdded, function (i, academicPeriod) {
                                $("#ddlAcademicTerm").append($('<option></option>').val(academicPeriod.Term).html(academicPeriod.TermDesc));
                            });

                            $.each(periodSessionsAdded, function (i, academicPeriod) {
                                $("#ddlAcademicSession").append($('<option></option>').val(academicPeriod.Session).html(academicPeriod.SessionDesc));
                            });
                        }
                    }

                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                }
            });
        }
    }

    function isDateRangeValid() {
        var startDate = $("#StartDate").val();
        var endDate = $("#EndDate").val();

        if (startDate && endDate) {
            var startMomentDate = moment(startDate, dateFormat);
            var endMomentDate = moment(endDate, dateFormat);

            return endMomentDate.isSameOrAfter(startMomentDate);
        }
        return false;
    }

    $("#btnClear").click(function () {
        $('#StartDate').val('');
        $('#StartDate').data("DateTimePicker").clear();

        $('#EndDate').val('');
        $('#EndDate').data("DateTimePicker").clear();

        $('#noFiscalRecordsDiv').hide();

        $('#StartDateRequired').hide();
        $('#StartDateError').hide();
        $('#StartDateIcon').hide();
        $("#StartDateGroup").removeClass('esg-has-error');
        $('#EndDateRequired').hide();
        $('#EndDateIcon').hide();
        $("#EndDateGroup").removeClass('esg-has-error');

        $("#ddlPaymentType").empty();
        $("#ddlAcademicYear").empty();
        $("#ddlAcademicTerm").empty();
        $("#ddlAcademicSession").empty();

        $("#ddlPaymentType").append($('<option></option>').val('').html(lblSelect));
        $("#ddlAcademicYear").append($('<option></option>').val('').html(lblSelect));
        $("#ddlAcademicTerm").append($('<option></option>').val('').html(lblSelect));
        $("#ddlAcademicSession").append($('<option></option>').val('').html(lblSelect));

        $('#records_table').empty();
        $('#PaymentType').html(paymentTypesBackup);
    });

    var IssuerTaxPayerId = $("#IssTaxPayerId");
    var IssuerExpAdd = $('#IssIssuingAddress');
    var IssPlaceIssue = $('#IssPlaceIssue');
    var IssTaxRegimenC = $('#IssTaxRegimen');
    var IssuerTaxRegimenCode = '';
    var IssuerTaxRegimenDesc = '';

    var IssTaxPayers = [];
    var IssIssuingAddress;

    var IdIssTaxPayer = 0;

    //ISSUER DEFAULT-----------------------------------
    if (IsSubstitution) {
        var IssTaxPayerIdValue = $("#IssTaxPayerId").val();
        $.ajax({
            url: urlGetIssuers,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: IssTaxPayerIdValue },
            success: function (data) {
                IssTaxPayers = data;
                onIssTaxPayerIdFocusOut();
            }
        });
    }
    else {
        LoadIssuerSetUp();
    }

    $('#Year option:eq(1)').prop('selected', true);

    function LoadIssuerSetUp() {
        $.ajax({
            url: urlSelectIssuerSetUp,
            type: "GET",
            cache: false,
            success: function (data) {

                if (data.IssInvoiceOrganizationId !== 0) {
                    //Existe registro en la tabla se muestra boton de Actualizar
                    $("#IssSerial").empty();
                    $("#IssIssuingAddress").empty();
                    $("#IssTaxPayerId").val(data.IssInvoiceTaxpayerId);
                    $("#IssIssuingAddress").append($('<option></option>').val(data.IssInvoiceExpeditionId).html(data.IssIssuingAddress));
                    $("#IssSerial").append($('<option></option>').val(data.IssSerialNumber).html(data.IssSerialNumber));
                    $("#IssNameCorpName").val(data.IssCorporateName);

                    var InvoiceOrganizationId = data.IssInvoiceOrganizationId;
                    var InvoiceExpId = data.IssInvoiceExpeditionId;
                    var SerialNum = $("#IssSerial").val().trim();

                    $.ajax({
                        url: urlGetTaxRegimen,
                        type: "GET",
                        cache: false,
                        dataType: "json",
                        data: { id: InvoiceOrganizationId },
                        success: function (response) {
                            if (response.IssTaxRegimen != null) {

                                var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element; });
                                IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                                IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                                $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                                $("#IssTaxRegimenIcon").css('display', 'none');
                                $("#IssTaxRegimenGroup").removeClass('esg-has-error');
                                IssTaxRegimenC.removeClass('has-error');
                                $("#IssTaxPayerDivLookup").show();
                            }
                            else {
                                $("#IssTaxRegimenIcon").css('display', 'block');
                                $("#IssTaxRegimenGroup").addClass('esg-has-error');
                                IssTaxRegimenC.removeClass('has-success').addClass('has-error');
                                $("#IssTaxPayerDivLookup").hide();
                                $("#IssTaxRegimen").val('');
                            }

                            if (response.IssIssuingAdd != null) {

                                IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element; });

                                $.each(IssIssuingAddress, function (i, val) {

                                    if (InvoiceExpId != val.IssInoviceExpeditionId) {
                                        $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                    }
                                });

                                var IssuingAddress = $.grep(response.IssIssuingAdd, function (elem) { return elem.IssInoviceExpeditionId == InvoiceExpId; });

                                if (IssuingAddress[0].IssPlaceIssue != null) {
                                    $("#IssPlaceIssue").val(IssuingAddress[0].IssPlaceIssue);
                                    $("#IssPlaceIssueIcon").css('display', 'none');
                                    $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-error');

                                    $("#RecTaxAddressGroup").parent().removeClass('esg-has-error');
                                    $("#RecTaxAddressIcon").css('display', 'none');
                                }
                                else {
                                    $("#IssPlaceIssue").val("");
                                    $("#IssPlaceIssueIcon").css('display', 'block');
                                    $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                                    IssPlaceIssue.parent().removeClass('has-success');

                                    $("#RecTaxAddressIcon").css('display', 'block');
                                    $("#RecTaxAddressGroup").parent().addClass('esg-has-error');
                                }

                                if (IssIssuingAddress[0].IssByExpedition === 0) {
                                    InvoiceExpId = null;
                                }
                                //GET SerialNumber
                                $.ajax({
                                    url: urlGetSerialNumber,
                                    type: "GET",
                                    cache: false,
                                    dataType: "json",
                                    data: { id: InvoiceOrganizationId, expId: InvoiceExpId },
                                    success: function (response) {

                                        $("#IssSerial").empty();
                                        $("#lblWarnSerial").css('display', 'none');

                                        if (response.IssSerial != null) {
                                            var IssSerial = $.grep(response.IssSerial, function (element) { return element; });

                                            var SerialPrefer = $.grep(IssSerial, function (e) { return e.IssSerialNumber == SerialNum; });

                                            if (SerialPrefer[0].IssLastFolio == '2147483647') {

                                                $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 1));

                                            }
                                            else {
                                                $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 0));

                                            }

                                            $.each(IssSerial, function (i, val) {
                                                if ($("#IssSerial").val() != val.IssInvoiceReceipt) {
                                                    if (val.IssLastFolio == '2147483647') {
                                                        $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                                                    }
                                                    else {
                                                        $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                                                    }
                                                }
                                            });

                                            var selected = $("#IssSerial").find('option:selected');
                                            var optionVal = selected.attr('id');

                                            if (optionVal == "1") {
                                                $("#lblWarnSerial").css('display', 'block');
                                            }
                                            else {
                                                $("#lblWarnSerial").css('display', 'none');
                                            }

                                        }
                                        else {

                                            $("#IssSerial").empty();
                                        }
                                    }
                                });
                            }
                        }
                    });
                }

            }
        });
    }

    //LIST GLOBAL FISCAL RECORDS-----------------------
    $("#SearchGlobal").click(function () {
        var sDate = $("#StartDate").val();
        var eDate = $("#EndDate").val();

        var paymentType = $("#ddlPaymentType").val();
        var academicYear = $("#ddlAcademicYear").val();
        var academicTerm = $("#ddlAcademicTerm").val();
        var academicSession = $("#ddlAcademicSession").val();
        var invAddss = $("#IssIssuingAddress").val();

        $('#noFiscalRecordsDiv').hide();
        $('#records_table').empty();

        if (sDate && eDate) {
            if (isDateRangeValid()) {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                $('#StartDateRequired').hide();
                $('#StartDateError').hide();
                $('#StartDateIcon').hide();
                $("#StartDateGroup").removeClass('esg-has-error');
                $('#EndDateRequired').hide();
                $('#EndDateIcon').hide();
                $("#EndDateGroup").removeClass('esg-has-error');

                //Get list of all cash receipts
                $.ajax({
                    url: urlGetGlobalCashReceipts,
                    type: "POST",
                    cache: false,
                    dataType: "html",
                    data: {
                        startDate: sDate,
                        endDate: eDate,
                        paymentTypeId: Number(paymentType),
                        academicYear: academicYear,
                        academicTerm: academicTerm,
                        academicSession: academicSession,
                        invExpeditionId: invAddss
                    },
                    success: function (data) {
                        $('#records_table').html(data);
                        $('#noFiscalRecordsDiv').show();
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        ShowHideRows("#divCharges", 1);
                        $('#filtersPanel').click();
                    }
                });

                $.ajax({
                    url: urlGetGlobalPaymentType,
                    type: "POST",
                    cache: false,
                    dataType: "json",
                    data: {
                        startDate: sDate,
                        endDate: eDate,
                        paymentTypeId: Number(paymentType),
                        academicYear: academicYear,
                        academicTerm: academicTerm,
                        academicSession: academicSession,
                        invExpeditionId: invAddss
                    },
                    success: function (res) {
                        if (res.length > 0) {
                            $("#PaymentType").empty();
                            $("#PaymentTypeError").hide();
                            var mappingExists = 0;
                            var selectedValue = 0;
                            $.each(res, function (i, val) {
                                if (val.Status == "1") {
                                    mappingExists = 1;
                                    if (selectedValue == 0) {
                                        $("#PaymentType").append($('<option selected style="font-weight: bold"> </option>').val(val.Code).html(val.Description));
                                        selectedValue = 1;
                                    }
                                    else {
                                        $("#PaymentType").append($('<option style="font-weight: bold"> </option>').val(val.Code).html(val.Description));
                                    }
                                }
                                else {
                                    $("#PaymentType").append($('<option></option>').val(val.Code).html(val.Description));
                                }
                                $("#PaymentTypeError").hide();
                                $("#NotePaymentType").show();
                            });
                            if (mappingExists == 0) {
                                if ($('#noFiscalRecordsDiv').is(":visible")) {
                                    $("#PaymentTypeError").hide();
                                    $("#NotePaymentType").show();
                                }
                                else {
                                    $("#PaymentTypeError").show();
                                    $("#NotePaymentType").hide();
                                }
                            }
                        }
                    }
                });
            }
            else {
                $('#StartDateError').show()
                $('#StartDateIcon').show();
                $("#StartDateGroup").addClass('esg-has-error');
            }
        }
        else {
            if (sDate == "") {
                $('#StartDateRequired').show();
                $('#StartDateError').hide();
                $('#StartDateIcon').show();
                $("#StartDateGroup").addClass('esg-has-error');
            }
            else {
                $('#StartDateRequired').hide();

                if (isDateRangeValid()) {
                    $('#StartDateIcon').hide();
                    $("#StartDateGroup").removeClass('esg-has-error');
                }
            }

            if (eDate == "") {
                $('#EndDateRequired').show();
                $('#EndDateIcon').show();
                $("#EndDateGroup").addClass('esg-has-error');
            }
            else {
                $('#EndDateRequired').hide();
                $('#EndDateIcon').hide();
                $("#EndDateGroup").removeClass('esg-has-error');
            }
            $('.successMessageDiv').hide();
        }
    });

    $("#PreferredEmail").change(function () {
        var Email = $("#PreferredEmail");
        if (!Email.val()) {
            Email.parent().addClass('has-error');
            $("#EmailGroup").addClass('esg-has-error');
            $("#EmailIcon").css('display', 'block');
        }
        else {

            $("#EmailGroup").removeClass('esg-has-error');
            $("#EmailIcon").css('display', 'none');
            Email.parent().removeClass('has-error');
        }
    });

    //ISSUER-------------------------------------------

    $("#IssTaxPayerId").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: urlGetIssuers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {
                    IssTaxPayers = data;
                    response($.map(data, function (item) {
                        return { value: item.IssTaxpayerId, id: item.IssTaxpayerId };
                    }));
                }
            });
        }
    });

    $("#IssTaxPayerId").focusout(onIssTaxPayerIdFocusOut);

    function onIssTaxPayerIdFocusOut() {
        IdIssTaxPayer = $("#IssTaxPayerId").val();

        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];

        if (!response == "") {

            $("#IssNameCorpName").val("");
            $("#IssTaxRegimen").empty();
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").empty();
            $("#IssPlaceIssue").val("");

            IssuerTaxPayerId.parent().removeClass('has-error');
            $('#IssTaxPayerIcon').css('display', 'none');
            $('#IssTaxPayerGroup').parent().removeClass('esg-has-error');
            $("#IssTaxPayerDivLookup").show();
            $("#IssNameCorpName").val(response.IssCorporateName);
            $("#IssIssuingAddress").parent().removeClass('esg-has-error');

            //GET TAXREGIMEN
            $.ajax({
                url: urlGetTaxRegimen,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: response.IssInvoiceOrganizationId },
                success: function (response) {

                    if (response.IssTaxRegimen != null) {

                        var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element; });
                        IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                        IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                        $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                        $("#IssTaxRegimenIcon").css('display', 'none');
                        $("#IssTaxRegimenGroup").parent().removeClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-error');
                    }
                    else {
                        $("#IssTaxRegimenIcon").css('display', 'block');
                        $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                        $("#IssTaxPayerDivLookup").hide();
                        $("#IssTaxRegimen").val('');
                    }

                    if (response.IssIssuingAdd != null) {

                        var InoviceExpeditionId;

                        IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element; });

                        $.each(IssIssuingAddress, function (i, val) {
                            $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                        });

                        var issPlaceIssueFound;
                        if (SubsInvoiceExpeditionId != null && SubsInvoiceExpeditionId > 0) {
                            issPlaceIssueFound = IssIssuingAddress.find(a => a.IssInoviceExpeditionId == SubsInvoiceExpeditionId);
                            if (issPlaceIssueFound == null) {
                                issPlaceIssueFound = IssIssuingAddress[0];
                            }
                        }
                        else {
                            issPlaceIssueFound = IssIssuingAddress[0];
                        }
                        SubsInvoiceExpeditionId = null;

                        if (issPlaceIssueFound.IssByExpedition === 0) {
                            InoviceExpeditionId = null;
                        }
                        else {
                            InoviceExpeditionId = issPlaceIssueFound.IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound.IssPlaceIssue != null) {
                            $("#IssIssuingAddress").val(issPlaceIssueFound.IssInoviceExpeditionId);
                            $("#IssPlaceIssue").val(issPlaceIssueFound.IssPlaceIssue);
                            $("#IssPlaceIssueIcon").css('display', 'none');
                            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-error');
                        }
                        else {
                            $("#IssPlaceIssue").val("");
                            $("#IssPlaceIssueIcon").css('display', 'block');
                            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                            IssPlaceIssue.parent().removeClass('has-success');
                        }

                        //GET SerialNumber
                        $.ajax({
                            url: urlGetSerialNumber,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                            success: function (response) {

                                $("#lblWarnSerial").css('display', 'none');

                                if (response.IssSerial != null) {
                                    var IssSerial = $.grep(response.IssSerial, function (element) { return element; });

                                    $.each(IssSerial, function (i, val) {

                                        if (val.IssLastFolio == '2147483647') {
                                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                                        }
                                        else {
                                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                                        }

                                    });
                                    if (SusbsSerialNumber != null && IssSerial.length > 0 && IssSerial.find(s => s.IssSerialNumber == SusbsSerialNumber)) {
                                        $("#IssSerial").val(SusbsSerialNumber);
                                    }
                                }
                                else {

                                    $("#IssSerial").empty();
                                }
                                SusbsSerialNumber = null;

                                var selected = $("#IssSerial").find('option:selected');
                                var optionVal = selected.attr('id');

                                if (optionVal == "1") {
                                    $("#lblWarnSerial").css('display', 'block');
                                }
                                else {
                                    $("#lblWarnSerial").css('display', 'none');
                                }

                            }
                        });

                        $("#IssIssuingAddressIcon").css('display', 'none');
                        $("#IssIssuingAddressGroup").parent().removeClass('esg-has-error');
                        IssuerExpAdd.parent().removeClass('has-error');

                    }
                    else {
                        $("#IssIssuingAddress").empty();
                        $("#IssIssuingAddressIcon").css('display', 'block');
                        $("#IssIssuingAddressGroup").parent().addClass('esg-has-error');
                        $("#IssIssuingAddress").parent().addClass('esg-has-error');
                        IssuerExpAdd.parent().addClass('has-error');
                        $("#IssPlaceIssue").val("");
                        $("#IssPlaceIssueIcon").css('display', 'block');
                        $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                        IssPlaceIssue.parent().addClass('has-error');
                    }
                }
            });

        }
        else {

            if (IdIssTaxPayer == "") {
                IssuerTaxPayerId.parent().addClass('has-error');
                $('#IssTaxPayerIcon').css('display', 'block');
                $('#IssTaxPayerGroup').parent().addClass('esg-has-error');
                $("#IssTaxRegimenIcon").css('display', 'block');
                $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
                IssTaxRegimenC.parent().addClass('has-error');
                $("#IssPlaceIssueIcon").css('display', 'block');
                $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
                IssPlaceIssue.parent().addClass('has-error');
                $("#IssIssuingAddressIcon").css('display', 'block');
                $("#IssIssuingAddressGroup").parent().addClass('esg-has-error');
                IssuerExpAdd.parent().addClass('has-error');
                $("#IssTaxRegimen").val("");
                $("#IssNameCorpName").val("");
                $("#IssSerial").empty();
                $("#IssIssuingAddress").val("");
                $("#IssPlaceIssue").val("");
                $("#IssTaxPayerDivLookup").hide();
            }
        }

    }

    $('#IssIssuingAddress').change(function () {

        var InoviceExpeditionId = 0;
        var val = $('#IssIssuingAddress option:selected').text();
        var PlaceIssue = $.grep(IssIssuingAddress, function (element) { return element.IssIssuingAddress == val; });
        var IssByExpedition = PlaceIssue[0].IssByExpedition;

        if (IssByExpedition === 0) {
            InoviceExpeditionId = null;
        }
        else {
            InoviceExpeditionId = PlaceIssue[0].IssInoviceExpeditionId;
        }

        //GET SerialNumber
        $.ajax({
            url: urlGetSerialNumber,
            type: "GET",
            cache: false,
            dataType: "json",
            data: { id: PlaceIssue[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
            success: function (response) {

                if (response.IssSerial != null) {
                    $("#IssSerial").empty();
                    $("#lblWarnSerial").css('display', 'none');

                    var IssSerial = $.grep(response.IssSerial, function (element) { return element; });

                    $.each(IssSerial, function (i, val) {

                        if (val.IssLastFolio == '2147483647') {
                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 1));

                        }
                        else {
                            $("#IssSerial").append($('<option></option>').val(val.IssSerialNumber).html(val.IssSerialNumber).attr("id", 0));
                        }

                    });

                    var selected = $("#IssSerial").find('option:selected');
                    var optionVal = selected.attr('id');

                    if (optionVal == "1") {
                        $("#lblWarnSerial").css('display', 'block');
                    }
                    else {
                        $("#lblWarnSerial").css('display', 'none');
                    }

                }
                else {

                    $("#IssSerial").empty();
                }
            }
        });

        $("#IssPlaceIssue").val(PlaceIssue[0].IssPlaceIssue);

    });

    $('#IssPlaceIssue').focusout(function () {

        if (IssPlaceIssue.val() != "") {

            $("#IssPlaceIssueIcon").css('display', 'none');
            $("#IssPlaceIssueGroup").parent().removeClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-error');
        }
        else {

            $("#IssPlaceIssueIcon").css('display', 'block');
            $("#IssPlaceIssueGroup").parent().addClass('esg-has-error');
            IssPlaceIssue.parent().removeClass('has-success').addClass('has-error');

        }

    });

    $("#IssSerial").change(function () {

        var selected = $(this).find('option:selected');
        var optionVal = selected.attr('id');

        if (optionVal == "0") {
            $("#lblWarnSerial").css('display', 'none');
        }
        else {
            $("#lblWarnSerial").css('display', 'block');
        }

    });

    //PAYMENT----------------------------------------

    $("#PaymentCondition").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }
    });

    $("#IssComments").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }
    });

    //CHARGE

    $(".ChargeDesc").keypress(function (e) {

        if (e.keyCode == 124 || e.key == "|") {
            return false;
        }

    });

    // FISCAL RECORDS
    $("#Frequency").change(function () {
        var frequencyValue = $("#Frequency option:selected").val();
        if (frequencyValue) {
            $("#Frequency").removeClass('esg-has-error');
            $("#FrequencyGroup").removeClass('esg-has-error');
            $("#FrequencyIcon").css('display', 'none');
            $("#FrequencyRequired").hide();
        }
    });

    $("#Month").change(function () {
        var monthValue = $("#Month option:selected").val();
        if (monthValue) {
            $("#Month").removeClass('esg-has-error');
            $("#MonthGroup").removeClass('esg-has-error');
            $("#MonthIcon").css('display', 'none');
            $("#MonthRequired").hide();

        }
    });

    $("#Year").change(function () {
        var monthValue = $("#Year option:selected").val();
        if (monthValue) {
            $("#Year").removeClass('esg-has-error');
            $("#YearGroup").removeClass('esg-has-error');
            $("#YearIcon").css('display', 'none');
            $("#YearRequired").hide();
        }
    });

    //CREATE-----------------------------------------

    var createForm = $('#formCreate');

    $('body').on('click', '#btnProcessC', function () {

        var SerialError = $("#lblWarnSerial").css('display');

        if (moment(ExpeditionDate) > moment(validateDate)) {
            // console.log($("#IssueAlert").is(":visible"));
            if (!$("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'block');
            }
        }
        else {
            // console.log($("#IssueAlert").is(":visible"));
            if ($("#IssueAlert").is(":visible")) {
                $("#IssueAlert").css('display', 'none');
            }
            if (SerialError == 'none') {
                $('.errorMessageSerialError').css('display', 'none');

                var PaymentTypeSelected = $("#PaymentType option:selected").text();
                var PaymentType = PaymentTypeSelected.split('-');
                var PaymentTypeCode = PaymentType[0].trim();
                var PaymentTypeDesc = PaymentType[1].trim();
                var PaymentMethodSelected = $("#PaymentMethod").val();
                var PaymentMethod = PaymentMethodSelected.split('-');
                var PaymentMethodCode = PaymentMethod[0].trim();
                var PaymentMethodDesc = PaymentMethod[1].trim();

                var FrequencySelected = $("#Frequency option:selected").text();
                var FrequencyCode = $("#Frequency option:selected").val();
                var FrequencyDesc = FrequencySelected;

                var MonthSelected = $("#Month option:selected").text();
                var MonthCode = $("#Month option:selected").val();
                var MonthDesc = MonthSelected;

                var YearCode = $("#Year option:selected").val();

                var SerialNumber = $("#IssSerial option:selected").text();

                $('.errorMessageDiv').hide();
                $('.errorMessageRequiredDiv').hide();

                createForm.validate();

                if (createForm.valid() && formIsValid()) {

                    /*Read Details of Fiscal Record*/
                    var $table = $("#records_table tbody");
                    var cols = [];
                    $table.find('tr').each(function (rowIndex, r) {
                        var frd = new Object();
                        var frDetails = [];
                        $(this).find(":input[type=hidden][name='CashRe.ChargeCreditCode']").each(function (index, v) {
                            frDetails.push(v.value);
                        });
                        jQuery.each(frDetails, function (i, val) {
                            var rowId = val;
                            $table.find('tr').find('td').each(function (colIndex, c) {
                                if (colIndex === 0) {
                                    frd.chargeCreditCode = $("#ccCode_" + rowId).val();
                                    frd.ProductServiceCode = $("#ccPrdSrv_" + rowId).text();
                                }
                            });
                            cols.push(frd);
                        });
                    });

                    var receiptNumbersIncludedForGlobal = [];
                    var rows = $('input[name=chkIncludeReceipt]:checked');
                    rows.each(function () {
                        var receiptNumber = $(this).val();
                        receiptNumbersIncludedForGlobal.push(receiptNumber);
                    });

                    $("#btnProcessC").css('display', 'none');
                    $("#Processing").css('display', 'block');
                    $("#Overlaydiv").css('display', 'block');

                    var ParamInsInvoiceHeaderList = {
                        CancelReasonKey: "",
                        CancelReasonName: CancelReasonName,
                        CFDIRelated: CFDIRelated,
                        CFDIRelatedId: CFDIRelatedId,
                        CFDIRelationTypeKey: "",
                        CFDIRelationTypeName: null,
                        CFDIUsageCode: 'S01',
                        CFDIUsageDesc: 'Sin efectos fiscales',
                        CityOfIssue: $("#IssPlaceIssue").val(),
                        Comments: $("#IssComments").val(),
                        Currency: $("#IssCurrency").val(),
                        Detail: cols,
                        EndDate: $("#EndDate").val(),
                        ExchangeRate: 0.00,
                        FiscalIdentityNumber: '',
                        FiscalResidency: '',
                        FiscalResidencyDesc: '',
                        FrequencyCode: FrequencyCode,
                        FrequencyDesc: FrequencyDesc,
                        InvoiceExpeditionId: $("#IssIssuingAddress").val(),
                        IssuerTaxPayerId: $("#IssTaxPayerId").val(),
                        MonthCode: MonthCode,
                        MonthDesc: MonthDesc,
                        PaymentAccountNumber: "",
                        PaymentCondition: '',
                        PaymentMethod: PaymentMethodCode,
                        PaymentMethodDesc: PaymentMethodDesc,
                        PaymentType: PaymentTypeCode,
                        PaymentTypeDesc: PaymentTypeDesc,
                        PeopleOrgCodeId: '',
                        ReceiptNumber: 0,
                        ReceiptNumbersIncludedForGlobal: receiptNumbersIncludedForGlobal,
                        ReceiverEmail: $("#PreferredEmail").val(),
                        ReceiverTaxpayerId: $("#RecTaxPayerId").val(),
                        RecTaxRegimen: '616',
                        RecTaxRegimenDesc: 'Sin obligaciones fiscales',
                        SerialNumber: SerialNumber,
                        StartDate: $("#StartDate").val(),
                        Subtotal: $("#SubTotal").val(),
                        TaxRegimen: IssuerTaxRegimenCode,
                        TaxRegimenDesc: IssuerTaxRegimenDesc,
                        Total: $("#Total").val(),
                        TotalTransferTaxes: $("#TotalTT").val(),
                        Year: YearCode,
                        // ACOLLI 08112021 EMISIONFACT
                        ExpeditionDate: ExpeditionDate
                        //END
                    };

                    $.ajax({
                        url: urlProcessGlobal,
                        dataType: "json",
                        cache: false,
                        type: "POST",
                        data: ParamInsInvoiceHeaderList,
                        success: function (response) {
                            if (response.id <= 0) {
                                $("#Processing").css('display', 'none');
                                $("#Overlaydiv").css('display', 'none');
                                $("#btnProcessC").css('display', 'block');
                                $('.errorMessageDiv').show();
                                $(".errorMessageResult").html(response.message);
                            }
                            else {
                                $("#Processing").css('display', 'none');
                                $("#Overlaydiv").css('display', 'none');
                                $('.successMessageDiv').show();
                                $('.errorMessageDiv').hide();
                                $(".successMessageResult").html(response.message);
                                window.location.href = urlViewAll;
                            }
                        }
                    });

                }
                else {
                    $("#Processing").css('display', 'none');
                    $("#Overlaydiv").css('display', 'none');
                    $('.errorMessageRequiredDiv').show();
                    $('.successMessageDiv').hide();
                    $("#btnProcessC").css('display', 'block');

                    // UI validations for frequency

                    var Email = $("#PreferredEmail");
                    if (!Email.val()) {
                        Email.parent().addClass('has-error');
                        $("#EmailGroup").addClass('esg-has-error');
                        $("#EmailIcon").css('display', 'block');
                    }
                    else {
                        $("#EmailGroup").removeClass('esg-has-error');
                        $("#EmailIcon").css('display', 'none');
                        Email.parent().removeClass('has-error');
                    }

                    // UI validations for frequency
                    if (FrequencyCode) {
                        $("#Frequency").removeClass('has-error');
                        $("#FrequencyIcon").css('display', 'none');
                        $("#FrequencyRequired").hide();
                        $("#FrequencyGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#Frequency").val("");
                        $("#FrequencyIcon").css('display', 'block');
                        $("#FrequencyRequired").show();
                        $("#FrequencyGroup").addClass('esg-has-error');
                        $("#Frequency").removeClass('has-success');
                    }

                    // UI validations for month
                    if (MonthCode) {
                        $("#Month").removeClass('has-error');
                        $("#MonthIcon").css('display', 'none');
                        $("#MonthRequired").hide();
                        $("#MonthGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#Month").val("");
                        $("#MonthIcon").css('display', 'block');
                        $("#MonthRequired").show();
                        $("#MonthGroup").addClass('esg-has-error');
                        $("#Month").removeClass('has-success');
                    }

                    // UI validations for year
                    if (YearCode) {
                        $("#Year").removeClass('has-error');
                        $("#YearIcon").css('display', 'none');
                        $("#YearRequired").hide();
                        $("#YearGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#Year").val("");
                        $("#YearIcon").css('display', 'block');
                        $("#YearRequired").show();
                        $("#YearGroup").addClass('esg-has-error');
                        $("#Year").removeClass('has-success');
                    }

                    // UI validations for RecTaxAddress
                    var RecTaxAddress = $("#RecTaxAddress").val();
                    var IssPlaceOfIssue = $("#IssPlaceIssue").val();

                    if (RecTaxAddress == IssPlaceOfIssue) {
                        $("#RecTaxAddress").removeClass('has-error');
                        $("#RecTaxAddressIcon").css('display', 'none');
                        $("#RecTaxAddressIncorrect").hide();
                        $("#RecTaxAddressGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#RecTaxAddressIcon").css('display', 'block');
                        $("#RecTaxAddressIncorrect").show();
                        $("#RecTaxAddressGroup").addClass('esg-has-error');
                        $("#RecTaxAddress").removeClass('has-success');
                    }

                    // UI validations for RecTaxRegimen
                    var RecTaxRegimen = $("#RecTaxRegimen").val();

                    if (RecTaxRegimen == '616 - Sin obligaciones fiscales') {
                        $("#RecTaxRegimen").removeClass('has-error');
                        $("#RecTaxRegimenIcon").css('display', 'none');
                        $("#RecTaxRegimenIncorrect").hide();
                        $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#RecTaxRegimenIcon").css('display', 'block');
                        $("#RecTaxRegimenIncorrect").show();
                        $("#RecTaxRegimenGroup").addClass('esg-has-error');
                        $("#RecTaxRegimen").removeClass('has-success');
                    }

                    // UI validations for RecCfdiUsage
                    var RecCfdiUsage = $("#RecCfdiUsage").val();

                    if (RecCfdiUsage == 'S01 - Sin efectos fiscales') {
                        $("#RecCfdiUsage").removeClass('has-error');
                        $("#RecCfdiUsageIcon").css('display', 'none');
                        $("#RecCfdiUsageIncorrect").hide();
                        $("#RecCfdiUsageGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#RecCfdiUsageIcon").css('display', 'block');
                        $("#RecCfdiUsageIncorrect").show();
                        $("#RecCfdiUsageGroup").addClass('esg-has-error');
                        $("#RecCfdiUsage").removeClass('has-success');
                    }

                    // UI validations for RecName
                    var RecName = $("#RecName").val();

                    if (RecName == 'PUBLICO EN GENERAL' || RecName == 'PÚBLICO EN GENERAL') {
                        $("#RecName").removeClass('has-error');
                        $("#RecNameIcon").css('display', 'none');
                        $("#RecNameIncorrect").hide();
                        $("#RecNameGroup").removeClass('esg-has-error');
                    }
                    else {
                        $("#RecNameIcon").css('display', 'block');
                        $("#RecNameIncorrect").show();
                        $("#RecNameGroup").addClass('esg-has-error');
                        $("#RecName").removeClass('has-success');
                    }
                }
            }
            else {

                $('.errorMessageSerialError').css('display', 'block');
                $('.errorMessageRequiredDiv').hide();
                $('.errorMessageDiv').hide();
            }
        }
    });

    function formIsValid() {
        var PaymentTypeCode = $("#PaymentType option:selected").val();
        var FrequencyCode = $("#Frequency option:selected").val();
        var MonthCode = $("#Month option:selected").val();
        var SerialNumber = $("#IssSerial option:selected").text();

        var isValid = PaymentTypeCode && FrequencyCode && MonthCode && SerialNumber;

        var RecTaxAddress = $("#RecTaxAddress").val();
        var RecTaxRegimen = $("#RecTaxRegimen").val();
        var RecCfdiUsage = $("#RecCfdiUsage").val();
        var RecName = $("#RecName").val();

        var IssPlaceOfIssue = $("#IssPlaceIssue").val();

        isValid = isValid
            && RecTaxAddress == IssPlaceOfIssue
            && RecTaxRegimen == '616 - Sin obligaciones fiscales'
            && RecCfdiUsage == 'S01 - Sin efectos fiscales'
            && (RecName == 'PUBLICO EN GENERAL' || RecName == 'PÚBLICO EN GENERAL');

        return isValid;
    }
});

function calculateTotals() {
    var sDate = $("#StartDate").val();
    var eDate = $("#EndDate").val();

    var paymentType = $("#ddlPaymentType").val();
    var academicYear = $("#ddlAcademicYear").val();
    var academicTerm = $("#ddlAcademicTerm").val();
    var academicSession = $("#ddlAcademicSession").val();
    var invAddss = $("#IssIssuingAddress").val();

    if (sDate != "" && eDate != "") {

        var receiptNumbersIncludedForGlobal = [];
        var rows = $('input[name=chkIncludeReceipt]:checked');
        rows.each(function () {
            var receiptNumber = $(this).val();
            receiptNumbersIncludedForGlobal.push(receiptNumber);
        });

        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');

        $('#StartDateRequired').hide();
        $('#StartDateIcon').hide();
        $("#StartDateGroup").removeClass('esg-has-error');
        $('#EndDateRequired').hide();
        $('#EndDateIcon').hide();
        $("#EndDateGroup").removeClass('esg-has-error');

        $.ajax({
            url: urlGetGlobalCashReceiptsTotals,
            type: "POST",
            cache: false,
            dataType: "json",
            data: {
                startDate: sDate,
                endDate: eDate,
                paymentTypeId: Number(paymentType),
                academicYear: academicYear,
                academicTerm: academicTerm,
                academicSession: academicSession,
                receiptNumbers: receiptNumbersIncludedForGlobal,
                invExpeditionId: invAddss
            },
            success: function (data) {
                $('#SubTotal').val(data.Subtotal);
                $('#TotalTT').val(data.TotalTaxes);
                $('#Total').val(data.Total);

                $("#Processing").css('display', 'none');
                $("#Overlaydiv").css('display', 'none');
                $('#totalsSection').show();
                $('#processSection').show();
                $('#calculateTotalsSection').hide();
            }
        });

        $.ajax({
            url: urlGetGlobalPaymentType,
            type: "POST",
            cache: false,
            dataType: "json",
            data: {
                startDate: sDate,
                endDate: eDate,
                paymentTypeId: Number(paymentType),
                academicYear: academicYear,
                academicTerm: academicTerm,
                academicSession: academicSession,
                receiptNumbers: receiptNumbersIncludedForGlobal,
                invExpeditionId: invAddss
            },
            success: function (res) {
                if (res.length > 0) {
                    $("#PaymentType").empty();
                    $("#PaymentTypeError").hide();
                    var mappingExists = 0;
                    var selectedValue = 0;
                    $.each(res, function (i, val) {
                        if (val.Status == "1") {
                            mappingExists = 1;
                            if (selectedValue == 0) {
                                $("#PaymentType").append($('<option selected style="font-weight: bold"> </option>').val(val.Code).html(val.Description));
                                selectedValue = 1;
                            }
                            else {
                                $("#PaymentType").append($('<option style="font-weight: bold"> </option>').val(val.Code).html(val.Description));
                            }
                        }
                        else {
                            $("#PaymentType").append($('<option></option>').val(val.Code).html(val.Description));
                        }
                        $("#PaymentTypeError").hide();
                        $("#NotePaymentType").show();
                    });
                    if (mappingExists == 0) {
                        if ($('#noFiscalRecordsDiv').is(":visible")) {
                            $("#PaymentTypeError").hide();
                            $("#NotePaymentType").show();
                        }
                        else {
                            $("#PaymentTypeError").show();
                            $("#NotePaymentType").hide();
                        }
                    }
                }
            }
        });
    }
}

function onCheckboxChange() {
    $('#totalsSection').hide();
    $('#processSection').hide();
    $('#calculateTotalsSection').show();

    var checkedCount = $('input[name=chkIncludeReceipt]:checked').length;

    if (checkedCount > 0) {
        $('#btnCalculateTotals').show();
    }
    else {
        $('#btnCalculateTotals').hide();
    }
}