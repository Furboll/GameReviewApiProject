using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameReviewApi.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [ForeignKey("ReviewId")]
        public Review Review { get; set; }

        public int ReviewId { get; set; }
    }
}