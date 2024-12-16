$(document).ready(function () {
    ClearElements();
    $('#forPerson').css('display', 'block');
    $('#forOrganization').css('display', 'none');
    $("#rdoPerson").prop("checked", true);

    $("#btnSearch").click(function () {

        var Search = $('input[type=radio]:checked').val();
        $('#PeopleRecords').css('display', 'none');
        $('#OrganizationRecords').css('display', 'none');

        if (Search == "RadioPerson") {

            if ($('#PeopleId').val() == "" && $('#FirstName').val() == "" && $('#MiddleName').val() == "" && $('#LastName').val() == "" && $('#Email').val() == "" && $('#RecordType option:selected').text() == "") {

                $('#divAlertNoCriteria').css('display', 'block');
                $('#divAlertNoResults').css('display', 'none');

            }
            else {
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                var PeopleModel = {
                    PeopleCodeId: $("#PeopleId").val(),
                    FirstName: $("#FirstName").val(),
                    MiddleName: $("#MiddleName").val(),
                    LastName: $("#LastName").val(),
                    PrimaryEmail: $("#Email").val(),
                    RecordType: $("#RecordType option:selected").val()
                };

                $.ajax({
                    url: urlPeopleList,
                    type: "POST",
                    cache: false,
                    dataType: "html",
                    data: { ModelPeople: JSON.stringify(PeopleModel) },
                    success: function (data) {

                        if (data != null && data != "") {
                            $('#divAlertNoResults').css('display', 'none');
                            $('#divAlertNoCriteria').css('display', 'none');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            $("#getRecords").html(data);
                            ShowHideRows("#PeopleRecords", 1);
                        }
                        else {
                            $('#divAlertNoResults').css('display', 'block');
                            $('#divAlertNoCriteria').css('display', 'none');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            ClearElements();
                        }

                    }
                });
            }

        }
        else {

            if ($("#OrganizationId").val() == "" && $('#OrganizationName').val() == "" && $('#OrganizationEmail').val() == "" && $('#RecordTypeOrg option:selected').text() == "") {

                $('#divAlertNoCriteria').css('display', 'block');
                $('#divAlertNoResults').css('display', 'none');

            }
            else {
                //Aqui poner los campos del organization y hacer el modelo
                $("#Processing").css('display', 'block');
                $("#Overlaydiv").css('display', 'block');

                var OrganizationModel = {
                    OrganizationCodeId: $('#OrganizationId').val(),
                    OrganizationName: $('#OrganizationName').val(),
                    Email: $("#OrganizationEmail").val(),
                    RecordType: $('#RecordTypeOrg option:selected').val()
                };

                $.ajax({
                    url: urlOrganizationList,
                    type: "POST",
                    cache: false,
                    dataType: "html",
                    data: { modelOrganization: JSON.stringify(OrganizationModel) },
                    success: function (data) {
                        if (data != null && data != "") {
                            $('#divAlertNoCriteria').css('display', 'none');
                            $('#divAlertNoResults').css('display', 'none');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            $("#getRecords").html(data);
                            ShowHideRows("#OrganizationRecords", 1);
                        }
                        else {
                            $('#divAlertNoCriteria').css('display', 'none');
                            $('#divAlertNoResults').css('display', 'block');
                            $("#Processing").css('display', 'none');
                            $("#Overlaydiv").css('display', 'none');
                            ClearElements();
                        }
                    }
                });

                $('#PeopleRecords').css('display', 'none');
                $('#OrganizationRecords').css('display', 'block');

            }
        }

    });

    $("#btnClearAll").click(function () {
        ClearElements();
        $('#divAlertNoResults').css('display', 'none');
        $('#divAlertNoCriteria').css('display', 'none');
    });

    $("#btnCancel").click(function () {
        window.location.href = urlMenu;
    });

    $("#rdoPerson").click(function () {
        $('#forOrganization').css('display', 'none');
        $('#forPerson').css('display', 'block');
        $('#PeopleRecords').css('display', 'none');
        $('#OrganizationRecords').css('display', 'none');
        $('#divAlertNoResults').css('display', 'none');
        $('#divAlertNoCriteria').css('display', 'none');
        ClearElements();

    });

    $("#rdoOrganization").click(function () {
        $('#forOrganization').css('display', 'block');
        $('#forPerson').css('display', 'none');
        $('#PeopleRecords').css('display', 'none');
        $('#OrganizationRecords').css('display', 'none');
        $('#divAlertNoResults').css('display', 'none');
        $('#divAlertNoCriteria').css('display', 'none');
        ClearElements();
    });

    $('body').on('click', '#peopleCodeId', function (e) {

        var Search = $('input[type=radio]:checked').val();

        var PeopleCodeId = e.currentTarget.innerText;

        if (Search == "RadioPerson") {
            $.ajax({
                url: urlPeopleMenu,
                type: "GET",
                cache: false,
                dataType: "html",
                data: { id: PeopleCodeId },
                success: function (data) {
                    window.location.href = urlPeopleTaxpayerId;
                }
            });
        }
        else {
            $.ajax({
                url: urlOrganizationMenu,
                type: "GET",
                cache: false,
                dataType: "html",
                data: { id: PeopleCodeId },
                success: function (data) {
                    window.location.href = urlOrganizationTaxpayerId;
                }
            });
        }

    });

    function ClearElements() {
        $('#PeopleId').val('');
        $('#FirstName').val('');
        $('#MiddleName').val('');
        $('#LastName').val('');
        $('#Email').val('');
        $('#RecordType option[value=""]').prop('selected', true);
        $("#OrganizationId").val('');
        $('#OrganizationName').val('');
        $('#OrganizationEmail').val('');
        $('#RecordTypeOrg option[value=""]').prop('selected', true);
        $('#PeopleRecords').css('display', 'none');
        $('#OrganizationRecords').css('display', 'none');
    }


});

