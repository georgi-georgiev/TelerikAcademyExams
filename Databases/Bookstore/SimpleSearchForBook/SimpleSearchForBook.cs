using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bookstore.Data;

namespace SimpleSearchForBook
{
    static class SimpleSearchForBook
    {
        static void Main(string[] args)
        {
            ReadFromXML();
        }
        static void ReadFromXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("../../simple-query.xml");
            string xPathQuery = "/query";

            XmlNodeList bookList = xmlDoc.SelectNodes(xPathQuery);
            foreach (XmlNode bookNode in bookList)
            {
                string title = bookNode.GetNodeContent("title");
                string author = bookNode.GetNodeContent("author");
                string isbn = bookNode.GetNodeContent("isbn");
                var books = BookstoreDAL.SearchForBook(title, author, isbn);
                if (books.Count > 0)
                {
                    Dictionary<string, int> booksTitles = new Dictionary<string, int>();
                    string sameTitle = books[0].Title;
                    int sameTitleCounter = 0;
                    for (int i = 1; i < books.Count; i++)
			        {
                        if (books[i].Title == sameTitle)
                        {
                            sameTitleCounter++;
                        }
                        else if(books[i].Title != sameTitle && i == books.Count - 1)
                        {
                            booksTitles.Add("asd", 5);
                            booksTitles.Add(sameTitle, sameTitleCounter);
                            sameTitle = books[i].Title;
                            sameTitleCounter = 0;
                        }
                        if (i == books.Count - 1)
                        {
                            booksTitles.Add(sameTitle, sameTitleCounter);
                            sameTitle = books[i].Title;
                            sameTitleCounter = 0;
                        }
			        }

                    foreach (var booktitle in booksTitles)
                    {
                        Console.WriteLine(booktitle.Key + " --> " + booktitle.Value + " reviews"); 
                    }
                    Console.WriteLine();
                    
                }
                else
                {
                    Console.WriteLine("Nothing Found");
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
