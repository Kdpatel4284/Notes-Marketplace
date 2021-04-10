/*======================================
|   |   |   |   Mobile Menu
======================================*/
$(function () {
    // Show mobile nav
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });
    // Hide mobile nav
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {
        $("#mobile-nav").css("height", "0%");
    });
});


/* //new

/*======================================
|   |   |   |   Navigation
======================================
$(function () {
    showHideNav();
    $(window).scroll(function () {
        //show hide nav on windows scroll
        showHideNav();
    });

    function showHideNav() {
        if ($(window).scrollTop() > 50) {
            //show white nav
            $("nav").addClass("white-nav-top");
            //show dark logoimages/pre-login/logo.png
            $(".sticky-logo img").attr("src", "~/Content/Front_Content/images/pre-login/logo.png");
            //Show back-to-top Button
            $("#back-to-top").fadeIn();
        } else {
            //hide white nav 
            $("nav").removeClass("white-nav-top");
            $(".sticky-logo img").attr("src", "~/Content/Front_Content/images/pre-login/top-logo.png");
            $("#back-to-top").fadeOut();
        }
    }
});
// Smooth Scrolling
$(function () {
    $("a.smooth-scroll").click(function (event) {
        event.preventDefault();
        // get section id like #about, #services, #work, #team and etc.
        var section_id = $(this).attr("href");
        $("html, body").animate({
            scrollTop: $(section_id).offset().top - 68
        }, 1250, "easeInOutExpo");
    });
});


//end new */

/*======================================
|   |   |   |   Navigation
======================================*/


$(function () {
    showHideNav();
    $(window).scroll(function () {
        //show hide nav on windows scroll
        showHideNav();
    });

    function showHideNav() {
        if ($(window).scrollTop() > 50) {
            //show white nav
            $("nav").addClass("white-nav-top");
            //show dark logoimages/pre-login/logo.png
            $(".sticky-logo img").attr("src", "/Content/Front_Content/images/pre-login/logo.png");
            //Show back-to-top Button
            $("#back-to-top").fadeIn();
        } else {
            //hide white nav 
            $("nav").removeClass("white-nav-top");
            $(".sticky-logo img").attr("src", "/Content/Front_Content/images/pre-login/top-logo.png");
            $("#back-to-top").fadeOut();
        }
    }
});
// Smooth Scrolling
$(function () {
    $("a.smooth-scroll").click(function (event) {
        event.preventDefault();
        // get section id like #about, #services, #work, #team and etc.
        var section_id = $(this).attr("href");
        $("html, body").animate({
            scrollTop: $(section_id).offset().top - 68
        }, 1250, "easeInOutExpo");
    });
});
