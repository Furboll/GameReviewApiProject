using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string CommentContent { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}