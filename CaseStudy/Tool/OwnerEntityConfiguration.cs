using CaseStudy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace CaseStudy.Tool
{
    public class OwnerEntityConfiguration : EntityTypeConfiguration<Owner>
    {
        public OwnerEntityConfiguration()
        {
            this.HasKey<int>(s => s.OwnerId)
                .Property(s => s.OwnerId)
                .IsRequired();

            this.Property(s => s.OwnerName)
                .IsRequired();

            this.Property(s => s.PhoneNumber)
                .IsRequired();

            this.Property(s => s.Email)
                .IsRequired();
        }
    }
}