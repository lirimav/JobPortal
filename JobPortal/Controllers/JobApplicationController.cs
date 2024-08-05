using JobPortal.Models.ViewModels;
using JobPortal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Controllers
{
    [Authorize]
    public class JobApplicationController : Controller
    {
        private ApplicationDbContext context;

        public JobApplicationController()
        {
            context = new ApplicationDbContext();
        }


        // GET: JobApplication
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            // Retrieve all JobApplication entities for the current user
            var jobApplications = context.JobApplication.Where(ja => ja.ClientId == userId).ToList();

            // Retrieve the corresponding Job entities
            var jobs = new List<Jobs>();
            foreach (var jobApp in jobApplications)
            {
                var job = context.Jobs.FirstOrDefault(j => j.Id == jobApp.JobsId);
                if (job != null)
                {
                    jobs.Add(job);
                }
            }

            List<JobsVm> lista = new List<JobsVm>();

            foreach (var item in jobs)
            {
                var obj = new JobsVm();
                obj.Id = item.Id;
                obj.Company = item.Company;
                obj.Job = item.Job;
                obj.Description = item.Description;
                obj.Location = item.Location;
                obj.ExpirationDate = item.ExpirationDate;
                obj.InsertedOn = item.InsertedOn;
                lista.Add(obj);

            }

            return View(lista);

            
        }
    }
}