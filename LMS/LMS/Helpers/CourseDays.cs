using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Helpers
{
    /// <summary>
    /// Calculates course days from(incl) first date with or without limit, depending on course weekdays and holidays. Half-days is not considered.
    /// </summary>
    public class CourseDays
    {
        public CourseDays() : this( new DateTime( 1962, 9, 17 ) ) {
        }

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
            return courseDays.IndexOf( d ) >= 0;            
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

        public DateTime NextDay( DateTime preDay ) {
            DateTime day = preDay.Date;
            while ( true ) {
                day = AddDays( day, 1 );
                if ( IsCourseDay( day ) )
                    return day;
            }
        }

        public int NrCourseDays( DateTime refDay, DateTime relDay ) {
            return 0;
        }

        public DateTime NthDayAfter( DateTime refDay, int courseDaysAfter ) {
            return refDay;
        }


        #region Private
        private List<DateTime> courseDays = null;
        private DayOfWeek[] courseWeekdays;
        //private bool excludeHolidays;
        private DateTime firstDate;
        private DateTime lastDate;
        private bool noEnd;

        static private DateTime[] easterFriday = {
            new DateTime(2000, 4, 21),
            new DateTime(2001, 4, 13),
            new DateTime(2002, 3, 29),
            new DateTime(2003, 4, 18),
            new DateTime(2004, 4, 9), 
            new DateTime(2005, 3, 25),
            new DateTime(2006, 4, 14),
            new DateTime(2007, 4, 6), 
            new DateTime(2008, 3, 21),
            new DateTime(2009, 4, 10),
            new DateTime(2010, 4, 2), 
            new DateTime(2011, 4, 22),
            new DateTime(2012, 4, 6), 
            new DateTime(2013, 3, 29),
            new DateTime(2014, 4, 18),
            new DateTime(2015, 4, 3), 
            new DateTime(2016, 3, 25),
            new DateTime(2017, 4, 14),
            new DateTime(2018, 3, 30),
            new DateTime(2019, 4, 19),
            new DateTime(2020, 4, 10),
            new DateTime(2021, 4, 2), 
            new DateTime(2022, 4, 15),
            new DateTime(2023, 4, 7), 
            new DateTime(2024, 3, 29),
            new DateTime(2025, 4, 18),
            new DateTime(2026, 4, 3), 
            new DateTime(2027, 3, 26),
            new DateTime(2028, 4, 14),
            new DateTime(2029, 3, 30),
            new DateTime(2030, 4, 19),
            new DateTime(2031, 4, 11),
            new DateTime(2032, 3, 26),
            new DateTime(2033, 4, 15),
            new DateTime(2034, 4, 7 ),
            new DateTime(2035, 3, 23),
            new DateTime(2036, 4, 11),
            new DateTime(2037, 4, 3), 
            new DateTime(2038, 4, 23),
            new DateTime(2039, 4, 8), 
            new DateTime(2040, 3, 30),
            new DateTime(2041, 4, 19),
            new DateTime(2042, 4, 4), 
            new DateTime(2043, 3, 27),
            new DateTime(2044, 4, 15),
            new DateTime(2045, 4, 7), 
            new DateTime(2046, 3, 23),
            new DateTime(2047, 4, 12),
            new DateTime(2048, 4, 3), 
            new DateTime(2049, 4, 16),
            new DateTime(2050, 4, 8), 
            new DateTime(2051, 3, 31),
            new DateTime(2052, 4, 19),
            new DateTime(2053, 4, 4), 
            new DateTime(2054, 3, 27),
            new DateTime(2055, 4, 16),
            new DateTime(2056, 3, 31),
            new DateTime(2057, 4, 20),
            new DateTime(2058, 4, 12),
            new DateTime(2059, 3, 28),
            new DateTime(2060, 4, 16),
            new DateTime(2061, 4, 8), 
            new DateTime(2062, 3, 24),
        };
        static private readonly int firstStoredEasterYear = easterFriday[0].Year;
        static private readonly int lastStoredEasterYear = easterFriday[easterFriday.Length - 1].Year;

        static private HashSet<DateTime>[] swedishHolidays = new HashSet<DateTime>[easterFriday.Length];

        static private HashSet<DateTime> GetHolidays(int year) {
            if (year < firstStoredEasterYear || lastStoredEasterYear < year)
                return CalcSwedishHolidays(year);
            if (swedishHolidays[year - firstStoredEasterYear] == null)
                swedishHolidays[year - firstStoredEasterYear] = CalcSwedishHolidays(year);
            return swedishHolidays[year - firstStoredEasterYear];
        }

        static private HashSet<DateTime> CalcSwedishHolidays(int year) {
            var set = new HashSet<DateTime>();
            if (year < 1 || 9999 < year)
                return set;

            //Easter + kristi himelsf.
            if (firstStoredEasterYear <= year && year <= lastStoredEasterYear) {
                DateTime ef_0 = easterFriday[year - firstStoredEasterYear];
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

        static private DateTime AddDays(DateTime date, int days) {
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
        #endregion
    }
}