using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public abstract class GameForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You should fill out a genre")]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Required(ErrorMessage = "You should fill out a publisher")]
        [MaxLength(50)]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "You should fill out a developer")]
        [MaxLength(50)]
        public string Developer { get; set; }

        [Required(ErrorMessage = "You should fill out a release date")]
        public DateTime ReleaseDate { get; set; }
    }
}
