namespace SmartHome.Model
{
    public class Light
    {
        public string Name { get; set; }
        public int Output { get; set; }
        public string NiceName { get; set; }
        public bool State { get; set; }

        public Light(string name, int output, string niceName, bool state)
        {
            Name = name;
            Output = output;
            NiceName = niceName;
            State = state;
        }
    }
}