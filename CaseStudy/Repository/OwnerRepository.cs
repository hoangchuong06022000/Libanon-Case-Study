using CaseStudy.Models;
using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaseStudy.Repository
{
    public class OwnerRepository : IRepository<Owner>
    {
        private LibanonContext db = new LibanonContext();
        public bool Delete(int Id)
        {
            try
            {
                Owner owner = db.Owners.Where(x => x.OwnerId == Id).FirstOrDefault();
                db.Owners.Remove(owner);
                db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Owner> GetAll()
        {
            return db.Owners.ToList();
        }

        public Owner GetItem(int Id)
        {
            return db.Owners.Where(x => x.OwnerId == Id).FirstOrDefault();
        }

        public bool Insert(Owner item)
        {
            try
            {
                db.Owners.Add(item);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public SendMail SendMail()
        {
            throw new NotImplementedException();
        }

        public bool Update(Owner item)
        {
            try
            {
                Owner owner = db.Owners.Find(item.OwnerId);
                db.Owners.Attach(owner);
                owner.OwnerName = item.OwnerName;
                owner.PhoneNumber = item.PhoneNumber;
                owner.Email = item.Email;

                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateBookBorrower(Owner item)
        {
            throw new NotImplementedException();
        }
    }
}