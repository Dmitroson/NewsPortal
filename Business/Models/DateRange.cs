using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class DateRange
    {
        private const string TodayOption = "today";
        private const string YesterdayOption = "yesterday";
        private const string ThisWeekOption = "week";
        private const string AllOption = "all";
        private string options;

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public DateRange()
        {
            Start = new DateTime(1753, 1, 1);
            End = DateTime.MaxValue;
        }

        public DateRange(string filterString)
        {
            if (!string.IsNullOrEmpty(filterString))
            {
               options = filterString;
            }
            else
            {
                options = AllOption;
            }

            if (options.Contains(TodayOption))
            {
                Start = DateTime.Today.Date;
                End = Start;
            }

            if (options.Contains(YesterdayOption))
            {
                Start = DateTime.Today.Date.AddDays(-1);
                End = Start;
            }

            if (options.Contains(YesterdayOption) && options.Contains(TodayOption))
            {
                Start = DateTime.Today.Date.AddDays(-1);
                End = DateTime.Today.Date;
            }

            if (options.Contains(ThisWeekOption))
            {
                Start = DateTime.Today.Date.AddDays(-7);
                End = DateTime.Today.Date;
            }

            if (options.Contains(AllOption))
            {
                Start = new DateTime(1753, 1, 1);
                End = DateTime.MaxValue;
            }
        }

        public bool Includes(DateTime? dateTime)
        {
            return (Start.Date <= dateTime.Value.Date) && (dateTime.Value.Date <= End.Date);
        }
    }
}
