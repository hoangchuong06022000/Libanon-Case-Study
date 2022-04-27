using CaseStudy.Models;
using CaseStudy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CaseStudy.Controllers.api
{
    public class BookController : ApiController
    {
        private IRepository<Book> bookRepository;

        public BookController(IRepository<Book> repository)
        {
            this.bookRepository = repository;
        }
        public IHttpActionResult GetAllBooks()
        {
            IList<Book> bookList = bookRepository.GetAll();

            if (bookList.Count == 0)
            {
                return NotFound();
            }

            return Ok(bookList);
        }

        public IHttpActionResult GetDetail(int Id)
        {
            Book book = bookRepository.GetItem(Id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        public IHttpActionResult PostNewBook(Book book)
        {
            if (ModelState.IsValid)
            {
                if (bookRepository.Insert(book))
                {
                    return Ok();
                }
            }
            return BadRequest("Invalid data.");
        }

        public IHttpActionResult PutExistedBook(Book book)
        {
            if (ModelState.IsValid)
            {
                if (bookRepository.Update(book))
                {
                    return Ok();
                }
            }
            return BadRequest("Invalid data.");
        }

        public IHttpActionResult DeleteExistedBook(int Id)
        {
            if (ModelState.IsValid)
            {
                if (bookRepository.Delete(Id))
                {
                    return Ok();
                }
            }
            return BadRequest("This book is not exist in list book!.");
        }
    }
}
