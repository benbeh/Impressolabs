jQuery(document).ready(function () {

    $(".open-feed").each(function () {
        $(this).bind("click", function () {
            $("#" + $(this).attr("data-form-id")).submit();
        });
    });

    $(".more-menu-popover").each(function () {
        $(this).bind("mouseleave", function () {
            $(this).hide();
        });
        $(this).bind("click", function (e) {
            e.stopPropagation();
        });
    });

    $(".more-menu").each(function () {
        $(this).bind("click", function (e) {
            $("#" + $(this).attr("data-popover-id")).show();
            e.stopPropagation();
        });
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
            var personCard = $(this).closest(".open-feed");
            var userName = $(this).attr("data-user-name");
            var userId = $(this).attr("data-user-id");
            var userIndustry = $(this).attr("data-user-industry");
            var userLocation = personCard.find(".person-location").text();
            var userImage = personCard.find(".person-image > img").attr("src");
            openSavePersonForProjectClick(userName, userIndustry, userLocation, userImage, userId);
            return false;
        });
    });

    $(".hide-profile-button").each(function () {
        $(this).bind("click", function (e) {
            // block user
            var userId = $(this).attr("data-user-id");
            var openFeed = $("#" + $(this).attr("data-open-feed-id"));
            var closeFeed = $("#" + $(this).attr("data-closed-feed-id"));
            $.ajax({
                type: "POST",
                url: "/Relationship/BlockUser",
                data: { userId: userId },
                success: function (result) {
                    openFeed.switchClass("shown", "closed", 1);
                    closeFeed.switchClass("closed", "shown", 1);
                },
                error: function (response, textStatus, errorThrown) {
                    console.log("Relationship.BlockUser: some problem happened. ErrorMessage: " + textStatus + " ErrorThrown:" + errorThrown);
                }
            });
        });
    });

    $(".undo-hiding-feed-button").each(function () {
        $(this).bind("click", function (e) {
            // unblock user
            var userId = $(this).attr("data-user-id");
            var openFeed = $("#" + $(this).attr("data-open-feed-id"));
            var closeFeed = $("#" + $(this).attr("data-closed-feed-id"));
            $.ajax({
                type: "POST",
                url: "/Relationship/UnblockUser",
                data: { userId: userId },
                success: function (result) {
                    openFeed.switchClass("closed", "shown", 1);
                    closeFeed.switchClass("shown", "closed", 1);
                },
                error: function (response, textStatus, errorThrown) {
                    console.log("Relationship.UnblockUser: some problem happened. ErrorMessage: " + textStatus + " ErrorThrown:" + errorThrown);
                }
            });
        });
    });

    $(".close-feed-anchor").each(function () {
        $(this).bind("click", function (e) {
            $("#" + $(this).attr("data-closed-feed-id")).switchClass("shown", "closed", 1);
        });
    });

});