$(function () {
    $("#signup").click(function () {
        $("#signin_panel").hide();
        $("#signup_panel").show();
    });

    $("#signin").click(function () {
        $("#signup_panel").hide();
        $("#signin_panel").show();
    });
});