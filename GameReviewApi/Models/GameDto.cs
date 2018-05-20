using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; }

        public string Developer { get; set; }

        public string ReleaseDate { get; set; }

        public int ReviewId { get; set; }
    }
}
