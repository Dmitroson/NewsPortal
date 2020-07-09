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

        public FilterByDate(string filterString)
        {
            //string s = context["today"] as string;
            //if (s == "1")
            //{
            //    Orders.Add(1);
            //}
            
            //if ((string)context.Session["yesterday"] == "1")
            //{
            //    Orders.Add(2);
            //}

            //if ((string)context.Session["week"] == "1")
            //{
            //    Orders.Add(3);
            //}

            //if ((string)context.Session["all"] == "1")
            //{
            //    Orders.Add(4);
            //}
        }


    }

    public enum Order
    {
        Today,
        Yesterday,
        ThisWeek,
        All
    }
}