//Current date NOT USER FOR NOW
var date = $.datepicker.formatDate('yy-mm-dd', new Date());
//global variables
var carId;
var isTrue = true;
var eventId = "";
var closeLogin = "";
//Global variable End

$(document).ready(function (ev) {
    $.getJSON('fastlane/Book/getBrand', function (result) {

        $.each(result, function (row, field) {
            $("#brand").append("<option value=" + field.Brand + ">" + field.Brand + "</option>")
        })
    })
});


$(document).on("change", "#brand", function (ev) {
    var brandVal = $("#brand").val();
    $.getJSON('fastlane/Book/byBrand', {Brand:brandVal}, function (result) {

        $.each(result, function (row, field) {
            
            $("#model").empty().append("<option disabled selected value=''>Select model</option><option value='" + field.Model + "'>" + field.Model + "</option>");
            $("#plate").empty().append("<option disabled selected value=''>Select plate</option><option value='" + field.Plate + "'>" + field.Plate + "</option>");
            $("#power").empty().append("<option disabled selected value=''>Select power</option><option value='" + field.Power + "'>" + field.Power + "</option>");
            $("#color").empty().append("<option disabled selected value=''>Select color</option><option value='" + field.Color + "'>" + field.Color + "</option>");
            $("#engine").empty().append("<option disabled selected value=''>Select engine</option><option value='" + field.Engine + "'>" + field.Engine + "</option>");
        
        })
    })
});

$.fn.scrollBottom = function () {
    return $(document).height() - this.scrollTop() - this.height();
};


