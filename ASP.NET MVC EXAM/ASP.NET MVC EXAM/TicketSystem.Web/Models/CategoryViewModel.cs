using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketSystem.Web.Models
{
    public class CategoryViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}