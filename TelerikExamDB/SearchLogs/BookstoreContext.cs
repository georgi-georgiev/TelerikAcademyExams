using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Data.Migrations;

namespace SearchLogs
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext()
            : base("Bookstore")
        {
        }

        public DbSet<SearchLog> SearchLogs { get; set; }
    }
}
