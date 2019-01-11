$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: "/Filter/GetFilters",
        contentType: "application/json",
        success: function (result) {

            $("#IndustryTags").tagit({
                availableTags: result["industries"],
                showAutocompleteOnFocus: true,
                fieldName: "industries"
            });

            $("#JobTypeTags").tagit({
                availableTags: result["jobTypes"],
                showAutocompleteOnFocus: true,
                fieldName: "jobTypes"
            });

            $("#SkillsTags").tagit({
                availableTags: result["skills"],
                showAutocompleteOnFocus: true,
                fieldName: "skills"
            });

            $("#ExperienceTags").tagit({
                availableTags: result["experience"],
                showAutocompleteOnFocus: true,
                fieldName: "experience"
            });

            $("#CertificatesTags").tagit({
                availableTags: result["certificates"],
                showAutocompleteOnFocus: true,
                fieldName: "certificates"
            });

            $("#CompanyTags").tagit({
                availableTags: result["companyNames"],
                showAutocompleteOnFocus: true,
                fieldName: "companyNames"
            });

        },
        error: function (response, textStatus, errorThrown) {
            console.log("Person.GetFilterValues: some problem happened. ErrorMessage: " + textStatus + " ErrorThrown:" + errorThrown);
        }
    });

    // autocomplete
    $.get("/Filter/GetPeopleLocations", function (data) {
        $(".location").autocomplete({
            source: data
        });
    });

});