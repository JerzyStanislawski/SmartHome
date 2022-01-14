﻿namespace SmartHome.Model
{
    public class Blind
    {
        public string Name { get; set; }
        public string NiceName { get; set; }

        public Blind(string name, string niceName)
        {
            Name = name;
            NiceName = niceName;
        }
    }
}