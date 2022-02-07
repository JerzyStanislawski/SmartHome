using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartHome.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHome.Speech
{
    public class SpeechRecognitionLogic
    {
        private readonly Activity _activity;

        public SpeechRecognitionLogic(Activity activity)
        {
            _activity = activity;
        }

        public Command Interpret(string text)
        {
            var words = text.Split(' ');

            CommandType commandType = CommandType.LightOn;

            if (WordIs(words[0], Resource.String.speech_recognition_turn_on))
            {
                if (WordIs(words[1], Resource.String.speech_recognition_light))
                    commandType = CommandType.LightOn;
                else
                    return null;
            }
            else if (WordIs(words[0], Resource.String.speech_recognition_turn_off))
            {
                if (WordIs(words[1], Resource.String.speech_recognition_light))
                    commandType = CommandType.LightOff;
                else if (words[1] == Command.All && WordIs(words[2], Resource.String.speech_recognition_lights))
                    commandType = CommandType.AllLightsOff;
                else
                    return null;
            }
            if (WordIs(words[0], Resource.String.speech_recognition_move_up))
            {
                if (WordIs(words[1], Resource.String.speech_recognition_blind))
                    commandType = CommandType.BlindUp;
                else if (words[1] == Command.All && WordIs(words[2], Resource.String.speech_recognition_blinds))
                    commandType = CommandType.AllBlindsUp;
                else
                    return null;
            }
            else if (WordIs(words[0], Resource.String.speech_recognition_move_down))
            {
                if (WordIs(words[1], Resource.String.speech_recognition_blind))
                    commandType = CommandType.BlindDown;
                else if (words[1] == Command.All && WordIs(words[2], Resource.String.speech_recognition_blinds))
                    commandType = CommandType.AllBlindsDown;
                else
                    return null;
            }
            else
                return null;

            var startIndex = commandType == CommandType.AllLightsOff ||
                commandType == CommandType.AllBlindsUp ||
                commandType == CommandType.AllBlindsDown
                ? 3 : 2;
            var room = String.Join(' ', words[startIndex..(words.Length - 1)]);

            switch (commandType)
            {
                case CommandType.AllLightsOff:
                    return CommandForAll(commandType, room);
                case CommandType.AllBlindsUp:
                    return CommandForAll(commandType, room);
                case CommandType.AllBlindsDown:
                    return CommandForAll(commandType, room);
                case CommandType.LightOn:
                    if (LightFound(StaticData.GroundLights, room, out var roomName))
                        return CommandForAll(commandType, roomName);
                    else
                        return LightFound(StaticData.AtticLights, room, out roomName) ? new Command(commandType, roomName) : null;
                case CommandType.LightOff:
                    if (LightFound(StaticData.GroundLights, room, out roomName))
                        return CommandForAll(commandType, roomName);
                    else
                        return LightFound(StaticData.AtticLights, room, out roomName) ? new Command(commandType, roomName) : null;
                case CommandType.BlindUp:
                    if (BlindFound(StaticData.GroundBlinds, room, out roomName))
                        return CommandForAll(commandType, roomName);
                    else
                        return BlindFound(StaticData.AtticBlinds, room, out roomName) ? new Command(commandType, roomName) : null;
                case CommandType.BlindDown:
                    if (BlindFound(StaticData.GroundBlinds, room, out roomName))
                        return CommandForAll(commandType, roomName);
                    else
                        return BlindFound(StaticData.AtticBlinds, room, out roomName) ? new Command(commandType, roomName) : null;
            }
        }

        private bool BlindFound(Dictionary<int, Blind> blinds, string room, out string roomName)
        {
            var blind = blinds.Select(x => x.Value).FirstOrDefault(x => x.SpeechName.Contains(room));
            roomName = blind != null ? blind.Name : String.Empty;
            return blind != null;
        }

        private bool LightFound(Dictionary<int, Light> lights, string room, out string roomName)
        {
            var light = lights.Select(x => x.Value).FirstOrDefault(x => x.SpeechName.Contains(room));
            roomName = light != null ? light.Name : String.Empty;
            return light != null;
        }

        private Command CommandForAll(CommandType commandType, string room)
        {
            if (WordIs(room, Resource.String.speech_recognition_ground))
                return new Command(commandType, _activity.GetString(Resource.String.speech_recognition_ground));
            else if (WordIs(room, Resource.String.speech_recognition_attic))
                return new Command(commandType, _activity.GetString(Resource.String.speech_recognition_attic));
            else
                return null;
        }

        private bool WordIs(string word, int stringResource)
        {
            return _activity.GetString(stringResource).Split(',').Contains(word);
        }
    }
}