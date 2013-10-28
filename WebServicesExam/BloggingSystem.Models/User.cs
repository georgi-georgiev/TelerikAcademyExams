using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingSystem.Models
{
    public class User
    {
        
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string SessionKey { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public User()
        {
            this.Posts = new HashSet<Post>();
        }
    }
}
