using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string CommentContent { get; set; }
        public string DatePosted { get; set; }
    }
}
