using CaseStudy.Models;
using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaseStudy.Repository
{
    public class ISBNRepository : IRepository<ISBN>
    {
        private LibanonContext db = new LibanonContext();
        public bool Delete(int Id)
        {
            try
            {
                ISBN isbn = db.ISBNs.Where(x => x.ISBNId == Id).FirstOrDefault();
                db.ISBNs.Remove(isbn);
                db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<ISBN> GetAll()
        {
            return db.ISBNs.ToList();
        }

        public ISBN GetItem(int Id)
        {
            return db.ISBNs.Where(x => x.ISBNId == Id).FirstOrDefault();
        }

        public bool Insert(ISBN item)
        {
            try
            {
                db.ISBNs.Add(item);
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

        public bool Update(ISBN item)
        {
            try
            {
                ISBN isbn = db.ISBNs.Find(item.ISBNId);
                db.ISBNs.Attach(isbn);
                isbn.ISBNString = item.ISBNString;
                isbn.RateScore = item.RateScore;
                db.Entry(isbn).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateBookBorrower(ISBN item)
        {
            throw new NotImplementedException();
        }
    }
}