using CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetItem(int Id);
        bool Insert(T item);
        bool Update(T item);
        bool Delete(int Id);
        bool UpdateBookBorrower(T item);
    }
}
