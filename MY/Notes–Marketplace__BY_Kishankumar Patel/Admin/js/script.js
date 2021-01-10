/*==================
    Home page  js  
=================*/
function sticky_header() {
    var header_height = jQuery('.site-header').innerHeight() / 2;
    var scrollTop = jQuery(window).scrollTop();;
    if (scrollTop > header_height) {
        jQuery('body').addClass('sticky-header');
    } else {
        jQuery('body').removeClass('sticky-header');
    }
}

jQuery(document).ready(function () {
    sticky_header();
});

jQuery(window).scroll(function () {
    sticky_header();
});
jQuery(window).resize(function () {
    sticky_header();
});
/*==================
   end of Home page  js  
=================*/
/*==================
   Search  page  js  
=================*/

// Material Select Initialization
$(document).ready(function () {
    $('.mdb-select').materialSelect();
});

/*==================
end   Search  page  js  
=================*/
/* ================================
        Navigation Section Is Here
================================*/

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

$(".toggle-arrow").click(function () {
    $(this).toggleClass("fa-chevron-down fa-chevron-up");
});

/* =============================
        end Navigation Section Is Here
==============================*/
/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */

function my_drop() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}


// top nav link dasbord drop section is here
//============================================
function my_drop_Notes() {
    document.getElementById("myDropdown_Notes").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

//============================================
//    Report link
//============================================
function my_drop_Reports() {
    document.getElementById("myDropdown_Reports").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

//============================================
/* =============================
        FAQ Is Here
==============================*/
function change_faq() {

    var button_text = $('#faq_expand_btn').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn").html('+');

    }
}

//btn 02 
function change_faq_2() {

    var button_text = $('#faq_expand_btn_2').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn_2").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn_2").html('+');

    }
}
//btn 03 
function change_faq_3() {

    var button_text = $('#faq_expand_btn_3').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn_3").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn_3").html('+');

    }
}

//btn 04 
function change_faq_4() {

    var button_text = $('#faq_expand_btn_4').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn_4").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn_4").html('+');

    }
}

//btn 05 
function change_faq_5() {

    var button_text = $('#faq_expand_btn_5').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn_5").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn_5").html('+');

    }
}

//btn 06 
function change_faq_6() {

    var button_text = $('#faq_expand_btn_6').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn_6").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn_6").html('+');

    }
}

//btn 07 
function change_faq_7() {

    var button_text = $('#faq_expand_btn_7').text();
    if (String(button_text) == String("+")) {
        $("#faq_expand_btn_7").html('-');

    }
    if (String(button_text) == String("-")) {
        $("#faq_expand_btn_7").html('+');

    }
}
