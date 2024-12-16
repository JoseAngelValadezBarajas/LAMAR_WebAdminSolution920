function ShowHideRows(contentId, page) {
    var totalRows = parseInt($(contentId + ' #hdnModelCount').val());
    var minRows = parseInt($(contentId + ' #hdnMinRows').val());
    if (minRows > 0 && totalRows > minRows) {
        if (page < 1)
            page = 1;
        var resultsByPage = $(contentId + ' #pageOptions').find(":selected").text();
        var totalOfPages = Math.floor(totalRows / resultsByPage);

        var mod = totalRows % resultsByPage;

        if (mod > 0)
            totalOfPages = totalOfPages + 1;

        if (page > totalOfPages) {
            page = totalOfPages;
        }

        var rows = $(contentId + " table:not([data-ignored='true']) > tbody > tr");
        for (var i = 0; i < rows.length; i++) {
            var pagMin = (page - 1) * resultsByPage;
            var pagMax = page * resultsByPage;
            if (pagMin <= i && i < pagMax) {
                $(rows[i]).show();
            }
            else {
                $(rows[i]).hide();
            }
        }

        $(contentId + ' #totalOfPages').text(totalOfPages);

        $(contentId + ' #labelModelCount').text(totalRows);

        $(contentId + ' #paginationInput').val(page);

        $(contentId + ' .esg-pagination').show();
    }
    else {
        $(contentId + ' #labelModelCount').text(totalRows);
        $(contentId + ' #paginationInput').val('0');
        $(contentId + ' .esg-pagination').hide();
    }
}

$(document).ready(function () {
    $.each($("div[data-pagination='true']"), function (key, value) {
        var contentId = '#' + value.id;

        if ($(value).attr('data-pagination') === 'true') {
            $.each($(contentId + ' #pageOptions > option'), function (key, value) {
                $(value).removeAttr('selected');
                if ($(value).attr('value') === $('#hdnDefaultSelected').val()) {
                    $(value).attr('selected', 'selected');
                }
            });
            $(contentId + ' #paginationInput').change(function (event) {
                event.preventDefault();
                ShowHideRows(contentId, parseInt($(contentId + ' #paginationInput').val()));
            });

            $(contentId + ' #pageOptions').change(function (event) {
                event.preventDefault();
                ShowHideRows(contentId, 1);
            });

            $(contentId + ' #firstButton').click(function (event) {
                event.preventDefault();
                ShowHideRows(contentId, 1);
            });

            $(contentId + ' #previousButton').click(function (event) {
                event.preventDefault();
                ShowHideRows(contentId, parseInt($(contentId + ' #paginationInput').val()) - 1);
            });

            $(contentId + ' #nextButton').click(function (event) {
                event.preventDefault();
                ShowHideRows(contentId, parseInt($(contentId + ' #paginationInput').val()) + 1);
            });

            $(contentId + ' #lastButton').click(function (event) {
                event.preventDefault();
                ShowHideRows(contentId, parseInt($(contentId + ' #totalOfPages').text()));
            });
            ShowHideRows(contentId, 1);
        }
    });

});
