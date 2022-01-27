using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Interop;
using System.Net;
using System.Net.Http;

namespace SmartHome.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : BasePageActivity
    {
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

        [Export("NavigateLightsGround")]
        public void NavigateLightsGround(View _)
        {
            var intent = new Intent(this, typeof(LightsGroundActivity));
            StartActivity(intent);
        }

        [Export("NavigateLightsAttic")]
        public void NavigateLightsAttic(View _)
        {
            var intent = new Intent(this, typeof(LightsAtticActivity));
            StartActivity(intent);
        }

        [Export("NavigateBlindsGround")]
        public void NavigateBlindsGround(View _)
        {
            var intent = new Intent(this, typeof(BlindsGroundActivity));
            StartActivity(intent);
        }

        [Export("NavigateBlindsAttic")]
        public void NavigateBlindsAttic(View _)
        {
            var intent = new Intent(this, typeof(BlindsAtticActivity));
            StartActivity(intent);
        }

        [Export("NavigateBlindsGroundUp")]
        public void NavigateBlindsGroundUp(View _)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyUp")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        [Export("NavigateBlindsGroundDown")]
        public void NavigateBlindsGroundDown(View _)
        {
            var host = GetString(Resource.String.attic_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyDown")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        [Export("NavigateBlindsAtticUp")]
        public void NavigateBlindsAtticUp(View _)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyUp")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        [Export("NavigateBlindsAtticDown")]
        public void NavigateBlindsAtticDown(View _)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsRolety";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allRoletyDown")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        [Export("NavigateLightsGroundOff")]
        public void NavigateLightsGroundOff(View _)
        {
            var host = GetString(Resource.String.ground_host);
            var url = $"http://{host}/impulsOswietlenie";
            var responseCode = _httpClient.PostAsync(url, new StringContent("allOff=0")).Result.StatusCode;
            NotifyOnFailure(responseCode);
        }

        [Export("NavigateLightsAtticOff")]
        public void NavigateLightsAtticOff(View _)
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