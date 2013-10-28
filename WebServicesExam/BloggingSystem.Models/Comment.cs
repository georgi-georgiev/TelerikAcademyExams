using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloggingSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual User Author { get; set; }
        public DateTime PostDate { get; set; }
        public virtual Post Post { get; set; }
    }
}
