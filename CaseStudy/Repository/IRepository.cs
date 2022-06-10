using CaseStudy.Models;
using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy.Repository
{
    public interface IRepository<T>
    {
        SendMail SendMail();
        List<T> GetAll();
        T GetItem(int Id);
        bool Insert(T item);
        bool Update(T item);
        bool Delete(int Id);
        bool UpdateBookBorrower(T item);
    }
}
