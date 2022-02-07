using Android.Runtime;
using System;

namespace SmartHome.Model
{
    public class Light : Java.Lang.Object
    {
        public string Name { get; }
        public int Output { get; }
        public string NiceName { get; }
        public string[] SpeechName { get; }
        public bool State { get; set; }

        public Light(string name, int output, string niceName, string[] speechName)
        {
            Name = name;
            Output = output;
            NiceName = niceName;
            SpeechName = speechName;
        }

        public override string ToString() => NiceName;
    }
}