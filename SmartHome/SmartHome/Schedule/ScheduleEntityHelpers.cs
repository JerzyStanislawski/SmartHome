using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHome.Schedule
{
    public class ScheduleEntityHelpers
    {
        public static ScheduleEntity CreateFromIntent(Intent data)
        {

            ScheduleType scheduleType = (ScheduleType)data.GetIntExtra("scheduleType", 0);
            var room = data.GetStringExtra("room");
            var roomNiceName = data.GetStringExtra("roomNiceName");
            var hour = data.GetIntExtra("hour", 0);
            var minute = data.GetIntExtra("minute", 0);
            var daysMask = data.GetIntExtra("daysMask", 255);
            var onOrUp = data.GetBooleanExtra("onOrUp", false);
            var area = (Area)data.GetIntExtra("area", 0);
            var id = data.GetIntExtra("id", 0);

            return new ScheduleEntity(id, room, roomNiceName, scheduleType, new TimeSpan(hour, minute, 0), daysMask, onOrUp, area);
        }

        public static void FillIntent(ScheduleEntity entity, Intent intent)
        {
            intent.PutExtra("scheduleType", (int)entity.Type);
            intent.PutExtra("room", entity.Room);
            intent.PutExtra("roomNiceName", entity.RoomNiceName);
            intent.PutExtra("hour", entity.Time.Hours);
            intent.PutExtra("minute", entity.Time.Minutes);
            intent.PutExtra("daysMask", entity.DaysMask);
            intent.PutExtra("onOrUp", entity.OnOrUp);
            intent.PutExtra("area", (int)entity.Area);
            if (entity.Id != null)
                intent.PutExtra("id", entity.Id.Value);
        }

        public static ScheduleEntity FromHttpResponseLine(int id, string line, Area lightArea, Area blindArea)
        {
            var parts = line.Split(",");

            ScheduleType type = parts[1] == "0" ? ScheduleType.LIGHTS : ScheduleType.BLINDS;
            var niceName = type == ScheduleType.LIGHTS 
                ? StaticData.LightNames[parts[0]] 
                : StaticData.BlindNames[parts[0]];
            var time = TimeSpan.ParseExact(parts[2], "h\\:m", null);
            var daysMask = int.Parse(parts[3]);
            var onOrUp = parts[4] == "1";
            var area = type == ScheduleType.LIGHTS ? lightArea : blindArea;

            return new ScheduleEntity(id, parts[0], niceName, type, time, daysMask, onOrUp, area);
        }

        public static string BuildHttpRequestLine(ScheduleEntity entity)
        {
            return String.Format("%s,%c,%02d:%02d,%03d,%d;",
                        entity.Room,
                        entity.Type == ScheduleType.LIGHTS ? 'L' : 'B',
                        entity.Time.Hours,
                        entity.Time.Minutes,
                        entity.DaysMask,
                        entity.OnOrUp ? 1 : 0);
        }
    }
}