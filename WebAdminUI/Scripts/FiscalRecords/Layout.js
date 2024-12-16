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

function showFormError(name) {
    $(`#${name}Icon`).show();
    $(`#${name}Group`).addClass('esg-has-error');
}

function hideFormError(name) {
    $(`#${name}Icon`).hide();
    $(`#${name}Group`).removeClass('esg-has-error');
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

function hideProcessing() {
    $("#Processing").addClass("no-display");
    $("#Overlaydiv").addClass("no-display");
}

function showProcessing() {
    $("#Processing").removeClass("no-display");
    $("#Overlaydiv").removeClass("no-display");
}

function hidePageLoader() {
    $('#Processing').hide();
    $('#Processing').css('z-index', '1220');
    $('#Overlaydiv').hide();
    $('#Overlaydiv').css('z-index', '1220');
}

function showPageLoader() {
    $('#Processing').show();
    $('#Processing').css('z-index', '1220');
    $('#Overlaydiv').show();
    $('#Overlaydiv').css('z-index', '1220');
}

$.fn.clearValidation = function () { var v = $(this).validate(); $('[name]', this).each(function () { v.successList.push(this); v.showErrors(); }); v.resetForm(); v.reset(); };

$(document).ready(function () {
    animateSideBar();
    animateHeaderBar();
    animateFooter();

    $("#fiscalRecordsMenuItem + ul > li > span.esg-sidebar__submenu-title").click(function (e) {
        e.stopPropagation();
        window.location.href = urlMenu;
    });

    $("#fiscalRecordsSettingsItem + ul > li > span.esg-sidebar__submenu-title").click(function (e) {
        e.stopPropagation();
        window.location.href = urlSettings;
    });

    $("[data-toggle=collapse]").click(function (e) {
        e.preventDefault();
        $(this).find(".esg-icon").toggleClass("esg-icon--up").toggleClass("esg-icon--down");
    });
});