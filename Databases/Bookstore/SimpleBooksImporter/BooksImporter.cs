using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Bookstore.Data;

namespace SimpleBooksImporter
{
    static class BooksImporter
    {
        static void Main(string[] args)
        {
            ReadFromXML();
        }

        static void ReadFromXML()
        {
            TransactionScope tran = new TransactionScope(
            TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.RepeatableRead
                });
            using (tran)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("../../simple-books.xml");
                string xPathQuery = "/catalog/book";

                XmlNodeList bookList = xmlDoc.SelectNodes(xPathQuery);
                foreach (XmlNode bookNode in bookList)
                {
                    string title = bookNode.GetNodeContent("title");
                    string isbn = bookNode.GetNodeContent("isbn");
                    string author = bookNode.GetNodeContent("author");
                    decimal price = ParseNullableDecimal(bookNode.GetNodeContent("price"));
                    string website = bookNode.GetNodeContent("web-site");

                    BookstoreDAL.AddBook(title, isbn, author, price, website);
                }
                tran.Complete();
            }
        }

        private static string GetNodeContent(this XmlNode node, string tagName)
        {
            XmlNode childNode = node.SelectSingleNode(tagName);
            if (childNode == null)
            {
                return null;
            }
            return childNode.InnerText.Trim();
        }

        private static decimal ParseNullableDecimal(this string value)
        {
            decimal i;
            if (decimal.TryParse(value, out i)) return i;
            return 0;
        }
    }
}
