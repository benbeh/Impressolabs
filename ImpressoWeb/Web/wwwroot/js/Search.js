jQuery(document).ready(function () {
    $(".connect > .connect-person-modal-button").click(function () {
        var userName = $(this).attr("data-user-name");
        var userId = $(this).attr("data-user-id");
        openConnectPersonClick(userName, userId);
    });

    $("#FilterNameInput").bind("keyup", function () {
        var value = $("#FilterNameInput").val().toUpperCase();

        $("#ListCards .person-card").each(function () {
            if ($(this).find(".person-name").text().toUpperCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    });
});