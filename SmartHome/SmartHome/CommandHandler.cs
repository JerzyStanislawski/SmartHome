using Android.App;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SmartHome
{
    public class CommandHandler
    {
        private readonly Activity _activity;

        public CommandHandler(Activity activity)
        {
            _activity = activity;
        }

        public async Task<HttpStatusCode> Handle(Command command)
        {
            switch (command.Type)
            {
                case CommandType.LightOn:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsOswietlenie", $"{command.Room}=true", _activity);
                case CommandType.LightOff:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsOswietlenie", $"{command.Room}=false", _activity);
                case CommandType.BlindUp:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsRolety", $"{command.Room}_up", _activity);
                case CommandType.BlindDown:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsRolety", $"{command.Room}_down", _activity);
                case CommandType.AllLightsOff:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsOswietlenie", "allOff=0", _activity);
                case CommandType.AllBlindsUp:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsRolety", "allRoletyUp", _activity);
                case CommandType.AllBlindsDown:
                    return await HttpClientWrapper.Post($"http://{command.Host}/impulsRolety", "allRoletyDown", _activity);
                case CommandType.Unknown:
                default:
                    throw new ArgumentException("Unknown command");
            }
        }
    }
}