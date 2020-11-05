using System;
using System.Collections.Generic;

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

            // Test Stuff Here
            List<int> DayBlock = DayBlocker(blockHours, availableWorkHours);
            foreach (int testElement in DayBlock)
            {
                Console.WriteLine(testElement);
            }

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

    }



}
