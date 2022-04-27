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
    public class BorrowerController : ApiController
    {
        private IRepository<Borrower> borrowerRepository;

        public BorrowerController(IRepository<Borrower> repository)
        {
            this.borrowerRepository = repository;
        }
        public IHttpActionResult GetAllBorrowers()
        {
            IList<Borrower> borrowerList = borrowerRepository.GetAll();

            if (borrowerList.Count == 0)
            {
                return NotFound();
            }

            return Ok(borrowerList);
        }

        public IHttpActionResult GetDetail(int Id)
        {
            Borrower borrower = borrowerRepository.GetItem(Id);

            if (borrower == null)
            {
                return NotFound();
            }

            return Ok(borrower);
        }
        public IHttpActionResult PostNewOwner(Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                if (borrowerRepository.Insert(borrower))
                {
                    return Ok();
                }
            }
            return BadRequest("Invalid data.");
        }

        public IHttpActionResult PutExistedOwner(Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                if (borrowerRepository.Update(borrower))
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
                if (borrowerRepository.Delete(Id))
                {
                    return Ok();
                }
            }
            return BadRequest("The borrower is not exist in list borrowers!.");
        }
    }
}
