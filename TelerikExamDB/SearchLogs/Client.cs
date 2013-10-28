using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Data;
using SearchLogs.Migrations;

namespace SearchLogs
{
    public  class Client
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion
            //    <BookstoreContext, Configuration>());
        }

        public static void InsertSearchLog(DateTime date)
        {
            BookstoreContext db = new BookstoreContext();
            
            string queryXml = System.IO.File.ReadAllText(@"..\..\..\SearchForReviews\reviews-search-results.xml");

            SearchLog log = new SearchLog();
            log.Date = date;
            log.QueryXml = queryXml;
            db.SearchLogs.Add(log);
            db.SaveChanges();
        }
    }
}
