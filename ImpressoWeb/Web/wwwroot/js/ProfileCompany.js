jQuery(document).ready(function () {

    $('.project-start-date').timeago();

    $(".project-description-form").bind("click", function() {
        $(this).submit();
    });

    $(".job-description-form").bind("click", function () {
        $(this).submit();
    });

    $("#FileInput").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.company-photo-image').attr('src', e.target.result);
            }

            reader.readAsDataURL(this.files[0]);

            // send image to back end
            var formData = new FormData();
            formData.append('file', this.files[0]);

            $.ajax({
                url: '/Profile/ChangeCompanyImage',
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false
            });
        }
    });

    $("#CreateProjectButton").bind("click", function () {
        var title = $(".project-title").val();
        if (!title)
            alert("Title can't be empty");
        else {
            $.post("CreateProject", { title: title, description: $(".project-description").val() }, function (projectId) {
                $("#CreateProjectModal").modal('hide');
                $("#CreateModal").modal();

                $("#ListOfProjects").prepend("<option value='" + projectId + "' selected>" + title + "</option>");
            });
        }
    });
});