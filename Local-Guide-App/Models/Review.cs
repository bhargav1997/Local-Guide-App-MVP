using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Local_Guide_App.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
