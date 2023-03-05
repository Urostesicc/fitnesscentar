using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBProject.Models;
using WEBProject.Models.XML;

namespace WEBProject.Controllers
{
    public class GroupTrainingController : Controller
    {
        // GET: GroupTraining
        public ActionResult Index()
        {
            var users = (List<User>)HttpContext.Application["users"];
            User trainer = users.Where(x => x.Username == Session["username"].ToString() && x.UserType == "Trainer").FirstOrDefault();
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];

            if (trainer != null)
            {
                trainings = trainings.Where(x => x.FitnessCenterId == ((Trainer)trainer).FitnessCenterId && x.TrainerUsername == trainer.Username && x.IsDeleted == false).ToList();
                return View("Trainer", trainings);
            }

            return View();
        }

        public ActionResult MyTrainings()
        {
          

            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            var myTrainings = new List<GroupTraining>();
            var username = Session["username"].ToString();
            var users = (List<User>)HttpContext.Application["users"];
            var user = users.Where(x => x.Username == username).FirstOrDefault();


            foreach (var tr in trainings)
            {
                if (((Visitor)user).TrainingIds.Contains(tr.Id)){
                    myTrainings.Add(tr);
                }
            }

            return View("MyTrainings", myTrainings);

        }

        public ActionResult SignUp(string groupTrainingId, string fitnessCenterId)
        {
            var centers = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            var center = centers.Where(x => x.Id == fitnessCenterId).FirstOrDefault();

            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            var users = (List<User>)HttpContext.Application["users"];
            var index = users.FindIndex(x => x.Username == Session["username"].ToString());
            var trainingIndex = trainings.FindIndex(x => x.Id == groupTrainingId);
            var comments = (List<Comment>)HttpContext.Application["comments"];
            comments = comments.Where(x => x.FitnessCenterId == fitnessCenterId && x.IsApproved == true).ToList();
            ViewData["Comments"] = comments;
            ViewData["Trainings"] = trainings.Where(x => x.FitnessCenterId == fitnessCenterId && x.IsDeleted == false && x.TrainingDate > DateTime.Now).ToList();


            if (trainings[trainingIndex].Visitors.Count >= trainings[trainingIndex].MaxVisitors)
            {
                ViewBag.Error = "Visitor limit reached! Sign up for a different training";
                return View("~/Views/FitnessCenter/DetailsVisitorView.cshtml", model: center);

            }

            
            if (((Visitor)users[index]).TrainingIds.Contains(groupTrainingId))
            {
                ViewBag.Error = "You have already signed up for this training!";
                return View("~/Views/FitnessCenter/DetailsVisitorView.cshtml", model: center);
            }

            ((Visitor)users[index]).TrainingIds.Add(trainings[trainingIndex].Id);
            trainings[trainingIndex].Visitors.Add((Visitor)users[index]);
            ViewData["Trainings"] = trainings.Where(x => x.FitnessCenterId == fitnessCenterId && x.IsDeleted == false && x.TrainingDate > DateTime.Now).ToList();

            XMLWriter.WriteGroupTrainings(trainings);
            XMLWriter.WriteUsers(users);

   


            ViewBag.Success = "You have successfully signed up for this training!";
            return View("~/Views/FitnessCenter/DetailsVisitorView.cshtml", model: center);



        }


        public ActionResult PastTrainings()
        {
            var users = (List<User>)HttpContext.Application["users"];
            User trainer = users.Where(x => x.Username == Session["username"].ToString() && x.UserType == "Trainer").FirstOrDefault();
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            if (trainer != null)
            {
                trainings = trainings.Where(x => x.FitnessCenterId == ((Trainer)trainer).FitnessCenterId && x.TrainerUsername == trainer.Username && x.TrainingDate < DateTime.Now).ToList();
                return View("PastTrainingsView", trainings);
            }
            return View();
        }

        public ActionResult NewTraining(string groupTrainingId)
        {
            return View("NewTraining");
        }

        public ActionResult ModifyTraining(string groupTrainingId)
        {
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            var training = trainings.Where(x => x.Id == groupTrainingId).FirstOrDefault();
            return View("ModifyTraining", training);
        }

        public ActionResult AddGroupTraining(GroupTraining groupTraining)
        {
            groupTraining.Id = Guid.NewGuid().ToString();
            groupTraining.TrainerUsername = Session["username"].ToString();

            var username = Session["username"].ToString();
            var users = (List<User>)HttpContext.Application["users"];
            var user = users.Where(x => x.Username == username).FirstOrDefault();
            groupTraining.FitnessCenterId = ((Trainer)user).FitnessCenterId;

            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            trainings.Add(groupTraining);

            XMLWriter.WriteGroupTrainings(trainings);
            HttpContext.Application["group_trainings"] = trainings;

            return RedirectToAction("Index");
        }

