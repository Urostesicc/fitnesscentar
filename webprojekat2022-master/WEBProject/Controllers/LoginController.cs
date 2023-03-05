using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBProject.Models;
using WEBProject.Models.XML;

namespace WEBProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        public ActionResult Login(string username, string password)
        {
            if(Session["username"] == null)
            {
                List<User> users = (List<User>)HttpContext.Application["users"];
                var user = users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                if (user != null)
                {
                    if (!user.CanLogin)
                    {
                        ViewBag.Error = "Your account was blocked, please contact your supervisor";
                        return View();
                    }

                    Session["username"] = username;
                    if(user.UserType == "Owner")
                    {
                        return Redirect("~/FitnessCenter/Index");
                    }
                    if (user.UserType == "Visitor")
                    {
                        return Redirect("~/Home/VisitorHome");
                    }
                    if(user.UserType == "Trainer")
                    {
                        return Redirect("~/GroupTraining/Index");
                    }
                
                    return RedirectToAction("Index");
                }
             
                ViewBag.Error = "Wrong username or password";
                return View();
            }
            ViewBag.Error = "Already logged in";
            return View();
        }

        public ActionResult RegisterUser(Visitor newUser)
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            var user = users.Where(x => x.Username == newUser.Username).FirstOrDefault();
            if(user != null)
            {
                ViewBag.Error = "Username already exists!";
                return View();
            }
            newUser.CanLogin = true;
            newUser.UserType = "Visitor";
            users.Add(newUser);
            HttpContext.Application["users"] = users;
            XMLWriter.WriteUsers(users);
            return View("Register");
        }

        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("Index");
        }
    }
}