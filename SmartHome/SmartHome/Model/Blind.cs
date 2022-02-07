using Android.Runtime;
using System;

namespace SmartHome.Model
{
    public class Blind : Java.Lang.Object, IJavaObject
    {
        public string Name { get; private set; }
        public string NiceName { get; private set; }
        public string[] SpeechName { get; private set; }

        public Blind(string name, string niceName, string[] speechName)
        {
            Name = name;
            NiceName = niceName;
            SpeechName = speechName;
        }

        public override string ToString() => NiceName;

        public override bool Equals(object obj)
        {
            if (!(obj is Blind))
                return false;

            var blind = (Blind)obj;
            return Name == blind.Name && NiceName == blind.NiceName;
        }
    }
}