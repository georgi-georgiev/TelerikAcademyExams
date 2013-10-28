using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using TicketSystem.Models;

namespace TicketSystem.Data
{
    public class DatabaseInitializer : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DatabaseInitializer()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.Tickets.Count() > 0 && context.Users.Count() > 0)
            {
                return;
            }

            Random rand = new Random();

            var user = new ApplicationUser()
            {
                UserName = "Gerasim",
                Points=15,
                Logins = new Collection<UserLogin>()
                {
                    new UserLogin()
                    {
                        LoginProvider = "Local",
                        ProviderKey = "admin",
                    }
                },
                Roles = new Collection<UserRole>()
                {
                    new UserRole()
                    {
                        Role = new Role("Admin")
                    }
                }
            };

            context.Users.Add(user);
            context.UserSecrets.Add(new UserSecret("admin",
                "ACQbq83L/rsvlWq11Zor2jVtz2KAMcHNd6x1SN2EXHs7VuZPGaE8DhhnvtyO10Nf5Q=="));

            var category = new Category()
            {
                Name = "Lili ivanova"
            };

            for (int i = 0; i < 10; i++)
            {
                Ticket ticket = new Ticket();
                ticket.Author = user;
                ticket.Priority = "Medium";
                ticket.ScreenshotURL = "http://www.ticketpro.bg/public/97/16/8f/1146220_715598__2410_Analgin_ticketpro_180p.jpg";
                ticket.Description = "Some description";
                ticket.Category = category;
                ticket.Title = "The home page is not working!";

                var commentsCount = rand.Next(0, 10);
                for (int j = 0; j < commentsCount; j++)
                {
                    ticket.Comments.Add(new Comment { Content = "Baaasi ticketa", User = user });
                }

                context.Tickets.Add(ticket);
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
