using Local_Guide_App.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Local_Guide_App.Controllers
{
    public class ReviewController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ReviewController()
        {
         
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44395/api/ReviewData");
        }

        [HttpGet]
        public ActionResult Add(int locationId)
        {
            Debug.WriteLine("Location:"+ locationId);
            ViewData["LocationId"] = locationId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Review review)
        {
            string url = "AddReview"; // Corrected URL to match the route in API controller

            string jsonpayload = jss.Serialize(review);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", "Location", new { id = review.LocationId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        [HttpGet]
        public ActionResult List(int locationId)
        {
            Debug.WriteLine("LocationId"+ locationId);
            string url = $"/ListReviewsForLocation/{locationId}";
            Debug.WriteLine("Url:::" + url);
            var response = client.GetAsync(url).Result;

            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                // Set media type explicitly to JSON
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Deserialize the response content
                IEnumerable<ReviewDto> reviews = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;
                ViewBag.LocationId = locationId;
                return View(reviews);
            }
            else
            {
                // Handle unsuccessful response
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}