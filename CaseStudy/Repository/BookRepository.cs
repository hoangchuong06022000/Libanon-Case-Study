using CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CaseStudy.Repository
{
    public class BookRepository : IRepository<Book>
    {
        private LibanonContext db = new LibanonContext();
        public bool Delete(int Id)
        {
            try
            {
                Book book = db.Books.Where(x => x.Id == Id).FirstOrDefault();
                db.Books.Remove(book);
                db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Book> GetAll()
        {
            return db.Books.ToList();
        }

        public Book GetItem(int Id)
        {
            return db.Books.Where(x => x.Id == Id).FirstOrDefault();
        }

        public bool Insert(Book item)
        {
            try
            {
                Book book = db.Books.Where(x => x.Owner.Email == item.Owner.Email).FirstOrDefault();
                
                if(book == null)
                {
                    db.Books.Add(item);
                    db.SaveChanges();
                }
                else
                {
                    Book booK = new Book()
                    {
                        Title = item.Title,
                        Author = item.Author,
                        Image = item.Image,
                        PublishYear = item.PublishYear,
                        Summary = item.Summary,
                        Category = item.Category,
                        OwnerId = book.OwnerId,
                        IsBorrowed = true
                    };
                    db.Books.Add(booK);
                    db.SaveChanges();
                    Debug.WriteLine(booK.OwnerId);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Update(Book item)
        {
            try
            {
                Book book = db.Books.Find(item.Id);
                db.Books.Attach(book);
                book.Title = item.Title;
                book.Author = item.Author;
                book.Image = item.Image;
                book.PublishYear = item.PublishYear;
                book.ISBN.ISBNString = item.ISBN.ISBNString;
                book.ISBN.RateScore = item.ISBN.RateScore;
                book.Summary = item.Summary;
                book.Category = item.Category;
                book.IsBorrowed = item.IsBorrowed;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateBookBorrower(Book item)
        {
            try
            {
                Book book = db.Books.Find(item.Id);
                db.Books.Attach(book);
                book.BorrowerId = item.BorrowerId;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}