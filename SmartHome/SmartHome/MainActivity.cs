using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Net;
using System.Net.Http;

namespace SmartHome
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        HttpClient _httpClient = HttpClientFactory.Get();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void NavigateLightsGround(View _)
        {
            //var intent = new Intent(this, typeof(LightsBasementActivity));
            //StartActivity(intent);
        }

        public void NavigateLightsAttic(View _)
        {
            //var intent = new Intent(this, typeof(LightsAtticActivity));
            //StartActivity(intent);
        }

        public void NavigateBlindsGround(View _)
        {
            //var intent = new Intent(this, typeof(BlindsBasementActivity));
            //StartActivity(intent);
        }

        public void NavigateBlindsAttic(View _)
        {
            //var intent = new Intent(this, typeof(BlindsAtticActivity));
            //StartActivity(intent);
        }

        public void NavigateBlindsGroundUp(View view)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyUp")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        public void NavigateBlindsGroundDown(View view)
        {
            var host = GetString(Resource.String.attic_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyDown")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        public void NavigateBlindsAtticUp(View view)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyUp")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        public void NavigateBlindsAtticDown(View view)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyDown")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        public void NavigateLightsGroundOff(View view)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsOswietlenie";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allOff=0")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        public void NavigateLightsAtticOff(View view)
        {
            var host = GetString(Resource.String.attic_host);
            var url = $"http://{host}/impulsOswietlenie";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allOff=0")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        private void NotifyOnFailure(HttpStatusCode responseCode)
        {
            if (responseCode != HttpStatusCode.OK)
            {
                Toast.MakeText(ApplicationContext, $"Odpowiedź z rodzielni: {responseCode}", ToastLength.Short).Show();
            }
        }
    }
}