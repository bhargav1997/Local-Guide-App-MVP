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

            public int Ratings { get; set; }

            public DateTime CreatedDate { get; set; }
        }

        public class LocationDto
        {
            public int LocationId { get; set; }
            public string LocationName { get; set; }
            public string LocationDescription { get; set; }
            public string Category { get; set; }
            public string Address { get; set; }
            public int Ratings { get; set; }
            public DateTime CreatedDate { get; set; }
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
        }
}