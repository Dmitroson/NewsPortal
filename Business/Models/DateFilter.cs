using System;
using System.Linq;

namespace Business.Models
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

        public IQueryable<Article> FilterByDate(IQueryable<Article> articles)
        {
            IQueryable<Article> result = null;

            if (Options.Contains(AllOption))
            {
                result = articles;
                return result;
            }

            if (Options.Contains(ThisWeekOption))
            {
                result = articles.Where(a => a.PubDate.Value.Date >= DateTime.Today.Date.AddDays(-7) &&
                                             a.PubDate.Value.Date <= DateTime.Today.Date);
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