using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Models.ViewModels
{
    public class JobDetailsVm
    {
        public string ClientId { get; set; }
        public int JobsId { get; set; }
        public DateTime AppliedOn { get; set; }
    }
}