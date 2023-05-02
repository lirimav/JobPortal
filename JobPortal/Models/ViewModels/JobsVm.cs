using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models.ViewModels
{
    public class JobsVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Company field is required.")]
        [StringLength(20, ErrorMessage = "Company cannot be longer than 20 characters.")]
        public string Company { get; set; }
        [Required(ErrorMessage = "The Job Title field is required.")]
        [StringLength(40, ErrorMessage = "Job title cannot be longer than 40 characters.")]
        public string Job { get; set; }
        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The Location field is required.")]
        [StringLength(20, ErrorMessage = "Location cannot be longer than 20 characters.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "The Expiration Date field is required.")]
        public DateTime ExpirationDate { get; set; }
        public DateTime InsertedOn { get; set; }
        public string UserId { get; set; }
        public List<JobApplication> JobApplications { get; set; }
        public int CategoriesId { get; set; }
        public List<Categories> Categories { get; set; }
    }
}