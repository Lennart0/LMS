using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Helpers
{
    public class CourseDays
    {
        /// <summary>
        /// Calculates course days from(incl) first date to(incl) last date, depending on weekdays and holidays
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="lastDate"></param>
        /// <param name="weekdays"></param>
        public CourseDays( DateTime firstDate, DateTime lastDate, params DayOfWeek[] weekdays ) {
            this.firstDate = firstDate;
            this.lastDate = lastDate;
            this.weekdays = weekdays;
            if ( this.weekdays.Length == 0 )
                this.weekdays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        }

        public List<DateTime> Days {
            get {
                if ( courseDays == null )
                    CalcCourseDays();
                return courseDays;
            }
        }
        public bool IsCourseDay( DateTime day ) {
            if ( courseDays == null )
                CalcCourseDays();
            return courseDays.IndexOf( day.Date ) >= 0;
        }
        public DateTime? NextCourseDay( DateTime preDay ) {
            DateTime day = preDay.Date;
            while (lastDate < firstDate || day <= lastDate) {
                day += new TimeSpan( 1, 0, 0, 0 );
                if ( IsCourseDay( day ) )
                    return day;
            }
            return null;
        }


        struct Date
        {
            public int month;
            public int day;
        }

        private List<DateTime> courseDays = null;
        private DayOfWeek[] weekdays;
        private bool excludeHolidays;
        private DateTime firstDate;
        private DateTime lastDate;

        Dictionary<int, Date> easterSunday = new Dictionary<int, Date>() {
            { 2015, new Date {month = 4, day = 5 } },
            { 2016, new Date {month = 3, day = 27 } },
            { 2017, new Date {month = 4, day = 16 } },
            { 2018, new Date {month = 4, day = 1 } },
            { 2019, new Date {month = 4, day = 21 } },
            { 2020, new Date {month = 4, day = 12 } },
            { 2021, new Date {month = 4, day = 4 } },
            { 2022, new Date {month = 4, day = 17 } },
            { 2023, new Date {month = 4, day = 9 } },
            { 2024, new Date {month = 3, day = 31 } },
            { 2025, new Date {month = 4, day = 20 } },
        };

        HashSet<Date> fixedHolidays = new HashSet<Date>() {
            new Date { month = 1, day = 1 },
            new Date { month = 1, day = 6 },
            new Date { month = 5, day = 1 },
            new Date { month = 6, day = 6 },
            new Date { month = 12, day = 24 },
            new Date { month = 12, day = 25 },
            new Date { month = 12, day = 26 },
            new Date { month = 12, day = 31 },
        };

        Dictionary<int, HashSet<Date>> movingHolidays = new Dictionary<int, HashSet<Date>>();

        private void CalcMovingHolidays(int year) {
            var set = new HashSet<Date>();

            if ( easterSunday.ContainsKey(year) ) {
                Date es = easterSunday[year];
                DateTime esDate = new DateTime( year, es.month, es.day ).Date;
                DateTime esDateM2 = esDate - new TimeSpan( 2, 0, 0, 0 );
                DateTime esDateM1 = esDate - new TimeSpan( 1, 0, 0, 0 );
                DateTime esDateP1 = esDate + new TimeSpan( 1, 0, 0, 0 );
                DateTime esDateP39 = esDate + new TimeSpan( 39, 0, 0, 0 );

                set.Add( es );
                set.Add( new Date { month = esDateM2.Month, day = esDateM2.Day } );
                set.Add( new Date { month = esDateM1.Month, day = esDateM1.Day } );
                set.Add( new Date { month = esDateP1.Month, day = esDateP1.Day } );
                set.Add( new Date { month = esDateP39.Month, day = esDateP39.Day } );
            }

            var midsummer = new DateTime( year, 6, 19 ).Date;
            int wdDiff = (int)DayOfWeek.Friday -( int)midsummer.DayOfWeek;
            if ( wdDiff < 0 )
                wdDiff += 7;
            midsummer += new TimeSpan( wdDiff, 0, 0, 0 );
            set.Add( new Date { month = midsummer.Month, day = midsummer.Day } );
            midsummer += new TimeSpan( 1, 0, 0, 0 );
            set.Add( new Date { month = midsummer.Month, day = midsummer.Day } );

            var saintsDay = new DateTime( year, 10, 31 ).Date;
            wdDiff = (int)DayOfWeek.Saturday - (int)saintsDay.DayOfWeek;
            if ( wdDiff < 0 )
                wdDiff += 7;
            saintsDay += new TimeSpan( wdDiff, 0, 0, 0 );
            set.Add( new Date { month = saintsDay.Month, day = saintsDay.Day } );

            movingHolidays[year] = set;
        }

        private void CalcCourseDays() {
            courseDays = new List<DateTime>();
            for ( DateTime d = firstDate; d <= lastDate; d = d + new TimeSpan( 1, 0, 0, 0 ) ) {
                if ( _IsCourseDay( d ) )
                    courseDays.Add( d );
            }
        }

        private bool _IsCourseDay( DateTime d ) {

            if ( weekdays.Contains( d.DayOfWeek ) ) {
                Date date = new Date { month = d.Month, day = d.Day };

                if ( fixedHolidays.Contains( date ) )
                    return false;

                if ( !movingHolidays.ContainsKey( d.Year ) )
                    CalcMovingHolidays( d.Year );
                if ( movingHolidays[d.Year].Contains( date ) )
                    return false;

                return true;
            }

            return false;
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