$(document).ready(function () {

    var IssPlaceIssue = $('#IssPlaceIssue');
    var IssTaxRegimenC = $('#IssTaxRegimen');
    var countRows = 0;
    var fIdentityNumber = "";
    var fResidency = "";
    var fResidencyDesc = "";
    var TotalTax;
    var cols = [];
    var val = 0;
    var ColsAmounts = [];
    var createForm = $('#formCreate');
    var TaxPayers;
    var SubsInvoiceExpeditionId;

    LoadIssuerSetUp();

    $('#CFDI option[value="G02"]').prop('selected', true)

    $("input[type=text][name='RowAmount']").keypress(function (evt) {

        var self = $(this);
        //self.val(self.val().replace(/[^0-9\.]/g, ''));
        if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57) && evt.which != 36 && evt.which != 8 && evt.which != 0) {
            evt.preventDefault();
        }
    });

    $('input[type=text][name=UnitName]').keyup(function (e) {
        var inputElement = $(this);
        var idElement = inputElement.prop('id');
        if (idElement.startsWith('ccUnitDesc_')) {
            idElement = inputElement.attr('data-index');
        }
        if (inputElement.val().length > 20) {
            $(`#msgUnitMaxLength_${idElement}`).show();
            $(`#ccUnitDescGroup_${idElement}`).addClass('esg-has-error');
            $(`#ccUnitDescIcon_${idElement}`).show();
        }
        else {
            $(`#msgUnitMaxLength_${idElement}`).hide();
            $(`#ccUnitDescGroup_${idElement}`).removeClass('esg-has-error');
            $(`#ccUnitDescIcon_${idElement}`).hide();
        }
    });

    // #region OnChange
    $('#IssIssuingAddress').change(function () {

        var InoviceExpeditionId = 0;
        var val = $('#IssIssuingAddress option:selected').text();
        var PlaceIssue = $.grep(IssIssuingAddress, function (element) { return element.IssIssuingAddress == val });
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

                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

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

    $("input[type=text][name='RowAmount']").on('change', function (e) {
        var id = e.currentTarget.id;
        var MontoCreditNote = e.currentTarget.value.split("$");
        var Monto = $('td[id=' + id + '][name="UnitAmount"]').text().trim().split('$');
        $('input[name="RowTaxAmount"][id=' + id + ']').val('');
        $('input[name="RowTotalAmount"][id=' + id + ']').val('');
        var ValCero = "0.0000"
        var Cero = parseFloat(ValCero).toFixed(val);

        MontoTotal = Monto[1].replace(/,/g, "");

        if (MontoCreditNote[0] == "") {

            var MontoCredit = MontoCreditNote[1];
        }
        else {
            var MontoCredit = MontoCreditNote[0];

        }

        if (parseFloat(MontoCredit) <= parseFloat(MontoTotal) && parseFloat(MontoCredit) > Cero) {

            //Si el monto de la nota de credito es menor o igual al monto total se van a realizar los calculos correspondientes

            $('#RecTaxPayerIcon_' + id).css('display', 'none');
            $('input[name="RowAmount"][id=' + id + ']').parent().removeClass('has-error');
            $('#RecAmountGroup_' + id).removeClass('esg-has-error');

            CreditNoteRow = $(this).closest("tr").children("td");

            var ChargeCreditNumber = $("input[id='ccCode_" + id + "'][name='CashRe.ChargeCreditCodeId'][type=hidden]").val();
            var ReceiptNumber = $("input[id='ccReceiptNumber_" + id + "'][name='CashRe.ReceiptNumber'][type=hidden]").val();
            var x = $('td[id=' + id + '][name="CashRe.ChargeCreditCodeId"][type=hidden]').val();

            //Consumir la API para regresar los valores
            CalculateTaxes(MontoCredit, ChargeCreditNumber, id, ReceiptNumber);

            $('#btnCalculateTotals').css('display', 'block');
            $('#btnCalculateTotals').focus();
        }
        else {
            //En caso contrario se van a mostar la caja de texto con rojo de error y volver a ocultar el boton de calcular
            $('input[name="RowAmount"][id=' + id + ']').parent().addClass('has-error');
            $('#RecAmountGroup_' + id).addClass('esg-has-error');
            $('#RecTaxPayerIcon_' + id).css('display', 'block');
            $('label[id=' + id + '][for="RowTaxAmount"]').css('display', 'none');
            $('label[id=' + id + '][for="RowTotalAmount"]').css('display', 'none');
            $(this).val('$');
            $(this).focus();
            $('#btnCalculateTotals').css('display', 'none');
        }
    });

    $("input[type=text][name='CreditDesc']").on('change', function (e) {
        var id = e.currentTarget.id;

        if (e.currentTarget.value === "") {
            $('input[name="CreditDesc"][id=' + id + ']').parent().addClass('has-error');
            $('#DescriptionGroup_' + id).addClass('esg-has-error');
            $('#DescIcon_' + id).css('display', 'block');
        }
        else {

            $('input[name="CreditDesc"][id=' + id + ']').parent().removeClass('has-error');
            $('#DescriptionGroup_' + id).removeClass('esg-has-error');
            $('#DescIcon_' + id).css('display', 'none');
        }

    });

    $("#PreferredEmail").on('change', function () {

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
    // #endregion OnChange

    // #region Autocomplete
    $("#TaxPayerId").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetReceivers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { id: request.term },
                success: function (data) {

                    $("#TaxPayerId").parent().removeClass('has-error');
                    $("#receiverTaxPayerIdGroup").removeClass('esg-has-error');
                    $("#receiverTaxPayerIcon").hide();
                    $("#receiverTaxPayerDivLookup").hide();

                    TaxPayers = data;
                    response($.map(data, function (item) {
                        if (item.TaxPayerId === 'XEXX010101000')
                            return { label: item.TaxPayerId + '-' + item.FiscalIdentityNumber, value: item.TaxPayerId + '-' + item.FiscalIdentityNumber }
                        else
                            return { label: item.TaxPayerId, value: item.TaxPayerId }
                    }))

                }
            })
        }
    });

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
                        return { value: item.IssTaxpayerId, id: item.IssTaxpayerId }
                    }))

                }
            })
        }
    });
    // #endregion Autocomplete

    // #region Click
    $(".AddCreditNote").click(function (e) {
        if (e.target.type == "checkbox") {

            // stop the bubbling to prevent firing the row's click event
            e.stopPropagation();
        }
        else {
            $(".errorMessageRequiredDiv").hide();
            $(".errorMessageDiv").hide();

            var $checkbox = $(this).find(':checkbox');
            $checkbox.attr('checked', !$checkbox.attr('checked'));

            var trId = $checkbox.attr('id');
            $("#btnProcessCreditNote").css('display', 'none');

            if ($checkbox.attr('checked')) {

                var row = $(this).closest('tr').html();
                countRows += 1;

                var val = $('.Total').get(trId);
                var valOriginalUnitAmout = $('label[for="lblUnitAmountDecimal"][id="' + trId + '"]').text();

                $('#btnCalculateTotals').css('display', 'none');

                $("input[type=text][name=UnitName][id=" + trId + "]").attr('readonly', false);
                $("input[type=text][name=UnitName][id=" + trId + "]").css('display', 'block');
                $(`#msgLabelUnitMaxLength_${trId}`).hide();
                if ($("input[type=text][name=UnitName][id=" + trId + "]").val().length > 20) {
                    $(`#ccUnitDescGroup_${trId}`).addClass('esg-has-error');
                    $(`#msgUnitMaxLength_${trId}`).show();
                    $(`#ccUnitDescIcon_${trId}`).show();
                }
                $('label[for="lblUnitName"][id="ccUnitDesc_' + trId + '"]').css('display', 'none');

                $("input[type=text][name=CreditDesc][id=" + trId + "]").attr('readonly', false);
                $("input[type=text][name=CreditDesc][id=" + trId + "]").css('display', 'block');
                $('label[for="lblCreditDesc"][id="ccDesc_' + trId + '"]').css('display', 'none');

                $('input[name="RowAmount"][id=' + trId + ']').prop('readonly', false);
                $("input[type=text][name=RowAmount][id=" + trId + "]").css('display', 'block');
                $("input[type=text][name=RowAmount][id=" + trId + "]").val(valOriginalUnitAmout);
                $("input[type=text][name=RowAmount][id=" + trId + "]").focus();

                var ChargeCreditNumber = $("input[id='ccCode_" + trId + "'][name='CashRe.ChargeCreditCodeId'][type=hidden]").val();
                var ReceiptNumber = $("input[id='ccReceiptNumber_" + trId + "'][name='CashRe.ReceiptNumber'][type=hidden]").val();
                var UnitAmount = valOriginalUnitAmout.split("$");
                CalculateTaxes(UnitAmount[1], ChargeCreditNumber, trId, ReceiptNumber);

                $('#divTotals').css('display', 'none');
                $('td[name="lblTotalTaxes"][id=' + trId + ']').css('font-style', 'italic');
                $('td[name="lblTotalAmount"][id=' + trId + ']').css('font-style', 'italic');
                $('label[for="lblUnitAmountDecimal"][id=' + trId + ']').css('font-style', 'italic');
            }
            else {
                $("input[type=text][name=CreditDesc][id=" + trId + "]").attr('readonly', true);
                $("input[type=text][name=CreditDesc][id=" + trId + "]").css('display', 'none');
                $('label[for="lblCreditDesc"][id="ccDesc_' + trId + '"]').css('display', 'block');

                $('input[name="CreditDesc"][id=' + trId + ']').parent().removeClass('has-error');
                $('#DescriptionGroup_' + trId).removeClass('esg-has-error');
                $('#DescIcon_' + trId).css('display', 'none');

                $("input[type=text][name=UnitName][id=" + trId + "]").attr('readonly', true);
                $("input[type=text][name=UnitName][id=" + trId + "]").css('display', 'none');
                if ($(`#ccUnitDesc_${trId}`).text().length > 20) {
                    $(`#msgLabelUnitMaxLength_${trId}`).show();
                }
                $(`#ccUnitDescGroup_${trId}`).removeClass('esg-has-error');
                $(`#msgUnitMaxLength_${trId}`).hide();
                $(`#ccUnitDescIcon_${trId}`).hide();
                $('label[for="lblUnitName"][id="ccUnitDesc_' + trId + '"]').css('display', 'block');

                $("input[type=text][name=RowAmount][id=" + trId + "]").css('display', 'none');
                $("input[type=text][name=RowAmount][id=" + trId + "]").val('');

                $('input[id=' + trId + '][name="RowTaxAmount"]').css('display', 'none');
                $('input[id=' + trId + '][name="RowTotalAmount"]').css('display', 'none');
                $('input[id=' + trId + '][name="RowTaxAmount"]').val('');
                $('input[id=' + trId + '][name="RowTotalAmount"]').val('');

                $('#RecTaxPayerIcon_' + trId).css('display', 'none');
                $('input[name="RowAmount"][id=' + trId + ']').parent().removeClass('has-error');
                $('#RecAmountGroup_' + trId).removeClass('esg-has-error');

                $('#divTotals').css('display', 'none');
                $('input[name="RowAmount"][id=' + trId + ']').prop('readonly', false);

                $('td[name="lblTotalTaxes"][id=' + trId + ']').css('font-style', 'normal');
                $('td[name="lblTotalAmount"][id=' + trId + ']').css('font-style', 'normal');
                $('label[for="lblUnitAmountDecimal"][id=' + trId + ']').css('font-style', 'normal');

                $('#btnCalculateTotals').css('display', 'block');

                while (ColsAmounts.length > 0) {
                    ColsAmounts.pop();
                }

                while (cols.length > 0) {
                    cols.pop();
                }

                countRows -= 1;

                if (countRows == 0) {
                    $('#btnCalculateTotals').css('display', 'none');
                    $('.msjError').css('display', 'none');
                }
            }
        }
    });

    $('#btnCalculateTotals').click(function () {
        var countOK = 0;
        GralOInd = 1;
        while (ColsAmounts.length > 0) {
            ColsAmounts.pop();
        }
        while (cols.length > 0) {
            cols.pop();
        }

        $('#divTotals').css('display', 'block');
        $('#SubTotal').val('');
        $('#TotalTT').val('');
        $('#Total').val('');

        $("input[type=checkbox]:checked").each(function () {

            var frd = new Object();
            var Amounts = new Object();
            var valores = $(this).closest('td').siblings();

            valores.each(function (colIndex, val) {
                rowId = val.firstElementChild.value;

                Amounts.InvoiceHeaderId = $("#InoviceHeaderId").val();
                Amounts.PeopleOrgCodeId = $("#PeopleOrgCodeId").val();
                Amounts.IsAGlobalCreditNote = $("#IsAGlobalCreditNote").val();
                Amounts.IsAPPDTaxCreditNote = $("#IsAPPDTaxCreditNote").val();

                if (colIndex === 0) {
                    var id = val.id;
                    frd.ChargeCreditCodeId = rowId;
                    frd.ChargeCreditCode = $("#ccCode_" + id).val();
                    frd.ChargeCreditNumber = $("#ccCode_" + id).val();
                    Amounts.ReceiptNumber = $("input[id='ccReceiptNumber_" + id + "'][name='CashRe.ReceiptNumber'][type=hidden]").val();
                    Amounts.ChargeCreditNumber = $("#ccCode_" + id).val();
                    frd.ReceiptNumber = $("input[id='ccReceiptNumber_" + id + "'][name='CashRe.ReceiptNumber'][type=hidden]").val();
                }
                if (colIndex === 1) {
                    var id = val.id;
                    frd.InvoiceDetailId = $("#ccInvoiceDetailId_" + id).val();
                    frd.ChargeCreditCodeId = rowId;
                    frd.ChargeCreditCode = $("#ccCode_" + id).val();
                    frd.ChargeCreditNumber = $("#ccCode_" + id).val();
                    Amounts.ChargeCreditNumber = $("#ccCode_" + id).val();
                }
                if (colIndex === 2) {
                    var id = val.id;
                    frd.UnitDescription = $("input[type=text][name=UnitName][id=" + id + "]").val();
                }
                if (colIndex === 3) {
                    var id = val.id;
                    frd.Description = $("input[type=text][name=CreditDesc][id=" + id + "]").val();
                }
                if (colIndex === 4) {
                    var id = val.id;
                    var DetailUnitAmount = $('input[id=' + id + '][type=text][name="RowAmount"]').val().trim().split('$');
                    frd.Amount = DetailUnitAmount[1];
                    Amounts.Amount = DetailUnitAmount[1];
                }
                if (colIndex === 5) {
                    var id = val.id;
                    var DetailTotalTaxes = $('label[id=' + id + '][for="RowTaxAmount"]').text().trim().split('$');
                    frd.DetailTotalTaxes = DetailTotalTaxes[1];
                }
                if (colIndex === 6) {
                    var id = val.id;
                    var DetailTotal = $('label[id=' + id + '][for="RowTotalAmount"]').text().trim().split('$');
                    frd.DetailTotal = DetailTotal[1];
                }


            });
            cols.push(frd);
            ColsAmounts.push(Amounts);

        });

        ColsAmounts.forEach(function (val) {
            if (val.Amount === "") {
                countOK += 1;
            }
        });

        if (countOK === 0) {
            $('.emptyMessageDiv').css('display', 'none');
            $.ajax({
                url: urlCalculateTotals,
                type: "POST",
                cache: false,
                dataType: "json",
                data: { model: JSON.stringify(ColsAmounts) },
                success: function (response) {

                    $('#SubTotal').val(response.subTotal);
                    $('#TotalTT').val(response.totalTaxes);
                    $('#Total').val(response.total);
                    TotalTax = $("#TotalTT").val().split('$');
                    var decimalval = response.subTotal.split('.');

                    var IsTotalTax = TotalTax[1];
                    var ValCero = "0.0000"
                    var ValCeroO = ".00"
                    var Cero = parseFloat(ValCero).toFixed(decimalval[1].length);

                    if (IsTotalTax === Cero || IsTotalTax === ValCeroO) {
                        $("#CodeTaxRefund").prop("disabled", true);
                        $("#CodeTaxRefund").val('');
                    }
                    else {

                        $("#CodeTaxRefund").prop("disabled", false);
                    }
                }
            });

            $("#btnProcessCreditNote").css('display', 'block');
            $(this).css('display', 'none');

        }
        else {
            $('#divTotals').css('display', 'none');
            $('.emptyMessageDiv').css('display', 'block');
            $("#btnProcessCreditNote").css('display', 'none');

        }

    });

    $("#btnProcessCreditNote").click(function () {

        $(".errorMessageRequiredDiv").hide();
        $(".errorMessageDiv").hide();
        $("#missingRequiredFieldsAlert").hide();

        var unitMaxLengthError = false;
        var inputCharges = $('input[type=text][name=UnitName]');
        for (var i = 0; i < inputCharges.length; i++) {
            if ($(inputCharges[i]).is(':visible') && $(inputCharges[i]).val().length > 20) {
                unitMaxLengthError = true;
            }
        }
        var labelCharges = $('label[id^="ccUnitDesc_"]');
        for (var i = 0; i < labelCharges.length; i++) {
            if ($(labelCharges[i]).is(':visible') && $(labelCharges[i]).text().length > 20) {
                unitMaxLengthError = true;
            }
        }

        if (unitMaxLengthError) {
            $('.errorMessageRequiredDiv').show();
            $('.successMessageDiv').hide();
            return false;
        }

        createForm.validate();

        var Email = $("#PreferredEmail");

        if (!createForm.valid()) {

            if (!$("#TaxPayerId").val()) {
                $("#TaxPayerId").parent().addClass('has-error');
                $("#receiverTaxPayerIdGroup").addClass('esg-has-error');
                $("#receiverTaxPayerIcon").css('display', 'block');
                $("#receiverTaxPayerDivLookup").hide();
            }

            if (!$("#PreferredEmail").val()) {
                $("#PreferredEmail").parent().addClass('has-error');
                $("#EmailGroup").addClass('esg-has-error');
                $("#EmailIcon").css('display', 'block');
            }

            if (!$("#IssTaxPayerId").val()) {
                $("#IssTaxPayerId").parent().addClass('has-error');
                $("#IssTaxPayerIdGroup").addClass('esg-has-error');
                $("#issuerIssTaxpayerIdIcon").css('display', 'block');
                $("#IssTaxPayerDivLookup").hide();
            }

            if (!$("#RecTaxAddress").val()) {
                $("#RecTaxAddress").parent().addClass('has-error');
                $("#RecTaxAddressGroup").addClass('esg-has-error');
                $("#RecTaxAddressIcon").css('display', 'block');
            }

            if (!$("#RecTaxRegimen").val()) {
                $("#RecTaxRegimen").parent().addClass('has-error');
                $("#RecTaxRegimenGroup").addClass('esg-has-error');
                $("#RecTaxRegimenIcon").css('display', 'block');
            }

            $("#missingRequiredFieldsAlert").show();
        }
        else {
            if ($("#PreferredEmail").val()) {
                $("#PreferredEmail").parent().removeClass('has-error');
                $("#EmailGroup").removeClass('esg-has-error');
                $("#EmailIcon").hide();
            }

            if ($("#RecTaxAddress").val()) {
                $("#RecTaxAddress").parent().removeClass('has-error');
                $("#RecTaxAddressGroup").removeClass('esg-has-error');
                $("#RecTaxAddressIcon").hide();
            }

            if ($("#RecTaxRegimen").val()) {
                $("#RecTaxRegimen").parent().removeClass('has-error');
                $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                $("#RecTaxRegimenIcon").hide();
            }

            if ($("#RecTaxRegimen").val()) {
                $("#RecTaxRegimen").parent().removeClass('has-error');
                $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                $("#RecTaxRegimenIcon").hide();
            }

            showProcessing();
            $("#btnProcessCreditNote").css('display', 'none');

            var CFDISelected = $("#CFDI option:selected").text();
            var CFDICode = $("#CFDI option:selected").val();
            var CFDIDesc = CFDISelected.substring(CFDICode.length + 1, CFDISelected.length);

            var PaymentTypeSelected = $("#paymentType option:selected").text();
            var PaymentTypeCode = $("#paymentType option:selected").val();
            var PaymentTypeDesc = PaymentTypeSelected;

            var Taxregimen = $("#IssTaxRegimen").val();
            var TaxregimenCode = Taxregimen.split("-");
            var SerialNumber = $("#IssSerial option:selected").text();
            var Total = $("#Total").val().split('$');
            var SubTotal = $("#SubTotal").val().split('$');
            var TotalTaxes = $("#TotalTT").val().split('$');

            fIdentityNumber = $("#FiscalIdNum").val();

            fResidencyDesc = $("#FiscalResidency").val();
            if (fResidencyDesc == '') {
                fResidency = '';
            }
            else {
                var residency = $("#FiscalResidency").val().split('-');
                fResidency = residency[0];
                fResidencyDesc = residency[1];
            }

            var ChargeCodeIDAmount = $("#CodeAmountRefund option:selected").val();
            var ChargeCodeIDTax = $("#CodeTaxRefund option:selected").val();

            if (ChargeCodeIDTax == null || ChargeCodeIDTax == "undefined") {
                ChargeCodeIDTax = 0;
            }

            var PaymentMethod = $("#PaymentMethod").val().split('-');
            var PaymentMethodId = PaymentMethod[0];
            var PaymentMethodDesc = PaymentMethod[1];


            var ParamInsInvoiceHeaderList = {
                CancelReasonName: CancelReasonName,
                CFDIRelatedId: CFDIRelatedId,
                PeopleOrgCodeId: $('#PeopleOrgCodeId').val(),
                IssuerTaxPayerId: $("#IssTaxPayerId").val(),
                InvoiceExpeditionId: isSubstitution ? $("#IssIssuingAddress").val() : modelInvoiceExpeditionId,
                PaymentType: PaymentTypeCode,
                PaymentCondition: $("#PaymentCondition").val(),
                ReceiverTaxpayerId: $("#TaxPayerId").val(),
                ReceiverEmail: $("#PreferredEmail").val(),
                Subtotal: SubTotal[1],
                Total: Total[1],
                TotalTransferTaxes: TotalTaxes[1],
                ExchangeRate: 0.00,
                Currency: $("#IssCurrency").val(),
                PaymentMethod: PaymentMethodId,
                PaymentMethodDesc: PaymentMethodDesc,
                CityOfIssue: $("#IssPlaceIssue").val(),
                PaymentAccountNumber: "",
                TaxRegimen: TaxregimenCode[0],
                Comments: $("#IssComments").val(),
                Detail: cols,
                CFDIUsageCode: CFDICode,
                CFDIUsageDesc: CFDIDesc,
                TaxRegimenDesc: TaxregimenCode[1],
                PaymentTypeDesc: PaymentTypeDesc,
                StartDate: null,
                EndDate: null,
                FiscalIdentityNumber: fIdentityNumber,
                FiscalResidency: fResidency,
                FiscalResidencyDesc: fResidencyDesc,
                SerialNumber: SerialNumber,
                ReceiptNumber: $('#ReceiptNumber').val(),
                ChargeCreditId: ChargeCodeIDAmount,
                ChargeCreditIdTax: ChargeCodeIDTax,
                InvoiceHeaderId: $("#InoviceHeaderId").val(),
                IsAGlobalCreditNote: $('#IsAGlobalCreditNote').val(),
                IsAPPDTaxCreditNote: $('#IsAPPDTaxCreditNote').val()
            };

            var isSustitution = $('#isSustitution').val();

            if (isSustitution === 'True') {
                ParamInsInvoiceHeaderList.CFDIRelated = CFDIRelated;
                ParamInsInvoiceHeaderList.CFDIRelated2 = $('#UUIDFolio').val();
            }
            else {
                ParamInsInvoiceHeaderList.CFDIRelated = $('#UUIDFolio').val();

            }

            if (isSustitution === 'True') {
                ParamInsInvoiceHeaderList.CFDIRelated2 = $('#UUIDFolio').val();

                var $table = $("#records_table tbody");
                $table.find('tr').each(function (rowIndex, r) {
                    var frd = new Object();
                    var frDetails = [];
                    $(this).find(":input[type=hidden][name='CashRe.ChargeCreditCodeId']").each(function (index, v) {
                        frDetails.push(v.value);
                    });
                    jQuery.each(frDetails, function (i, val) {
                        rowId = val;
                        $table.find('tr').find('td').each(function (colIndex, c) {
                            if (colIndex === 0) {
                                frd.ChargeCreditCodeId = rowId;
                                frd.ChargeCreditCode = $("#ccCode_" + rowId).val();
                                frd.IsATax = $("#ccIsATax_" + rowId).val();
                            }
                            if (colIndex === 1) {
                                frd.InvoiceDetailId = $("#ccInvoiceDetailId_" + rowId).val();
                            }
                            if (colIndex === 4) {
                                frd.UnitDescription = $("#ccUnitDesc_" + rowId).val();
                            }
                            if (colIndex === 5) {
                                frd.Description = $("#ccDesc_" + rowId).val();
                            }
                            if (colIndex === 6) {
                                frd.Amount = $("#ccTotalUnit_" + rowId).text().split('$')[1];
                                frd.TotalTaxes = $("#ccTotalTaxes_" + rowId).text().split('$')[1];
                                frd.UnitAmount = $("#ccUnitAmount_" + rowId).text().split('$')[1];
                            }
                        });

                        if (frd.IsATax.toLowerCase() === "true")
                            frd.IsATax = 1;
                        else
                            frd.IsATax = 0;

                        cols.push(frd);
                    });
                });
                ParamInsInvoiceHeaderList.Detail = cols;
            }

            $.ajax({
                url: urlCreateCreditNote,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { Model: JSON.stringify(ParamInsInvoiceHeaderList) },
                success: function (response) {
                    if (response.id <= 0) {
                        while (ColsAmounts.length > 0) {
                            ColsAmounts.pop();
                        }
                        while (cols.length > 0) {
                            cols.pop();
                        }
                        hideProcessing();
                        $("#btnProcessCreditNote").css('display', 'block');
                        $(".errorMessageDiv").css('display', 'block');
                        $(".errorMessageResult").html(response.message);
                    }
                    else {
                        while (ColsAmounts.length > 0) {
                            ColsAmounts.pop();
                        }
                        while (cols.length > 0) {
                            cols.pop();
                        }
                        hideProcessing();
                        $('.successMessageDiv').css('display', 'block');
                        $('.successMessageResult').html(response.message);
                        window.location.href = urlViewAll;
                    }
                }
            });
        }

    });

    $("#btnBack").click(function () {
        showProcessing();
        $("#btnBack").prop("disabled", true);
        $.ajax({
            url: urlEditFiscalRecord,
            type: "GET",
            cache: false,
            dataType: "html",
            success: function () {
                window.location.href = urlEditFiscalRecord;
            }
        });
    });
    // #endregion Click

    // #region Focusout
    $("#IssTaxPayerId").focusout(onIssTaxPayerIdFocusOut);

    $("#TaxPayerId").focusout(onTaxPayerIdFocusOut);

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
    // #endregion Focusout

    // #region Functions
    function LoadIssuerSetUp() {
        $.ajax({
            url: urlSelectIssuerSetUp,
            type: "GET",
            cache: false,
            success: function (data) {

                if (data.IssInvoiceOrganizationId != 0 && data.IssInvoiceOrganizationId != null) { //Si existe registro en la tabla de IssuerDefault se valida esa información

                    var IssuerTaxpayer = $('#IssTaxPayerId').val();

                    if (IssuerTaxpayer === data.IssInvoiceTaxpayerId) {
                        $("#IssSerial").empty();

                        if (isSubstitution) {
                            $("#IssIssuingAddress").empty();
                            $("#IssIssuingAddress").append($('<option></option>').val(data.IssInvoiceExpeditionId).html(data.IssIssuingAddress));
                        }
                        else {
                            $("#btnProcessCreditNote").css('display', 'none');
                        }

                        $("#IssSerial").append($('<option></option>').val(data.IssSerialNumberCreditNote).html(data.IssSerialNumberCreditNote));
                        var SerialNum = $("#IssSerial").val().trim();
                        var InvoiceOrganizationId = isSubstitution ? data.IssInvoiceOrganizationId : modelInvoiceOrganizationId;
                        var InvoiceExpId = isSubstitution ? data.IssInvoiceExpeditionId : modelInvoiceExpeditionId;

                        $.ajax({
                            url: urlGetTaxRegimen,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: InvoiceOrganizationId },
                            success: function (response) {

                                if (response.IssIssuingAdd != null) {

                                    IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                                    if (isSubstitution) {
                                        $.each(IssIssuingAddress, function (i, val) {
                                            if (InvoiceExpId != val.IssInoviceExpeditionId) {
                                                $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                            }
                                        });

                                        var IssuingAddress = $.grep(response.IssIssuingAdd, function (elem) { return elem.IssInoviceExpeditionId == InvoiceExpId });

                                        if (IssuingAddress[0].IssPlaceIssue != null) {
                                            $("#IssPlaceIssue").val(IssuingAddress[0].IssPlaceIssue);
                                        }

                                        if (IssIssuingAddress[0].IssByExpedition === 0) {
                                            InvoiceExpId = null;
                                        }
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
                                                var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                                if (isSubstitution) {
                                                    var SerialPrefer = $.grep(IssSerial, function (e) { return e.IssSerialNumber == SerialNum })

                                                    if (SerialPrefer[0].IssLastFolio == '2147483647') {

                                                        $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 1));

                                                    }
                                                    else {
                                                        $("#IssSerial").append($('<option></option>').val(SerialPrefer[0].IssInvoiceReceipt).html(SerialPrefer[0].IssSerialNumber).attr("id", 0));

                                                    }
                                                }

                                                var issInvoiceReceiptSelected = null;;
                                                $.each(IssSerial, function (i, val) {
                                                    if ($("#IssSerial").val() != val.IssInvoiceReceipt) {
                                                        if (val.IssSerialNumber == SerialNum) {
                                                            issInvoiceReceiptSelected = val.IssInvoiceReceipt;
                                                        }
                                                        if (val.IssLastFolio == '2147483647') {
                                                            $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 1));

                                                        }
                                                        else {
                                                            $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 0));
                                                        }
                                                    }
                                                });
                                                if (issInvoiceReceiptSelected) {
                                                    $("#IssSerial").val(issInvoiceReceiptSelected);
                                                }

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
                    else { //En caso contrario se va a cargar la información del Issuer que fue configurado en la factura electrónica
                        $("#IssSerial").empty();
                        $("#IssIssuingAddress").empty();
                        var IssuerInvoiceOrganizationId = $("#IssInvoiceOrganizationId").val();

                        $.ajax({
                            url: urlGetTaxRegimen,
                            type: "GET",
                            cache: false,
                            dataType: "json",
                            data: { id: IssuerInvoiceOrganizationId },
                            success: function (response) {

                                var InoviceExpeditionId;

                                IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                                $.each(IssIssuingAddress, function (i, val) {
                                    $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                                });

                                if (IssIssuingAddress[0].IssByExpedition === 0) {
                                    InoviceExpeditionId = null;
                                }
                                else {
                                    InoviceExpeditionId = IssIssuingAddress[0].IssInoviceExpeditionId;
                                }

                                //Get Serial
                                $.ajax({
                                    url: urlGetSerialNumber,
                                    type: "GET",
                                    cache: false,
                                    dataType: "json",
                                    data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                                    success: function (response) {

                                        $("#lblWarnSerial").css('display', 'none');

                                        if (response.IssSerial != null) {
                                            var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                            $.each(IssSerial, function (i, val) {

                                                if (val.IssLastFolio == '2147483647') {
                                                    $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 1));

                                                }
                                                else {
                                                    $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 0));
                                                }

                                            });
                                        }
                                        else {

                                            $("#IssSerial").empty();
                                        }

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

                            }

                        });
                    }
                }
                else { //En caso contrario se va a cargar la información del Issuer que fue configurado en la factura electrónica
                    $("#IssSerial").empty();
                    $("#IssIssuingAddress").empty();
                    var IssuerInvoiceOrganizationId = $("#IssInvoiceOrganizationId").val();

                    $.ajax({
                        url: urlGetTaxRegimen,
                        type: "GET",
                        cache: false,
                        dataType: "json",
                        data: { id: IssuerInvoiceOrganizationId },
                        success: function (response) {

                            var InoviceExpeditionId;

                            IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

                            $.each(IssIssuingAddress, function (i, val) {
                                $("#IssIssuingAddress").append($('<option></option>').val(val.IssInoviceExpeditionId).html(val.IssIssuingAddress));
                            });

                            if (IssIssuingAddress[0].IssByExpedition === 0) {
                                InoviceExpeditionId = null;
                            }
                            else {
                                InoviceExpeditionId = IssIssuingAddress[0].IssInoviceExpeditionId;
                            }

                            //Get Serial
                            $.ajax({
                                url: urlGetSerialNumber,
                                type: "GET",
                                cache: false,
                                dataType: "json",
                                data: { id: IssIssuingAddress[0].IssInvoiceOrganizationId, expId: InoviceExpeditionId },
                                success: function (response) {

                                    $("#lblWarnSerial").css('display', 'none');

                                    if (response.IssSerial != null) {
                                        var IssSerial = $.grep(response.IssSerial, function (element) { return element });

                                        $.each(IssSerial, function (i, val) {

                                            if (val.IssLastFolio == '2147483647') {
                                                $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 1));

                                            }
                                            else {
                                                $("#IssSerial").append($('<option></option>').val(val.IssInvoiceReceipt).html(val.IssSerialNumber).attr("id", 0));
                                            }

                                        });
                                    }
                                    else {

                                        $("#IssSerial").empty();
                                    }

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

                        }

                    });
                }

            }
        });
    }

    function CalculateTaxes(UnitAmount, CreditNumber, id, receiptNumber) {
        var Model = {

            InvoiceHeaderId: $("#InoviceHeaderId").val(),
            ChargeCreditNumber: CreditNumber,
            Amount: UnitAmount,
            PeopleOrgCodeId: $("#PeopleOrgCodeId").val(),
            IsAGlobalCreditNote: $("#IsAGlobalCreditNote").val(),
            IsAPPDTaxCreditNote: $("#IsAPPDTaxCreditNote").val(),
            ReceiptNumber: receiptNumber
        };

        $.ajax({
            url: urlCalculateTaxTotalsByCharge,
            type: "POST",
            cache: false,
            dataType: "json",
            data: { model: JSON.stringify(Model) },
            success: function (response) {

                var getDecimals = $('label[for="lblUnitAmountDecimal"]').text().trim().split("$");
                var m = getDecimals[1].split('.');
                val = m[1].length;

                $('input[name="RowAmount"][id=' + id + ']').val(response.subTotal);
                $('input[name="RowTaxAmount"][id=' + id + ']').val(response.totalTaxes);
                $('input[name="RowTotalAmount"][id=' + id + ']').val(response.total);

                $('input[name="RowTaxAmount"][id=' + id + ']').css('display', 'block');
                $('input[name="RowTotalAmount"][id=' + id + ']').css('display', 'block');
                $('input[name="RowAmount"][id=' + id + ']').prop('readonly', false);

                $('#btnCalculateTotals').css('display', 'block');
                $("#btnProcessCreditNote").css('display', 'none');

            }
        });
    }

    function onTaxPayerIdFocusOut() {
        var IdTaxpayer = $("#TaxPayerId").val();
        var RecTaxPayId = $("#TaxPayerId");
        var response;
        var FiscalIdentityNumber;
        var TaxPayerIdVal;

        if (IdTaxpayer.indexOf("-") >= 0) {
            var res = IdTaxpayer.split("-");
            TaxPayerIdVal = res[0];
            FiscalIdentityNumber = res[1];
            response = $.grep(TaxPayers, function (element) { return element.FiscalIdentityNumber == FiscalIdentityNumber })

            $("#NameCorpName").val(response[0].CorporateName);
            $("#FiscalAddress").val(response[0].FiscalResidencyDesc);
            $("#FiscalAddressCode").val(response[0].FiscalResidency);
            fResidency = response[0].FiscalResidency;
            fResidencyDesc = response[0].FiscalResidencyDesc;
            $("#FiscalIdNum").val(response[0].FiscalIdentityNumber);
            $("#RecTaxAddress").val(response[0].PostalCode);
            fIdentityNumber = response[0].FiscalIdentityNumber;
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').css('display', 'none');
            $('#RecTaxPayerGroup').removeClass('esg-has-error');
            $("#TaxPayerId").val(TaxPayerIdVal);
        }
        else if (TaxPayers) {
            response = $.grep(TaxPayers, function (element) { return element.TaxPayerId == IdTaxpayer; })
        }
        else {
            response = [];
        }

        if (IdTaxpayer.length > 0 || response.length > 0) {
            $("#NameCorpName").val(response[0].CorporateName);
            $("#FiscalAddress").val(response[0].FiscalResidencyDesc);
            $("#FiscalAddressCode").val(response[0].FiscalResidency);
            $("#FiscalIdNum").val(response[0].FiscalIdentityNumber);
            $("#RecTaxAddress").val(response[0].PostalCode);
            $("#RecTaxRegimen").val(response[0].TaxRegimenCode + " - " + response[0].TaxRegimenDesc);
            fIdentityNumber = response[0].FiscalIdentityNumber;
            RecTaxPayId.parent().removeClass('has-error')
            $('#RecTaxPayerIcon').css('display', 'none');
            $('#RecTaxPayerGroup').removeClass('esg-has-error');
            $("#receiverTaxPayerDivLookup").show();

            //VALIDATE IF TAXPAYER ID ITS COMPANY O PERSON
            $.ajax({
                url: urlGetCFDIReceivers,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { length: IdTaxpayer.length },
                success: function (res) {
                    if ($('#CFDI option').length == 1) {
                        $.each(res, function (i, val) {
                            $("#CFDI").append($('<option></option>').val(val.Code).html(val.Description));
                        });
                    }

                    if ($("#PreferredEmail").val()) {
                        $("#PreferredEmail").parent().removeClass('has-error');
                        $("#EmailGroup").removeClass('esg-has-error');
                        $("#EmailIcon").hide();
                    }

                    if ($("#RecTaxAddress").val()) {
                        $("#RecTaxAddress").parent().removeClass('has-error');
                        $("#RecTaxAddressGroup").removeClass('esg-has-error');
                        $("#RecTaxAddressIcon").hide();
                    }

                    if ($("#RecTaxRegimen").val()) {
                        $("#RecTaxRegimen").parent().removeClass('has-error');
                        $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                        $("#RecTaxRegimenIcon").hide();
                    }

                    if ($("#RecTaxRegimen").val()) {
                        $("#RecTaxRegimen").parent().removeClass('has-error');
                        $("#RecTaxRegimenGroup").removeClass('esg-has-error');
                        $("#RecTaxRegimenIcon").hide();
                    }

                    $("#formCreate").clearValidation();
                }
            });

        }
        else {
            $("#NameCorpName").val("");
            $("#FiscalAddress").val("");
            $("#FiscalAddressCode").val("");
            $("#FiscalIdNum").val("");
            $("#RecTaxAddress").val("");
            $("#RecTaxRegimen").val("");

            RecTaxPayId.parent().addClass('has-error');
            $('#RecTaxPayerIcon').css('display', 'block');
            $("#receiverTaxPayerDivLookup").hide();
            $('#RecTaxPayerGroup').addClass('esg-has-error');
        }
    }

    function onIssTaxPayerIdFocusOut() {
        IdIssTaxPayer = $("#IssTaxPayerId").val();

        if (IdIssTaxPayer !== "") {
            $("#IssTaxPayerId").parent().removeClass('has-error');
            $("#IssTaxPayerIdGroup").removeClass('esg-has-error');
            $('#issuerIssTaxpayerIdIcon').css('display', 'none');
            $("#IssTaxPayerDivLookup").show();
        }

        var response = $.grep(IssTaxPayers, function (element) { return element.IssTaxpayerId == IdIssTaxPayer; })[0];
        if (!response == "") {
            $("#IssNameCorpName").val("");
            $("#IssTaxRegimen").empty();
            $("#IssNameCorpName").val("");
            $("#IssSerial").empty();
            $("#IssIssuingAddress").empty();
            $("#IssPlaceIssue").val("");

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

                        var IssTaxRegimen = $.grep(response.IssTaxRegimen, function (element) { return element });
                        IssuerTaxRegimenCode = IssTaxRegimen[0].IssCodeValue;
                        IssuerTaxRegimenDesc = IssTaxRegimen[0].IssLongDesc;

                        $("#IssTaxRegimen").val(IssTaxRegimen[0].IssCodeValue + " - " + IssTaxRegimen[0].IssLongDesc);
                        $("#IssTaxRegimenIcon").css('display', 'none');
                        $("#IssTaxRegimenGroup").parent().removeClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-error');
                        $("#IssTaxPayerDivLookup").show();
                    }
                    else {
                        $("#IssTaxRegimenIcon").css('display', 'block');
                        $("#IssTaxRegimenGroup").parent().addClass('esg-has-error');
                        IssTaxRegimenC.parent().removeClass('has-success').addClass('has-error');
                        $("#IssTaxPayerDivLookup").hide();
                        $("#IssTaxRegimen").val('');
                    }

                    if (response.IssIssuingAdd != null) {

                        var InvoiceExpeditionId;

                        IssIssuingAddress = $.grep(response.IssIssuingAdd, function (element) { return element });

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
                            InvoiceExpeditionId = null;
                        }
                        else {
                            InvoiceExpeditionId = issPlaceIssueFound.IssInoviceExpeditionId;
                        }

                        if (issPlaceIssueFound != null && issPlaceIssueFound.IssPlaceIssue != null) {
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
                            data: { id: issPlaceIssueFound.IssInvoiceOrganizationId, expId: InvoiceExpeditionId },
                            success: function (response) {

                                $("#lblWarnSerial").css('display', 'none');

                                if (response.IssSerial != null) {
                                    var IssSerial = $.grep(response.IssSerial, function (element) { return element });

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
                $("#IssTaxPayerDivLookup").hide();
                $("#IssNameCorpName").val("");
                $("#IssSerial").empty();
                $("#IssIssuingAddress").val("");
                $("#IssPlaceIssue").val("");
            }
        }
    }
    // #endregion Functions
});