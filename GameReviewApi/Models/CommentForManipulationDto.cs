using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public abstract class CommentForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out your name")]
        [MaxLength(50)]
        public string Author { get; set; }

        [MaxLength(500)]
        public virtual string CommentContent { get; set; }

        [Required(ErrorMessage = "There should be a date when you posted this")]
        public string DatePosted { get; set; }
    }
}
