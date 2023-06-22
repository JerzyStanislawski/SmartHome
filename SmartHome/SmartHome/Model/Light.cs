using Android.Runtime;
using System;

namespace SmartHome.Model
{
    public class Light : Java.Lang.Object
    {
        public string Name { get; }
        public int Id { get; }
        public string NiceName { get; }
        public string[] SpeechName { get; }
        public bool State { get; set; }

        public Light(string name, int id, string niceName, string[] speechName)
        {
            Name = name;
            Id = id;
            NiceName = niceName;
            SpeechName = speechName;
        }

        public override string ToString() => NiceName;
    }
}