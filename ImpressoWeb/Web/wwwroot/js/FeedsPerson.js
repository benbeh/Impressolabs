$(".show-more a").on("click", function () {
    var $this = $(this);
    var $content = $this.parent().prev("div.content");
    var linkText = $this.text().toUpperCase();

    if (linkText === "MORE") {
        linkText = "less";
        $content.switchClass("hideContent", "showContent", 800);
    } else {
        linkText = "more";
        $content.switchClass("showContent", "hideContent", 800);
    };

    $this.text(linkText);
});

$(".connect-button-person-modal").each(function () {
    $(this).bind("click", function () {
        $("#ConnectPersonModal").modal();
        var userName = $(this).attr("data-user-name");
        var userId = $(this).attr("data-user-id");
        openConnectPersonClick(userName, userId);
        return false;
    });
});

$(".connected-button-person-modal").each(function () {
    $(this).bind("click", function () {
        return false;
    });
});

$(".save-for-project-button").each(function () {
    $(this).bind("click", function () {
        $("#SavePersonForProjectModal").modal();
        var profileCard = $(this).closest(".profile-card");
        var userName = $(this).attr("data-user-name");
        var userId = $(this).attr("data-user-id");
        var userIndustry = $(this).attr("data-user-industry");
        var userLocation = profileCard.find(".person-location-text").text();
        var userImage = profileCard.find(".person-image > img").attr("src");
        openSavePersonForProjectClick(userName, userIndustry, userLocation, userImage, userId);
        return false;
    });
});

$(".verify-button").bind("click", function () {
    var testimonial = $(this).closest(".content");
    var verifyButton = $(this);

    if ((verifyButton).hasClass("verified-button"))
        return;

    $.post("VerifyTestimonial", { testimonialId: $(this).attr("data-testimonial-id") }, function (data) {
        // change class
        $(verifyButton).switchClass("verify-button", "verified-button");
        // change button name
        $(verifyButton).text("verified");
        // change amount of peple
        var amountOfPeopleWhoVerified = parseInt(testimonial.find(".verify-amount-of-people-number").text());
        testimonial.find(".verify-amount-of-people-number").text(amountOfPeopleWhoVerified + 1);

        if ($(testimonial).find(".verify-image-person-2").length == 0) {
            $.get("GetCurrentUserPhoto", function (data) {
                if (data == null)
                    data = "../images/Profile.jpg";

                for (var i = 0; i < 3; i++) {
                    if ($(testimonial).find(".verify-image-person-" + i).length == 0) {
                        $(testimonial).find(".verify-images").append("<img src='" + data + "' alt='user image' class='rounded-circle verify-image-person-" + i + "'/>");
                        break;
                    }
                }
            });
        }
    });

});