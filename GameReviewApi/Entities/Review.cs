using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReviewTitle { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        [Required]
        public string Introduction { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string Conclusion { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePosted { get; set; }

        public Game Game { get; set; }
        public ICollection<Comment> Comments { get; set; }
            = new List<Comment>();
    }
}
