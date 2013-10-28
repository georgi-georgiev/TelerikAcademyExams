using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketSystem.Models;

namespace TicketSystem.Web.Models
{
    public class CreateTicketViewModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [ShouldNotContainWordBug]
        public string Title { get; set; }

        [Required]
        public string Priority { get; set; }

        public string ScreenshotURL { get; set; }

        [ShouldNotContainHTML]
        public string Description { get; set; }
    }
}