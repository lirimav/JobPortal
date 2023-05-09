using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Models
{
    public class Jobs
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Company field is required.")]
        [StringLength(40, ErrorMessage = "Company cannot be longer than 40 characters.")]
        public string Company { get; set; }
        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "The Job Title field is required.")]
        [StringLength(40, ErrorMessage = "Job title cannot be longer than 40 characters.")]
        public string Job { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Location field is required.")]
        [StringLength(20, ErrorMessage = "Location cannot be longer than 20 characters.")]
        public string Location { get; set; }
        [Display(Name = "Expiration Date")]
        [Required(ErrorMessage = "The Expiration Date field is required.")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public DateTime InsertedOn { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [DisplayName("Category of Job")]
        public int CategoriesId { get; set; }
        public virtual Categories Categories { get; set; }
    }
}