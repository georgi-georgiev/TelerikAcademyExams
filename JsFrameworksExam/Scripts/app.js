/// <reference path="libs/_references.js" />


(function () {

    String.prototype.escape = function () {
        var tagsToReplace = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;'
        };
        return this.replace(/[&<>]/g, function (tag) {
            return tagsToReplace[tag] || tag;
        });
    };

	var appLayout =
		new kendo.Layout('<div id="main-content"></div>');
	var navLayout = new kendo.Layout('<div id="nav"></div>');

	var data = persisters.get("api/");
	vmFactory.setPersister(data);

	var router = new kendo.Router();

	router.route("/", function () {
	    if (data.isUserLoggedIn()) {
	        router.navigate("/home");
	    }
	    else {
	        viewsFactory.getNavView()
				.then(function (navViewHtml) {
				    $("#main-content").html(navViewHtml);
				}, function (error) {
				    console.log(error);
				});
	    }
	});


	router.route("/login", function () {
	    if (data.isUserLoggedIn()) {
	        router.navigate("/home");
	    }
	    else {
	        viewsFactory.getLoginView()
				.then(function (loginViewHtml) {
				    var loginVm = vmFactory.getLoginVM(
						function () {
						    router.navigate("/home");
						});
				    console.log("login");
				    var view = new kendo.View(loginViewHtml,
						{ model: loginVm });
				    appLayout.showIn("#main-content", view);
				}, function (error) {
				    console.log(error);
				});
	    }
	});

	router.route("/register", function () {
	    if (data.isUserLoggedIn()) {
	        router.navigate("/home");
	    }
	    else {
	        viewsFactory.getRegisterView()
				.then(function (registerViewHtml) {
				    var registerVm = vmFactory.getRegisterVM(function(){
				        router.navigate("/login");
				    });

				    var view = new kendo.View(registerViewHtml,
						{ model: registerVm });
				    appLayout.showIn("#main-content", view);
				}, function (error) {
				    console.log(error);
				});
	    }
	});

	router.route("/home", function () {
	    viewsFactory.getMenuView()
            .then(function (menuHTML) {
                var nicknameVm = vmFactory.getNicknameVM();
                var view = new kendo.View(menuHTML,
						{ model: nicknameVm });
                $("#main-content .k-menu").remove();
                navLayout.showIn("#nav", view);
            });
	});

	router.route("/logout", function () {
	    data.users.logout()
            .then(function (data) {
                router.navigate("/");
                $("#nav").remove();
            }, function (error) {
                console.log(error);
            });
	});

	
	router.route("/appointments", function () {
	    console.log("appointments");
	    viewsFactory.getAppointmentsView()
			.then(function (appointmentsViewHtml) {
			    vmFactory.getAppointmentsVM()
                    .then(function (appointmentsVM) {
                        var view = new kendo.View(appointmentsViewHtml,
                    { model: appointmentsVM });
                        appLayout.showIn("#main-content", view);
                        console.log("appointments");
                    });
			    
				}, function (err) {
				    console.log(err);
				});
	});

	router.route("/appointments/:type", function (type) {
	    console.log("type");
	    viewsFactory.getAppointmentsView()
			.then(function (appointmentsViewHtml) {
			    vmFactory.getAppointmentsByTypeVM(type)
                    .then(function (appointmentsVM) {
                        var view = new kendo.View(appointmentsViewHtml,
                    { model: appointmentsVM });
                        appLayout.showIn("#main-content", view);
                        console.log("appointments");
                    });

			}, function (err) {
			    console.log(err);
			});
	});

	router.route("/appointmentsCreate", function () {
	    viewsFactory.getAddAppointmentsView()
			.then(function (appointmentsViewHtml) {

			    var appointmentsVm = vmFactory.getAddAppointmentsVM(function () {
			        router.navigate("/appointments");
			    });
			    var view = new kendo.View(appointmentsViewHtml,
                    { model: appointmentsVm });
			    appLayout.showIn("#main-content", view);
			    console.log("appointments");
			}, function (err) {
			    console.log(err);
			});
	});


	router.route("/todo-lists", function () {
	    viewsFactory.getTodosView()
			.then(function (todosViewHtml) {
			    vmFactory.getTodosVM()
                    .then(function (todosVM) {
                        var view = new kendo.View(todosViewHtml,
                    { model: todosVM });
                        appLayout.showIn("#main-content", view);
                    });

			}, function (err) {
			    console.log(err);
			});
	});

	router.route("/todo-list/:id", function (id) {
	    viewsFactory.getTodoView()
			.then(function (todoViewHtml) {
			    vmFactory.getTodoVM(id)
                    .then(function (todoVM) {
                        var view = new kendo.View(todoViewHtml,
                    { model: todoVM });
                        appLayout.showIn("#main-content", view);
                    });

			}, function (err) {
			    console.log(err);
			});
	});

	router.route("/todoCreate", function () {
	    viewsFactory.getAddTodoView()
			.then(function (todoViewHtml) {

			    var todoVm = vmFactory.getAddTodoVM(function () {
			        router.navigate("/todo-lists");
			    });
			    var view = new kendo.View(todoViewHtml,
                    { model: todoVm });
			    appLayout.showIn("#main-content", view);
			}, function (err) {
			    console.log(err);
			});
	});

	router.route("/todoForList/:listId", function (listId) {
	    viewsFactory.getAddTodoForListView()
			.then(function (todoViewHtml) {

			    var todoVm = vmFactory.getAddTodoForListVM(listId, function () {
			        router.navigate("/todo-lists");
			    });
			    var view = new kendo.View(todoViewHtml,
                    { model: todoVm });
			    appLayout.showIn("#main-content", view);
			}, function (err) {
			    console.log(err);
			});
	});

	router.route("/changeDone/:todoId", function (todoId) {
	    data.todo.changeDone(todoId)
            .then(function () {
                router.navigate("/todo-lists");
            }, function(err){
                console.log(err);
            });
	});


	$(function () {
	    navLayout.render("#app");
	    appLayout.render("#app");
		router.start();
	});
}());