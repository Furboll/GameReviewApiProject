using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Author { get; set; }
        public string CommentContent { get; set; }
        public DateTime DatePosted { get; set; }

        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}