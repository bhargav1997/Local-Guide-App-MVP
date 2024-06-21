using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Local_Guide_App.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace Local_Guide_App.Controllers
{
    public class LocationController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44395/api/locationdata/");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Location location)
        {
            string url = "AddLocation";
            location.CreatedDate = DateTime.Now;

            string jsonpayload = jss.Serialize(location);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine("==>>>"+response.IsSuccessStatusCode);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Handle specific bad request (e.g., ModelState errors) if needed
                return View("Add", location); // Return to the add view with model errors
            }
            else
            {
                // Handle other status codes (e.g., 500 Internal Server Error)
                return RedirectToAction("Error");
            }
        }

        public ActionResult List()
        {
            string url = "ListLocations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<LocationDto> locations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            return View(locations);
        }

        public ActionResult LocationWithReviews(int id)
        {
            string url = $"ListReviewsForLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                LocationWithReviewsDto locationWithReviews = response.Content.ReadAsAsync<LocationWithReviewsDto>().Result;
                return View(locationWithReviews);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                LocationDto selectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
                return View(selectedLocation);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(LocationDto location, HttpPostedFileBase LocationPic)
        {
            string url = $"UpdateLocation/{location.LocationId}";
            location.CreatedDate = DateTime.Now; // Assuming you want to update the CreatedDate as well

            string jsonpayload = jss.Serialize(location);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                if (LocationPic != null && LocationPic.ContentLength > 0)
                {
                    url = $"UploadLocationPic/{location.LocationId}";
                    MultipartFormDataContent multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StreamContent(LocationPic.InputStream), "LocationPic", LocationPic.FileName);

                    response = client.PostAsync(url, multipartContent).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Error");
                    }
                }

                return RedirectToAction("Details", new { id = location.LocationId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Location/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = $"DeleteLocation/{id}";

            // Assuming you have configured HttpClient properly
            var response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return HttpNotFound();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Details(int id)
        {
            string url = $"FindLocation/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                LocationDto location = response.Content.ReadAsAsync<LocationDto>().Result;
                return View(location);
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