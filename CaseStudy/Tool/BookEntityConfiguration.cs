using CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace CaseStudy.Tool
{
    public class BookEntityConfiguration : EntityTypeConfiguration<Book>
    {
        public BookEntityConfiguration()
        {
            this.HasKey<int>(s => s.Id)
            .Property(s => s.Id)
            .IsRequired();

            this.Property(s => s.Title)
                .IsRequired();

            this.Property(s => s.Author)
                .IsRequired();

            this.Property(s => s.PublishYear)
                .IsRequired();

            this.Property(s => s.Summary)
                .IsRequired();

            this.HasRequired(s => s.ISBN)
                .WithRequiredPrincipal(ad => ad.Book)
                .WillCascadeOnDelete();

            this.HasRequired<Owner>(s => s.Owner)
                .WithMany(g => g.Books)
                .HasForeignKey<int>(s => s.OwnerId)
                .WillCascadeOnDelete(false);

            this.HasOptional<Borrower>(s => s.Borrower)
                .WithMany(g => g.Books)
                .HasForeignKey<int?>(s => s.BorrowerId)
                .WillCascadeOnDelete(false);
        }
    }
}