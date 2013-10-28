/// <reference path="class.js" />
/// <reference path="http-requester.js" />
/// <reference path="crypto-js-sha1.js" />

var persister = (function () {
    var nickname = localStorage.getItem("nickname");
    var sessionKey = localStorage.getItem("sessionKey");

    function saveUserData(userData) {
        localStorage.setItem("nickname", userData.nickname);
        localStorage.setItem("sessionKey", userData.sessionKey);
        nickname = userData.nickname;
        sessionKey = userData.sessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        nickname = "";
        sessionKey = "";
    }

    var MainPersister = Class.create({
        init: function (url) {
            this.rootUrl = url;
            this.user = new UserPersister(this.rootUrl);
            this.game = new GamePersister(this.rootUrl);
            this.battle = new BattlePersister(this.rootUrl);
            this.message = new MessagePersister(this.rootUrl);
        },
        isUserLoggedIn: function () {
            var isLoggedIn = nickname !== null && sessionKey !== null;
            return isLoggedIn;
        },
        nickname: function () {
            return nickname;
        }
    });

    var UserPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "/user";
        },
        login: function (data, success, error) {
            var url = this.rootUrl + "/login";
            var userData = {
                username : data.username,
                authCode : CryptoJS.SHA1(data.username + data.password).toString()
            }
            httpRequester.postJSON(url, userData,
                function (data) {
                    saveUserData(data);
                    success(data);
                }, error
            );
        },
        register: function (data, success, error) {
            var url = this.rootUrl + "/register";
            var userData = {
                username: data.username,
                nickname: data.nickname,
                authCode: CryptoJS.SHA1(data.username + data.password).toString()
            }
            httpRequester.postJSON(url, userData,
                function (data) {
                    saveUserData(data);
                    success(data);
                }, error
            );
        },
        logout: function (success, error) {
            var url = this.rootUrl + "/logout/" + sessionKey;
            httpRequester.getJSON(url, function (data) {
                clearUserData();
                success(data);
            }, error)
        },
        score: function (succes, error) {
            var url = this.rootUrl + "/scores/" + sessionKey;
            httpRequester.getJSON(url, succes, error);
        }
    });

    var GamePersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "/game";
        },
        open: function (success, error) {
            var url = this.rootUrl + "/open/" + sessionKey;
            httpRequester.getJSON(url, success, error);
        },
        active: function (success, error) {
            var url = this.rootUrl + "/my-active/" + sessionKey;
            httpRequester.getJSON(url, success, error);
        },
        create: function (data, success, error) {
            var url = this.rootUrl + "/create/" + sessionKey;
            var gameData = {
                title: data.title
            }
            var password = data.password;
            if (password) {
                gameData.password = CryptoJS.SHA1(password).toString();
            }
            httpRequester.postJSON(url, gameData, success, error);
        },
        join: function (data, success, error) {
            var url = this.rootUrl + "/join/" + sessionKey;
            var gameData = {
                id: data.id,
            }
            var password = data.password;
            if (password) {
                gameData.password = password;
            }
            httpRequester.postJSON(url, gameData, success, error);
        },
        start: function (data, success, error) {
            var url = this.rootUrl + "/" + data.id + "/start/" + sessionKey;
            httpRequester.getJSON(url, success, error);
        }
    });

    var MessagePersister = Class.create({
        init: function(url){
            this.rootUrl = url + "/messages";
        },
        unread: function (success, error) {
            var url = this.rootUrl + "/unread/" + sessionKey;
            httpRequester.getJSON(url, success, error);
        },
        all: function (success, error) {
            var url = this.rootUrl + "/all/" + sessionKey;
            httpRequester.getJSON(url, success, error);
        }

    });

    var BattlePersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "/battle";
        },
        move: function (data, sucess, error) {
            var url = this.rootUrl + "/" + data.gameId + "/move/" + sessionKey;
            var unitdata = {
                unitId: data.unitId,
                position: {
                    x: data.position.x,
                    y: data.position.y
                }
            }
            httpRequester.postJSON(url, unitData, sucess, error);
        },
        atack: function (data, sucess, error) {
            var url = this.rootUrl + "/" + data.gameId + "/atack/" + sessionKey;
            var unitdata = {
                unitId: data.unitId,
                position: {
                    x: data.position.x,
                    y: data.position.y
                }
            }
            httpRequester.postJSON(url, unitData, sucess, error);
        },
        defend: function (data, sucess, error) {
            var url = this.rootUrl + "/" + data.gameId + "/defend/" + sessionKey;
            httpRequester.postJSON(url, data.unitId, sucess, error);
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    }
}());