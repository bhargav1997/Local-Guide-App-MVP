    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Local_Guide_App.Models
    {
        public class Location
        {
            [Key]
            public int LocationId { get; set; }

            [Required]
            public string LocationName { get; set; }
            public string LocationDescription { get; set; }
            public string Category { get; set; }
            public string Address { get; set; }

            // Remove Ratings property as average ratings will be calculated dynamically

            public DateTime CreatedDate { get; set; }

            // Navigation property to reviews
            public virtual ICollection<Review> Reviews { get; set; }

            // Data needed for keeping track of Location images uploaded
            // Images deposited into /Content/Images/Locations/{id}.{extension}
            public bool LocationHasPic { get; set; }
            public string PicExtension { get; set; }
    }

        public class LocationDto
        {
            public int LocationId { get; set; }
            public string LocationName { get; set; }
            public string LocationDescription { get; set; }
            public string Category { get; set; }
            public string Address { get; set; }
            public double AverageRating { get; set; } // New property for average rating
            public DateTime CreatedDate { get; set; }

            //data needed for keeping track of Location images uploaded
            //images deposited into /Content/Images/Locations/{id}.{extension}
            public bool LocationHasPic { get; set; }
            public string PicExtension { get; set; }
    }

    public class LocationWithReviewsDto
        {
            public int LocationId { get; set; }
            public string LocationName { get; set; }
            public string LocationDescription { get; set; }
            public string Category { get; set; }
            public string Address { get; set; }
            public DateTime CreatedDate { get; set; }
            public List<ReviewDto> Reviews { get; set; }
            public bool LocationHasPic { get; set; }
            public string PicExtension { get; set; }
    }
}