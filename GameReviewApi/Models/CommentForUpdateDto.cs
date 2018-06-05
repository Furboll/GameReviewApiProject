using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class CommentForUpdateDto : CommentForManipulationDto
    {
        [Required(ErrorMessage = "There should fill out some text for your comment")]
        public override string CommentContent
        {
            get
            {
                return base.CommentContent;
            }
            set
            {
                base.CommentContent = value;
            }
        }
    }
}
