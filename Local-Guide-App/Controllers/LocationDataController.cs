using Local_Guide_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.IO;

namespace Local_Guide_App.Controllers
{
    public class LocationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of all locations.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All locations in the database.
        /// </returns>
        /// <example>
        /// GET: api/LocationData/LocationsList
        /// </example>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/LocationData/LocationsList")]
        [ResponseType(typeof(LocationDto))]
        public List<LocationDto> LocationsList()
        {
            List<Location> locations = db.Locations.ToList();
            List<LocationDto> locationDtos = new List<LocationDto>();

            locations.ForEach(l => locationDtos.Add(new LocationDto()
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                LocationDescription = l.LocationDescription,
                Category = l.Category,
                Address = l.Address,
                CreatedDate = l.CreatedDate,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
            }));

            return locationDtos;
        }

        /// <summary>
        /// Returns a list of all locations.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All locations in the database.
        /// </returns>
        /// <example>
        /// GET: api/LocationData/ListLocations
        /// </example>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/LocationData/ListLocations")]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocations()
        {
            List<Location> locations = db.Locations.ToList();
            List<LocationDto> locationDtos = new List<LocationDto>();

            locations.ForEach(l => locationDtos.Add(new LocationDto()
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                LocationDescription = l.LocationDescription,
                Category = l.Category,
                Address = l.Address,
                CreatedDate = l.CreatedDate,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
            }));

            return Ok(locationDtos);
        }

        /// <summary>
        /// Returns details of a specific location along with all its reviews.
        /// </summary>
        /// <param name="id">The ID of the location</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The location details and its reviews.
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// GET: api/LocationData/ListReviewsForLocation/1
        /// </example>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/LocationData/ListReviewsForLocation/{id}")]
        [ResponseType(typeof(LocationWithReviewsDto))]
        public IHttpActionResult ListReviewsForLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            List<Review> reviews = db.Reviews.Where(r => r.LocationId == id).ToList();
            List<ReviewDto> reviewDtos = new List<ReviewDto>();

            reviews.ForEach(r => reviewDtos.Add(new ReviewDto()
            {
                ReviewId = r.ReviewId,
                Content = r.Content,
                Rating = r.Rating,
                LocationId = r.LocationId,
                CreatedDate = r.CreatedDate,
            }));

            LocationWithReviewsDto locationWithReviewsDto = new LocationWithReviewsDto()
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                LocationDescription = location.LocationDescription,
                Category = location.Category,
                Address = location.Address,
                CreatedDate = location.CreatedDate,
                Reviews = reviewDtos,
                LocationHasPic = location.LocationHasPic,
                PicExtension = location.PicExtension,
            };

            return Ok(locationWithReviewsDto);
        }

        /// <summary>
        /// Finds a specific location by ID.
        /// </summary>
        /// <param name="id">The ID of the location</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The location with the specified ID.
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// GET: api/LocationData/FindLocation/1
        /// </example>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/LocationData/FindLocation/{id}")]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult FindLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            LocationDto locationDto = new LocationDto()
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                LocationDescription = location.LocationDescription,
                Category = location.Category,
                Address = location.Address,
                CreatedDate = location.CreatedDate,
                AverageRating = location.Reviews.Any() ? Math.Round(location.Reviews.Average(r => r.Rating), 2) : 0,
                LocationHasPic = location.LocationHasPic, 
                PicExtension = location.PicExtension
            };

            return Ok(locationDto);
        }

        /// <summary>
        /// Updates a specific location.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="locationDto">The data transfer object containing updated location information.</param>
        /// <returns>
        /// (No Content) if the location is successfully updated.
        /// (Bad Request) if the ModelState is invalid or if the ID in the URL does not match the ID in the locationDto.
        /// (Not Found) if the location with the specified ID is not found in the database.
        /// </returns>
        /// <example>
        /// POST: api/LocationData/UpdateLocation/5
        /// BODY: {
        ///   "LocationId": 5,
        ///   "LocationName": "Updated Location Name",
        ///   "LocationDescription": "Updated Description",
        ///   "Category": "Updated Category",
        ///   "Address": "Updated Address"
        /// }
        /// </example>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/LocationData/UpdateLocation/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateLocation(int id, LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationDto.LocationId)
            {
                return BadRequest();
            }

            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            // Update location properties
            location.LocationName = locationDto.LocationName;
            location.LocationDescription = locationDto.LocationDescription;
            location.Category = locationDto.Category;
            location.Address = locationDto.Address;
            location.CreatedDate = locationDto.CreatedDate;

            // Save changes
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Checks if a location exists in the database.
        /// </summary>
        /// <param name="id">The ID of the location to check.</param>
        /// <returns>True if the location exists; otherwise, false.</returns>
        private bool LocationExists(int id)
        {
            return db.Locations.Any(e => e.LocationId == id);
        }

        /// <summary>
        /// Adds a new location to the system.
        /// </summary>
        /// <param name="location">The location to add</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: The newly created location.
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/LocationData/AddLocation
        /// BODY: { "LocationName": "Park", "LocationDescription": "A nice park", "Category": "Recreation", "Address": "123 Park Lane", "CreatedDate": "2023-06-01T00:00:00" }
        /// </example>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/LocationData/AddLocation")]
        [ResponseType(typeof(Location))]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Debug.WriteLine($"Error adding location: {ex.Message}");
                return InternalServerError(); // Return 500 Internal Server Error on failure
            }
        }

        /// <summary>
        /// Deletes a specific location by ID.
        /// </summary>
        /// <param name="id">The ID of the location to delete</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The deleted location.
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/LocationData/DeleteLocation/1
        /// </example>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/LocationData/DeleteLocation/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.OK);
        }

        /// <summary>
        /// Uploads a picture for a specific location.
        /// </summary>
        /// <param name="id">The ID of the location.</param>
        /// <returns>HTTP status code 200 (OK) if the upload is successful.</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/LocationData/UploadLocationPic/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UploadLocationPic(int id)
        {
            bool hasPic = false;
            string picExtension;

            if (Request.Content.IsMimeMultipartContent())
            {
                int numFiles = HttpContext.Current.Request.Files.Count;

                if (numFiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var locationPic = HttpContext.Current.Request.Files[0];

                    Debug.WriteLine("locationPic.FileName" + locationPic.FileName);
                    if (locationPic.ContentLength > 0)
                    {
                        var validTypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(locationPic.FileName).Substring(1);

                        Debug.WriteLine("extension" + extension);

                        if (validTypes.Contains(extension))
                        {
                            try
                            {
                                string fn = id + "." + extension;
                                string directoryPath = HttpContext.Current.Server.MapPath("~/Content/Images/Locations/");
                                string path = Path.Combine(directoryPath, fn);

                                Debug.WriteLine($"{path}");
                                // Ensure the directory exists
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                locationPic.SaveAs(path);

                                hasPic = true;
                                picExtension = extension;

                                Location selectedLocation = db.Locations.Find(id);
                                selectedLocation.LocationHasPic = hasPic;
                                selectedLocation.PicExtension = extension;
                                db.Entry(selectedLocation).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Location image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }
                }

                return Ok();
            }
            else
            {
                return BadRequest("Not multipart content");
            }
        }
    }
}
