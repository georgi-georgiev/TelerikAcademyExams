﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Category
    {
        public Category()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
