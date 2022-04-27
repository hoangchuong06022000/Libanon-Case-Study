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
    public class OwnerController : ApiController
    {
        private IRepository<Owner> ownerRepository;

        public OwnerController(IRepository<Owner> repository)
        {
            this.ownerRepository = repository;
        }
        public IHttpActionResult GetAllOwners()
        {
            IList<Owner> ownerList = ownerRepository.GetAll();

            if (ownerList.Count == 0)
            {
                return NotFound();
            }

            return Ok(ownerList);
        }

        public IHttpActionResult GetDetail(int Id)
        {
            Owner owner = ownerRepository.GetItem(Id);

            if (owner == null)
            {
                return NotFound();
            }

            return Ok(owner);
        }
        public IHttpActionResult PostNewOwner(Owner owner)
        {
            if (ModelState.IsValid)
            {
                if (ownerRepository.Insert(owner))
                {
                    return Ok();
                }
            }
            return BadRequest("Invalid data.");
        }

        public IHttpActionResult PutExistedOwner(Owner owner)
        {
            if (ModelState.IsValid)
            {
                if (ownerRepository.Update(owner))
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
                if (ownerRepository.Delete(Id))
                {
                    return Ok();
                }
            }
            return BadRequest("The owner is not exist in list owners!.");
        }
    }
}
