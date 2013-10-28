using System;
using System.Collections.Generic;
using System.Net;
using System.Transactions;
using System.Web.Http;
using BloggingSystem.Services.Controllers;
using BloggingSystem.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BloggingSystem.Services.Tests.Controllers
{
    [TestClass]
    public class UsersControllerIntegrationTests
    {
        static TransactionScope tran;
        private InMemoryHttpServer httpServer;

        [TestInitialize]
        public void TestInit()
        {
            var type = typeof(UsersController);
            tran = new TransactionScope();

            var routes = new List<Route>
            {
                new Route(
                     "UsersApi",
                     "api/users/{action}",
                     new
                        {
                            controller = "users"
                        }),
                new Route(
                    "DefaultApi",
                    "api/{controller}/{id}",
                    new { id = RouteParameter.Optional }),
            };
            this.httpServer = new InMemoryHttpServer("http://localhost/", routes);
        }

        [TestCleanup]
        public void TearDown()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void RegisterWhenUserIsValid()
        {
            var testUser = new UserModel()
            {
                Username = "ValidUsername",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.AreEqual(testUser.DisplayName, userModel.DisplayName);
            Assert.IsNotNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenUsernameIsNull()
        {
            var testUser = new UserModel()
            {
                Username = null,
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenUsernameIsInvalid()
        {
            var testUser = new UserModel()
            {
                Username = "Invalid username",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenUsernameIsLong()
        {
            var testUser = new UserModel()
            {
                Username = "LoremIpsumisimplydummytextoftheprintingandtypesettingindustryLoremIpsum",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenUsernameIsShort()
        {
            var testUser = new UserModel()
            {
                Username = "a",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenDisplayNameIsInvalid()
        {
            var testUser = new UserModel()
            {
                Username = "username",
                DisplayName = "Invalid@DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.DisplayName);
            Assert.IsNull(userModel.SessionKey);
        }
        [TestMethod]
        public void RegisterWhenDisplayNameIsLong()
        {
            var testUser = new UserModel()
            {
                Username = "Validusername",
                DisplayName = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.DisplayName);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenDisplayNameIsShort()
        {
            var testUser = new UserModel()
            {
                Username = "username",
                DisplayName = "a",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.DisplayName);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void RegisterWhenDisplayNameIsNull()
        {
            var testUser = new UserModel()
            {
                Username = "validusername",
                DisplayName = null,
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);
            Assert.IsNull(userModel.DisplayName);
            Assert.IsNull(userModel.SessionKey);
        }

        [TestMethod]
        public void LogoutWhenUserSessionIsCorrect()
        {
            var testUser = new UserModel()
            {
                Username = "ValidUsername",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);

            var headers = new Dictionary<string, string>();
            headers["X-sessionKey"] = userModel.SessionKey;
            var responseLogout = httpServer.Put("api/users/logout", headers);

            Assert.AreEqual(HttpStatusCode.OK, responseLogout.StatusCode);
        }

        public void LogoutWhenUserSessionIsNull()
        {
            var testUser = new UserModel()
            {
                Username = "ValidUsername",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);

            var headers = new Dictionary<string, string>();
            headers["X-sessionKey"] = null;
            var responseLogout = httpServer.Put("api/users/logout", headers);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseLogout.StatusCode);
        }

        public void LogoutWhenUserSessionIsIncorrect()
        {
            var testUser = new UserModel()
            {
                Username = "ValidUsername",
                DisplayName = "Valid DisplayName",
                AuthCode = new string('*', 40)
            };

            var response = httpServer.Post("api/users/register", testUser);
            var contentString = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var userModel = JsonConvert.DeserializeObject<LoggedUserModel>(contentString);

            var headers = new Dictionary<string, string>();
            headers["X-sessionKey"] = "incorrect";
            var responseLogout = httpServer.Put("api/users/logout", headers);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseLogout.StatusCode);
        }
    }
}
