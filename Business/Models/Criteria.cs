using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class Criteria
    {
        public string SearchString { get; set; }
        public DateRange FilterRange { get; set; }
        public SortOrder SortOrder { get; set; }
        public int Page { get; set; }
        public int ArticlesPerPage { get; set; }
    }

    public enum SortOrder
    {
        DateAscending,
        DateDescending,
        TitleAscending,
        TitleDescending,
        DescriptionAscending,
        DescriptionDescending
    }
}
