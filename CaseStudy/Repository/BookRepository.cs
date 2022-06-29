using CaseStudy.Models;
using CaseStudy.Tool;
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
        private SendMail sendMail = new SendMail();
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
                        ISBN = new ISBN() { ISBNString = item.ISBN.ISBNString},
                        Summary = item.Summary,
                        Category = item.Category,
                        OwnerId = book.OwnerId,
                        IsBorrowed = null
                    };
                    db.Books.Add(booK);
                    db.SaveChanges();
                    Debug.WriteLine(booK.ISBN.ISBNString);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public SendMail SendMail()
        {
            return sendMail;
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
                book.ISBN = new ISBN() 
                { 
                    ISBNString = item.ISBN.ISBNString,
                    RateScore = item.ISBN.RateScore,
                    NumberOfRating = item.ISBN.NumberOfRating
                };
                book.Summary = item.Summary;
                book.Category = item.Category;
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
                Borrower borrower = null;
                if (item.Borrower.Email != null)
                {
                    borrower = db.Borrowers.Where(x => x.Email == item.Borrower.Email).FirstOrDefault();
                }
                
                db.Books.Attach(book);
                if (item.IsBorrowed != null)
                {
                    book.IsBorrowed = item.IsBorrowed;
                }
                if (item.IsBorrowed == false)
                {
                    item.BorrowerId = null;
                    book.IsBorrowed = item.IsBorrowed;
                }
                if (borrower != null)
                {
                    book.Borrower = borrower;
                }
                else
                {
                    book.Borrower = item.Borrower;
                }
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