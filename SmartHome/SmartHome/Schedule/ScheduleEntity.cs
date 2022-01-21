using Android.Runtime;
using System;

namespace SmartHome.Schedule
{
    public class ScheduleEntity : Java.Lang.Object
    {
        public int? Id { get; set; }
        public string Room { get; set; }
        public string RoomNiceName { get; set; }
        public ScheduleType Type { get; set; }
        public TimeSpan Time { get; set; }
        public int DaysMask { get; set; }
        public bool OnOrUp { get; set; }
        public Area Area { get; set; }

        public ScheduleEntity(int? id, string room, string roomNiceName, ScheduleType type, TimeSpan time, int daysMask, bool onOrUp, Area area)
        {
            Id = id;
            Room = room;
            RoomNiceName = roomNiceName;
            Type = type;
            Time = time;
            DaysMask = daysMask;
            OnOrUp = onOrUp;
            Area = area;
        }
    }
}