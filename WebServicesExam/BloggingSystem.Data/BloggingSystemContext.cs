using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloggingSystem.Models;

namespace BloggingSystem.Data
{
    public class BloggingSystemContext : DbContext
    {
        public BloggingSystemContext()
            :base("BloggingSystemDb")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
