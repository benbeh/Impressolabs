function openConnectPersonClick(userName, userId) {
    $(".invitation-text>.user-name").text(userName);
    $("#ChosenConnectUserId").val(userId);
    $.ajax({
        type: "GET",
        url: "/Relationship/GetCurrentCompanyJobsNotConnectedWithUser",
        data: { userId: userId },
        success: function (result) {
            var select = $(".job-dropdown>select");
            $(select).find("option").remove();
            jQuery(result).each(function (i, item) {
                $(select).append("<option value='" + item.id + "'>" + item.title + "</option>");
            });
        },
        error: function (response, textStatus, errorThrown) {
            console.log("Relationship.GetCurrentCompanyJobsThatNotConnectedWithUser: some problem happened. ErrorMessage: " + textStatus + " ErrorThrown:" + errorThrown);
        }
    });
}