using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TicketSystem.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Required]
        public string Priority { get; set; }

        public string ScreenshotURL { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
