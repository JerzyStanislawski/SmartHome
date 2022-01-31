using Android.Runtime;
using System;

namespace SmartHome.Model
{
    public class Blind : Java.Lang.Object, IJavaObject
    {
        public string Name { get; set; }
        public string NiceName { get; set; }

        public Blind(string name, string niceName)
        {
            Name = name;
            NiceName = niceName;
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