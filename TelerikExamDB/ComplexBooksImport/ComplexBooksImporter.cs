using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bookstore.Data;

namespace ComplexBooksImport
{
    static class ComplexBooksImporter
    {
        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("../../complex-books.xml");
            string xPathQuery = "/catalog/book";

            XmlNodeList bookList = xmlDoc.SelectNodes(xPathQuery);
            foreach (XmlNode bookNode in bookList)
            {
                string title = bookNode.GetNodeContent("title");
                string isbn = bookNode.GetNodeContent("isbn");

                List<string> authors = new List<string>();
                string authorsNode = bookNode.GetNodeContent("authors");
                if (authorsNode != null)
                {
                    foreach (XmlNode author in bookNode.SelectSingleNode("authors").SelectNodes("author"))
                    {
                        authors.Add(author.InnerText);
                    }
                }

                decimal price = ParseNullableDecimal(bookNode.GetNodeContent("price"));
                string website = bookNode.GetNodeContent("web-site");

                List<string> reviews = new List<string>();
                List<string> reviewsDates = new List<string>();
                List<string> reviewsAuthors = new List<string>();
                string reviewsNode = bookNode.GetNodeContent("reviews");
                if (reviewsNode != null)
                {
                    foreach (XmlNode review in bookNode.SelectNodes("reviews"))
                    {
                        foreach (XmlNode rev in review)
                        {
                            reviews.Add(rev.InnerText.Trim());

                            if (rev.Attributes["date"] != null)
                            {
                                reviewsDates.Add(rev.Attributes["date"].Value.ToString());
                            }
                            else
                            {
                                reviewsDates.Add(DateTime.Now.ToString("dd-MMM-yyy"));
                            }

                            if (rev.Attributes["author"] != null)
                            {
                                reviewsAuthors.Add(rev.Attributes["author"].Value);
                            }
                            else
                            {
                                reviewsAuthors.Add("anonymous");
                            }
                        }
                    }
                }

                BookstoreDAL.AddBooksReview(title, isbn, authors, price, website,
                    reviews, reviewsDates, reviewsAuthors);
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
