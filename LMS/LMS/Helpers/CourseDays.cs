using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Helpers
{
    public class CourseDays
    {
        struct Date {
            int month;
            int day;
        }

        private int[] weekdays;
        private bool excludeHolidays;
        private DateTime startDate;

        public CourseDays( DateTime startDate )
        {

        }

        // 1/1 helg
        // 6/1 helg

        //Påsk: långfredag påskafton ... annandag påsk
        //Kristi himelsffärdsdag torsdag ca 40 dagar efter påsk

        // 1/5 helg
        // 6/6 helg

        //Midsommarafton: fredag mellan 19 och 25 juni
        //Midsommardagen (röd dag): lördag mellan 20 och 26 juni



    }
}