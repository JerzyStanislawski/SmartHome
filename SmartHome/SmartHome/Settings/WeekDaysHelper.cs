using System;
using System.Collections.Generic;

namespace SmartHome.Settings
{
    public static class WeekDaysHelper
    {
        public static int GetDaysMask(IEnumerable<DayOfWeek> days)
        {
            int mask = 1;

            foreach (var weekDay in days)
            {
                mask |= (1 << (((byte)weekDay % 7) + 1));
            }

            return mask;
        }

        public static IEnumerable<DayOfWeek> WeekDaysFromMask(int weekDaysMask)
        {
            var weekDays = new List<DayOfWeek>();
            var initialDay = DayOfWeek.Sunday;
            for (var i = 1; i < 8; i++)
            {
                if (((weekDaysMask >> i) & 1) == 1)
                {
                    weekDays.Add(initialDay + i - 1);
                }
            }

            return weekDays;
        }
    }
}