using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace LMS.Helpers
{
    /// <summary>
    /// Calculates course days from(incl) first date with or without limit, depending on course weekdays and holidays. Half-days is not considered.
    /// </summary>
    public class CourseDays
    {
        /// <summary>
        /// Calculates course days from(incl) first date with no limit, depending on course weekdays and holidays. Half-days is not considered.
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
        /// Calculates course days from(incl) first date to(incl) last date, depending on course weekdays and holidays. Half-days is not considered.
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
            { 2000, new DateTime(2000, 4, 21) },
            { 2001, new DateTime(2001, 4, 13) },
            { 2002, new DateTime(2002, 3, 29) },
            { 2003, new DateTime(2003, 4, 18) },
            { 2004, new DateTime(2004, 4, 9) },
            { 2005, new DateTime(2005, 3, 25) },
            { 2006, new DateTime(2006, 4, 14) },
            { 2007, new DateTime(2007, 4, 6) },
            { 2008, new DateTime(2008, 3, 21) },
            { 2009, new DateTime(2009, 4, 10) },
            { 2010, new DateTime(2010, 4, 2) },
            { 2011, new DateTime(2011, 4, 22) },
            { 2012, new DateTime(2012, 4, 6) },
            { 2013, new DateTime(2013, 3, 29) },
            { 2014, new DateTime(2014, 4, 18) },
            { 2015, new DateTime(2015, 4, 3) },
            { 2016, new DateTime(2016, 3, 25) },
            { 2017, new DateTime(2017, 4, 14) },
            { 2018, new DateTime(2018, 3, 30) },
            { 2019, new DateTime(2019, 4, 19) },
            { 2020, new DateTime(2020, 4, 10) },
            { 2021, new DateTime(2021, 4, 2) },
            { 2022, new DateTime(2022, 4, 15) },
            { 2023, new DateTime(2023, 4, 7) },
            { 2024, new DateTime(2024, 3, 29) },
            { 2025, new DateTime(2025, 4, 18) },
            { 2026, new DateTime(2026, 4, 3) },
            { 2027, new DateTime(2027, 3, 26) },
            { 2028, new DateTime(2028, 4, 14) },
            { 2029, new DateTime(2029, 3, 30 ) },
            { 2030, new DateTime(2030, 4, 19) },
            { 2031, new DateTime(2031, 4, 11) },
            { 2032, new DateTime(2032, 3, 26) },
            { 2033, new DateTime(2033, 4, 15) },
            { 2034, new DateTime(2034, 4, 7 ) },
            { 2035, new DateTime(2035, 3, 23) },
            { 2036, new DateTime(2036, 4, 11) },
            { 2037, new DateTime(2037, 4, 3) },
            { 2038, new DateTime(2038, 4, 23) },
            { 2039, new DateTime(2039, 4, 8) },
            { 2040, new DateTime(2040, 3, 30) },
            { 2041, new DateTime(2041, 4, 19) },
            { 2042, new DateTime(2042, 4, 4) },
            { 2043, new DateTime(2043, 3, 27) },
            { 2044, new DateTime(2044, 4, 15) },
            { 2045, new DateTime(2045, 4, 7) },
            { 2046, new DateTime(2046, 3, 23) },
            { 2047, new DateTime(2047, 4, 12) },
            { 2048, new DateTime(2048, 4, 3) },
            { 2049, new DateTime(2049, 4, 16) },
            { 2050, new DateTime(2050, 4, 8) },
            { 2051, new DateTime(2051, 3, 31) },
            { 2052, new DateTime(2052, 4, 19) },
            { 2053, new DateTime(2053, 4, 4) },
            { 2054, new DateTime(2054, 3, 27) },
            { 2055, new DateTime(2055, 4, 16) },
            { 2056, new DateTime(2056, 3, 31) },
            { 2057, new DateTime(2057, 4, 20) },
            { 2058, new DateTime(2058, 4, 12) },
            { 2059, new DateTime(2059, 3, 28) },
            { 2060, new DateTime(2060, 4, 16) },
            { 2061, new DateTime(2061, 4, 8) },
            { 2062, new DateTime(2062, 3, 24) },
        };

        static private HashSet<DateTime>[] swedishHolidays = new HashSet<DateTime>[63];

        HashSet<DateTime> GetHolidays(int year) {
            if (year < 2000 || 2062 < year)
                return CalcSwedishHolidays(year);
            if (swedishHolidays[year - 2000] == null)
                swedishHolidays[year - 2000] = CalcSwedishHolidays(year);
            return swedishHolidays[year - 2000];
        }

        private HashSet<DateTime> CalcSwedishHolidays(int year) {
            var set = new HashSet<DateTime>();
            if (year < 1 || 9999 < year)
                return set;

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

            return set;
        }

        private void CalcCourseDays() {
            courseDays = new List<DateTime>();
            for (DateTime d = firstDate; d <= lastDate; d = AddDays(d, 1)) {
                if (IsCourseDay_NoLimits(d))
                    courseDays.Add(d);
            }
        }

        private bool IsHoliday( DateTime d) {
            return GetHolidays(d.Year).Contains(d);
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