using Local_Guide_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

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
                CreatedDate = l.CreatedDate
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
                CreatedDate = l.CreatedDate
            }));

            return Ok(locationDtos);
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
        [System.Web.Http.Route("api/LocationData/FindLocation")]
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
                CreatedDate = location.CreatedDate
            };

            return Ok(locationDto);
        }

        /// <summary>
        /// Updates a specific location.
        /// </summary>
        /// <param name="id">The ID of the location to update</param>
        /// <param name="location">The updated location data</param>
        /// <returns>
        /// HEADER: 204 (No Content)
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/LocationData/UpdateLocation/1
        /// BODY: { "LocationId": 1, "LocationName": "Park", "LocationDescription": "A nice park", "Category": "Recreation", "Address": "123 Park Lane", "CreatedDate": "2023-06-01T00:00:00" }
        /// </example>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/LocationData/UpdateLocation")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationId)
            {
                return BadRequest();
            }

            db.Entry(location).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return StatusCode(System.Net.HttpStatusCode.NoContent);
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
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.LocationId }, location);
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
        [System.Web.Http.Route("api/LocationData/DeleteLocation")]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }
    }
}
