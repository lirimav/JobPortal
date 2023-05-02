using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models.ViewModels
{
    public class UsersVm
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "The Full Name field is required.")]
        [StringLength(40, ErrorMessage = "Full name cannot be longer than 40 characters.")]
        public string FullName { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "The Phone Nr. field is required.")]
        [StringLength(30, ErrorMessage = "Phone number cannot be longer than 30 characters.")]
        public int PhoneNr { get; set; }
        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; }
        public DateTime InsertedOn { get; set; }

        public int CategoriesId { get; set; }
    }
}