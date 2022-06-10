using CaseStudy.Models;
using CaseStudy.Repository;
using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaseStudy.Controllers
{
    public class BookController : Controller
    {
        private static int borrowerId, ownerId;
        // GET: Book
        readonly IRepository<Book> bookRepository;
        readonly IRepository<Borrower> borrowerRepository;
        public BookController(IRepository<Book> repositoryBook, IRepository<Borrower> repositoryBorrower)
        {
            this.bookRepository = repositoryBook;
            this.borrowerRepository = repositoryBorrower;
        }

        public ActionResult Index()
        {
            List<Book> books = bookRepository.GetAll().Where(x => x.IsBorrowed != null).ToList();
            return View(books);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book, HttpPostedFileBase imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    string fileName = Path.GetFileName(imageUrl.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Image/"));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    imageUrl.SaveAs(path + fileName);

                    book.Image = fileName;
                    if (bookRepository.Insert(book))
                    {
                        var booK = bookRepository.GetAll().OrderByDescending(u => u.Id).FirstOrDefault();
                        string mess = "http://localhost:61698/Book/ConfirmAddBook/" + booK.Id;
                        bookRepository.SendMail().sendConfirm("Xác nhận", book.Owner.Email, mess);
                        return RedirectToAction("Index");
                    }

                }
            }
            return View();
        }

        public ActionResult ConfirmAddBook(int Id)
        {
            var book = bookRepository.GetItem(Id);
            book.IsBorrowed = false;
            if (bookRepository.Update(book) == true)
            {
                return View(book);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int Id)
        {
            var book = bookRepository.GetItem(Id);
            return View(book);
        }

        public ActionResult Delete(int Id)
        {
            if (bookRepository.Delete(Id) == true)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            var book = bookRepository.GetItem(Id);
            
            return View(book);
        }

        public ActionResult ConfirmOTP(int Id)
        {
            var booK = bookRepository.GetItem(Id);
            bookRepository.SendMail().sendOTP("Mã OTP của bạn là ", booK.Owner.Email);
            ViewBag.OTP = bookRepository.SendMail().getOTP();
            Debug.WriteLine(bookRepository.SendMail().getOTP());
            return View(booK);
        }

        [HttpPost]
        public ActionResult ConfirmOTP(Book book, HttpPostedFileBase ImageUrl)
        {
            book.IsBorrowed = false;
            if (ImageUrl == null)
            {
                book.Image = bookRepository.GetItem(book.Id).Image;
                if (bookRepository.Update(book) == true)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string fileName = Path.GetFileName(ImageUrl.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Image/"));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                ImageUrl.SaveAs(path + fileName);

                book.Image = fileName;
                if (bookRepository.Update(book) == true)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Edit", book);           
        }

        public ActionResult Borrow(int Id)
        {
            var book = bookRepository.GetItem(Id);
            return View(book);
        }
        [HttpPost]
        public ActionResult Borrow(Book book)
        {
            Borrower borrower = new Borrower()
            {
                BorrowerName = book.Borrower.BorrowerName,
                PhoneNumber = book.Borrower.PhoneNumber,
                Email = book.Borrower.Email
            };
            
            if (borrowerRepository.Insert(borrower) == true)
            {
                book.Borrower = borrower;
                if (bookRepository.UpdateBookBorrower(book) == true)
                {
                    string mess = "http://localhost:61698/Book/ConfirmBorrowBook/" + book.Id;
                    bookRepository.SendMail().sendConfirm("Xác nhận", book.Borrower.Email, mess);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult ConfirmBorrowBook(int Id)
        {
            var book = bookRepository.GetItem(Id);
            string mess = "Accept: http://localhost:61698/Book/Accept/" + book.Id + "\nReject: http://localhost:61698/Book/Reject/" + book.Id;
            bookRepository.SendMail().sendConfirm("Xác nhận", book.Owner.Email, mess);
            return View();
        }

        public ActionResult Reject(int Id)
        {
            var book = bookRepository.GetItem(Id);
            var borrower = book.Borrower;
            book.BorrowerId = null;
            if (bookRepository.UpdateBookBorrower(book) == true)
            {
                string messToOwner = "Bạn đã từ chối yêu cầu mượn sách của " + borrower.BorrowerName;
                string messToBorrower = "Chủ sở hữu sách đã từ chối yêu cầu mượn sách của bạn";
                bookRepository.SendMail().sendConfirm("Thông báo", borrower.Email, messToBorrower);
                bookRepository.SendMail().sendConfirm("Thông báo", book.Owner.Email, messToOwner);
            }
            return View();
        }

        public ActionResult Accept(int Id)
        {
            var book = bookRepository.GetItem(Id);
            var borrower = borrowerRepository.GetItem((int)book.BorrowerId);
            book.BorrowerId = borrower.BorrowerId;
            string messToOwner = "Bạn đã đồng ý yêu cầu mượn sách với các thông tin:\nTên sách: " + book.Title + "\nNgười mượn: " + borrower.BorrowerName
                + "\nXác nhận đã trao: " + "http://localhost:61698/Book/ConfirmBorrowingBook/" + book.BorrowerId;
            string messToBorrower = "Yêu cần mượn sách của bạn đã được chấp nhận, thông tin sách:\nTên sách: " + book.Title + "\nChủ sở hữu: " + book.Owner.OwnerName
                + "\nXác nhận đã nhận: " + "http://localhost:61698/Book/ConfirmBorrowingBook/" + book.OwnerId;
            bookRepository.SendMail().sendConfirm("Thông báo", borrower.Email, messToBorrower);
            bookRepository.SendMail().sendConfirm("Thông báo", book.Owner.Email, messToOwner);
            return View();
        }

        public ActionResult ConfirmBorrowingBook(int Id)
        {
            var owner = bookRepository.GetAll().Where(x => x.OwnerId == Id).FirstOrDefault();
            var borrower = borrowerRepository.GetItem(Id);
            if(owner != null)
            {
                ownerId = Id;
            }
            if (borrower != null)
            {
                borrowerId = Id;
            }
            var borrowingBook = bookRepository.GetAll().Where(x => x.BorrowerId == borrowerId && x.OwnerId == ownerId).FirstOrDefault();
            if(borrowingBook != null)
            {
                borrowingBook.IsBorrowed = true;
                if (bookRepository.Update(borrowingBook) == true)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Return(int Id)
        {
            var book = bookRepository.GetItem(Id);
            return View();
        }

        [HttpPost]
        public ActionResult Return(Book book)
        {
            var booK = bookRepository.GetItem(book.Id);
            var borrower = borrowerRepository.GetItem((int)booK.BorrowerId);
            if (booK.Borrower.Email == borrower.Email)
            {
                string messToOwner = "Sách của bạn sẽ được trả lại bởi " + borrower.BorrowerName + ", vui lòng liên hệ: " + borrower.PhoneNumber + " để nhận sách"
                    + "\nXác nhận đã nhận lại: " + "http://localhost:61698/Book/ConfirmReturningBook/" + booK.OwnerId;
                string messToBorrower  = "Yêu cần của bạn đã được gửi về hệ thống, thông tin chủ sở hữu:\nChủ sở hữu: " + booK.Owner.OwnerName + "\nLiên hệ: " + booK.Owner.PhoneNumber
                    + "\nXác nhận đã trả sách: " + "http://localhost:61698/Book/ConfirmReturningBook/" +booK.BorrowerId;
                bookRepository.SendMail().sendConfirm("Thông báo", borrower.Email, messToBorrower);
                bookRepository.SendMail().sendConfirm("Thông báo", booK.Owner.Email, messToOwner);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ConfirmReturningBook(int Id)
        {
            var owner = bookRepository.GetAll().Where(x => x.OwnerId == Id).FirstOrDefault();
            var borrower = borrowerRepository.GetItem(Id);
            if (owner != null)
            {
                ownerId = Id;
            }
            if (borrower != null)
            {
                borrowerId = Id;
            }
            var borrowingBook = bookRepository.GetAll().Where(x => x.BorrowerId == borrowerId && x.OwnerId == ownerId).FirstOrDefault();
            if (borrowingBook != null)
            {
                borrowingBook.IsBorrowed = false;
                borrowingBook.BorrowerId = null;
                if (bookRepository.Update(borrowingBook) == true)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Rate(int Id)
        {
            var book = bookRepository.GetItem(Id);
            return View(book);
        }

        [HttpPost]
        public ActionResult Rate(Book book)
        {
            var booK = bookRepository.GetItem(book.Id);
            double newRateScore = (double)book.ISBN.RateScore;
            if(booK.ISBN.RateScore != null)
            {
                double oldRateScore = (double)booK.ISBN.RateScore;
                booK.ISBN.RateScore = (newRateScore + oldRateScore) / 2;
            }
            else
            {
                booK.ISBN.RateScore = newRateScore;
            }
            if (bookRepository.Update(booK) == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}