        public ActionResult RemoveGroupTraining(string groupTrainingId)
        {
            var users = (List<User>)HttpContext.Application["users"];
            User trainer = users.Where(x => x.Username == Session["username"].ToString() && x.UserType == "Trainer").FirstOrDefault();
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            var groupTraining = trainings.Where(x => x.Id == groupTrainingId).FirstOrDefault();
            trainings = trainings.Where(x => x.FitnessCenterId == ((Trainer)trainer).FitnessCenterId && x.TrainerUsername == trainer.Username && x.IsDeleted == false).ToList();
            if (groupTraining.Visitors.Any())
            {
                ViewBag.Error = $"Cannot remove this group training, {groupTraining.Visitors.Count} people have already signed up!";
                return View("Trainer", trainings);
            }
            var index = trainings.FindIndex(x => x.Id == groupTraining.Id);
            trainings[index].IsDeleted = true;
           
            XMLWriter.WriteGroupTrainings(trainings);
            HttpContext.Application["group_trainings"] = trainings;

            return RedirectToAction("Index");
        }
        
        public ActionResult ModifyGroupTraining(GroupTraining groupTraining)
        {
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            var index = trainings.FindIndex(x => x.Id == groupTraining.Id);
            groupTraining.TrainerUsername = trainings[index].TrainerUsername;
            trainings[index] = groupTraining;

            XMLWriter.WriteGroupTrainings(trainings);
            HttpContext.Application["group_trainings"] = trainings;

            return RedirectToAction("Index");
        }
        public ActionResult FilterTrainings(string sortOrder, string searchName, string searchType, string searchYearMin, string searchYearMax)
        {
            
            var users = (List<User>)HttpContext.Application["users"];
            User trainer = users.Where(x => x.Username == Session["username"].ToString() && x.UserType == "Trainer").FirstOrDefault();
            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            trainings = trainings.Where(x => x.FitnessCenterId == ((Trainer)trainer).FitnessCenterId && x.TrainerUsername == trainer.Username).ToList();

            if (!String.IsNullOrEmpty(searchName))
            {
                trainings = trainings.Where(x => x.Name.Contains(searchName)).ToList();
            }
            if (!String.IsNullOrEmpty(searchType))
            {
                trainings = trainings.Where(x => x.TrainingType.Contains(searchType)).ToList();
            }
            if (!String.IsNullOrEmpty(searchYearMin) && !String.IsNullOrEmpty(searchYearMax))
            {
                int miny;
                int maxy;
                var success = Int32.TryParse(searchYearMin, out miny);
                var success1 = Int32.TryParse(searchYearMin, out maxy);
                if (success && success1)
                {
                    trainings = trainings.Where(x => x.TrainingDate.Year < maxy && x.TrainingDate.Year > miny).ToList();
                }
            }

            switch (sortOrder)
            {
                case "name_asc":
                    trainings = trainings.OrderBy(x => x.Name).ToList();
                    break;
                case "name_desc":
                    trainings = trainings.OrderByDescending(x => x.Name).ToList();
                    break;
                case "type_asc":
                    trainings = trainings.OrderBy(x => x.TrainingType).ToList();
                    break;
                case "type_desc":
                    trainings = trainings.OrderByDescending(x => x.TrainingType).ToList();
                    break;
                case "date_asc":
                    trainings = trainings.OrderBy(x => x.TrainingDate).ToList();
                    break;
                case "date_desc":
                    trainings = trainings.OrderByDescending(x => x.TrainingDate).ToList();
                    break;
                default:
                    break;
            }
            return View("Trainer", trainings.ToList());
        }
        public ActionResult FilterTrainingsVisitor(string sortOrder, string searchName, string searchType, string searchYearMin, string searchYearMax)
        {

            var trainings = (List<GroupTraining>)HttpContext.Application["group_trainings"];
            var myTrainings = new List<GroupTraining>();
            var username = Session["username"].ToString();
            var users = (List<User>)HttpContext.Application["users"];
            var user = users.Where(x => x.Username == username).FirstOrDefault();


            foreach (var tr in trainings)
            {
                if (((Visitor)user).TrainingIds.Contains(tr.Id))
                {
                    myTrainings.Add(tr);
                }
            }


            if (!String.IsNullOrEmpty(searchName))
            {
                myTrainings = myTrainings.Where(x => x.Name.Contains(searchName)).ToList();
            }
            if (!String.IsNullOrEmpty(searchType))
            {
                myTrainings = myTrainings.Where(x => x.TrainingType.Contains(searchType)).ToList();
            }
            if (!String.IsNullOrEmpty(searchYearMin) && !String.IsNullOrEmpty(searchYearMax))
            {
                int miny;
                int maxy;
                var success = Int32.TryParse(searchYearMin, out miny);
                var success1 = Int32.TryParse(searchYearMin, out maxy);
                if (success && success1)
                {
                    myTrainings = myTrainings.Where(x => x.TrainingDate.Year < maxy && x.TrainingDate.Year > miny).ToList();
                }
            }

            switch (sortOrder)
            {
                case "name_asc":
                    myTrainings = trainings.OrderBy(x => x.Name).ToList();
                    break;
                case "name_desc":
                    myTrainings = trainings.OrderByDescending(x => x.Name).ToList();
                    break;
                case "type_asc":
                    myTrainings = trainings.OrderBy(x => x.TrainingType).ToList();
                    break;
                case "type_desc":
                    myTrainings = trainings.OrderByDescending(x => x.TrainingType).ToList();
                    break;
                case "date_asc":
                    myTrainings = trainings.OrderBy(x => x.TrainingDate).ToList();
                    break;
                case "date_desc":
                    myTrainings = trainings.OrderByDescending(x => x.TrainingDate).ToList();
                    break;
                default:
                    break;
            }
            return View("Trainer", trainings.ToList());
        }
    }
}