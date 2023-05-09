using JobPortal.Models;
using JobPortal.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Security;

namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;

        public HomeController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index(int? categoryId)
        {
            List<CategoryVm> categories = new List<CategoryVm>();
            foreach (var category in context.Categories)
            {
                var categoryVm = new CategoryVm();
                categoryVm.Id = category.Id;
                categoryVm.Name = category.Name;
                categories.Add(categoryVm);
            }

            var joblist = context.Jobs.ToList();

            if (Request.IsAuthenticated)
            {
                var user = User.Identity;
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var currentUser = userManager.FindById(user.GetUserId());

                var userCategoryId = currentUser.CategoriesId;

                if (userManager.IsInRole(user.GetUserId(), "Admin") || userManager.IsInRole(user.GetUserId(), "Employer"))
                {
                    joblist = context.Jobs.ToList();
                }

                if (categoryId.HasValue)
                {
                    if (categoryId.Value == 1)
                    {
                        joblist = context.Jobs.ToList();
                    }
                    else
                    {
                        joblist = joblist.Where(j => j.CategoriesId == categoryId.Value).ToList();
                    }
                }
                else if(Request.IsAuthenticated && userManager.IsInRole(user.GetUserId(), "Client") && !categoryId.HasValue)
                {
                    joblist = context.Jobs.Where(j => j.CategoriesId == userCategoryId).ToList();
                }

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

                    lista.Add(obj);
                }
                ViewBag.CategoriesVm = categories;
                return View(lista);
            }
            else
            {
                joblist = context.Jobs.ToList();

                if (categoryId.HasValue)
                {
                    if(categoryId.Value == 1) 
                    {
                        joblist = context.Jobs.ToList();
                    }
                    else
                    {
                        joblist = joblist.Where(j => j.CategoriesId == categoryId.Value).ToList();
                    }
                }

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

                    lista.Add(obj);

                }
                ViewBag.CategoriesVm = categories;
                return View(lista);
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailTo = "lirimav1@gmail.com";
                var subject = "Job Portal Contact";
                var message = $"From: {model.Name}\nEmail: {model.Email}\n\n{model.Message}";

                await SendMail(emailTo, subject, message);

                ViewBag.Success = true;
            }

            return View(model);
        }

        public async Task SendMail(string emailTo, string subject, string message)
        {
            var messageObj = new MimeMessage();
            messageObj.From.Add(new MailboxAddress("Job Portal", "lirimav1@gmail.com"));
            messageObj.To.Add(new MailboxAddress("", emailTo));
            messageObj.Subject = subject;
            messageObj.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync("sartechcontact4", "mjsvjbajeodckrio");
                await client.SendAsync(messageObj);
                await client.DisconnectAsync(true);
            }
        }


        [HttpPost]
        public ActionResult ApplyForJob(int jobId)
        {
            var currentUser = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else if(userManager.IsInRole(currentUser.GetUserId(), "Admin") || userManager.IsInRole(currentUser.GetUserId(), "Employer"))
            {
                return RedirectToAction("Index", "Jobs");
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var job = context.Jobs.Find(jobId);

                if (job == null)
                {
                    return HttpNotFound();
                }

                // Check if the user has already applied for this job
                var existingApplication = context.JobApplication
                    .FirstOrDefault(a => a.JobsId == jobId && a.ClientId == userId);

                if (existingApplication != null)
                {
                    ModelState.AddModelError("", "You have already applied for this job.");
                    return RedirectToAction("Index", "JobApplication", new { id = jobId });
                }

                var application = new JobApplication
                {
                    ClientId = userId,
                    JobsId = jobId,
                    AppliedOn = DateTime.Now
                };

                context.JobApplication.Add(application);
                context.SaveChanges();

                return RedirectToAction("Index", "JobApplication", new { id = jobId });
            }
        }
    }
}