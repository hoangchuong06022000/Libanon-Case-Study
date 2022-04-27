using CaseStudy.Models;
using CaseStudy.Repository;
using Ninject;
using Ninject.Extensions.ChildKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace CaseStudy.Tool
{
    public class NinjectResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectResolver()
        {
            this.kernel = new StandardKernel(new NinjectSettings() { LoadExtensions = false });
        }

        public NinjectResolver(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
            {
                AddBindings(kernel);
            }
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectResolver(AddRequestBindings(new ChildKernel(kernel)), true);
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void Dispose()
        {

        }

        private void AddBindings(IKernel kernel)
        {
            // singleton and transient bindings go here
        }
        private IKernel AddRequestBindings(IKernel kernel)
        {
            kernel.Bind<IRepository<Book>>().To<BookRepository>().InSingletonScope();
            kernel.Bind<IRepository<ISBN>>().To<ISBNRepository>().InSingletonScope();
            kernel.Bind<IRepository<Owner>>().To<OwnerRepository>().InSingletonScope();
            kernel.Bind<IRepository<Borrower>>().To<BorrowerRepository>().InSingletonScope();
            return kernel;
        }
    }
}