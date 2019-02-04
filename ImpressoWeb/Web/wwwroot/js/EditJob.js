jQuery(document).ready(function () {

    $(".certificates .add-item").bind("click", function () {
        var content = $(".certificates .certificate-item").first().clone();
        $(content).css("display", "block");
        $(content).find(".content").attr("name", "JobCertificates");

        $(".certificates").append(content);
    });

    $(".skills .add-item").bind("click", function () {
        var content = $(".skills .skill-item").first().clone();
        $(content).css("display", "block");
        $(content).find(".content").attr("name", "Skills");

        $(".skills").append(content);
    });

    $(".update-button").bind("click", function () {
        $("#StartDateInput").val($(".mdy.start-date").val() + " " + $(".time.start-date").val());
        $("#EndDateInput").val($(".mdy.end-date").val() + " " + $(".time.end-date").val());
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

    // autocomplete
    $.get("GetJobsLocations", function (data) {
        $(".content-location").autocomplete({
            source: data
        });
    });
});

function CloseCertificateSelectButtonClick(object) {
    $(object).closest(".certificate-item").remove();
}

function CloseSkillSelectButtonClick(object) {
    $(object).closest(".skill-item").remove();
}