using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBProject.Models;
using WEBProject.Models.XML;

namespace WEBProject.Controllers
{
    public class FitnessCenterController : Controller
    {
        // GET: FitnessCenter
        public ActionResult Index()
        {
            var users = (List<User>)HttpContext.Application["users"];
            User owner = users.Where(x => x.Username == Session["username"].ToString() && x.UserType == "Owner").FirstOrDefault();
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            List<User> trainers = users.Where(x => x.UserType == "Trainer").ToList();
            var fcTrainers = new List<Trainer>();
            if(owner != null)
            {
                fitnessCenters = fitnessCenters.Where(x => x.OwnerId == owner.Username && x.IsDeleted == false).ToList();
                foreach (var fc in fitnessCenters)
                {
                    foreach (var tr in trainers)
                    {
                        if (((Trainer)tr).FitnessCenterId == fc.Id)
                        {
                            fcTrainers.Add((Trainer)tr);
                        }
                    }
                }
                ViewData["trainers"] = fcTrainers;
                return View("Owner", fitnessCenters);
            }

           

            return View();
        }

        public ActionResult AddFitnessCenter(FitnessCenter fitnessCenter)
        {
            fitnessCenter.Id = Guid.NewGuid().ToString();
            fitnessCenter.OwnerId = (string)Session["username"];
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];

            fitnessCenters.Add(fitnessCenter);
            XMLWriter.WriteFitnessCenters(fitnessCenters);

            HttpContext.Application["fitness_centers"] = fitnessCenters;

            return RedirectToAction("Index");
        }
        

        public ActionResult AddCenter()
        {
            return View("NewFitnessCenter");
        }

        
        public ActionResult ModifyFitnessCenter(string fitnessCenterId)
        {
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            var center = fitnessCenters.Where(x => x.Id == fitnessCenterId).FirstOrDefault();
            return View("ModifyFitnessCenter", center);
        }

        public ActionResult Modify(FitnessCenter fitnessCenter)
        {
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            var index = fitnessCenters.FindIndex(x => x.Id == fitnessCenter.Id);
            fitnessCenters[index] = fitnessCenter;

            XMLWriter.WriteFitnessCenters(fitnessCenters);
            HttpContext.Application["fitness_centers"] = fitnessCenters;

            return RedirectToAction("Index");
        }

        


        public ActionResult RemoveFitnessCenter(string fitnessCenterId)
        {
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            trainings = trainings.Where(x => x.FitnessCenterId == fitnessCenterId && x.TrainingDate > DateTime.Now).ToList();
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];

            if (trainings == null)
            {
                ViewBag.Error = "Can't delete the fitness center, group trainings have been scheduled in the future.";
                return View();
            }

            var index = fitnessCenters.FindIndex(x => x.Id == fitnessCenterId);
            fitnessCenters[index].IsDeleted = true;

            var users = (List<User>)HttpContext.Application["users"];
            var trainers = users.Where(x => x.UserType == "Trainer").ToList();

            trainers = trainers.Where(x => ((Trainer)x).FitnessCenterId == fitnessCenterId).ToList();

            foreach(User u in trainers)
            {
                index = users.FindIndex(x => x.Username == u.Username);
                users[index].CanLogin = false;
                HttpContext.Application["users"] = users;
                XMLWriter.WriteUsers(users);

            }

            HttpContext.Application["fitness_centers"] = fitnessCenters;
            XMLWriter.WriteFitnessCenters(fitnessCenters);
            return RedirectToAction("Index");
        }


        public ActionResult AddTrainer()
        {
            var username = Session["username"].ToString();
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];

            fitnessCenters = fitnessCenters.Where(x => x.OwnerId == username).ToList();


            return View("RegisterTrainer", fitnessCenters);
        }

        public ActionResult RegisterTrainer(Trainer newUser)
        {
            var username = Session["username"].ToString();
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];

            fitnessCenters = fitnessCenters.Where(x => x.OwnerId == username).ToList();
            List<User> users = (List<User>)HttpContext.Application["users"];
            var user = users.Where(x => x.Username == newUser.Username).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Error = "Username already exists!";
                return View("RegisterTrainer", fitnessCenters);
            }
            newUser.CanLogin = true;
            newUser.UserType = "Trainer";
            string ownerId = Session["username"].ToString();
            


            users.Add(newUser);
            HttpContext.Application["users"] = users;
            XMLWriter.WriteUsers(users);
            return RedirectToAction("Index");
        }


        public ActionResult ApproveComment(string commentId)
        {
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            fitnessCenters = fitnessCenters.Where(x => x.OwnerId == Session["username"].ToString()).ToList();
            var comments = (List<Comment>)HttpContext.Application["comments"];
            var index = comments.FindIndex(x => x.Id == commentId);

            comments[index].IsApproved = true;

            HttpContext.Application["comments"] = comments;

            
            XMLWriter.WriteComments(comments);

            return View("Comments", comments);

;       }
      
        public ActionResult SeeComments(string fitnessCenterId)
        {
            var comments = (List<Comment>)HttpContext.Application["comments"];
            comments = comments.Where(x => x.FitnessCenterId == fitnessCenterId).ToList();
            return View("Comments", comments);

        }




        [HttpPost]
        public ActionResult Details(string fitnessCenterId)
        {
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            var mod = fitnessCenters.Where(x => x.Id == fitnessCenterId).FirstOrDefault();
            var groupTrainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            groupTrainings = groupTrainings.Where(x => x.FitnessCenterId == fitnessCenterId && x.TrainingDate > DateTime.Now).ToList();
            var comments = (List<Comment>)HttpContext.Application["comments"];
            comments = comments.Where(x => x.FitnessCenterId == fitnessCenterId && x.IsApproved == true).ToList();
            ViewData["Comments"] = comments;
            ViewData["Trainings"] = groupTrainings;
            return View("DetailsView", model:mod);
        }

        [HttpPost]
        public ActionResult DetailsVisitor(string fitnessCenterId)
        {
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            var mod = fitnessCenters.Where(x => x.Id == fitnessCenterId).FirstOrDefault();
            var groupTrainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            groupTrainings = groupTrainings.Where(x => x.FitnessCenterId == fitnessCenterId && x.TrainingDate > DateTime.Now).ToList();
            var comments = (List<Comment>)HttpContext.Application["comments"];
            comments = comments.Where(x => x.FitnessCenterId == fitnessCenterId && x.IsApproved == true).ToList();
            ViewData["Comments"] = comments;
            ViewData["Trainings"] = groupTrainings;
            return View("DetailsVisitorView", model: mod);
        }
    }
}