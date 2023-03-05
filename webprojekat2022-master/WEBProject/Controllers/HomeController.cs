using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBProject.Models;

namespace WEBProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<FitnessCenter> fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            return View(fitnessCenters.Where(x=>x.IsDeleted == false).ToList());
        }
        
        public ActionResult VisitorHome()
        {
            List<FitnessCenter> fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];
            return View(fitnessCenters.Where(x => x.IsDeleted == false).ToList());
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult FilterCenters(string sortOrder, string searchName, string searchAddress, string searchYearMin, string searchYearMax)
        {
            var fitnessCenters = (List<FitnessCenter>)HttpContext.Application["fitness_centers"];

            if (!String.IsNullOrEmpty(searchName))
            {
                fitnessCenters = fitnessCenters.Where(x => x.Name.Contains(searchName)).ToList();
            }
            if (!String.IsNullOrEmpty(searchAddress))
            {
                fitnessCenters = fitnessCenters.Where(x => x.Address.Contains(searchAddress)).ToList();
            }
            if (!String.IsNullOrEmpty(searchYearMin))
            {
                int y;
                var success = Int32.TryParse(searchYearMin, out y);
                if (success)
                {
                    fitnessCenters = fitnessCenters.Where(x => x.FoundedIn >= y).ToList();
                }
            }
            if (!String.IsNullOrEmpty(searchYearMax))
            {
                int y;
                var success = Int32.TryParse(searchYearMax, out y);
                if (success)
                {
                    fitnessCenters = fitnessCenters.Where(x => x.FoundedIn <= y).ToList();
                }
            }
            switch (sortOrder)
            {
                case "name_asc":
                    fitnessCenters = fitnessCenters.OrderBy(x => x.Name).ToList();
                    break;
                case "name_desc":
                    fitnessCenters = fitnessCenters.OrderByDescending(x => x.Name).ToList();
                    break;
                case "address_asc":
                    fitnessCenters = fitnessCenters.OrderBy(x => x.Address).ToList();
                    break;
                case "address_desc":
                    fitnessCenters = fitnessCenters.OrderByDescending(x => x.Address).ToList();
                    break;
                case "year_asc":
                    fitnessCenters = fitnessCenters.OrderBy(x => x.FoundedIn).ToList();
                    break;
                case "year_desc":
                    fitnessCenters = fitnessCenters.OrderByDescending(x => x.FoundedIn).ToList();
                    break;
                default:
                    break;
            }
            return View("Index", fitnessCenters.ToList());
        }
    }
}