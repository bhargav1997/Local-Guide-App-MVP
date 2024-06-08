using Local_Guide_App.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            client.BaseAddress = new Uri("https://localhost:44395/api/reviewdata/");
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
            try
            {
                review.CreatedDate = DateTime.Now;

                string jsonpayload = jss.Serialize(review);
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync("AddReview", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List", "Review", new { id = review.LocationId });
                } else
                {
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "An error occurred during review creation.");

                ModelState.AddModelError("==>", "An unexpected error occurred. Please try again later.");
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult List(int locationId)
        {
            Debug.WriteLine("LocationId"+ locationId);
            string url = $"ListReviewsForLocation/{locationId}";
            Debug.WriteLine("Url:::" + url);
            var response = client.GetAsync(url).Result;

            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                IEnumerable<ReviewDto> reviews = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;
                ViewBag.LocationId = locationId;
                return View(reviews);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}