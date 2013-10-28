window.persisters = (function () {

    var nickname = localStorage.getItem("nickname");
    var accessToken = localStorage.getItem("accessToken");

    var clearUserData = function () {
        localStorage.removeItem("nickname");
        localStorage.removeItem("accessToken");
        nickname = null;
        accessToken = null;
    };

    var saveUserData = function (userData) {
        localStorage.setItem("nickname", userData.username);
        localStorage.setItem("accessToken", userData.accessToken);
        nickname = userData.username;
        accessToken = userData.accessToken;
    };

    var DataPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
            this.users = new UsersPersister(apiUrl);
            this.appointments = new AppointmentsPersister(apiUrl);
            this.todos = new TodosPersister(apiUrl);
            this.todo = new TodoPersister(apiUrl);
        },
        isUserLoggedIn: function () {
            if (nickname !== null && accessToken !== null) {
                return true;
            }

            return false;
        },
        getNickname: function () {
            return nickname;
        },
        getAccessToken: function () {
            return accessToken;
        }
    });
	var UsersPersister = Class.create({
		init: function (apiUrl) {
			this.apiUrl = apiUrl;
		},
		login: function (username, password) {
			var user = {
				username: username,
				authCode: CryptoJS.SHA1(password).toString()
			};
			return requester.postJSON(this.apiUrl + "auth/token", user)
				.then(function (data) {
				    saveUserData(data);
					return data;
				}, function (err) {
				    console.log(err);
				});
		},
		register: function (username, password, email) {
		    /*{
		        "username" : "georgi",
                "email": "abv@abv.bg",
                "password": "7c4a8d09ca3762af61e59520943dc26494f8941b"
		    }*/
			var user = {
			    username: username,
                email: email,
                authCode: CryptoJS.SHA1(password).toString()
			};
			return requester.postJSON(this.apiUrl +"users/register", user).then(function(data){
			    console.log(data);
			},function(err){
			    console.log(err);
			});
		},
		logout: function () {
		    if (!accessToken) {
		        alert("you accessToken is expired");
			}
			var headers = {
			    "X-accessToken": accessToken
			};
			clearUserData();
			return requester.putJSON(this.apiUrl + "users", headers);
		}
	});

	var AppointmentsPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl + "appointments";
	    },
	    all: function (type) {
	        if (typeof type == "undefined") {
	            type = "all";
	        }
	        var headers = {
	            "X-accessToken": accessToken
	        };
	        if (type == "byDate") {
	            var date = $("#byDate").val();
	            return requester.getJSON(this.apiUrl + "?date=" + date , headers);
	        }
	        return requester.getJSON(this.apiUrl + "/" + type, headers);
	    },
	    create: function (subject, appointmentDate, duration, description) {
	        var headers = {
	            "X-accessToken": accessToken
	        };
	        var appointment = {
	            subject:subject,
	            appointmentDate: appointmentDate,
	            duration:duration,
	            description: description
	        }
	        return requester.postJSON(this.apiUrl + "/new", appointment, headers);
	    }
	});

	var TodosPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl + "lists/";
	    },
	    all: function () {
	        var headers = {
	            "X-accessToken": accessToken
	        };
	        return requester.getJSON(this.apiUrl, headers);
	    },
	    create: function (title) {
	        var headers = {
	            "X-accessToken": accessToken
	        };
	        var todo = {
	            title: title,
                todos: new Array()
	        }
	        return requester.postJSON(this.apiUrl + "new", todo, headers);
	    },
	    getById: function(id){
	        var headers = {
	        "X-accessToken": accessToken
	        };
	        return requester.getJSON(this.apiUrl + id + "/todos", headers);
	    },
	    createTodoForList: function (listId, todoText) {
	        var headers = {
	            "X-accessToken": accessToken
	        };
	        var todo = {
                text: todoText
	        }
	        return requester.postJSON(this.apiUrl + listId + "/todos", todo, headers);
	    }
	});

	var TodoPersister = Class.create({
	    init: function (apiUrl) {
	        this.apiUrl = apiUrl + "todos/";
	    },
	    changeDone: function (todoId) {
	        var headers = {
	            "X-accessToken": accessToken
	        };
	        return requester.putJSON(this.apiUrl + todoId, headers);
	    }
	});

	return {
		get: function (apiUrl) {
			return new DataPersister(apiUrl);
		}
	}
}());