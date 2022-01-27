using Android.Runtime;
using System;

namespace SmartHome.Model
{
    public class Light : Java.Lang.Object
    {
        public string Name { get; set; }
        public int Output { get; set; }
        public string NiceName { get; set; }
        public bool State { get; set; }

        public Light(string name, int output, string niceName)
        {
            Name = name;
            Output = output;
            NiceName = niceName;
        }

        public override string ToString() => NiceName;
    }
}