$(document).on("click", "#search", function (ev) {
    $("#book-result").empty();
    var brand = $("#brand").val();
    if (brand == "null") { brand = null; }
    var model = $("#model").val();
    var engine = $("#engine").val();
    var power = $("#power").val();
    var color = $("#color").val();
    var plate = $("#plate").val();
    var from = $("#From").val();
    var to = $("#To").val();
    if ( to == "" && from=="") {
        $("#modal-date-error").modal("show");
        $("#error-result").html("<p class='error'>Enter from-to date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    if (from == "") {
        
        $("#modal-date-error").modal("show");
        $("#error-result").html("<p class='error'>Enter from date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    if (to == "") {

        $("#modal-date-error").modal("show");
        $("#error-result").html("<p class='error'>Enter to date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    if ((new Date(from).getTime() > new Date(to).getTime())) {
        $("#modal-date-error").modal("show");
        $("#modal-date-error").css("z-index", "1500");

        $("#error-result").html("<p class='error'>Enter valid date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    $.getJSON('fastlane/Book/Search', {
        Brand: brand,
        Model: model,
        Color: color,
        Engine: engine,
        Power: power,
        Plate: plate,
        From: from,
        To:to
    }, function (result) {

        if (result.length <= 0) {
            $("#book-result").append("<p class='text-center'>No cars</p>")
        }
        $.each(result, function (row, field) {
            var fieldX = from;
            var fX = new Date(fieldX);
            var fieldY = to;
            var fY = new Date(fieldY);
            var dateResult = daydiff(fX, fY);
            var strPrice = parseInt(field.Price, 10);
            var Price = Math.round(dateResult * strPrice);

           
            if(screen.width > 1280 && isTrue==true){
                $("#book-result").append("<table class='table'><thead><th><strong>Brand</strong></th><th><strong>Model</strong></th><th><strong>Engine</strong></th>" +
                "<th><strong>Power</strong></th><th><strong>Plate</strong></th><th><strong>Price</strong></th><th></th></thead><tbody id='srchRes'>");
                isTrue=false;
            }
            if (screen.width < 1280) {
                $("#book-result").append("<div class='table-responsive'><table class='table'><tr><td><strong>Brand</strong> : " + field.Brand + "</td></tr><tr><td><strong>Model</strong> : " + field.Model + "</td></tr><tr><td><strong>Engine</strong> : " + field.Engine + "</td></tr>" +
                "<tr><td><strong>Power</strong> : " + field.Power + "</td></tr><tr><td><strong>Color</strong> : " + field.Color + "</td></tr><tr><td><strong>Plate</strong> : " + field.Plate + "</td><tr><td><strong>Price :</strong> $ "+Price+"</td></tr><td><button id='car-" + field.idCar + "' type='button' class='rent-btn btn btn-primary btn-xs'>Rent it</button>" +
                "</td><tr></table></div>");
            } else {
                 
            $("#srchRes").append("<tr><td >" + field.Brand + "</td><td> " + field.Model + "</td><td>" + field.Engine + "</td>" +
                "<td>" + field.Power + "</td><td>" + field.Plate + "</td><td>$ " + Price + "</td><td><button id='car-" + field.idCar + "' type='button' class='rent-btn btn btn-primary btn-xs'>Rent it</button></td></tr>");
           
            }
  
        });
        if(isTrue==false){
            $("#srcRes").append("</tbody></table>");
            isTrue = true;
    }
        
        $("#modal-book-car").scrollBottom();
    })
});

//hide modal-Explicit
$(document).on("click", "#bookFalse", function () {
    $("#modal-Excplicit").modal("hide");
});

//Sign Out function
$(document).on("click", "#signOut", function () {
    location.reload();
});
/*Rent car by btn if no user is logged login pop up should appear*/

$(document).on("click", '.rent-btn', function (ev) {

    var userVal = $(".username").html();

    if (userVal == "My Account") {

        $("#modal-account").modal("show");
        eventId = ev.target.id;

    } else {
        carId = ev.target.id;
        $("#modal-Excplicit").modal("show");
    }
});


$(document).on("click", '#bookTrue', function (ev) {
    var userVal = $(".username").html();
    var from = $("#From").val();
    var to = $("#To").val();
    var user = $('#accId').attr("data-userid");

    carId = carId.replace("car-", "");


    if (userVal != "My Account") {
        $.post("fastlane/Book/Insert", {
            CarId: carId,
            RentDate: from,
            ReturnDate: to,
            UserId: user

        }, function (result) {
            $("#book-result").empty();
            $("#modal-Excplicit").modal("hide");
            $("#modal-book-success").modal("show");
            setTimeout(function () { $("#modal-book-success").modal("hide"); }, 3500)

        });
    }
});
//Date Difference

function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[0] - 1, mdy[1]);
}


function parseDateHistory(str) {
    var mdy = str.split('-');
    return new Date(mdy[2], mdy[0] - 1, mdy[1]);
}


function daydiff(first, second) {
    return (second - first) / (1000 * 60 * 60 * 24);
}

//My Account Modal START
$(document).on("click touch", "#login-History", function () {
    $("#x").empty();
    $("#srcRes").empty();
    if (lastClick == "#login-History") {
        return false;
    } else {
        lastClick = "#login-History";

        $("#result").html("<div class='row'> <div class='col-xs-4'>From: <span class='glyphicon glyphicon-calendar'></span><input type='text'  id='historyFrom'" +
            "class='datepicker input-sm'></div><div class='col-xs-4'>To: <span class='glyphicon glyphicon-calendar'></span><input type='text'  id='historyTo' class='datepicker input-sm'></div></div>" +
            "<button  id='srcHistory' class='btn btn-primary btn-sm' value='Search'>Search</button>");
        $(".datepicker").datepicker({ dateFormat: 'yy-mm-dd' });


    }
});

//LOGIN SUMMARY
$(document).on("click touch", "#login-Summary", function () {
    if (lastClick == "#login-Summary") {
        return false;
    } else {
        lastClick = "#login-Summary";
        $("#result").empty();
        var from = $.datepicker.formatDate('yy-mm-dd', new Date());
        
        var user = $('#accId').attr("data-userid");

        $.getJSON("/fastlane/History/Summary", { From: from, IdUser: user }, function (result) {
            $("#srcRes").empty();
            if (result.length <= 0) {
                $("#srcRes").append("<p class='text-center'>No cars</p>");
                return false;
            }
            $("#srcRes").append("<table class='table'><thead><th><strong>Brand</strong></th><th><strong>Price</strong></th><th><strong>Rent</strong></th>" +
                "<th><strong>Return</strong></th></thead><tbody id='xRes'>");
            $.each(result, function (row, field) {
                var strPrice = parseInt(field.Price, 10);
                var fieldX = field.From;
                var x = fieldX.replace(" 12:00:00 AM", "");
                var fX = parseDate(x);
                var fieldY = field.To;
                var y = fieldY.replace(" 12:00:00 AM", "");
                var fY = parseDate(y);
                var dateResult = daydiff(fX, fY);

                var Price = Math.round(dateResult * strPrice);
                $("#xRes").append("<tr><td >" + field.Brand + "</td><td>$ " + Price + "</td><td>" + x + "</td>" +
                "<td>" + y + "</td></tr>")
            });
            $("#srcRes").append("</tbody></table>");
        })
    }
});
//LOGIN SUMMARY END
///GET HISTORY DATE

$(document).on("click touch", "#srcHistory", function () {

    var from = $("#historyFrom").val();
    var to = $("#historyTo").val();
    var user = $('#accId').attr("data-userid");

    if ((new Date(from).getTime() > new Date(to).getTime())) {
        $("#modal-date-error").modal("show");
        $("#modal-date-error").css("z-index", "1500");

        $("#error-result").html("<p class='error'>Enter valid date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    if (to == "" && from == "") {
        $("#modal-date-error").modal("show");
        $("#error-result").html("<p class='error'>Enter from-to date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    if (from == "") {
        $("#modal-date-error").modal("show");
        $("#error-result").html("<p class='error'>Enter from date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }
    if (to == "") {
        $("#modal-date-error").modal("show");
        $("#error-result").html("<p class='error'>Enter to date please.</p>");
        setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
        return false;
    }

    $.getJSON("/fastlane/History/History", { From: from, To: to, IdUser: user }, function (result) {
        $("#srcRes").empty();
        $("#srcRes").append("<table class='table'><thead><th><strong>Brand</strong></th><th><strong>Price</strong></th><th><strong>Rent</strong></th>" +
            "<th><strong>Return</strong></th></thead><tbody id='x'>");
        $.each(result, function (row, field) {
            var fieldX = field.From;
            var x = fieldX.replace(" 12:00:00 AM", "");
            var fX = new Date(x);
            var fieldY = field.To;
            var y = fieldY.replace(" 12:00:00 AM", "");
            var fY = new Date(y);
            var dateResult = daydiff(fX, fY);
            var strPrice = parseInt(field.Price, 10);
            var Price = Math.round(dateResult * strPrice);
            $("#x").append("<tr><td >" + field.Brand + "</td><td>$ " + Price + "</td><td>" + x + "</td>" +
            "<td>" + y + "</td></tr>")
        });
        $("#srcRes").append("</tbody></table>");
    })

});
//GET HISTORY DATE END

$(function () {
    $(".datepicker").datepicker({ dateFormat: 'yy-mm-dd' });
});


$(document).on("click touch", "#login-My-Acc", function (ev) {

    if (lastClick == "#login-My-Acc") {
        return false;
    } else {
        lastClick = "#login-My-Acc";
        $("#srcRes").empty();
        var userName = $('[name=logUser').val();
        var pass = $('[name=logPass]').val();

        $.post('/fastlane/Login/getUser/', {
            Username: userName,
            Password: pass
        }, function (result) {
            $("#result").html("<p>Username :  " + result[0].Username + "</p><br/>" +
           "<p>Email :  " + result[0].Email + "</p><br/><p>Firstname :  " + result[0].Firstname + "</p><br/>" +
           "<p>Lastname :  " + result[0].Lastname + "</p>");
        });
    }

});

//My Account modal END
$(document).on("click touch", "#LoginButton", function (ev) {
    var userName = $('[name=logUser').val();
    var pass = $('[name=logPass]').val();

    $.post('/fastlane/Login/getUser', {
        Username: userName,
        Password: pass
    }, function (result) { 
        if (result.length === 0) {
            $("#modal-date-error").css("z-index", "1500");
            $("#modal-date-error").modal("show");

            $("#error-result").html("<p class='error'>Enter correct username and password.</p>");
            setTimeout(function () { $("#modal-date-error").modal("hide"); }, 3000);
            return false;
        }
        if (userName == result.Username && pass == result.Password) {
            $('#modal-account').modal("hide");
            $('.username').text(userName);
            $('#accId').attr("data-userid", result[0].IdUser);            
            $("#accId").removeAttr('data-target').attr({ 'data-target': '#modal-login-success' });

            $("#modal-login-success").modal("show");
            lastClick = "#login-My-Acc";
            $("#result").html("<p>Username :  " + result[0].Username + "</p><br/>" +
                "<p>Email :  " + result[0].Email + "</p><br/><p>Firstname :  " + result[0].Firstname + "</p><br/>" +
                "<p>Lastname :  " + result[0].Lastname + "</p>");
        }
    });
});


function successHider() {
    $("#modal-signIn-success").modal("hide");
}


function errorHider() {
    $("#modal-signIn-error").modal("hide");
}


$(document).on("click touch", "#SignButton", function (ev) {
    var userEx = /[a-zA-Z0-9]{6,14}/;
    var passEx = /[a-zA-Z0-9]{6,14}/;
    var nameEx = /[a-zA-Z]{2,18}/;
    var whitespace = /\s/;
    var emailEx = /[a-zA-Z0-9+@@a-zA-Z]{6,40}/;

    var username = $('[name=username]').val();
    var password = $('[name=password]').val();
    var firstname = $('[name=firstname]').val();
    var lastname = $('[name=lastname]').val();
    var email = $('[name=email]').val();

    var isUser = userEx.test(username);
    var isPassword = passEx.test(password);
    var isFirstName = nameEx.test(firstname);
    var isLastName = nameEx.test(lastname);
    var isEmail = emailEx.test(email);

    if (whitespace.test(username)) {
        isUser = false;
    } if (whitespace.test(password)) {
        isPassword = false;
    }
    if (whitespace.test(firstname)) {
        isFirstName = false;
    } if (whitespace.test(lastname)) {
        isLastName = false;
    }
    if (whitespace.test(email)) {
        isEmail = false;
    }
    if (isUser == false) {
        ev.preventDefault();
        $('.userError').html("Please enter valid username");
    } else {
        $('.userError').html("");
    }
    if (isPassword == false) {
        ev.preventDefault();
        $('.passError').html("Please enter valid password");
    } else {
        $('.passError').html("");
    }
    if (isFirstName == false) {
        ev.preventDefault();
        $('.firsError').html("Please enter valid first name");
    } else {
        $('.firsError').html("");
    }
    if (isLastName == false) {
        ev.preventDefault();
        $('.lastError').html("Please enter valid last name");
    } else {
        $('.lastError').html("");
    }
    if (isEmail == false) {
        ev.preventDefault();
        $('.emailError').html("Please enter valid email");
    } else {
        $('.emailError').html("");
    }

    if (isUser, isPassword, isFirstName, isLastName, isEmail == true) {
        ev.preventDefault();

        $.post('/fastlane/Register/Insert'
          , {
              Username: username,
              Password: password,
              Firstname: firstname,
              Lastname: lastname,
              Email: email
          }, function (data) {
              if (data == "False") {
                  $("#modal-signIn-error").modal("show");
                  setTimeout(errorHider, 3000);
              }
              if (data == "True") {
                  $("#modal-signIn").modal("hide");
                  $("#modal-signIn-success").modal("show");
                  setTimeout(successHider, 3000);
              }
          });
    }
});


$(document).on('click touch', "#FAQ", function (ev) {

    $.getJSON('/fastlane/FAQ/AllFaqs', function (result) {
       if ($("#FAQselect").length > 0) {
            $("#FAQselect").empty();
        }
        $(".FAQContent").empty();
        $("#FAQselect").append("<option disabled selected value='Select Question'>Select FAQ</option>");
        $.each(result, function (row, field) {
            $("#FAQselect").append("<option value=" + field.IdQuestion + ">" + field.Title + "</option>");
        })
    })
});


$(document).on("change", "#FAQselect", function (ev) {
    $.getJSON("/fastlane/FAQ/AllFaqs", function (result) {
        if ($(".FAQContent").length > 0) {
            $(".FAQContent").empty();
        }
        var valueId = $("#FAQselect").children(":selected").attr("value").toString();


        $.each(result, function (row, field) {
            if (field.IdQuestion == valueId) {
                $(".FAQContent").append("<p>" + field.Content + "</p>");
            }

        })
    })
});


$(document).on("click touch", "#mainMenu", function () { $("#modal-login-success").modal("hide"); });
