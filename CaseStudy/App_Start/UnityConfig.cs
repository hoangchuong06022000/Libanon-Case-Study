using CaseStudy.Models;
using CaseStudy.Repository;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace CaseStudy
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IRepository<Book>, BookRepository>();

            container.RegisterType<IRepository<ISBN>, ISBNRepository>();

            container.RegisterType<IRepository<Owner>, OwnerRepository>();

            container.RegisterType<IRepository<Borrower>, BorrowerRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}