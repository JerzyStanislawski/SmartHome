using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using SmartHome.Schedule;
using SmartHome.Settings;
using System.Net.Http;

namespace SmartHome.Activities
{
    [Activity(Label = "BasePageActivity")]
    public class BasePageActivity : AppCompatActivity
    {
        protected HttpClient _httpClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _httpClient = HttpClientFactory.Get();            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.home:
                    NavUtils.NavigateUpFromSameTask(this);
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
    }
}