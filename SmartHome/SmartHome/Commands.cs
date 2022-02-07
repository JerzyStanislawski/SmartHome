namespace SmartHome
{
    public enum CommandType
    {
        LightOn,
        LightOff,
        BlindUp,
        BlindDown,
        AllLightsOff,
        AllBlindsUp,
        AllBlindsDown
    }

    public class Command
    {
        public CommandType Type { get; }
        public string Room { get; }

        public const string All = "wszystkie";

        public Command(CommandType type, string room)
        {
            Type = type;
            Room = room;
        }
    }
}