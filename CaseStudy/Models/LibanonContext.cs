using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaseStudy.Models
{
    public class LibanonContext : DbContext
    {
        public LibanonContext() : base("name = LibanonDB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LibanonContext, CaseStudy.Migrations.Configuration>());
            //Database.SetInitializer<LibanonContext>(new DropCreateDatabaseIfModelChanges<LibanonContext>());
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<ISBN> ISBNs { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BookEntityConfiguration());
            modelBuilder.Configurations.Add(new ISBNEntityConfiguration());
            modelBuilder.Configurations.Add(new OwnerEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}