using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Local_Guide_App.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Web;

namespace Local_Guide_App.Controllers
{
    public class LocationController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44395/api/locationdata");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Location location)
        {
            string url = "/AddLocation";

            string jsonpayload = jss.Serialize(location);

            Console.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult List()
        {
            string url = "/ListLocations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<LocationDto> locations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            return View(locations);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}