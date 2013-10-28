/// <reference path="jquery-2.0.2.js" />
/// <reference path="class.js" />
/// <reference path="persister.js" />


var controller = (function () {

    var rootUrl = "http://localhost:22954/api";

    var MainController = Class.create({
        init: function () {
            this.persister = persister.get(rootUrl);
        },
        loadUI: function (selector) {
            if (this.persister.isUserLoggedIn()) {
                    this.loadGameUI(selector);
            }
            else {
                this.loadLoginForm(selector);
            }
            this.atachEventsHandlers(selector);
        },
        loadGameUI: function (selector) {
            var gameHTML =
                '<div id="header"><button id="logout-button">Logout</button>'+
                '<span id="user-nickname">Hello,' + this.persister.nickname() + '</span></div>';
            $(selector).html(gameHTML);

            $(selector).append(
                '<div id="create-game-form"><label for="game-title">Title:</label>'+
                '<input type="text" id="game-title" />'+
                '<label for="game-password">Password:</label>'+
                '<input type="password" id="game-password" />'+
                '<button id="create-game">Create</button></div>');

            $(selector).append('<div id="open-games"><h2>Open games</h2></div>');

            this.persister.game.open(function (data) {
                
                for (var i = 0; i < data.length; i++) {
                    var openGameHTML =
                        '<li id="open-game" data-game-id="' + data[i].id + '">' +
                        data[i].title + " by " + data[i].creator +
                        '</li>';
                    $("#open-games").append(openGameHTML);
                }
            }, function (error) {
                var response = JSON.parse(error.responseText);
                $("#open-games").before("<div class'error'>" + response.Message + "</div>");
            });

            $(selector).append('<div id="active-games"><h2>Active games</h2></div>');
            this.persister.game.active(function (data) {
                for (var i = 0; i < data.length; i++) {
                    var activeGameHTML =
                        '<li id="active-game" data-game-id="' + data[i].id + '">' +
                        data[i].title + " by " + data[i].creator;
                    if (data[i].status == "in-progress") {
                        activeGameHTML += "<button id='open-game-in-progress'>in progress</button>";
                            
                    }
                    else if(data[i].status == "full"){
                        activeGameHTML += '<button id="start-game">start</button>';
                    }
                    else
                    {
                        activeGameHTML += '(open)';
                    }
                    activeGameHTML +='</li>';
                    $("#active-games").append(activeGameHTML);
                }
            }, function (error) {
                var response = JSON.parse(error.responseText);
                $("#active-games").before("<div class'error'>" + response.Message + "</div>");
            });

            $(selector).append('<div id="score-board"><h2>Score board</h2></div>');
            this.persister.user.score(function (data) {
                for (var i = 0; i < data.length; i++) {
                    var activeGameHTML =
                        '<li>' +
                        data[i].nickname+ " - "+data[i].score+
                        '</li>';
                    $("#score-board").append(activeGameHTML);
                }
            }, function (error) {
                var response = JSON.parse(error.responseText);
                $("#score-board").before("<div class'error'>" + response.Message + "</div>");
            });

            
        },
        loadLoginForm: function (selector) {
            var loginFormHTML =
                '<div id="signup">' +
                    '<form id="login-form">'+
                        '<label for="username-log">Username:</label>' +
                        '<input type="text" id="username-log" />' +
                        '<label for="password-log">Password:</label>' +
                        '<input type="password" id="password-log" />' +
                        '<a href="#" id="register">Register</a>'+
                        '<button id="login-button">Login</button>' +
                    '</form>' +
                    '<form id="register-form" style="display: none;">'+
                        '<label for="username-reg">Username:</label>' +
                        '<input type="text" id="username-reg" />' +
                        '<label for="nickname-reg">Nickname:</label>' +
                        '<input type="text" id="nickname-reg" />' +
                        '<label for="password-reg">Password:</label>' +
                        '<input type="password" id="password-reg" />' +
                        '<a href="#" id="login">Login</a>' +
                        '<button id="register-button">Register</button>' +
                    '</form>'+
                '</div>';
            $(selector).html(loginFormHTML);
        },
        loadGameFieldUI: function (selector, gameId) {
            $("#game-field").remove();
            $(selector).append("<div id='game-field'></div>");
            $("#game-field").append('<div id="unread-messages"><h2>Unread messages</h2></div>');
            this.persister.message.unread(function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (gameId == data[i].gameId) {
                        var unreadMessageHTML =
                            '<li>' +
                            data[i].text +
                            '</li>';
                        $("#unread-messages").append(unreadMessageHTML);
                    }
                }
            }, function (error) {
                var response = JSON.parse(error.responseText);
                $("#unread-messages").before("<div class'error'>" + response.Message + "</div>");
            });

            $("#game-field").append('<div id="all-messages"><h2>All messages</h2></div>');
            this.persister.message.all(function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (gameId == data[i].gameId) {
                        var allMessageHTML =
                            '<li>' +
                            data[i].text +
                            '</li>';
                        $("#all-messages").append(allMessageHTML);
                    }
                }
            }, function (error) {
                var response = JSON.parse(error.responseText);
                $("#all-messages").before("<div class'error'>" + response.Message + "</div>");
            });

            for (var i = 0; i < 9; i++) {
                for (var j = 0; j < 9; j++) {
                    if (j == 0 && i % 2 != 0) {
                        $("#game-field").append("<div class='game-field-box-ranger-red game-figure'><li data-position-x='" + j + "' data-position-y='" + i + "'></li></div>");
                    }
                    else if (j == 2 && i % 2 != 1) {
                        $("#game-field").append("<div class='game-field-box-warrior-red game-figure'><li data-position-x='" + j + "' data-position-y='" + i + "'></li></div>");
                    }
                    else if (j==8 && i%2!=0) {
                        $("#game-field").append("<div class='game-field-box-ranger-blue game-figure'><li data-position-x='" + j + "' data-position-y='" + i + "'></li></div>");
                    }
                    else if (j == 6 && i%2!=1) {
                        $("#game-field").append("<div class='game-field-box-warrior-blue game-figure'><li data-position-x='" + j + "' data-position-y='" + i + "'></li></div>");
                    }
                    else {
                        $("#game-field").append("<div class='game-field-box'></div>");
                    }
                }
            }
            
        },
        atachEventsHandlers: function (selector) {
            var self = this;
            $(selector).on("click", "#login-button", function () {
                var user = {
                    username: $(selector + " #username-log").val(),
                    password: $(selector + " #password-log").val()
                }
                self.persister.user.login(user, function () {
                    self.loadGameUI(selector);
                }, function (error) {
                    $("#login-form").before("<div class'error'>Fail to login</div>");
                    var response = JSON.parse(error.responseText);
                    $("#login-form").before("<div class'error'>" + response.Message + "</div>");
                });
                return false;
            });

            $(selector).on("click", "#register-button", function () {
                var user = {
                    username: $(selector + " #username-reg").val(),
                    nickname: $(selector + " #nickname-reg").val(),
                    password: $(selector + " #password-reg").val()
                }
                self.persister.user.register(user, function () {
                    self.loadGameUI(selector);
                }, function (error) {
                    $("#register-form").before("<div class='error'>Fail to register</div>");
                    var response = JSON.parse(error.responseText);
                    $("#register-form").before("<div class'error'>" + response.Message + "</div>");
                });
                return false;
            });

            $(selector).on("click", "#logout-button", function () {
                self.persister.user.logout(function () {
                    self.loadLoginForm(selector);
                }, function (error) {
                    $("#register-form").before("<div class'error'>Fail to logout</div>");
                    var response = JSON.parse(error.responseText);
                    $("#register-form").before("<div class'error'>" + response.Message + "</div>");
                });
            });

            $(selector).on("click", "#register", function () {
                $("#register-form").show();
                $("#login-form").hide();
            });

            $(selector).on("click", "#login", function () {
                $("#login-form").show();
                $("#register-form").hide();
            });
            $(selector).on("click", "#create-game", function () {
                var gameData = {
                    title: $("#game-title").val()
                }
                var password = $("#game-password").val();
                if (password) {
                    gameData.password = password;
                }
                self.persister.game.create(gameData, function () {
                    $("#create-game-form").before("<div class'success'>Your game was created</div>");
                }, function (error) {
                    //var response = JSON.parse(error.responseText);
                    $("#create-game-form").before("<div>Fail to create a game</div>");
                });
            });
            $(selector).on("click", "#open-game", function () {
                $("#open-game-form").remove();
                $(this).append(
                    '<div id="open-game-form"><label for="join-game-password">Password</label>'+
                    '<input type="password" id="join-game-password" />'+
                    '<button id="join-game">Join</button></form>');
            });
            $(selector).on("click", "#join-game", function () {
                var gameData = {
                    id: $(this).parents("li").first().data("game-id")
                }
                var password = $("#join-game-password").val();
                if (password) {
                    gameData.password = password;
                }
                self.persister.game.join(gameData, function () {
                    $("#open-game-form").before("<div>Successfully join</div>");
                }, function (error) {
                    var response = JSON.parse(error.responseText);
                    $("#open-game-form").before("<div>"+response.Message+"</div>");
                });
            });

            $(selector).on("click", "#start-game", function () {
                var gameData = {
                    id: $(this).parents("li").first().data("game-id")
                }
                self.persister.game.start(gameData, function () {
                    self.loadGameFieldUI(selector, gameData.id);
                }, function (error) {
                    var response = JSON.parse(error.responseText);
                    $("#active-games").before("<div>"+ response.Message+"</div>");
                });
            });
            $(selector).on("click", "#open-game-in-progress", function () {
                var id = $(this).parents("li").first().data("game-id");
                self.loadGameFieldUI(selector, id);
            });

            $(selector).on("click", ".game-figure", function () {
                $(this).
                self.loadGameFieldUI(selector, id);
            });
        }
    });
    return {
        get: function () {
            return new MainController();
        }
    }
}());

$(function () {
    var cntrl = controller.get();
    cntrl.loadUI("#game-container");

});