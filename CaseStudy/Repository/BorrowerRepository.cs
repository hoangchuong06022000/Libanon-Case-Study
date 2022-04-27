using CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaseStudy.Repository
{
    public class BorrowerRepository : IRepository<Borrower>
    {
        private LibanonContext db = new LibanonContext();
        public bool Delete(int Id)
        {
            try
            {
                Borrower borrower = db.Borrowers.Where(x => x.BorrowerId == Id).FirstOrDefault();
                db.Borrowers.Remove(borrower);
                db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Borrower> GetAll()
        {
            return db.Borrowers.ToList();
        }

        public Borrower GetItem(int Id)
        {
            return db.Borrowers.Where(x => x.BorrowerId == Id).FirstOrDefault();
        }

        public bool Insert(Borrower item)
        {
            try
            {
                Borrower borrower = db.Borrowers.Where(x => x.Email == item.Email).FirstOrDefault();
                if(borrower == null)
                {
                    db.Borrowers.Add(item);
                    db.SaveChanges();  
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Update(Borrower item)
        {
            try
            {
                Borrower borrower = db.Borrowers.Find(item.BorrowerId);
                db.Borrowers.Attach(borrower);
                borrower.BorrowerName = item.BorrowerName;
                borrower.PhoneNumber = item.PhoneNumber;
                borrower.Email = item.Email;

                db.Entry(borrower).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateBookBorrower(Borrower item)
        {
            throw new NotImplementedException();
        }
    }
}