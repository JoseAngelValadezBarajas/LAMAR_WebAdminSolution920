$(document).ready(function () {
    $("#SearchPeople").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlSearchResults,
                type: "GET",
                cache: false,
                dataType: "json",
                data: { Keyword: request.term },
                success: function (data) {
                    if (data.length > 0) {
                        response($.map(data, function (item) {
                            if (item.PeopleOrgCodeId !== "") {
                                $("#Noresults").css('display', 'none');
                                return { label: item.PeopleOrgCodeId + '-' + item.FullName, value: item.PeopleOrgCodeId + '-' + item.FullName };
                            }
                            else {
                                $("#Noresults").css('display', 'block');
                            }
                        }));
                    }
                }
            });
        }
    });

    $("#SearchBtn").hover(
        () => {
            $("#lblAdvanced").css('display', 'block');
            $("#Noresults").css('display', 'none');
        },
        () =>  {
            $("#lblAdvanced").css('display', 'none');
        }
    );

    $("#SearchBtn").click(function () {
        Search($("#SearchPeople").val());
    });

    $('body').on('click', '.ui-menu-item-wrapper', function (e) {
        Search(e.currentTarget.innerText);
    });
});

function Search(CodeId) {
    if (CodeId !== "" && CodeId.indexOf("-") !== -1) {
        var Code = CodeId.split('-');
        var peopleCodeId = Code[0];
        $.ajax({
            url: urlPeopleMenu,
            type: "GET",
            cache: false,
            dataType: "html",
            data: { id: peopleCodeId },
            success: function (data) {
                window.location.href = urlPeopleTaxpayerId;
            }
        });
    }
    else {
        window.location.href = urlSearchAdvanced;
    }
}