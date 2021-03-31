$(function () {
    $(".drawer-menu>a.drawer-menu-controller").click(function (e) {
        e.preventDefault();
        if ($(this).parent(".drawer-menu").hasClass("drawer-menu-active") == true) {
            $(this).parent(".drawer-menu").removeClass("drawer-menu-active");
            $(this).children("i").addClass("glyphicon-menu-right");
            $(this).children("i").removeClass("glyphicon-menu-left");
            $(".sub-menu-items").hide();
            SetCookie("IsMenuOpen", 0, 1);
        }
        else {
            $(this).parent(".drawer-menu").addClass("drawer-menu-active");
            $(this).children("i").removeClass("glyphicon-menu-right");
            $(this).children("i").addClass("glyphicon-menu-left");
            SetCookie("IsMenuOpen", 1, 1);
        }
    });
    $(".sub-menu").click(function (e) {
        e.preventDefault();
        var target = $(this).data("target");

        if ($(this).parent(".drawer-menu").hasClass("drawer-menu-active") == true && $(target).is(":visible") == true) {
            $(target).hide();
            SetCookie("IsMenuOpen", 0, 1);
        }
        else{
            $(this).parent(".drawer-menu").addClass("drawer-menu-active");
            $(".drawer-menu>a.drawer-menu-controller").children("i").removeClass("glyphicon-menu-right");
            $(".drawer-menu>a.drawer-menu-controller").children("i").addClass("glyphicon-menu-left");
            $(target).show();
            SetCookie("IsMenuOpen", 1, 1);
        }

    });
    $(".drawer-menu a").each(function () {
        var location = document.location.href.toLowerCase().replace("#","");
        var anchorHref = $(this).prop("href").toLowerCase().replace("#","");
        if (location == anchorHref && $(this).hasClass("drawer-menu-controller") == false && $(this).hasClass("sub-menu")==false) {
            $(this).addClass("active");
        }
    });

    $('[data-toggle="tooltip"]', ".drawer-menu").tooltip();
    
});


function SetCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function GetCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}