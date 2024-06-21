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
        [Authorize]
        public ActionResult Add(int locationId)
        {
            Debug.WriteLine("Location:"+ locationId);
            ViewData["LocationId"] = locationId;
            return View();
        }

        [HttpPost]
        [Authorize]
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
                    return RedirectToAction("Index", "Home");
                }
                else
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
            Debug.WriteLine("LocationId" + locationId);
            string url = $"ListReviewsForLocation/{locationId}";
            Debug.WriteLine("Url:::" + url);

            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Deserialize JSON array into IEnumerable<ReviewDto>
                var json = response.Content.ReadAsStringAsync().Result;
                IEnumerable<ReviewDto> reviews = jss.Deserialize<IEnumerable<ReviewDto>>(json);

                ViewBag.LocationId = locationId;
                return View(reviews);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = $"FindReview/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ReviewDto review = response.Content.ReadAsAsync<ReviewDto>().Result;
                return View(review);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(ReviewDto review)
        {
            string url = $"UpdateReview/{review.ReviewId}";
            string jsonpayload = jss.Serialize(review);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("LocationWithReviews", "Location", new { id = review.LocationId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, int locationId)
        {
            string url = $"DeleteReview/{id}";
            HttpResponseMessage response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("LocationWithReviews", "Location", new { id = locationId });
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