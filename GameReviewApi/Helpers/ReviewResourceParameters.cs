using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Helpers
{
    public class ReviewResourceParameters
    {
        const int maxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string ReviewTitle { get; set; }

        public string GameTitle { get; set; }

        public string SearchQuery { get; set; }

        public string OrderBy { get; set; } = "ReviewTitle";

        public string Fields { get; set; }
    }
}
