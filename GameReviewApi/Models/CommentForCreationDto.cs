using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class CommentForCreationDto
    {
        public string Author { get; set; }
        public string CommentContent { get; set; }
        public string DatePosted { get; set; }
    }
}
