function openSavePersonForProjectClick(userName, userIndustry, userLocation, userImage, userId) {
    if (userName !== null)
        $(".user-information>.user-name").text(userName);
    else 
        $(".user-information>.user-name").text("");
    if (userIndustry !== "None")
        $(".user-information>.user-industry").text(userIndustry);
    else 
        $(".user-information>.user-industry").text("");
    if (userLocation !== null)
        $(".user-information>.user-location").text(userLocation);
    else 
        $(".user-information>.user-location").text("");

    $(".user>.user-image").attr("src", userImage);
    $("#ChosenSaveUserId").val(userId);

    $.ajax({
        type: "GET",
        url: "/Relationship/GetCurrentCompanyProjectsNotSavedWithUser",
        data: { userId: userId },
        success: function (result) {
            var select = $(".project-dropdown>select");
            $(select).find("option").remove();
            jQuery(result).each(function (i, item) {
                $(select).append("<option value='" + item.id + "'>" + item.name + "</option>");
            });
        },
        error: function (response, textStatus, errorThrown) {
            console.log("Relationship.GetCurrentCompanyProjectsNotSavedWithUser: some problem happened. ErrorMessage: " + textStatus + " ErrorThrown:" + errorThrown);
        }
    });
}