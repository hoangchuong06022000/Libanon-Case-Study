using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseStudy.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string PublishYear { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        [Required]
        [DisplayName("Owner")]
        public int OwnerId { get; set; }
        public bool? IsBorrowed { get; set; }
        public int? BorrowerId { get; set; }
        public virtual ISBN ISBN { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual Borrower Borrower { get; set; }
    }
}