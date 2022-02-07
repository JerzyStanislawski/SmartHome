using Android.App;
using Android.Content;
using Android.Speech;
using Android.Widget;
using SmartHome.Activities;
using System;

namespace SmartHome.Speech
{
    public class SpeechIntent
    {
        private readonly BasePageActivity _activity;
        private readonly SpeechRecognitionLogic _speechRecognitionLogic;
        public const int VOICE = 10;

        public SpeechIntent(BasePageActivity activity)
        {
            _activity = activity;
            _speechRecognitionLogic = new SpeechRecognitionLogic();

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
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 500);
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

                _speechRecognitionLogic.Interpret(text);
                Toast.MakeText(_activity.ApplicationContext, text, ToastLength.Short).Show();
            }
        }
    }
}