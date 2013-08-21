using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bookstore.Data
{
    public class BookstoreDAL
    {
        public static void AddBook(string title, string isbn, string author, decimal price, string website)
        {
            BookstoreEntities context = new BookstoreEntities();
            Book book = new Book();
            book.Title = title;
            book.ISBN = isbn;
            Author newAuthor = CreateOrLoadAuthor(context, author);
            book.Authors.Add(newAuthor);
            book.Price = price;
            book.Website = website;

            context.Books.Add(book);

            context.SaveChanges();

        }

        public static void AddBooksReview(string title, string isbn, List<string> authors, decimal price, string website,
            List<string> reviews, List<string> reviewsDates, List<string> reviewsAuthors)
        {
            BookstoreEntities context = new BookstoreEntities();

            Book book = new Book();
            book.Title = title;
            book.ISBN = isbn;
            foreach (var author in authors)
            {
                Author newAuthor = CreateOrLoadAuthor(context, author);
                book.Authors.Add(newAuthor);
            }
            book.Price = price;
            book.Website = website;
            context.Books.Add(book);
            for (int i = 0; i < reviews.Count; i++)
            {
                Review review = CreateReview(context, book, reviews[i], reviewsDates[i], reviewsAuthors[i]);
                context.Reviews.Add(review);
            }

            context.SaveChanges();
        }

        private static Review CreateReview(BookstoreEntities context, Book book, string reviewText, string reviewsDate, string reviewsAuthors)
        {
            Review review = new Review();

            review.Date = DateTime.Parse(reviewsDate);
            review.AuthorName = "anonymous";
            if (reviewsAuthors != null)
            {
                Author author = CreateOrLoadAuthor(context, reviewsAuthors);
                review.AuthorName = author.Name;

            }
            review.Text = reviewText;
            review.Book = book;
            context.Reviews.Add(review);
            context.SaveChanges();

            return review;
        }

        private static Author CreateOrLoadAuthor(BookstoreEntities context, string author)
        {
            Author existringAuthor =
                (from a in context.Authors
                 where a.Name == author
                 select a).FirstOrDefault();
            if (existringAuthor != null)
            {
                return existringAuthor;
            }

            Author newAuthor = new Author();
            newAuthor.Name = author;
            context.Authors.Add(newAuthor);
            context.SaveChanges();

            return newAuthor;
        }

        public static List<Book> SearchForBook(string title, string author, string isbn)
        {
            BookstoreEntities context = new BookstoreEntities();

            var bookstoreQuery =
                from b in context.Books
                select b;

            if (title != null)
            {
                bookstoreQuery =
                    from b in context.Books.Include("Authors")
                    where b.Title == title
                    select b;
            }

            if (author != null)
            {
                bookstoreQuery = bookstoreQuery.Where(b => b.Authors.Any(a => a.Name == author));
            }

            if (isbn != null)
            {
                bookstoreQuery = bookstoreQuery.Where(b => b.ISBN == isbn);
            }

            bookstoreQuery = bookstoreQuery.OrderBy(b => b.Title);

            return bookstoreQuery.ToList();
        }

        public static List<Review> FindReviewsByPeriod(string startDate, string endDate)
        {
            BookstoreEntities context = new BookstoreEntities();

            DateTime startedDate = DateTime.Parse(startDate);
            DateTime endedDate = DateTime.Parse(endDate);

            var reviewQuery =
                from r in context.Reviews
                select r;

            if (startDate != null)
            {
                reviewQuery =
                from r in context.Reviews
                where r.Date >= startedDate
                select r;
            }

            if (endDate != null)
            {
                reviewQuery =
                from r in context.Reviews
                where r.Date <= endedDate
                select r;
            }

            reviewQuery = reviewQuery.OrderBy(r => r.Date);

            return reviewQuery.ToList();
        }

        public static List<Review> FindReviewsByAuthor(string author)
        {
            BookstoreEntities context = new BookstoreEntities();

            var reviewQuery =
                from r in context.Reviews
                select r;
            if (author != null)
            {
                reviewQuery = reviewQuery.Where(r => r.AuthorName == author);
            }

            reviewQuery = reviewQuery.OrderBy(r => r.Date);

            return reviewQuery.ToList();
        }

        public static void WriteReviews(XmlTextWriter writer, List<Review> reviews)
        {
            writer.WriteStartElement("result-set");
            foreach (var review in reviews)
            {
                writer.WriteStartElement("review");
                if (review.Date != null)
                {
                    writer.WriteElementString("date", review.Date.ToString());
                }
                if (review.Text != null)
                {
                    writer.WriteElementString("content", review.Text);
                }
                if (review.Book != null)
                {
                    writer.WriteStartElement("book");
                    if (review.Book.Title != null)
                    {
                        writer.WriteElementString("title", review.Book.Title);
                    }
                    if (review.Book.Authors != null)
                    {
                        writer.WriteElementString("authors", string.Join(", ", review.Book.Authors));
                    }
                    if (review.Book.ISBN != null)
                    {
                        writer.WriteElementString("isbn", review.Book.ISBN);
                    }
                    if (review.Book.Website != null)
                    {
                        writer.WriteElementString("url", review.Book.Website);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
