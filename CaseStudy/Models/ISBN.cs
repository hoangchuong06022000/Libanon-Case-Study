using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseStudy.Models
{
    public class ISBN
    {
        public int ISBNId { get; set; }
        [Required]
        [DisplayName("ISBN")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid ISBN")]
        public string ISBNString { get; set; }
        [Range(1, 5, ErrorMessage = "Invalid!! Must be 1 to 5!!")]
        public double? RateScore { get; set; }
        public virtual Book Book { get; set; }
    }
}