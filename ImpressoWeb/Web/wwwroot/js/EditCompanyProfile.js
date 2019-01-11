jQuery(document).ready(function () {

    $("#PhotoFile").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.company-image img').attr('src', e.target.result);
            }

            reader.readAsDataURL(this.files[0]);
        }
    });

    // autocomplete
    $.get("GetCompaniesLocations", function (data) {
        $(".company-location-text").autocomplete({
            source: data
        });
    });

});