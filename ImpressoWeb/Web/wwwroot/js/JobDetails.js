jQuery(document).ready(function () {
    $(".connect-button-person-modal").click(function () {
        var userName = $(this).attr("data-user-name");
        var userId = $(this).attr("data-user-id");
        openConnectPersonClick(userName, userId);
    });

    $("#FilterNameInput").bind("keyup", function () {
        var value = $("#FilterNameInput").val().toUpperCase();

        $(".applicants .person-item").each(function () {
            if ($(this).find(".user-name").text().toUpperCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    });
});