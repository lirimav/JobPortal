using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int JobsId { get; set; }
        public virtual Jobs Jobs { get; set; }
        public DateTime AppliedOn { get; set; }
    }
}