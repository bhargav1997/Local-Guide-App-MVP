using Local_Guide_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Local_Guide_App.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var locations = db.Locations.ToList();

            var locationDtos = locations.Select(location => new LocationDto
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                LocationDescription = location.LocationDescription,
                Category = location.Category,
                Address = location.Address,
                CreatedDate = location.CreatedDate,
                AverageRating = location.Reviews.Any() ? Math.Round(location.Reviews.Average(r => r.Rating), 2) : 0
            }).ToList();

            return View(locationDtos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "A CMS for local guides to share information about attractions, restaurants, events, and services in their area.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}