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
                increase = 0.4;
            else if (counter >= 2 && counter < 4)
                increase = 0.8;
            else
                increase = 0.9;

            if (currentPos > max * increase) {
                getNews(++counter);
            }
        }, 50);
    });    $("#toggle_weather").click(function () {        $("#weather").toggle();
    });    $("#myInput").keyup(function () {
        var value = $(this).val().toLowerCase();

        if (value == "") {
            $('#weather a').hide().filter(':lt(0)').show();
        }
        else {
            $("#weather a").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });

            $('#weather a:visible').hide().filter(':lt(7)').show();
        }
    });    // hide all a and then show the first two
    $('#weather a').hide().filter(':lt(0)').show();

    function getWeather(city) {
        $.get("https://api.openweathermap.org/data/2.5/weather?q=" + city + "&APPID=c16e6afdfb6e410963c75506963bc8fb&units=metric", function (data) {

            $("#deg1").html(parseInt(data.main.temp))
            $("#weather_desc").html(data.weather[0].description);
            var daysWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            var date = new Date(parseInt(data.dt) * 1000);
            var ampm = date.getHours() >= 12 ? "PM" : "AM";
            $("#weather_date").html(daysWeek[date.getDay()] + ", " + date.getHours() + ":" + date.getMinutes() + " " + (ampm));
            $("#weather_humidity").html(data.main.humidity);
            $("#weather_winds").html(data.wind.speed);
            $("#weather_title").html(city);
            $("#weather_country").html(data.sys.country == "PS" ? "IL" : data.sys.country);

            $("#show_weather").show();
        });    }    getWeather("Holon");
    $('#weather a').click(function () {
        getWeather($(this).html());
        $("#weather").hide();
    });

    $("#selectLocation").click(function () {
        $("#locationSelected").toggle();
    });

    $(document).click(function () {
        //if ($(this).attr('id') != "weather" )
            //$("#weather").hide();

        if ($(this).attr('id') != "locationSelected")
            $("#locationSelected").hide();
    });

    $("#locationSelected a").click(function () {
        $("#gmap_canvas").attr("src", "https://maps.google.com/maps?q="+$(this).html()+"&t=&z=13&ie=UTF8&iwloc=&output=embed");
    });

    $.get("https://graph.facebook.com/v4.0/me?fields=id%2Cname%2Cposts&access_token=EAAdb2ZBoZCj40BAL6ZAvdAbnlHaahsvIiOPHmYFGoE9IZAYlLgOlIGJ0PPIDrM1n2M7q07ITekb8ZByNZBglDKCGDatkyFxXBaZBDpTvJNxf5akrh8PonwxPgnwUQ0hVLnABSoO60kuNXJv0pTIuuopd6f0wKDY4ZCNgBFPFuW606XmFpMo13xe7ctFlUrkjt6EZD", function (data) {
        var size = data.posts.data.length > 3 ? 3 : data.posts.data.length;
        for (var i = 0; i < size; i++) {
            var date = new Date(parseInt(data.posts.data[i].created_time));
            var string =
                '<div class="card text-white bg-primary mb-3" style="border-radius: 3px; max-width: 18rem; margin-left: 20px;">' +
                '<div class="card-body">' +
                '<h5 class="card-title">A post from facebook</h5>' +
                '<p class="card-text">“ ' + data.posts.data[i].message + ' ”</p>'+
                    '</div >'+
                '</div>';

            if (i == 0) {
                $("#facebook_posts").html(string);
            }
            else {
                $("#facebook_posts").append(string);
            }
        }
    });

});