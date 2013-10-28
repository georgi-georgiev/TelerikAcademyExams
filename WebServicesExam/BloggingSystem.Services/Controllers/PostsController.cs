using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BloggingSystem.Services.Models;
using BloggingSystem.Data;
using BloggingSystem.Services.Attributes;
using System.Web.Http.ValueProviders;
using BloggingSystem.Models;

namespace BloggingSystem.Services.Controllers
{
    public class PostsController : BaseApiController
    {
        public IQueryable<PostModel> GetAllPosts(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var context = new BloggingSystemContext();
            using (context)
            {
                if (sessionKey == null)
                {
                    throw new ArgumentNullException("You dont have session");
                }
                var user = context.Users.FirstOrDefault(
                                usr => usr.SessionKey == sessionKey);

                if (user == null)
                {
                    throw new InvalidOperationException("Your session is expired");
                }

                var postModel =
                    (from post in context.Posts
                     select new PostModel()
                     {
                         Id = post.Id,
                         Title = post.Title,
                         Author = post.Author.DisplayName,
                         PostDate = post.PostDate,
                         Text = post.Text,
                         Tags = (
                         (from tag in post.Tags
                          select tag.Name)
                         ),
                         Comments = (
                         from comment in post.Comments
                         select new CommentModel()
                         {
                             Text = comment.Text,
                             Author = comment.Author.DisplayName,
                             PostDate = comment.PostDate
                         }
                         )
                     }).ToList();
                return postModel.OrderByDescending(p => p.PostDate).AsQueryable();
            }
        }

        //api/posts?page=0&count=2
        public IQueryable<PostModel> GetAllPostsByPageAndCount(int page, int count,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            if (page < 0)
            {
                throw new ArgumentOutOfRangeException("Page does not exists");
            }
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("Count cannot be less than 1");
            }
            var models = this.GetAllPosts(sessionKey)
                .Skip(page * count)
                .Take(count);
            return models.OrderByDescending(p => p.PostDate);
        }

        //api/posts?keyword=webapi
        public IQueryable<PostModel> GetAllPostsByKeyword(string keyword,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var models = this.GetAllPosts(sessionKey).Where(p => p.Title.ToLower().Contains(keyword.ToLower()));
            return models.OrderByDescending(p => p.PostDate);
        }

        //api/posts?tags=web,webapi
        public IQueryable<PostModel> GetAllPostsByTags(string tags,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            if (tags == null)
            {
                throw new ArgumentNullException("You dont have keyword");
            }

            string[] splittedTags = tags.Split(new char[] { ',' });

            if (splittedTags.Length > 1)
            {
                var modelsTags = this.GetAllPosts(sessionKey).Where(
                    p => p.Tags.SequenceEqual(splittedTags));
                return modelsTags.OrderByDescending(p => p.PostDate);
            }

            var modelsTag = this.GetAllPosts(sessionKey).Where(
                    p => p.Tags.Contains(tags));
            return modelsTag.OrderByDescending(p => p.PostDate);

        }
        /*
         { "text": "Abe kefi me toq post" }
        */
        //api/posts/{postId}/comment
        [ActionName("comment")]
        public HttpResponseMessage PutComment(int postId, PostModel model,
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
                            throw new ArgumentNullException("You dont have session");
                        }
                        var user = context.Users.FirstOrDefault(
                                        usr => usr.SessionKey == sessionKey);

                        if (user == null)
                        {
                            throw new InvalidOperationException("Your session is expired");
                        }

                        if (model.Text == null)
                        {
                            throw new ArgumentNullException("You dont have text");
                        }
                        if (postId <= 0)
                        {
                            throw new ArgumentOutOfRangeException("Incorrect request");
                        }

                        var post = context.Posts.FirstOrDefault(p => p.Id == postId);

                        if (post == null)
                        {
                            throw new ArgumentNullException("No post found");
                        }


                        var comment = new Comment()
                        {
                            Text = model.Text,
                            Author = user,
                            PostDate = DateTime.Now
                        };

                        post.Comments.Add(comment);
                        context.SaveChanges();

                        var response =
                          this.Request.CreateResponse(HttpStatusCode.OK);
                        return response;
                    }
                });
            return responseMsg;
        }



        /*
        { "title": "NEW POST",
        "tags": ["post"],
        "text": "this is just a test post" }
         */
        public HttpResponseMessage PostAdd(PostModel model,
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
                            throw new ArgumentNullException("You dont have session");
                        }
                        var user = context.Users.FirstOrDefault(
                                        usr => usr.SessionKey == sessionKey);

                        if (user == null)
                        {
                            throw new InvalidOperationException("Your session is expired");
                        }
                        if (model.Title == null)
                        {
                            throw new ArgumentNullException("You dont have title");
                        }
                        if (model.Text == null)
                        {
                            throw new ArgumentNullException("You dont have text");
                        }

                        var post = new Post()
                        {
                            Title = model.Title,
                            Text = model.Text,
                            Author = user,
                            PostDate = DateTime.Now
                        };

                        string[] tagsTile = model.Title.Split(new char[] { ' ' });
                        for (int i = 0; i < tagsTile.Length; i++)
                        {
                            var tagEnt = new Tag()
                            {
                                Name = tagsTile[i].ToLower()
                            };

                            var tag = context.Tags.FirstOrDefault(t => t.Name == tagEnt.Name);

                            if (tag == null)
                            {
                                post.Tags.Add(tagEnt);
                            }
                            else
                            {
                                post.Tags.Add(tag);
                            }
                        }
                        if (model.Tags != null)
                        {
                            foreach (var tag in model.Tags)
                            {

                                var tagEnt = new Tag()
                                {
                                    Name = tag.ToLower()
                                };

                                var tagTitle = context.Tags.FirstOrDefault(t => t.Name == tagEnt.Name);

                                if (tagTitle == null)
                                {
                                    post.Tags.Add(tagEnt);
                                }
                                else
                                {
                                    post.Tags.Add(tagTitle);
                                }
                            }
                        }

                        context.Posts.Add(post);
                        context.SaveChanges();


                        var createdPost = new CreatedPostModel()
                        {
                            Id = post.Id,
                            Title = post.Title
                        };

                        var response =
                          this.Request.CreateResponse(HttpStatusCode.Created, createdPost);
                        return response;
                    }
                });
            return responseMsg;
        }
    }

}
