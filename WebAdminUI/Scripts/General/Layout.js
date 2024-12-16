$(document).ready(function () {
    animateSideBar();
    animateHeaderBar();
    animateFooter();

    $("#academicRecordsMenuItem + ul > li > span.esg-sidebar__submenu-title").click(function (e) {
        e.stopPropagation();
        window.location.href = urlMenu;
    });

    $("#cashRecepitsItem + ul > li > span.esg-sidebar__submenu-title").click(function (e) {
        e.stopPropagation();
        window.location.href = urlSettings;
    });

    $("[data-toggle=collapse]").click(function (e) {
        e.preventDefault();
        $(this).find(".esg-icon").toggleClass("esg-icon--up").toggleClass("esg-icon--down");
    });
});

function showAbout() {
    $.ajax({
        url: aboutUrl,
        type: "GET",
        cache: false,
        dataType: "html",
        success: function (data) {
            $("#divAbout").html(data);
            $('#divAbout').show();
            $("#divOverlay").css('display', 'block');
        }
    });
}

function closeAbout() {
    $("#divAbout").css('display', 'none');
    $("#divOverlay").css('display', 'none');
}

function animateSideBar() {
    $(".esg-sidebar__menu-toggle, .esg-sidebar__menu-toggle--mobile").click(function (e) {
        e.stopPropagation();
        $(".esg-sidebar__wrapper").toggleClass("esg-is-open");
        $("#contentPage").toggleClass("content-to-right");
        $(".esg-sidebar__menu-link.esg-submenu-toggle").removeClass("esg-is-open");

    });

    $(".esg-sidebar__menu-link.esg-submenu-toggle").click(function (e) {
        e.stopPropagation();
        if (!$(".esg-sidebar__wrapper").hasClass("esg-is-open")) {
            $(".esg-sidebar__wrapper").addClass("esg-is-open");
            $("#contentPage").addClass("content-to-right");
        }
        $(this).toggleClass("esg-is-open");

    });

    $(".esg-submenu-toggle--close").click(function (e) {
        e.stopPropagation();
        $(".esg-sidebar__menu-link.esg-submenu-toggle").removeClass("esg-is-open");
    });

    $(".esg-sidebar__submenu-title").click(function (e) {
        e.stopPropagation();
    });

    $(window).click(function () {
        $(".esg-sidebar__wrapper").removeClass("esg-is-open");
        $(".esg-sidebar__menu-link.esg-submenu-toggle").removeClass("esg-is-open");
        $("#contentPage").removeClass("content-to-right");
    });
}

function animateFooter() {
    var footer = $(".esg-footer").addClass("esg-is-fixed");

    setTimeout(function () {
        footer.removeClass("esg-is-fixed");
    }, 2000);
}

function animateHeaderBar() {
    $("#NavDropdown").click(function (e) {
        e.stopPropagation();
        showAbout();
    });
}

//#region Shared
function hideProcessing() {
    $("#Processing").addClass("no-display");
    $("#Overlaydiv").addClass("no-display");
}

function showProcessing() {
    $("#Processing").removeClass("no-display");
    $("#Overlaydiv").removeClass("no-display");
}

function hidePageLoader() {
    $('#Processing').css('display', 'none');
    $('#Overlaydiv').css('display', 'none');
    $('#Overlaydiv').css('z-index', '1200');
}

function showPageLoader() {
    $('#Processing').css('display', 'block');
    $('#Overlaydiv').css('display', 'block');
    $('#Overlaydiv').css('z-index', '1200');
}

function showPageLoaderOverModal() {
    $('#Processing').css('display', 'block');
    $('#Overlaydiv').css('display', 'block');
    $('#Overlaydiv').css('z-index', '1220');
}

function hideAllPopovers() {
    $.each($("[data-popover='true']"), function (_index, value) {
        $(value).hide();
    });
}

function validateIntupField(name) {
    let isValid = true;
    if ($(`#${name}`).val() === '') {
        showError(name);
        isValid = false;
    }
    else {
        hideError(name);
    }
    return isValid;
}

function hideError(name) {
    $(`#${name}`).parent().removeClass('has-error');
    $(`#${name}Error`).css('display', 'none');
    $(`#div${name}`).removeClass('esg-has-error');
    $(`#div${name}Err`).css('display', 'none');
}

function showError(name) {
    $(`#${name}`).parent().addClass('has-error');
    $(`#${name}Error`).css('display', 'block');
    $(`#div${name}`).addClass('esg-has-error');
    $(`#div${name}Err`).css('display', 'block');
}

function enableField(name) {
    $(`#${name}`).attr('readonly', false).attr('disabled', false);
}

function disableField(name) {
    $(`#${name}`).attr('readonly', true).attr('disabled', true);
}

function redirectUnauthorized() {
    showPageLoaderOverModal();
    window.location.assign(urlErrorUnauthorized);
}

function redirectException() {
    showPageLoaderOverModal();
    window.location.assign(urlErrorException);
}
//#endregion Shared