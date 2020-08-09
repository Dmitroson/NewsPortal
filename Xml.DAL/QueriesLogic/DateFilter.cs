﻿using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xml.DAL
{
    public class DateFilter
    {
        private const string TodayOption = "today";
        private const string YesterdayOption = "yesterday";
        private const string ThisWeekOption = "week";
        private const string AllOption = "all";

        protected string Options { get; set; }

        public DateFilter(string filterString)
        {
            if (!string.IsNullOrEmpty(filterString))
            {
                Options = filterString;
            }
            else
            {
                Options = AllOption;
            }
        }

        public IEnumerable<Article> FilterByDate(IEnumerable<Article> articles)
        {
            IEnumerable<Article> result = null;

            if (Options.Contains(AllOption))
            {
                result = articles;
                return result;
            }

            if (Options.Contains(ThisWeekOption))
            {
                result = articles.Where(a => a.PubDate.Value.Date >= DateTime.Today.Date.AddDays(-7));
                return result;
            }

            if (Options.Contains(YesterdayOption) && Options.Contains(TodayOption))
            {
                result = articles.Where(a => a.PubDate.Value.Date == DateTime.Today.Date.AddDays(-1) ||
                                             a.PubDate.Value.Date == DateTime.Today.Date);
                return result;
            }

            if (Options.Contains(YesterdayOption))
            {
                result = articles.Where(a => a.PubDate.Value.Date == DateTime.Today.Date.AddDays(-1));
                return result;
            }

            if (Options.Contains(TodayOption))
            {
                result = articles.Where(a => a.PubDate.Value.Date == DateTime.Today.Date);
                return result;
            }
            return result;
        }
    }
}