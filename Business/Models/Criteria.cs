using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class Criteria
    {
        public string SearchString { get; set; }
        public string FilterString { get; set; }
        public int SortOrder { get; set; }
        public int Page { get; set; }
    }
}
