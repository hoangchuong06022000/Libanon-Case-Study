using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseStudy.Models
{
    public class Owner
    {
        public int OwnerId { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}