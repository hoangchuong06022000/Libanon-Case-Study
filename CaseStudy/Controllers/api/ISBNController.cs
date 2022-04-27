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
    public class ISBNController : ApiController
    {
        private IRepository<ISBN> isbnRepository;

        public ISBNController(IRepository<ISBN> repository)
        {
            this.isbnRepository = repository;
        }
        public IHttpActionResult GetAllCategories()
        {
            IList<ISBN> isbnList = isbnRepository.GetAll();

            if (isbnList.Count == 0)
            {
                return NotFound();
            }

            return Ok(isbnList);
        }

        public IHttpActionResult GetDetail(int Id)
        {
            ISBN isbn = isbnRepository.GetItem(Id);

            if (isbn == null)
            {
                return NotFound();
            }

            return Ok(isbn);
        }
        public IHttpActionResult PostNewCategory(ISBN isbn)
        {
            if (ModelState.IsValid)
            {
                if (isbnRepository.Insert(isbn))
                {
                    return Ok();
                }
            }
            return BadRequest("Invalid data.");
        }

        public IHttpActionResult PutExistedCategory(ISBN isbn)
        {
            if (ModelState.IsValid)
            {
                if (isbnRepository.Update(isbn))
                {
                    return Ok();
                }
            }
            return BadRequest("Invalid data.");
        }

        public IHttpActionResult DeleteExistedCategory(int Id)
        {
            if (ModelState.IsValid)
            {
                if (isbnRepository.Delete(Id))
                {
                    return Ok();
                }
            }
            return BadRequest("This isbn is not exist in list isbns!.");
        }
    }
}
