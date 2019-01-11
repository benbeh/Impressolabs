jQuery(document).ready(function () {
    $(".connect-button-person-modal").click(function () {
        var userName = $(this).attr("data-user-name");
        var userId = $(this).attr("data-user-id");
        openConnectPersonClick(userName, userId);
    });

    $(".job-description-form").bind("click", function () {
        $(this).submit();
    });
});