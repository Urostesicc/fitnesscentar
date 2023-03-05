using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBProject.Models;
using WEBProject.Models.XML;

namespace WEBProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BlockUser(string username)
        {
            var users = (List<User>)HttpContext.Application["users"];

            var index = users.FindIndex(x => x.Username == username);
            users[index].CanLogin = false;

            HttpContext.Application["users"] = users;
            XMLWriter.WriteUsers(users);

            return RedirectToAction("Index", "FitnessCenter");
        }

        public ActionResult LeaveComment(Comment comment)
        {
            var comments = (List<Comment>)HttpContext.Application["comments"];
            comment.Id = Guid.NewGuid().ToString();
            comment.VisitorId = Session["username"].ToString();
            comments.Add(comment);

            XMLWriter.WriteComments(comments);

            return RedirectToAction("VisitorHome", "Home");

        }

        public ActionResult ModifyAccount()
        {
            var username = Session["username"].ToString();
            var users = (List<User>)HttpContext.Application["users"];
            var user = users.Where(x => x.Username == username).FirstOrDefault();
            return View("ModifyAccount", user);
        }


        public ActionResult Modify(User user)
        {
            var users = (List<User>)HttpContext.Application["users"];
            var index = users.FindIndex(x => x.Username == user.Username);
            user.IsDeleted = users[index].IsDeleted;
            user.CanLogin = users[index].CanLogin;
            user.UserType = users[index].UserType;
            if(users[index].UserType == "Trainer")
            {
                var strings = ((Trainer)users[index]).TrainingIds;

                Trainer trainer = new Trainer { Username = user.Username, UserType = user.UserType, Gender = user.Gender, CanLogin = user.CanLogin, IsDeleted = user.IsDeleted, Lastname = user.Lastname, Name = user.Name, TrainingIds = strings, Password = user.Password };
                users[index] = trainer;
            }

            if (users[index].UserType == "Visitor")
            {
                var strings = ((Visitor)users[index]).TrainingIds;
                Visitor visitor = new Visitor { Username = user.Username, UserType = user.UserType, Gender = user.Gender, CanLogin = user.CanLogin, IsDeleted = user.IsDeleted, Lastname = user.Lastname, Name = user.Name, TrainingIds = strings, Password = user.Password };
                users[index] = visitor;
            }


            if (users[index].UserType == "Owner")
            {
                var strings = ((Owner)users[index]).OwnedCentersIds;

                Owner owner = new Owner { Username = user.Username, UserType = user.UserType, Gender = user.Gender, CanLogin = user.CanLogin, IsDeleted = user.IsDeleted, Lastname = user.Lastname, Name = user.Name, OwnedCentersIds = strings, Password = user.Password };
                users[index] = owner;
            }

       

            XMLWriter.WriteUsers(users);
            HttpContext.Application["users"] = users;

            return View("ModifyAccount", user);
        }


    }
}