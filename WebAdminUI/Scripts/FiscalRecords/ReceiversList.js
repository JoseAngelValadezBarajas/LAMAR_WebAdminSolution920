$(document).ready(function () {
    $(".btnOpenDeleteModal").click(openDeleteModal);

    $(".btnCancelDelete").click(function () {
        $('#taxpayerField').empty();
        $('#nameField').empty();
        $('#btnDelete').data('taxpayerId', 0);

        $('#deleteModal').hide();
        $('#deleteModalOverlay').hide();
    });

    $("#btnDelete").click(function () {
        let taxpayerId = $(this).data('taxpayerId');

        if (taxpayerId) {
            showProcessing();
            $.ajax({
                url: urlDeleteReceiver,
                type: "POST",
                cache: false,
                dataType: "json",
                data: { id: taxpayerId },
                success: function (response) {
                    if (response) {
                        $('#deleteModal').hide();
                        $('#deleteModalOverlay').hide();

                        hideProcessing();
                        window.location.reload();
                    }
                }
            });
        }
    });

    $('#btnSearch').click(function () {
        search();
    });

    $('#ReceiverFilter').keypress(function (e) {
        if (e.keyCode === 13)
            search();
    });

    function search() {
        showProcessing();
        var keyword = $('#ReceiverFilter').val();
        $.ajax({
            url: urlSearchReceivers,
            type: "GET",
            cache: false,
            dataType: "html",
            data: { keyword: keyword },
            success: function (receiversTablePartialView) {
                if (receiversTablePartialView) {
                    $("#divTablePartialView").html(receiversTablePartialView);
                    hideProcessing();
                    $(".btnOpenDeleteModal").click(openDeleteModal);
                }
            }
        });
    }
});


function openDeleteModal() {
    let invoiceTaxpayerId = $(this).data('taxpayerId');
    let taxpayer = $(this).data('taxpayer');
    let name = $(this).data('name');

    $('#taxpayerField').html(taxpayer);
    $('#nameField').html(name);
    $('#btnDelete').data('taxpayerId', invoiceTaxpayerId);

    $('#deleteModal').show();
    $('#deleteModalOverlay').show();
}