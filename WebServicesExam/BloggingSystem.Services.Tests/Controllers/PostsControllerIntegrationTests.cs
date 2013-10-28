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
    public class PostsControllerIntegrationTests
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
                     "PostsApi",
                     "api/posts/{postId}/comment",
                     new { controller = "posts", action="comment" }),
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
        public void PostAddValid()
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
            var post = new PostModel()
            {
                Title = "Some title",
                Text = "Some text"
            };
            var responseAdd = httpServer.Post("api/posts", post, headers);

            Assert.AreEqual(HttpStatusCode.Created, responseAdd.StatusCode);
        }

        [TestMethod]
        public void PostAddNullTitle()
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
            var post = new PostModel()
            {
                Title = null,
                Text = "Some text"
            };
            var responseAdd = httpServer.Post("api/posts", post, headers);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseAdd.StatusCode);
        }

        [TestMethod]
        public void PostAddNullText()
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
            var post = new PostModel()
            {
                Title = "Some title",
                Text = null
            };
            var responseAdd = httpServer.Post("api/posts", post, headers);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseAdd.StatusCode);
        }

        [TestMethod]
        public void GetPostBySingleTag()
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
           
            var post = new PostModel()
            {
                Title = "Some title",
                Text = "Some text",
                Tags = new List<string>(){ "tag"}
            };
            httpServer.Post("api/posts", post, headers);
            var responseGet = httpServer.Get("api/posts?tags="+post.Tags, headers);

            Assert.AreEqual(HttpStatusCode.OK, responseGet.StatusCode);
        }

        [TestMethod]
        public void GetPostByTags()
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

            var post = new PostModel()
            {
                Title = "Some title",
                Text = "Some text",
                Tags = new List<string>() { "tag", "new" }
            };
            httpServer.Post("api/posts", post, headers);
            var responseGet = httpServer.Get("api/posts?tags=" + string.Join(",", post.Tags), headers);

            Assert.AreEqual(HttpStatusCode.OK, responseGet.StatusCode);
        }

        [TestMethod]
        public void GetPostCommenValidId()
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

            var post = new PostModel()
            {
                Title = "Some title",
                Text = "Some text"
            };
            var comment = new CommentModel()
            {
                Text = "some comment"
            };
            var responseAdd = httpServer.Post("api/posts", post, headers);

            var responseGet = httpServer.Put("api/posts/" + responseAdd.Content.ReadAsStringAsync().Id + "/comment", comment, headers);

            Assert.AreEqual(HttpStatusCode.OK, responseGet.StatusCode);
        }

        [TestMethod]
        public void GetPostCommenInvalidId()
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

            var post = new PostModel()
            {
                Title = "Some title",
                Text = "Some text"
            };
            var comment = new CommentModel()
            {
                Text = "some comment"
            };
            httpServer.Post("api/posts", post, headers);
            var responseGet = httpServer.Put("api/posts/0/comment", comment, headers);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseGet.StatusCode);
        }
    }
}
