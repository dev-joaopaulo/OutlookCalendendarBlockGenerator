using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace calendarBlockGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // User Inputs :
            //      DateTime initialDate, lastDate
            //      int blockHours

            // Parameters:
            //      list<int> availableWorkHours


            //-----------------------------------------------------------------------------------------------------//

            // User Inputs :
            Console.WriteLine("Insert the initial day in the format: MM/DD/YYYY");
            DateTime initialDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Insert the last day in the format: MM/DD/YYYY");
            DateTime lastDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("How many hours of your workday should be blocked? [1-8]");
            int blockHours = int.Parse(Console.ReadLine());

            // Parameters:

            List<int> availableWorkHours = new List<int>{ 8, 9, 10, 11, 13, 14, 15, 16 };

            // Program:
            List<string> daysList = CalendarDateList(WorkdaysList(initialDate, lastDate));

            int numberOfDays = daysList.Count;

            Console.WriteLine($"There are {numberOfDays} days to be blocked");

            List<List<int>> dayBlockersList = GenerateDayBlockersList(numberOfDays, blockHours, availableWorkHours);

            string calendarEvents = CreateCalendarEvents(daysList, dayBlockersList);

            File.WriteAllText(@"..\..\..\Output\Events.ics", calendarEvents);
            
            // Test Stuff Here

            Console.WriteLine(calendarEvents);

            // End:
            Console.ReadLine();
        }

        //-----------------------------------------------------------------------------------------------------//

        // User Functions:
        static List<DateTime> WorkdaysList(DateTime initialDate, DateTime lastDate)
        {
            List<DateTime> workdaysList = new List<DateTime>();

            DateTime current = initialDate;
            while(current <= lastDate)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    workdaysList.Add(current);
                    
                }
                current = current.AddDays(1);
            }

            return workdaysList;
        }

        static List<string> CalendarDateList (List<DateTime> workdaysList)
        {
            List<string> DateList = new List<string>();

            foreach (DateTime date in workdaysList)
            {
                string YYYY = date.ToString("yyyy");
                string MM = date.ToString("MM");
                string DD = date.ToString("dd");

                string day = $"{YYYY}{MM}{DD}";

                DateList.Add(day);
            }

            return DateList;
        }

        static List<int> DayBlocker (int blockHours, List<int> availableWorkHours)
        {
            List<int> blockedTimeList = new List<int>();
            int numberOfAvailableHours = availableWorkHours.Count;


            Random r = new Random();
            
            while(blockedTimeList.Count < blockHours)
            {
                int blockedHourIndex = r.Next(0, numberOfAvailableHours);

                int possibleBlockHour = availableWorkHours[blockedHourIndex];

                if (blockedTimeList.Contains(possibleBlockHour))
                {
                    continue;
                }
                else
                {
                    blockedTimeList.Add(possibleBlockHour);
                }

            }

            blockedTimeList.Sort();

            return blockedTimeList;
        }

        static List<List<int>> GenerateDayBlockersList (int numberOfDays, int blockHours, List<int> availableWorkHours)
        {
            List<List<int>> dayBlockersList = new List<List<int>>();

            List<int> dayBlock;

            for (int i = 0; i < numberOfDays; i++)
            {
                dayBlock = DayBlocker(blockHours, availableWorkHours);
                dayBlockersList.Add(dayBlock);
            }

            return dayBlockersList;
        }

        static string EventGenerator (string calendarDate, int eventTime)
        {
            string calendarEvent = "BEGIN:VEVENT\n" +
                                   $"DTSTART: {calendarDate}T{eventTime}0000\n" +
                                   $"DTEND: {calendarDate}T{eventTime + 1}0000\n" +
                                   "LOCATION:Teams\n" +
                                   "SUMMARY:Block Time\n" +
                                   "END:VEVENT\n\n";
                       
            return calendarEvent;
        }

        static string CreateCalendarEvents (List<string> daysList, List<List<int>> dayBlockersList)
        {
            string compiledEvents = "BEGIN:VCALENDAR\n\n" +
                                    "CALSCALE:GREGORIAN\n" +
                                    "METHOD:PUBLISH\n" +
                                    "X-WR-TIMEZONE:America/Sao_Paulo\n\n";

            int dayIndex = 0;

            foreach (string calendarDay in daysList)
            {
                List<int> blockedHours = dayBlockersList[dayIndex];

                foreach(int hour in blockedHours)
                {
                    compiledEvents += EventGenerator(calendarDay, hour);
                }
                dayIndex++;
            }

            compiledEvents += "END:VCALENDAR";

            return compiledEvents;
        }

    }



}
