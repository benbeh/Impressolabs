jQuery(document).ready(function () {
    document.body.style.paddingBottom = $(".footer-fluid").height() + "px";

    $(".close-menu-image").bind("click", function () {
        $(".side-menu").hide();
    });

    $(".header-menu .more-menu").bind("click", function () {
        $(".side-menu").show();
    });

    $.get("/Home/GetCurrentCompany", function (data) {
        if (data.photo == null)
            data.photo = "../images/Profile.jpg";
        else
            data.photo = data.photo;

        $(".side-menu .company-photo-image").attr("src", data.photo);
        $(".side-menu .name").text(data.name);
        $(".side-menu .industry").text(data.industry);
    });
});