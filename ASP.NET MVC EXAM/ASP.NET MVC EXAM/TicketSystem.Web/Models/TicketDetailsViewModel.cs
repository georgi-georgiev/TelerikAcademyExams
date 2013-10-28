using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketSystem.Web.Models
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string Priority { get; set; }

        public string ScreenshotURL { get; set; }

        public string Description { get; set; }

        public virtual IEnumerable<CommentViewModel> Comments { get; set; }
    }
}