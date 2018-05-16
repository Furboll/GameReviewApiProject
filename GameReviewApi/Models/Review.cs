using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        //public string Name { get; set; } <-- not sure if needed
        public string Author { get; set; }
        public string VideoUrl { get; set; }
        public string Introduction { get; set; }
        public string Body { get; set; }
        public string Conclusion { get; set; }
        public DateTime DatePosted { get; set; }

        public Game Game { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
