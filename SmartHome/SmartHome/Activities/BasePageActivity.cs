using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using SmartHome.Schedule;
using SmartHome.Settings;
using SmartHome.Speech;
using System.Net.Http;

namespace SmartHome.Activities
{
    public class BasePageActivity : AppCompatActivity
    {
        protected HttpClient _httpClient;
        private SpeechIntent _speechIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _httpClient = HttpClientWrapper.GetClient();            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:  
                    NavUtils.NavigateUpFromSameTask(this);
                    return true;
                case Resource.Id.action_speak:
                    _speechIntent = new SpeechIntent(this);
                    _speechIntent.Start();
                    return true;
                case Resource.Id.action_schedule:
                    var scheduleIntent = new Intent(this, typeof(ScheduleActivity));
                    StartActivity(scheduleIntent);
                    return true;
                case Resource.Id.action_settings:
                    var intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == SpeechIntent.VOICE)
            {
                _speechIntent.HandleResult(resultVal, data);
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }
    }
}