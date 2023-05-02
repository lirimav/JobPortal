using JobPortal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobPortal.Models.ViewModels;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace JobPortal.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private ApplicationDbContext context;

        public JobsController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var currentUser = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (userManager.IsInRole(currentUser.GetUserId(), "Admin"))
            {
                var joblist = context.Jobs.ToList();

                List<JobsVm> lista = new List<JobsVm>();

                foreach (var item in joblist)
                {
                    var obj = new JobsVm();
                    obj.Id = item.Id;
                    obj.Company = item.Company;
                    obj.Job = item.Job;
                    obj.Description = item.Description;
                    obj.Location = item.Location;
                    obj.ExpirationDate = item.ExpirationDate;
                    obj.InsertedOn = item.InsertedOn;
                    obj.UserId = item.UserId;
                    obj.JobApplications = context.JobApplication.Include(ja => ja.User).Where(ja => ja.JobsId == item.Id).ToList();
                    obj.CategoriesId = item.CategoriesId;
                    lista.Add(obj);
                    /*context.JobApplication.Where(ja => ja.JobsId == item.Id).Select(ja => ja.User).ToList();*/
                }
                DeleteExpiredJobs();
                return View(lista);
            }
            else if (userManager.IsInRole(currentUser.GetUserId(), "Employer"))
            {
                // Employer users can see only their own jobs
                var user = userManager.FindById(currentUser.GetUserId());
                var joblist = context.Jobs.Where(j => j.User.Id == user.Id).ToList();
                List<JobsVm> lista = new List<JobsVm>();

                foreach (var item in joblist)
                {
                    var obj = new JobsVm();
                    obj.Id = item.Id;
                    obj.Company = item.Company;
                    obj.Job = item.Job;
                    obj.Description = item.Description;
                    obj.Location = item.Location;
                    obj.ExpirationDate = item.ExpirationDate;
                    obj.InsertedOn = item.InsertedOn;
                    obj.JobApplications = context.JobApplication.Include(ja => ja.User).Where(ja => ja.JobsId == item.Id).ToList();
                    obj.CategoriesId = item.CategoriesId;
                    lista.Add(obj);

                }
                DeleteExpiredJobs();
                return View(lista);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult GoToAddNewJob ()
        {
            var currentUser = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (userManager.IsInRole(currentUser.GetUserId(), "Admin") ||
                userManager.IsInRole(currentUser.GetUserId(), "Employer"))
            {
                var categories = context.Categories.ToList();
                ViewBag.Categories = categories;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult SaveJob(Jobs j) 
        {
            var currentUser = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (userManager.IsInRole(currentUser.GetUserId(), "Admin") ||
                userManager.IsInRole(currentUser.GetUserId(), "Employer"))
            {
                if (j == null)
                {
                    return RedirectToAction("Index", "Error");
                }
                else
                {
                    var userId = User.Identity.GetUserId();
                    if (j.Id > 0)
                    {
                        Jobs jobs = context.Jobs.Where(x => x.Id == j.Id).SingleOrDefault();

                        jobs.Company = j.Company;
                        jobs.Job = j.Job;
                        jobs.Description = j.Description;
                        jobs.Location = j.Location;
                        jobs.ExpirationDate = j.ExpirationDate;
                        jobs.InsertedOn = DateTime.Now;
                        jobs.UserId = userId;
                        jobs.CategoriesId = j.CategoriesId;
                    }
                    else
                    {
                        Jobs jobs = new Jobs();
                        jobs.Company = j.Company;
                        jobs.Job = j.Job;
                        jobs.Description = j.Description;
                        jobs.Location = j.Location;
                        jobs.ExpirationDate = j.ExpirationDate;
                        jobs.InsertedOn = DateTime.Now;
                        jobs.UserId = userId;
                        jobs.CategoriesId = j.CategoriesId;
                        context.Jobs.Add(jobs);
                    }

                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult JobEdit(int Id)
        {
            var currentUser = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (userManager.IsInRole(currentUser.GetUserId(), "Admin") ||
                userManager.IsInRole(currentUser.GetUserId(), "Employer"))
            {
                var jobFromDb = context.Jobs.Where(x => x.Id == Id).SingleOrDefault();

                var categories = context.Categories.ToList();
                ViewBag.Categories = categories;


                return View(jobFromDb);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult DeleteJob(int Id)
        {
            var jobfromdb = context.Jobs.Where(x => x.Id == Id).SingleOrDefault();
            var jobApplications = context.JobApplication.Where(ja => ja.JobsId == Id).ToList();

            // Delete all the job applications related to the job being deleted
            foreach (var jobApplication in jobApplications)
            {
                context.JobApplication.Remove(jobApplication);
            }

            context.Jobs.Remove(jobfromdb);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public void DeleteExpiredJobs()
        {
            var expiredJobs = context.Jobs.Where(x => x.ExpirationDate < DateTime.Now).ToList();

            if (expiredJobs.Count > 0)
            {
                foreach(var eachjob in expiredJobs) { 
                    var jobApplications = context.JobApplication.Where(ja => ja.JobsId == eachjob.Id).ToList();

                    // Delete all the job applications related to the job being deleted
                    foreach (var jobApplication in jobApplications)
                    {
                        context.JobApplication.Remove(jobApplication);
                    }
                }
                context.Jobs.RemoveRange(expiredJobs);
                context.SaveChanges();
            }
        }
        public ActionResult ManageApplicants()
        {
            var currentUser = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (userManager.IsInRole(currentUser.GetUserId(), "Admin"))
            {
                var joblist = context.Jobs.ToList();

                List<JobsVm> lista = new List<JobsVm>();

                foreach (var item in joblist)
                {
                    var obj = new JobsVm();
                    obj.Id = item.Id;
                    obj.Company = item.Company;
                    obj.Job = item.Job;
                    obj.UserId = item.UserId;
                    obj.JobApplications = context.JobApplication.Include(ja => ja.User).Where(ja => ja.JobsId == item.Id).ToList();
                    lista.Add(obj);
                    /*context.JobApplication.Where(ja => ja.JobsId == item.Id).Select(ja => ja.User).ToList();*/
                }
                DeleteExpiredJobs();
                return View(lista);
            }
            else if (userManager.IsInRole(currentUser.GetUserId(), "Employer"))
            {
                // Employer users can see only their own jobs
                var user = userManager.FindById(currentUser.GetUserId());
                var joblist = context.Jobs.Where(j => j.User.Id == user.Id).ToList();
                List<JobsVm> lista = new List<JobsVm>();

                foreach (var item in joblist)
                {
                    var obj = new JobsVm();
                    obj.Id = item.Id;
                    obj.Company = item.Company;
                    obj.Job = item.Job;
                    obj.JobApplications = context.JobApplication.Include(ja => ja.User).Where(ja => ja.JobsId == item.Id).ToList();
                    lista.Add(obj);

                }
                DeleteExpiredJobs();
                return View(lista);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult RejectUser(string userId, int jobId)
        {
            var jobApplication = context.JobApplication.FirstOrDefault(ja => ja.ClientId == userId && ja.JobsId == jobId);
            if (jobApplication != null)
            {
                context.JobApplication.Remove(jobApplication);
                context.SaveChanges();
                return RedirectToAction("ManageApplicants");
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}