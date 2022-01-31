using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
            PostRequest(Resource.String.ground_host, "impulsRolety", "allRoletyUp");
        }

        [Export("NavigateBlindsGroundDown")]
        public void NavigateBlindsGroundDown(View _)
        {
            PostRequest(Resource.String.ground_host, "impulsRolety", "allRoletyDown");
        }

        [Export("NavigateBlindsAtticUp")]
        public void NavigateBlindsAtticUp(View _)
        {
            PostRequest(Resource.String.attic_host, "impulsRolety", "allRoletyUp");
        }

        [Export("NavigateBlindsAtticDown")]
        public void NavigateBlindsAtticDown(View _)
        {
            PostRequest(Resource.String.attic_host, "impulsRolety", "allRoletyDown");
        }

        [Export("NavigateLightsGroundOff")]
        public void NavigateLightsGroundOff(View _)
        {
            PostRequest(Resource.String.ground_host, "impulsOswietlenie", "allOff=0");
        }

        [Export("NavigateLightsAtticOff")]
        public void NavigateLightsAtticOff(View _)
        {
            PostRequest(Resource.String.attic_host, "impulsOswietlenie", "allOff=0");
        }

        private void PostRequest(int host, string relativeUrl, string payload)
        {
            var hostAddress = GetString(host);
            var url = $"http://{hostAddress}/{relativeUrl}";
            Task.Run(async () =>
            {
                HttpStatusCode responseCode;
                try
                {
                    responseCode = (await _httpClient.PostAsync(url, new StringContent("allOff=0"))).StatusCode;
                }
                catch
                {
                    responseCode = HttpStatusCode.InternalServerError;
                }
                NotifyOnFailure(responseCode);
            });
        }

        private void NotifyOnFailure(HttpStatusCode responseCode)
        {
            RunOnUiThread(() =>
            {
                if (responseCode != HttpStatusCode.OK)
                {
                    Toast.MakeText(ApplicationContext, $"Odpowiedź z rodzielni: {responseCode}", ToastLength.Short).Show();
                }
            });
        }
    }
}