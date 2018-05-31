﻿using GameReviewApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Models
{
    public class ReviewForCreationDto
    {
        public string ReviewTitle { get; set; }

        public string Author { get; set; }

        public string VideoUrl { get; set; }

        public string Introduction { get; set; }

        public string Body { get; set; }

        public string Conclusion { get; set; }

        public DateTime DatePosted { get; set; }

        public Game Game { get; set; }
        //public List<Comment> Comments { get; set; }
    }
}
