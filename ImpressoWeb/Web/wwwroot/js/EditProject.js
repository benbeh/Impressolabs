jQuery(document).ready(function () {
    $("#ChangeProjectNameInput").bind("keyup", function () {
        $(".project-header-text").text($(this).val());
    });
});