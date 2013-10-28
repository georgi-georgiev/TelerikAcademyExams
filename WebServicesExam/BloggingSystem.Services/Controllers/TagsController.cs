using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using BloggingSystem.Data;
using BloggingSystem.Services.Attributes;
using BloggingSystem.Services.Models;

namespace BloggingSystem.Services.Controllers
{
    public class TagsController : ApiController
    {
        public IQueryable<TagModel> GetAllTags(
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

                var tags =
                    (from tag in context.Tags
                    select new TagModel()
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        PostsNumber = tag.Post.Count()
                    }).ToList();

                return tags.OrderBy(t => t.Name).AsQueryable();
            }
        }

        //api/tags/{tagId}/posts
        [ActionName("posts")]
        public IQueryable<PostModel> GetPostsByTagId(int tagId,
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

                if (tagId <= 0)
                {
                    throw new ArgumentOutOfRangeException("Incorrect request");
                }

                var tag = context.Tags.FirstOrDefault(t => t.Id == tagId);

                if (tag == null)
                {
                    throw new ArgumentNullException("No tag found");
                }

                var postModel =
                    (from post in tag.Post
                     select new PostModel()
                     {
                         Id = post.Id,
                         Title = post.Title,
                         Author = post.Author.DisplayName,
                         PostDate = post.PostDate,
                         Text = post.Text,
                         Tags = (
                         (from tagPost in post.Tags
                          select tagPost.Name)
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
    }
}
