using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace NewsPortal.Models
{
    public class FilterByDate
    {
        public bool Today { get; private set; }
        public bool Yesterday { get; private set; }
        public bool ThisWeek { get; private set; }
        public bool All { get; private set; }


    }

    public enum Order
    {
        Today,
        Yesterday,
        ThisWeek,
        All
    }
}