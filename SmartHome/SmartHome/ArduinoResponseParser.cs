using System;
using System.Collections.Generic;
using SmartHome.Model;

namespace SmartHome
{
    public class ArduinoResponseParser
    {
        public IDictionary<int, bool> ParseLightsStatus(string response)
        {
            var statusMap = new Dictionary<int, bool>();
            var lines = response.Split("\n");
            foreach (var line in lines)
            {
                var lights = line.Split("=");
                statusMap[int.Parse(lights[0])] = lights[1].Trim() == "1";
            }
            return statusMap;
        }

        public Model.Settings ParseAllSettingsResponse(string response)
        {
            var lines = response.Split("\r\n");

            var timeString = lines[0].Substring("Time: ".Length);
            var dateString = lines[1].Substring("Date: ".Length);
            var dateTime = DateTime.ParseExact($"{dateString} {timeString}", "yyyy-M-d H:m:s", null);

            var holidayMode = ParseBoolValue(lines[2], "holidayMode");
            var twilightMode = ParseBoolValue(lines[3], "twilightMode");
            var morningMode = ParseBoolValue(lines[4], "morningMode");

            var morningDaysString = lines[5].Substring("morningDays: ".Length);
            var morningDaysMask = int.Parse(morningDaysString);

            var morningTimeString = lines[6].Substring("morningTime: ".Length);
            var morningTime = DateTime.ParseExact(morningTimeString, "H:m", null);

            return new Model.Settings(dateTime, holidayMode, twilightMode, morningMode, morningTime.TimeOfDay, morningDaysMask);
        }

        private bool ParseBoolValue(string line, string fieldName)
        {
            var boolValueString = line.Substring((fieldName + ": ").Length).Trim();
            return boolValueString == "1";
        }
    }
}