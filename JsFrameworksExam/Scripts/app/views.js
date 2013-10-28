/// <reference path="../libs/_references.js" />


window.viewsFactory = (function () {
	var rootUrl = "Scripts/partials/";

	var templates = {};

	function getTemplate(name) {
		var promise = new RSVP.Promise(function (resolve, reject) {
			if (templates[name]) {
				resolve(templates[name])
			}
			else {
				$.ajax({
					url: rootUrl + name + ".html",
					type: "GET",
					success: function (templateHtml) {
						templates[name] = templateHtml;
						resolve(templateHtml);
					},
					error: function (err) {
						reject(err)
					}
				});
			}
		});
		return promise;
	}

	function getLoginView() {
		return getTemplate("login-form");
	}

	function getRegisterView() {
	    return getTemplate("register-form");
	}

	function getNavView() {
		return getTemplate("nav");
	}

	function getMenuView() {
	    return getTemplate("menu");
	}

	function getAppointmentsView() {
	    return getTemplate("appointments");
	}
	
	function getAddAppointmentsView() {
	    return getTemplate("appointments-form");
	}

	function getTodosView() {
	    return getTemplate("todolists");
	}

	function getTodoView() {
	    return getTemplate("todolist");
	}

	function getAddTodoView() {
	    return getTemplate("todolists-form");
	}

	function getAddTodoForListView() {
	    return getTemplate("todoforlist-form");
	}

	return {
		getLoginView: getLoginView,
		getRegisterView: getRegisterView,
		getAppointmentsView: getAppointmentsView,
		getAddAppointmentsView: getAddAppointmentsView,
		getTodosView: getTodosView,
        getTodoView: getTodoView,
        getAddTodoView: getAddTodoView,
        getAddTodoForListView: getAddTodoForListView,
        getNavView: getNavView,
        getMenuView: getMenuView
	};
}());