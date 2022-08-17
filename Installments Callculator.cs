namespace CheckInstallments
{
    internal class InstallmentsCalculator
    {
        public InstallmentsCalculator(DateTime Date)
        {
            date = Date;
            for (int i = 0; i < 12; i++)
            {
                dateMonths[i] = date.AddDays(30*i);
            }

        }

        private DateTime date { get; }

        private DateTime[] dateMonths = new DateTime[12];

        internal void CheckInstallments()
        {
            int firstYear = Convert.ToInt32(date.Year);
            int lastYear = firstYear + 1;

            Console.WriteLine("\nChecking Now...\n");

            DateTime firstEaster = PickingEaster(firstYear);
            DateTime lastEaster = PickingEaster(lastYear);

            bool firstLeapYear = isLeapYear(firstYear);
            bool lastLeapYear = isLeapYear(lastYear);

            DateTime firstCarnival = !firstLeapYear ? firstEaster.AddDays(-47) : firstEaster.AddDays(-48);
            DateTime lastCarnival = !lastLeapYear ? lastEaster.AddDays(-47) : lastEaster.AddDays(-48);

            var firstHolidays = ListHolidays(firstEaster, firstCarnival);
            var lastHolidays = ListHolidays(lastEaster, lastCarnival);

            CheckIsHoliday(firstHolidays, lastHolidays);
            CheckIsWeekend();

            Result();
        }

        private void Result()
        {
            int installment = 1;
            foreach (var month in dateMonths)
            {
                Console.WriteLine($"The installment of the month {installment}, will be on the day {month.Day}/{month.Month}/{month.Year}");
                installment++;
            }
        }

        private void CheckIsWeekend()
        {
            for(int i = 0; i < dateMonths.Length; i++)
            {
                while (dateMonths[i].DayOfWeek is (DayOfWeek)6 or 0)
                {
                    for(int j = i; j < dateMonths.Length; j++)
                    {
                        dateMonths[j] = dateMonths[j].AddDays(1);
                    }
                }
            }
        }

        private void CheckIsHoliday(List<Holiday> firstHolidays, List<Holiday> lastHolidays)
        {
            for(int i = 0; i < dateMonths.Length; i++)
            {
                if (dateMonths[i].Year == date.Year)
                {
                    foreach (var holiday in firstHolidays)
                    {
                        if (dateMonths[i].Day == holiday.Day && dateMonths[i].Month == holiday.Month)
                        {
                            for (int j = i; j < dateMonths.Length; j++)
                            {
                                dateMonths[j] = dateMonths[j].AddDays(1);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var holiday in lastHolidays)
                    {
                        if (dateMonths[i].Day == holiday.Day && dateMonths[i].Month == holiday.Month)
                        {
                            for (int j = i; j < dateMonths.Length; j++)
                            {
                                dateMonths[j] = dateMonths[j].AddDays(1);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private static List<Holiday> ListHolidays(DateTime easter, DateTime carnival)
        {
            List<Holiday> holidays = new List<Holiday>
            {
                new Holiday(1,1,"New Year's Day"),
                new Holiday(carnival.Day,carnival.Month, "Carnival"),
                new Holiday(easter.AddDays(-2).Day,easter.AddDays(-2).Month,"Good Friday"),
                new Holiday(easter.Day,easter.Month,"Easter"),
                new Holiday(21,4,"Tiradentes Holiday"),
                new Holiday(easter.AddDays(60).Day,easter.AddDays(60).Month,"Corpus Christi"),
                new Holiday(7,9, "Independence of Brazil"),
                new Holiday(12,10,"Day of Our Lady"),
                new Holiday(2,11, "All Souls' Day"),
                new Holiday(15,11, "Proclamation of the Republic"),
                new Holiday(25,12, "Chrismas"),
            };
            return holidays;
        }
        private bool isLeapYear(double year)
        {
            if((year % 400) == 0)
            {
                return true;
            }
            else
            {
                if((year % 100) == 0)
                {
                    return false;
                }
                else
                {
                    if((year % 4) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private static DateTime PickingEaster(int easterYear)
        {
            
            int lunarCyclePosition = easterYear % 19;
            int century = easterYear / 100;
            int daysBetweenEquinoxAndFullmoon = (century - (int)(century / 4) - (int)((8 * century + 13) / 25) + 19 * lunarCyclePosition + 15) % 30;
            int daysBetweenFullmoonAndFirstSunday = daysBetweenEquinoxAndFullmoon - (int)(daysBetweenEquinoxAndFullmoon / 28) * (1 - (int)(daysBetweenEquinoxAndFullmoon / 28) * (int)(29 / (daysBetweenEquinoxAndFullmoon + 1)) * (int)((21 - lunarCyclePosition) / 11));

            int easterDay = daysBetweenFullmoonAndFirstSunday - ((easterYear + (int)(easterYear / 4) + daysBetweenFullmoonAndFirstSunday + 2 - century + (int)(century / 4)) % 7) + 28;
            int easterMonth = 3;

            if (easterDay > 31)
            {
                easterMonth++;
                easterDay -= 31;
            }

            DateTime easter = new DateTime(easterYear,easterMonth,easterDay);
            return easter;

            //https://stackoverflow.com/questions/2510383/how-can-i-calculate-what-date-good-friday-falls-on-given-a-year
        }

    }

    class Holiday
    {
        public Holiday(int day, int month, string holiday)
        {
            Day = day;
            Month = month;
            HolidayName = holiday;
        }

        public int Day { get; private set; }
        public int Month { get; private set; }
        public string HolidayName { get; private set; }

    }
}
