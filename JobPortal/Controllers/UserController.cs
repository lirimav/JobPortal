using JobPortal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using JobPortal.Models.ViewModels;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity;

namespace JobPortal.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext context;
        public UserController() 
        {
            context = new ApplicationDbContext();
        }


        public ActionResult Index()
        {
            var user = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (Request.IsAuthenticated && userManager.IsInRole(user.GetUserId(), "Admin"))
            {
                var userlist = context.Users.ToList();

                var adminId = userManager.FindByName(user.Name).Id;
                var nonAdminUsers = userlist.Where(u => !userManager.IsInRole(u.Id, "Admin") && u.Id != adminId);

                List<UsersVm> lista = new List<UsersVm>();

                foreach (var item in nonAdminUsers)
                {
                    var obj = new UsersVm
                    {
                        UserId = item.Id,
                        FullName = item.FullName,
                        Email = item.Email,
                        InsertedOn = item.InsertedOn
                    };

                    lista.Add(obj);
                }

                return View(lista);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult RemoveUser(string Id)
        {
            var user = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (userManager.IsInRole(user.GetUserId(), "Admin"))
            {
                var userfromdb = context.Users.Where(x => x.Id == Id).SingleOrDefault();

                // check if user is an employer and delete their jobs
                if (userManager.IsInRole(userfromdb.Id, "Employer"))
                {
                    var jobs = context.Jobs.Where(j => j.UserId == userfromdb.Id).ToList();
                    context.Jobs.RemoveRange(jobs);
                }

                // delete all job applications by the user
                var jobApplications = context.JobApplication.Where(ja => ja.ClientId == userfromdb.Id).ToList();
                context.JobApplication.RemoveRange(jobApplications);

                context.Users.Remove(userfromdb);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult SaveUser(ApplicationUser u)
        {
            var userr = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            if (u == null)
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                if (u.Id != null)
                {
                    ApplicationUser user = context.Users.Where(x => x.Id == u.Id).SingleOrDefault();

                    user.FullName = u.FullName;
                    user.PhoneNr = u.PhoneNr;
                    user.Address = u.Address;
                    user.CategoriesId = u.CategoriesId;
                }
                else
                {
                    ApplicationUser user = new ApplicationUser();
                    user.FullName = u.FullName;
                    user.PhoneNr = u.PhoneNr;
                    user.Address = u.Address;
                    user.CategoriesId = u.CategoriesId;

                    context.Users.Add(user);
                }
                context.SaveChanges();
                if(Request.IsAuthenticated && userManager.IsInRole(userr.GetUserId(), "Admin"))
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
        }

        public ActionResult EditUser(string Id)
        {
            var user = User.Identity;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (Request.IsAuthenticated)
            { 
                var userFromDb = context.Users.Where(x => x.Id == Id).SingleOrDefault();

                var categories = context.Categories.ToList();
                ViewBag.Categories = categories;

                return View(userFromDb);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}