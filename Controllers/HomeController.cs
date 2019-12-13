using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        public int? InSession { 
            get { return HttpContext.Session.GetInt32("userid"); }
            set { HttpContext.Session.SetInt32("userid", (int)value); }
        }
        
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            if(InSession != null)
            {
                return RedirectToAction("Dashboard");
            }
            IndexViewModel viewmod = new IndexViewModel();
            return View(viewmod);
        }
        [HttpPost("NewUser")]
        public IActionResult NewUser(IndexViewModel newuser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.users.Any(u => u.Email == newuser.SingleUser.Email))
                {
                    ModelState.AddModelError("Email", "Email alreayd in use!");
                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newuser.SingleUser.Password = Hasher.HashPassword(newuser.SingleUser, newuser.SingleUser.Password);

                dbContext.Add(newuser.SingleUser);
                dbContext.SaveChanges();
                InSession =  newuser.SingleUser.UserId;
                return RedirectToAction("Dashboard");
            }
            else{
                return View("Index");
            }
        }

        [HttpPost("NewLogin")]
        public IActionResult NewLogin(IndexViewModel user)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == user.SingleLogin.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }

                PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
                var result = Hasher.VerifyHashedPassword(user.SingleLogin, userInDb.Password, user.SingleLogin.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Incorrect Password");
                    return View("Index");
                }
                User currentUser = dbContext.users.FirstOrDefault(u => u.Email == user.SingleLogin.Email);
                HttpContext.Session.SetInt32("userid", currentUser.UserId);
                return RedirectToAction("Dashboard");
            }
            else{
                return View("Index");
            }
        }

        public IActionResult Dashboard()
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            IndexViewModel viewmod = new IndexViewModel();
            viewmod.SingleUser = dbContext.users
            .Include(c => c.AllPlans)
            .ThenInclude(p => p.plan)
            .FirstOrDefault( u => u.UserId == InSession);

            viewmod.AllPlans = dbContext.plans
            .OrderByDescending(f => f.Date)
            .Include(u => u.Creator)
            .Include(u => u.participants)
            .ThenInclude(y => y.user)
            .Where(p => p.Date > DateTime.Now).ToList();
            

            viewmod.Notgoing = dbContext.plans
            .Include(g => g.participants)
            .ThenInclude(l =>l.user)
            .Where(u => !viewmod.SingleUser.AllPlans
            .Any(x => x.plan.PlanId == u.PlanId)).ToList();
            
            viewmod.going = dbContext.plans
            .Include(g => g.participants)
            .ThenInclude(l =>l.user)
            .Where(u => viewmod.SingleUser.AllPlans
            .Any(x => x.plan.PlanId == u.PlanId)).ToList();

            viewmod.Creator = dbContext.plans
            .Where(q => q.UserId == InSession).ToList();

            viewmod.UserId = (int) InSession;

            return View(viewmod);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult NewActivity()
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            IndexViewModel viewmod = new IndexViewModel();
            return View(viewmod);
        }

        [HttpPost("AddActivity")]

        public IActionResult AddActivity(IndexViewModel formform)
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            formform.SinglePlan.UserId = (int)InSession;
            if(ModelState.IsValid)
            {
                dbContext.Add(formform.SinglePlan);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewActivity");

        }

        [HttpPost("Join")]
        public IActionResult Join(IndexViewModel fromform)
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            fromform.UserId = (int)HttpContext.Session.GetInt32("userid");
            if(!dbContext.associations.Any(a => a.UserId == fromform.UserId && a.PlanId == fromform.NewAssos.PlanId))
            {
                dbContext.Add(fromform.NewAssos);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost("Leave")]
        public IActionResult Leave(IndexViewModel fromform)
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            if(fromform.NewAssos.UserId != (int)InSession)
            {
                return RedirectToAction("Dashboard");
            }
            Association Breakthis = dbContext.associations.FirstOrDefault(a => a.UserId == fromform.NewAssos.UserId && a.PlanId == fromform.NewAssos.PlanId);
            if(Breakthis != null)
            {
                dbContext.Remove(Breakthis);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Dashboard");

        }

        [HttpPost("Delete")]
        public IActionResult Delete(IndexViewModel fromform)
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            if(fromform.NewAssos.UserId != (int)HttpContext.Session.GetInt32("userid"))
            {
                return RedirectToAction("Dashboard");
            }
            Plan Deletethis = dbContext.plans.FirstOrDefault(a => a.UserId == fromform.NewAssos.UserId && a.PlanId == fromform.NewAssos.PlanId);
            if(Deletethis != null)
            {
                dbContext.Remove(Deletethis);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Dashboard");
           
        }

        [HttpGet("PlanInfo/{PlanId}")]
        public IActionResult PlanInfo(int PlanId)
        {
            if(InSession == null)
            {
                return RedirectToAction("Index");
            }
            IndexViewModel viewmod = new IndexViewModel();
            viewmod.SinglePlan = dbContext.plans.Include(u => u.participants).ThenInclude(p => p.user).FirstOrDefault(a => a.PlanId == PlanId);
            viewmod.SingleUser = dbContext.users.Include(y => y.AllPlans).ThenInclude(z => z.plan).FirstOrDefault(u => u.UserId == viewmod.SinglePlan.UserId);
            viewmod.UserId= (int) InSession;

            viewmod.Notgoing = dbContext.plans
            .Include(g => g.participants)
            .ThenInclude(l =>l.user)
            .Where(u => !viewmod.SingleUser.AllPlans
            .Any(x => x.plan.PlanId == u.PlanId)).ToList();
            
            viewmod.going = dbContext.plans
            .Include(g => g.participants)
            .ThenInclude(l =>l.user)
            .Where(u => viewmod.SingleUser.AllPlans
            .Any(x => x.plan.PlanId == u.PlanId)).ToList();

            viewmod.Creator = dbContext.plans
            .Where(q => q.UserId == InSession).ToList();

            return View(viewmod);
        }

    }
}
