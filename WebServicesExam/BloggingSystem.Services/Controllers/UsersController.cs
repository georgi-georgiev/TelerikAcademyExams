using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using BloggingSystem.Data;
using BloggingSystem.Models;
using BloggingSystem.Services.Attributes;
using BloggingSystem.Services.Models;

namespace BloggingSystem.Services.Controllers
{
    public class UsersController : BaseApiController
    {
        private const int Sha1Length = 40;
        private const int MinUsernameLength = 6;
        private const int MaxUsernameLength = 30;
        private const int MinDisplayNameLength = 6;
        private const int MaxDisplayNameLength = 30;
        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";
        private const string ValidDisplayNameChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";
        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private static readonly Random rand = new Random();
        private const int SessionKeyLength = 50;

        /*
        {  "username": "DonchoMinkov",
           "displayName": "Doncho Minkov",
           "authCode":   "bfff2dd4f1b310eb0dbf593bd83f94dd8d34077e"
        }
        */

        [ActionName("register")]
        public HttpResponseMessage PostRegisterUser(UserModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new BloggingSystemContext();
                    using (context)
                    {
                        this.ValidateUsername(model.Username);
                        this.ValidateDisplayName(model.DisplayName);
                        this.ValidateAuthCode(model.AuthCode);
                        var usernameToLower = model.Username.ToLower();
                        var displayNameToLower = model.DisplayName.ToLower();
                        var user = context.Users.FirstOrDefault(
                            usr => usr.Username == usernameToLower
                            || usr.DisplayName == displayNameToLower);

                        if (user != null)
                        {
                            throw new InvalidOperationException("Username or displayname exist");
                        }

                        user = new User()
                        {
                            Username = usernameToLower,
                            DisplayName = model.DisplayName,
                            Password = model.AuthCode
                        };

                        context.Users.Add(user);
                        context.SaveChanges();

                        user.SessionKey = this.GenerateSessionKey(user.Id);
                        context.SaveChanges();

                        var loggedUser = new LoggedUserModel()
                        {
                            DisplayName = user.DisplayName,
                            SessionKey = user.SessionKey
                        };

                        var response =
                            this.Request.CreateResponse(HttpStatusCode.Created, loggedUser);
                        return response;
                    }
                });

            return responseMsg;
        }
        /*
         {"username": "DonchoMinkov",
          "authCode":   "bfff2dd4f1b310eb0dbf593bd83f94dd8d34077e"
         }

        */

        [ActionName("login")]
        public HttpResponseMessage PostLoginUser(UserModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new BloggingSystemContext();
                    using (context)
                    {
                        this.ValidateUsername(model.Username);
                        var username = model.Username.ToLower();
                        var user = context.Users.FirstOrDefault(
                            usr => usr.Username == username
                            && usr.Password == model.AuthCode);

                        if(user == null)
                        {
                            throw new InvalidOperationException(
                                "Username or password is incorrect");
                        }

                        if(user.SessionKey == null)
                        {
                            user.SessionKey = this.GenerateSessionKey(user.Id);
                            context.SaveChanges();
                        }

                        var loggedUser = new LoggedUserModel()
                        {
                            DisplayName = user.DisplayName,
                            SessionKey = user.SessionKey
                        };

                        var response =
                          this.Request.CreateResponse(HttpStatusCode.Created, loggedUser);
                      return response;
                    }
                });

            return responseMsg;
        }

        /*
        X-sessionKey: 3yZVhiTdkjxHRIVnwRSTLRrRQYLMlejvEjMNiSfmYtmaymcdzd
         */

        [ActionName("logout")]
        public HttpResponseMessage PutLogoutUser(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new BloggingSystemContext();
                    using (context)
                    {
                        if (sessionKey == null)
                        {
                            throw new ArgumentNullException("You do not have session");
                        }
                        var user = context.Users.FirstOrDefault(
                            usr => usr.SessionKey == sessionKey);

                        if (user == null)
                        {
                            throw new InvalidOperationException("Your session is expired");
                        }

                        user.SessionKey = null;
                        context.SaveChanges();

                        var response =
                          this.Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                });

            return responseMsg;
        }

        private string GenerateSessionKey(int userId)
        {
            StringBuilder sessionKeyBuilder = new StringBuilder(SessionKeyLength);
            sessionKeyBuilder.Append(userId);
            while (sessionKeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                sessionKeyBuilder.Append(SessionKeyChars[index]);
            }
            return sessionKeyBuilder.ToString();
        }

        private void ValidateAuthCode(string authCode)
        {
            if (authCode == null)
            {
                throw new ArgumentNullException("You have to enter password");
            }
            else if (authCode.Length != Sha1Length)
            {
                throw new ArgumentOutOfRangeException("Password must be encrypted.");
            }
        }

        private void ValidateDisplayName(string displayName)
        {
            if (displayName == null)
            {
                throw new ArgumentNullException("Your DisplayName cannot be empty");
            }
            else if (displayName.Length < MinDisplayNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Your DisplayName less than {0} symbols", MinDisplayNameLength));
            }
            else if (displayName.Length > MaxDisplayNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Your DisplayName is more than {0} symbols", MaxDisplayNameLength));
            }
            else if (displayName.Any(ch => !ValidDisplayNameChars.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException("DisplayName must contain only Latin letters, digits .,_ -");
            }
        }

        private void ValidateUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("You have to enter username");
            }
            else if (username.Length < MinUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Your username is less than {0} symbols", MinUsernameLength));
            }
            else if (username.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                                    string.Format("Your usrname is more than {0} symbols", MaxUsernameLength));
            }
            else if (username.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException("Username must contain only Latin letters, digits .,_");
            }
        }
    }
}
