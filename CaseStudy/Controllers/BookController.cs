using CaseStudy.Models;
using CaseStudy.Repository;
using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace CaseStudy.Controllers
{
    public class BookController : Controller
    {
        private static int? borrowerId, ownerId;
        private string url = string.Format("{0}://{1}:{2}", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port);
        private static string otpCheck;
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
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }
            List<Book> books = bookRepository.GetAll().Where(x => x.IsBorrowed != null).ToList();
            return View(books);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    string fileName = Path.GetFileName(Image.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Image/"));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Image.SaveAs(path + fileName);

                    book.Image = fileName;
                    if (bookRepository.Insert(book))
                    {
                        var booK = bookRepository.GetAll().OrderByDescending(u => u.Id).FirstOrDefault();
                        string mess = this.url + "/Book/ConfirmAddBook/" + booK.Id;
                        bookRepository.SendMail().sendConfirm("Xác nhận", book.Owner.Email, mess);
                        TempData["message"] = "Libanon sent link comfirm "+ book.Title + " book to " + book.Owner.Email + "!!";
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
                TempData["message"] = book.Title + " book is comfirmed!!";
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
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
                return View(booK);
            }
            bookRepository.SendMail().sendOTP("Mã OTP của bạn là ", booK.Owner.Email);
            otpCheck = bookRepository.SendMail().getOTP();
            return View(booK);
        }

        [HttpPost]
        public ActionResult ConfirmOTP(Book book, HttpPostedFileBase Image, string otp)
        {
            if(otp != otpCheck)
            {
                TempData["message"] = "OTP Incorrect!!";
                return ConfirmOTP(book.Id);
            }
            if (Image == null)
            {
                book.Image = bookRepository.GetItem(book.Id).Image;
                if (bookRepository.Update(book) == true)
                {
                    TempData["message"] = book.Title + " book is updated!!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string fileName = Path.GetFileName(Image.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Image/"));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Image.SaveAs(path + fileName);

                book.Image = fileName;
                if (bookRepository.Update(book) == true)
                {
                    TempData["message"] = book.Title + " book is updated!!";
                    return RedirectToAction("Index");
                }
            }
            return View(book);
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

            book.Borrower = borrower;
            if (bookRepository.UpdateBookBorrower(book) == true)
            {
                string mess = string.Format("{0}{1}{2}", this.url, "/Book/ConfirmBorrowBook/", book.Id);
                bookRepository.SendMail().sendConfirm("Xác nhận", book.Borrower.Email, mess);
                TempData["message"] = "Libanon sent link comfirm borrowing " + book.Title + " book in " + book.Borrower.Email;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ConfirmBorrowBook(int Id)
        {
            var book = bookRepository.GetItem(Id);
            string mess = string.Format("Accept: {0}{1}{2}\nReject: {3}{4}{5}", this.url, "/Book/Accept/", book.Id, this.url, "/Book/Reject/", book.Id);
            bookRepository.SendMail().sendConfirm("Xác nhận", book.Owner.Email, mess);
            TempData["message"] = "Libanon sent link accept or reject about borrowing " + book.Title + " book to owner email " + book.Owner.Email;
            return RedirectToAction("Index");
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
            TempData["message"] = book.Owner.OwnerName + " reject borrowing of " + borrower.BorrowerName;
            return RedirectToAction("Index");
        }

        public ActionResult Accept(int Id)
        {
            var book = bookRepository.GetItem(Id);
            var borrower = borrowerRepository.GetItem((int)book.BorrowerId);
            book.BorrowerId = borrower.BorrowerId;
            string messToOwner = "Bạn đã đồng ý yêu cầu mượn sách với các thông tin:\nTên sách: " + book.Title + "\nNgười mượn: " + borrower.BorrowerName
                + "\nXác nhận đã trao: " + this.url + "/Book/ConfirmBorrowingBook/" + book.OwnerId;
            string messToBorrower = "Yêu cần mượn sách của bạn đã được chấp nhận, thông tin sách:\nTên sách: " + book.Title + "\nChủ sở hữu: " + book.Owner.OwnerName
                + "\nXác nhận đã nhận: " + this.url + "/Book/ConfirmBorrowingBook/" + book.BorrowerId;
            bookRepository.SendMail().sendConfirm("Thông báo", borrower.Email, messToBorrower);
            bookRepository.SendMail().sendConfirm("Thông báo", book.Owner.Email, messToOwner);
            TempData["message"] = book.Owner.OwnerName + " accept borrowing of " + borrower.BorrowerName;
            return RedirectToAction("Index");
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
            var borrowingBook = bookRepository.GetAll().Where(x => x.BorrowerId == borrowerId && x.OwnerId == ownerId && x.IsBorrowed == false).FirstOrDefault();
            if(borrowingBook != null)
            {
                borrowingBook.IsBorrowed = true;
                if (bookRepository.UpdateBookBorrower(borrowingBook) == true)
                {
                    ownerId = null;
                    borrowerId = null;
                    TempData["message"] = "Borrowing " + borrowingBook.Title + " is done!!";
                }
            }          
            return RedirectToAction("Index");
        }

        public ActionResult Return(int Id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }
            var book = bookRepository.GetItem(Id);
            return View();
        }

        [HttpPost]
        public ActionResult Return(Book book)
        {
            var booK = bookRepository.GetItem(book.Id);
            var borrower = borrowerRepository.GetItem((int)booK.BorrowerId);
            if (book.Borrower.Email != borrower.Email)
            {
                TempData["message"] = "Email: " + book.Borrower.Email + " don't borrow " + booK.Title + " book!!";
                return Return(booK.Id);
            }
            string messToOwner = "Sách của bạn sẽ được trả lại bởi " + borrower.BorrowerName + ", vui lòng liên hệ: " + borrower.PhoneNumber + " để nhận sách"
                    + "\nXác nhận đã nhận lại: " + this.url + "/Book/ConfirmReturningBook/" + booK.OwnerId;
            string messToBorrower = "Yêu cần của bạn đã được gửi về hệ thống, thông tin chủ sở hữu:\nChủ sở hữu: " + booK.Owner.OwnerName + "\nLiên hệ: " + booK.Owner.PhoneNumber
                + "\nXác nhận đã trả sách: " + this.url + "/Book/ConfirmReturningBook/" + booK.BorrowerId;
            bookRepository.SendMail().sendConfirm("Thông báo", borrower.Email, messToBorrower);
            bookRepository.SendMail().sendConfirm("Thông báo", booK.Owner.Email, messToOwner);
            TempData["message"] = borrower.BorrowerName + " will return " + booK.Title + " book to " + booK.Owner.OwnerName;
            return RedirectToAction("Index");
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
            var borrowingBook = bookRepository.GetAll().Where(x => x.BorrowerId == borrowerId && x.OwnerId == ownerId && x.IsBorrowed == true).FirstOrDefault();
            if (borrowingBook != null)
            {
                borrowingBook.IsBorrowed = false;
                if (bookRepository.UpdateBookBorrower(borrowingBook) == true)
                {
                    ownerId = null;
                    borrowerId = null;
                    TempData["message"] = "Returning " + borrowingBook.Title + "is done!!";
                }
            }          
            return RedirectToAction("Index");
        }

        public ActionResult Rate(int Id)
        {
            IList<SelectListItem> rateList = new List<SelectListItem>
            {
                new SelectListItem{Text = "1", Value = "1"},
                new SelectListItem{Text = "2", Value = "2"},
                new SelectListItem{Text = "3", Value = "3"},
                new SelectListItem{Text = "4", Value = "4"},
                new SelectListItem{Text = "5", Value = "5"}

            };
            ViewBag.RateList = rateList;
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
                int numberOfRating = booK.ISBN.NumberOfRating;
                double totalRateScore = (newRateScore + oldRateScore * numberOfRating) / (numberOfRating + 1);
                booK.ISBN.RateScore = Math.Round(totalRateScore, 1);
                booK.ISBN.NumberOfRating = numberOfRating + 1;
            }
            else
            {
                booK.ISBN.RateScore = newRateScore;
                booK.ISBN.NumberOfRating = 1;
            }
            if (bookRepository.Update(booK) == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}