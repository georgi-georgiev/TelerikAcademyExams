using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloggingSystem.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Post> Post { get; set; }

        public Tag()
        {
            this.Post = new HashSet<Post>();
        }
    }
}
