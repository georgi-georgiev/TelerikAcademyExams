using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class ApplicationUser : User
    {
        public ApplicationUser()
        {
            this.Points = 10;
        }
        public int Points { get; set; }
    }
}
