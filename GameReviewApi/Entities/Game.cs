using System;
using System.Collections.Generic;

namespace GameReviewApi.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}