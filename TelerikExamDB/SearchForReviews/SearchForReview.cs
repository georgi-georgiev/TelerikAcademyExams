using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bookstore.Data;
using SearchLogs;

namespace SearchForReviews
{
    static class SearchForReview
    {
        static void Main(string[] args)
        {
            WriteXml();
            //Problem 7. Search Logs (Code First)
            Client.InsertSearchLog(DateTime.Now);
        }

        static void WriteXml()
        {
            string fileName = "../../reviews-search-results.xml";
            using (XmlTextWriter writer =
                new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;

                writer.WriteStartDocument();
                writer.WriteStartElement("search-results");

                ReadFromXML(writer);

                writer.WriteEndDocument();
            }
        }
        static void ReadFromXML(XmlTextWriter writer)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("../../review-queries.xml");
            string xPathQuery = "/review-queries";

            XmlNodeList bookList = xmlDoc.SelectNodes(xPathQuery);
            foreach (XmlNode bookNode in bookList)
            {
                foreach(XmlNode queryNode in bookNode.SelectNodes("query"))
                {
                    string type = queryNode.Attributes["type"].Value;
                    string startDate = queryNode.GetNodeContent("start-date");
                    string endDate = queryNode.GetNodeContent("end-date");
                    string author = queryNode.GetNodeContent("author-name");

                    if (type == "by-period")
                    {
                        var reviews = BookstoreDAL.FindReviewsByPeriod(startDate, endDate);
                        BookstoreDAL.WriteReviews(writer, reviews);
                    }
                    else
                    {
                        var reviews = BookstoreDAL.FindReviewsByAuthor(author);
                        BookstoreDAL.WriteReviews(writer, reviews);
                    }
                }
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
    }
}
