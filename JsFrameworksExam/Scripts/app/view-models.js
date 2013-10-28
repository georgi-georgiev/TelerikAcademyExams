/// <reference path="../libs/_references.js" />

window.vmFactory = (function () {
    var data = null;

    function getLoginViewModel(successCallback) {
        var viewModel = {
            username: $("#tb-username").val(),
            password: $("#tb-password").val(),
            login: function () {
                data.users.login(this.get("username"), this.get("password"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					});
            }
        };
        return kendo.observable(viewModel);
    };

    function getRegisterViewModel(successCallback) {
        var viewModel = {
            usernameReg: $("#tb-usernameReg").val(),
            passwordReg: $("#tb-passwordReg").val(),
            email: $("#tb-email").val(),
            register: function () {
                data.users.register(this.get("usernameReg"), this.get("passwordReg"), this.get("email"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					});
            }
        };
        return kendo.observable(viewModel);
    }


    function getNicknameViewModel() {
        var viewModel = {
            nickname: data.getNickname()
        }
        return kendo.observable(viewModel);
    }

    function getAppointmentsViewModel() {
        
        return data.appointments.all()
			.then(function (appointments) {
			    var viewModel = {
			        appointments: appointments,
			        message: ""
			    };
			    return kendo.observable(viewModel);
			});
    }

    function getAppointmentsByTypeViewModel(type) {
        return data.appointments.all(type)
			.then(function (appointments) {
			    var viewModel = {
			        appointments: appointments,
			        message: ""
			    };
			    return kendo.observable(viewModel);
			});
    }

    function getAddAppointmentViewModel(successCallback) {
        var viewModel = {
            subject: $("#tb-subject").val(),
            appointmentDate: $("#tb-appointmentDate").val(),
            duration: $("#tb-duration").val(),
            description: $("#tb-description").val(),
            create: function () {
                console.log("create");
                data.appointments.create(this.get("subject"), this.get("appointmentDate"), this.get("duration"), this.get("description"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					});
            }
        }
        return kendo.observable(viewModel);
    }

    function getTodosViewModel() {

        return data.todos.all()
			.then(function (todos) {
			    var viewModel = {
			        todosList: todos,
			        message: ""
			    };
			    return kendo.observable(viewModel);
			});
    }

    function getTodoViewModel(id) {
        return data.todos.getById(id)
			.then(function (todo) {
			    var viewModel = {
			        todosList: todo,
			        message: ""
			    };
			    console.log(viewModel);
			    return kendo.observable(viewModel);
			});
    }

    function getAddTodoViewModel(successCallback) {
        var viewModel = {
            title: $("#tb-title").val(),
            create: function () {
                data.todos.create(this.get("title"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					});
            }
        }

        return kendo.observable(viewModel);
    }

    function getAddTodoForListViewModel(listId, successCallback) {
        var viewModel = {
            listId: listId,
            text: $("#tb-text").val(),
            create: function () {
                data.todos.createTodoForList(this.get("listId"), this.get("text"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					});
            }
        }

        return kendo.observable(viewModel);
    }

    return {
        getLoginVM: getLoginViewModel,
        getRegisterVM: getRegisterViewModel,
        getNicknameVM: getNicknameViewModel,
        getAppointmentsVM: getAppointmentsViewModel,
        getAddAppointmentsVM: getAddAppointmentViewModel,
        getAppointmentsByTypeVM: getAppointmentsByTypeViewModel,
        getTodosVM: getTodosViewModel,
        getTodoVM: getTodoViewModel,
        getAddTodoVM: getAddTodoViewModel,
        getAddTodoForListVM: getAddTodoForListViewModel,
        setPersister: function (persister) {
            data = persister
        }
    }
}());