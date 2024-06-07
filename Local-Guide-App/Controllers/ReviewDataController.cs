using Local_Guide_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Local_Guide_App.Controllers
{
    public class ReviewDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all reviews for a specific location.
        /// </summary>
        /// <param name="locationId">The ID of the location</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All reviews for the specified location.
        /// </returns>
        /// <example>
        /// GET: api/ReviewData/ListReviewsForLocation/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ReviewDto>))]
        public IHttpActionResult ListReviewsForLocation(int locationId)
        {
            List<Review> reviews = db.Reviews.Where(r => r.LocationId == locationId).ToList();
            List<ReviewDto> reviewDtos = new List<ReviewDto>();

            reviews.ForEach(r => reviewDtos.Add(new ReviewDto()
            {
                ReviewId = r.ReviewId,
                Content = r.Content,
                Rating = r.Rating,
                CreatedDate = r.CreatedDate
            }));

            return Ok(reviewDtos);
        }

        /// <summary>
        /// Finds a specific review by ID.
        /// </summary>
        /// <param name="id">The ID of the review</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The review with the specified ID.
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// GET: api/ReviewData/FindReview/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ReviewDto))]
        public IHttpActionResult FindReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            ReviewDto reviewDto = new ReviewDto()
            {
                ReviewId = review.ReviewId,
                Content = review.Content,
                Rating = review.Rating,
                LocationId = review.LocationId,
                CreatedDate = review.CreatedDate
            };

            return Ok(reviewDto);
        }

        /// <summary>
        /// Updates a specific review.
        /// </summary>
        /// <param name="id">The ID of the review to update</param>
        /// <param name="review">The updated review data</param>
        /// <returns>
        /// HEADER: 204 (No Content)
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ReviewData/UpdateReview/1
        /// BODY: { "ReviewId": 1, "Content": "Great place!", "Rating": 5, "LocationId": 1, "CreatedDate": "2023-06-01T00:00:00" }
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            db.Entry(review).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new review to the system.
        /// </summary>
        /// <param name="review">The review to add</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: The newly created review.
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ReviewData/AddReview
        /// BODY: { "Content": "Great place!", "Rating": 5, "LocationId": 1, "CreatedDate": "2023-06-01T00:00:00" }
        /// </example>
        [ResponseType(typeof(Review))]
        [HttpPost]
        public IHttpActionResult AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = review.ReviewId }, review);
        }

        /// <summary>
        /// Deletes a specific review by ID.
        /// </summary>
        /// <param name="id">The ID of the review to delete</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The deleted review.
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ReviewData/DeleteReview/1
        /// </example>
        [ResponseType(typeof(Review))]
        [HttpPost]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return Ok(review);
        }
    }
}
