using CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace CaseStudy.Tool
{
    public class ISBNEntityConfiguration : EntityTypeConfiguration<ISBN>
    {
        public ISBNEntityConfiguration()
        {
            this.HasKey<int>(s => s.ISBNId)
                .Property(s => s.ISBNId)
                .IsRequired();

            this.Property(s => s.ISBNString)
                .IsRequired();
        }
    }
}