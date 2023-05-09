using System.ComponentModel.DataAnnotations;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JobPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            return userIdentity;
        }
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(40, ErrorMessage = "Full name cannot be longer than 40 characters.")]
        public string FullName { get; set; }
        [Display(Name = "Phone Nr.")]
        [Required]
        [StringLength(30, ErrorMessage = "Phone number cannot be longer than 30 characters.")]
        public string PhoneNr { get; set; }

        [Required]
        public string Address { get; set; }

        public DateTime InsertedOn { get; set; }
        [Display(Name = "Select Job Category")]
        public int? CategoriesId { get; set; }
        public virtual Categories Categories { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<JobApplication> JobApplication { get; set; }
        public DbSet<Categories> Categories { get; set; }
    }
}