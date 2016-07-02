using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Helpers
{
    public class CourseDays
    {
        /// <summary>
        /// Calculates course days from(incl) first date with no limit, depending on course weekdays and holidays
        /// </summary>
        /// <param name="firstDate">First day of course</param>
        /// <param name="courseWeekdays">Weekdays of course days (default monday - friday)</param>
        public CourseDays(DateTime firstDate, params DayOfWeek[] courseWeekdays) {
            this.firstDate = firstDate;
            this.lastDate = firstDate;
            noEnd = true;
            this.courseWeekdays = courseWeekdays;
            if (this.courseWeekdays.Length == 0)
                this.courseWeekdays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        }

        /// <summary>
        /// Calculates course days from(incl) first date to(incl) last date, depending on course weekdays and holidays
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        /// <param name="courseWeekdays"></param>
        public CourseDays( DateTime firstDate, DateTime lastDate, params DayOfWeek[] courseWeekdays ) : this(firstDate, courseWeekdays) {            
            this.lastDate = lastDate;
            noEnd = false;
        }

        /// <summary>
        /// List of all course days. (Only usefull if last date is specified)
        /// </summary>
        public List<DateTime> Days {
            get {
                if (courseDays == null)
                    CalcCourseDays();
                return courseDays;
            }
        }

        /// <summary>
        /// Is the day a course day?
        /// </summary>
        /// <param name="day">The day to test.</param>
        /// <returns>True if day is course day.</returns>
        public bool IsCourseDay( DateTime day ) {
            DateTime d = day.Date;
            if (noEnd)
                return d >= firstDate && IsCourseDay_NoLimits(d);

            if ( courseDays == null )
                CalcCourseDays();
            return courseDays.IndexOf( d.Date ) >= 0;            
        }

        /// <summary>
        /// Next course day after preDay, or null if none.
        /// </summary>
        /// <param name="preDay">The previous day.</param>
        /// <returns>Next course day after preDay, or null if none.</returns>
        public DateTime? NextCourseDay( DateTime preDay ) {
            DateTime day = preDay.Date;
            while (noEnd || day <= lastDate) {
                day = AddDays(day, 1);
                if ( IsCourseDay( day ) )
                    return day;
            }
            return null;
        }


        private List<DateTime> courseDays = null;
        private DayOfWeek[] courseWeekdays;
        //private bool excludeHolidays;
        private DateTime firstDate;
        private DateTime lastDate;
        private bool noEnd;

        static private Dictionary<int, DateTime> easterFriday = new Dictionary<int, DateTime>() {
            { 2015, new DateTime(2015, 4,3 ) },
            { 2016, new DateTime(2016, 3, 25 ) },
            { 2017, new DateTime(2017, 4, 14 ) },
            { 2018, new DateTime(2018, 3, 30 ) },
            { 2019, new DateTime(2019, 4, 19 ) },
            { 2020, new DateTime(2020, 4, 10 ) },
            { 2021, new DateTime(2021, 4, 2 ) },
            { 2022, new DateTime(2022, 4, 15 ) },
            { 2023, new DateTime(2023, 4, 7 ) },
            { 2024, new DateTime(2024, 3, 29 ) },
            { 2025, new DateTime(2025, 4, 18 ) },
        };

        private Dictionary<int, HashSet<DateTime>> holidays = new Dictionary<int, HashSet<DateTime>>();

        private void CalcHolidays(int year) {
            var set = new HashSet<DateTime>();

            //Easter + kristi himelsf.
            if (easterFriday.ContainsKey(year)) {
                DateTime ef_0 = easterFriday[year];
                set.Add(ef_0);
                set.Add(AddDays(ef_0, 1));
                set.Add(AddDays(ef_0, 2));
                set.Add(AddDays(ef_0, 3));
                set.Add(AddDays(ef_0, 41)); //Kristi himelsf.
            }

            //Midsummer
            var midsummer = new DateTime(year, 6, 19);
            int wdDiff = (int)DayOfWeek.Friday - (int)midsummer.DayOfWeek;
            if (wdDiff < 0)
                wdDiff += 7;
            midsummer = AddDays(midsummer, wdDiff);
            set.Add(midsummer);
            midsummer = AddDays(midsummer, 1);
            set.Add(midsummer);

            //All saints day
            var saintsDay = new DateTime(year, 10, 31);
            wdDiff = (int)DayOfWeek.Saturday - (int)saintsDay.DayOfWeek;
            if (wdDiff < 0)
                wdDiff += 7;
            set.Add(AddDays(saintsDay, wdDiff));

            //Fixed holidays
            set.Add(new DateTime(year, 1, 1));
            set.Add(new DateTime(year, 1, 6));
            set.Add(new DateTime(year, 5, 1));
            set.Add(new DateTime(year, 6, 6));
            set.Add(new DateTime(year, 12, 24));
            set.Add(new DateTime(year, 12, 25));
            set.Add(new DateTime(year, 12, 26));
            set.Add(new DateTime(year, 12, 31));

            holidays[year] = set;
        }

        private void CalcCourseDays() {
            courseDays = new List<DateTime>();
            for (DateTime d = firstDate; d <= lastDate; d = AddDays(d, 1)) {
                if (IsCourseDay_NoLimits(d))
                    courseDays.Add(d);
            }
        }

        private bool IsHoliday( DateTime d) {
            if (!holidays.ContainsKey(d.Year))
                CalcHolidays(d.Year);
            return holidays[d.Year].Contains(d);
        }

        private bool IsCourseDay_NoLimits( DateTime d ) {
            if ( courseWeekdays.Contains( d.DayOfWeek ) )
                return !IsHoliday(d);
            return false;
        }

        private DateTime AddDays(DateTime date, int days) {
            return date + new TimeSpan(days, 0, 0, 0);
        }

        // 1/1 helg
        // 6/1 helg

        //Påsk: långfredag påskafton ... annandag påsk
        //Kristi himelsffärdsdag torsdag ca 40 dagar efter påsk

        // 1/5 helg
        // 6/6 helg

        //Midsommarafton: fredag mellan 19 och 25 juni
        //Midsommardagen (röd dag): lördag mellan 20 och 26 juni

        //Alla helgons dag: lördag mellan 31 October–6 November

        // 24/12 Julafton
        // 25/12 helg
        // 26/12 helg

        //31/12 Nyårsafton
    }
}