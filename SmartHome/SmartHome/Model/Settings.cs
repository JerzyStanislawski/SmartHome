using System;

namespace SmartHome.Model
{
    public class Settings
    {
        public DateTime DateTime { get; set; }
        public bool HolidayMode { get; set; }
        public bool TwilightMode { get; set; }
        public bool MorningMode { get; set; }
        public TimeSpan MorningTime { get; set; }
        public int MorningDays { get; set; }

        public Settings(DateTime dateTime, bool holidayMode, bool twilightMode, bool morningMode, TimeSpan morningTime, int morningDays)
        {
            DateTime = dateTime;
            HolidayMode = holidayMode;
            TwilightMode = twilightMode;
            MorningMode = morningMode;
            MorningTime = morningTime;
            MorningDays = morningDays;
        }
    }
}