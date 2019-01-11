jQuery(document).ready(function () {

    $(".update-button").bind("click", function () {
        $("#StartDateInput").val($(".mdy.start-date").val() + " " + $(".time.start-date").val());
        $("#EndDateInput").val($(".mdy.end-date").val() + " " + $(".time.end-date").val());
    });

    $(".open-event").click(function () {
        window.location.href = "/Event/EventDetails?eventId=" + $(this).attr("data-form-id");
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

    $("#PhotoFile").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            var filename = $('#PhotoFile').val();
            $('#NameEventPhoto').text(filename);

            reader.onload = function (e) {
                $('.name-event-photo').attr('src', e.target.result);
            }

            reader.readAsDataURL(this.files[0]);          
        }
    });

});