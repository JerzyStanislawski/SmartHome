using Android.App;
using Android.Content;
using Android.Speech;
using Android.Widget;
using SmartHome.Activities;
using System;
using System.Threading.Tasks;

namespace SmartHome.Speech
{
    public class SpeechIntent
    {
        private readonly BasePageActivity _activity;
        private readonly SpeechRecognitionLogic _speechRecognitionLogic;
        private readonly CommandHandler _commandHandler;
        public const int VOICE = 10;

        public SpeechIntent(BasePageActivity activity)
        {
            _activity = activity;
            _speechRecognitionLogic = new SpeechRecognitionLogic(activity);
            _commandHandler = new CommandHandler(activity);

            SetupMic();
        }

        private void SetupMic()
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                Toast.MakeText(_activity.ApplicationContext, _activity.GetString(Resource.String.no_mic), ToastLength.Short).Show();
            }
        }

        public void Start()
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, String.Empty);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.ForLanguageTag("PL"));
            _activity.StartActivityForResult(voiceIntent, VOICE);
        }

        public void HandleResult(Result resultVal, Intent data)
        {
            if (resultVal == Result.Ok)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                var text = string.Join(" ", matches);

                var command = _speechRecognitionLogic.Interpret(text);

                if (command == null)
                {
                    Toast.MakeText(_activity.ApplicationContext, _activity.GetString(Resource.String.speech_recognition_command_not_recognized), ToastLength.Short).Show();
                    Start();
                }
                else
                {
                    Task.Run(() => _commandHandler.Handle(command));
                }
            }
        }
    }
}