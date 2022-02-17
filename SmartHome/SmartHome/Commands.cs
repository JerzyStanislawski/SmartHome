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
        AllBlindsDown,
        Unknown
    }

    public class Command
    {
        public CommandType Type { get; }
        public string Room { get; }
        public string Host { get; }

        public Command(CommandType type, string room, string host)
        {
            Type = type;
            Room = room;
            Host = host;
        }
    }
}