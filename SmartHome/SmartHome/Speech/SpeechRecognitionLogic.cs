using Android.Content.Res;
using SmartHome.Activities;
using SmartHome.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Speech
{
    public class SpeechRecognitionLogic
    {
        private BasePageActivity _activity;

        public SpeechRecognitionLogic(BasePageActivity activity)
        {
            _activity = activity;
        }

        public Command Interpret(string text)
        {
            var words = text.Split(' ');

            var commandType = ResolveFirstWord(words[0]) switch
            {
                Resource.String.speech_recognition_turn_on =>
                    ResolveSecondPart(words, Resource.String.speech_recognition_light, CommandType.LightOn, 0, CommandType.Unknown),
                Resource.String.speech_recognition_turn_off =>
                    ResolveSecondPart(words, Resource.String.speech_recognition_light, CommandType.LightOff, Resource.String.speech_recognition_lights, CommandType.AllLightsOff),
                Resource.String.speech_recognition_move_up =>
                    ResolveSecondPart(words, Resource.String.speech_recognition_blind, CommandType.BlindUp, Resource.String.speech_recognition_blinds, CommandType.AllBlindsUp),
                Resource.String.speech_recognition_move_down =>
                    ResolveSecondPart(words, Resource.String.speech_recognition_blind, CommandType.BlindDown, Resource.String.speech_recognition_blinds, CommandType.AllBlindsDown),
                _ => CommandType.Unknown
            };

            if (commandType == CommandType.Unknown)
                return null;
            
            var startIndex = commandType == CommandType.AllLightsOff ||
                commandType == CommandType.AllBlindsUp ||
                commandType == CommandType.AllBlindsDown
                ? 3 : 2;
            var room = String.Join(' ', words[startIndex..^0]);

            switch (commandType)
            {
                case CommandType.AllLightsOff:
                    return CommandForAll(commandType, room);
                case CommandType.AllBlindsUp:
                    return CommandForAll(commandType, room);
                case CommandType.AllBlindsDown:
                    return CommandForAll(commandType, room);
                case CommandType.LightOn:
                    return CommandForLight(commandType, room);
                case CommandType.LightOff:
                    return CommandForLight(commandType, room);
                case CommandType.BlindUp:
                    return CommandForBlind(commandType, room);
                case CommandType.BlindDown:
                    return CommandForBlind(commandType, room);
                default:
                    throw new Exception("Unknown commandType");
            }
        }

        private int ResolveFirstWord(string word)
        {
            var firstWordOptions = new int[] {
                Resource.String.speech_recognition_turn_on,
                Resource.String.speech_recognition_turn_off,
                Resource.String.speech_recognition_move_up,
                Resource.String.speech_recognition_move_down
            };

            foreach (var resource in firstWordOptions)
            {
                if (WordIs(word, resource))
                    return resource;
            }
            
            return 0;
        }

        private CommandType ResolveSecondPart(string[] words, int secondWordResource, CommandType cmdTypeForSecondWord, int thirdWordResource, CommandType cmdTypeForAll)
        {
            if (WordIs(words[1], secondWordResource))
                return cmdTypeForSecondWord;
            else if (words[1] == _activity.GetString(Resource.String.speech_recognition_all) && WordIs(words[2], thirdWordResource))
                return cmdTypeForAll;
            else
                return CommandType.Unknown;
        }

        private bool BlindFound(Dictionary<int, Blind> blinds, string room, out string roomName)
        {
            var blind = blinds.Select(x => x.Value).FirstOrDefault(x => x.SpeechName.Contains(room));
            roomName = blind != null ? blind.Name.Substring(0, blind.Name.LastIndexOf('_')) : String.Empty;
            return blind != null;
        }

        private bool LightFound(Dictionary<int, Light> lights, string room, out string roomName)
        {
            var light = lights.Select(x => x.Value).FirstOrDefault(x => x.SpeechName.Contains(room));
            roomName = light != null ? light.Name : String.Empty;
            return light != null;
        }

        private Command CommandForLight(CommandType commandType, string room)
        {
            if (LightFound(StaticData.GroundLights, room, out var roomName))
                return new Command(commandType, roomName, _activity.GetString(Resource.String.ground_host));
            else
                return LightFound(StaticData.AtticLights, room, out roomName) ? new Command(commandType, roomName, _activity.GetString(Resource.String.attic_host)) : null;
        }

        private Command CommandForBlind(CommandType commandType, string room)
        {
            if (BlindFound(StaticData.GroundBlinds, room, out var roomName))
                return new Command(commandType, roomName, _activity.GetString(Resource.String.attic_host));
            else
                return BlindFound(StaticData.AtticBlinds, room, out roomName) ? new Command(commandType, roomName, _activity.GetString(Resource.String.ground_host)) : null;
        }

        private Command CommandForAll(CommandType commandType, string room)
        {
            if (WordIs(room, Resource.String.speech_recognition_ground))
                return new Command(commandType, _activity.GetString(Resource.String.speech_recognition_ground), _activity.GetString(Resource.String.ground_host));
            else if (WordIs(room, Resource.String.speech_recognition_attic))
                return new Command(commandType, _activity.GetString(Resource.String.speech_recognition_attic), _activity.GetString(Resource.String.attic_host));
            else
                return null;
        }

        private bool WordIs(string word, int stringResource)
        {
            return _activity.GetString(stringResource).Split(',').Contains(word.ToLowerInvariant());
        }
    }
}