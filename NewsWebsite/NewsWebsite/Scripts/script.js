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
                data = JSON.parse(data);
                $("#articles-loading").hide();
                var len = data.length;
                var list = [
                    data.slice(0, len / 3),
                    data.slice(len / 3, 2 * len / 3),
                    data.slice(2 * len / 3, len)
                ];

                for (var i = 0; i < list.length; i++) {
                    for (var j = 0; j < list[i].length; j++) {
                        var row = '<article class="card mb-4">' +
                            '<header class="card-header">' +
                            '<div class="card-meta">' +
                            '<a href="#"><time class="timeago" datetime="' + list[i][j][0] + '">' + list[i][j][1] + '</time></a> in <a href="/Search/Index?Category=' + list[i][j][2] + '">' + list[i][j][3] + '</a>' +
                            '</div>' +
                            '<a href="#">' +
                            '<h4 class="card-title">' + list[i][j][5] + '</h4>' +
                            '</a>' +
                            '</header>';

                        if(list[i][j][8] == "Normal")
                            row = row + '<a href="post-image.html">' +
                            '<img class="card-img" src="' + list[i][j][6] + '" alt="">' +
                                '</a>';
                        else
                            row = row + '<a href="post-image.html" style="overflow: hidden;">' +
                                '<img class="card-img" src="' + list[i][j][6] + '" alt="" style="margin-left: 10px;">' +
                                '</a>';

                        row = row + '<div class="card-body">' +
                            '<p class="card-text">' + list[i][j][7] + '</p>' +
                            '</div>' +
                            '</article>';

                        $("#articles-col" + (i + 1)).append(row);
                    }
                }

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
    });

    $(document).click(function (event) {
        $target = $(event.target);
        if (!$target.closest('#weather').length &&
            $('#weather').is(":visible")) {
            $('#weather').hide();
        }
    });

    $("#toggle_weather").click(function () {
        //$("#weather").show();
        setTimeout(function (e) {
            $('#weather').show();
        }, 20);
    });

    $("#myInput").keyup(function () {
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
    });

    // hide all a and then show the first two
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
        });
    }

    getWeather("Holon");

    $('#weather a').click(function () {
        getWeather($(this).html());
        $("#weather").hide();
    });

    $("#selectLocation").click(function () {
        $("#locationSelected").show();
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

    $.get("https://graph.facebook.com/v4.0/me?fields=id%2Cname%2Cposts&access_token=EAAdb2ZBoZCj40BALKHJZBI4oja7Bd58d9MuQymZBbMkshJaomU1UySyf9iJoI9AM6BejZASxMuZAyC4FbiEAFoZBVNAdnJXaZBK3sO4F3MW8hhdJsD0rJJVb5HSTv1vmuItPhySXdQqyJ5vLiYcriZA1MnLXjGHmHJZBKUaEEQoommrA1DpPZAME1EPOsJtAPfF1vgZD", function (data) {
        var size = data.posts.data.length > 3 ? 3 : data.posts.data.length;
        for (var i = 0; i < size; i++) {
            //var date = new Date(parseInt(data.posts.data[i].created_time));
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