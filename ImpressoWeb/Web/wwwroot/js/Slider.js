var title =
[
    'Branding You',
    'Smart Profile & CV',
    'Quality Networking',
    'Quality Networking'
];
var bacgrounds = ["../images/slider/1.png", "../images/slider/2.png", "../images/slider/3.png", "../images/slider/4.png"];
var contents =
[
    '"It\'s About You - The next step of Professional Networking"',
    '"Be the first to have a trusted and secure online profile"',
    '"A Win-Win Connection"',
    '"A Win-Win Connection"'
];
var numberOfSlider = 0;

jQuery(document).ready(function () {
    $(".change-slide").bind("click", function () {

        if (numberOfSlider === 3) {
            // go to sign up
            var url = "RegistrationFirstStep";
            window.location.href = url;
            return;
        }

        // remove active from this dot
        $($(".slider-number-of-page")[numberOfSlider]).removeClass("active");

        // go to next slider
        numberOfSlider++;

        // change slider properties
        $(".slider").css("background-image", "url(" + bacgrounds[numberOfSlider] + ")");
        $(".title").text(title[numberOfSlider]);
        $(".content").text(contents[numberOfSlider]);
        $($(".slider-number-of-page")[numberOfSlider]).addClass("active");
    });
});