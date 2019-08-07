$(function () {
    $("#signup").click(function () {
        $("#signin_panel").hide();
        $("#signup_panel").show();
    });

    $("#signin").click(function () {
        $("#signup_panel").hide();
        $("#signin_panel").show();
    });

    $("#articles").on("click", "a.openLink", function () {
        $.post("Home/IncreaseView", {
            id: $(this).attr("val")
        }, function () {});
    });

    function getNews(page) {
        $.post("/Home/GetArticles", { pageIndex: page })
            .done(function (data) {
                if(page > 1)
                    $("#articles").append(data);
                else
                    $("#articles").html(data);
            });
    }

    var counter = 1;
    var timeout;

    getNews(counter);

    $(window).scroll(function () {
        clearTimeout(timeout);
        timeout = setTimeout(function () {
            var max = $(document).height() - $(window).height();
            var currentPos = $(document).scrollTop();

            var increase;
            if (counter < 2)
                increase = 0.5;
            else if (counter >= 2 && counter < 4)
                increase = 0.8;
            else
                increase = 0.9;

            if (currentPos > max * increase) {
                getNews(++counter);
            }
        }, 50);
    });
});