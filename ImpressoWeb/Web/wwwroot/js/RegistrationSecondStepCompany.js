jQuery(document).ready(function () {
    $("#FileInput").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#UserPhoto').attr('src', e.target.result);
            }

            reader.readAsDataURL(this.files[0]);
        }
    });
});