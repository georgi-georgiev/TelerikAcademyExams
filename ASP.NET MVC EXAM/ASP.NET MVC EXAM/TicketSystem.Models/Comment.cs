using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Ticket Ticket { get; set; }

        public string Content { get; set; }
    }
}
