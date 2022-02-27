using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Java.Interop;

namespace SmartHome.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@mipmap/ic_house")]
    public class MainActivity : BasePageActivity
    {
        private CommandHandler _commandHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _commandHandler = new CommandHandler(this);
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
        public async void NavigateBlindsGroundUp(View _)
        {
            await _commandHandler.Handle(new Command(CommandType.AllBlindsUp, string.Empty, GetString(Resource.String.attic_host)));
        }

        [Export("NavigateBlindsGroundDown")]
        public async void NavigateBlindsGroundDown(View _)
        {
            await _commandHandler.Handle(new Command(CommandType.AllBlindsDown, string.Empty, GetString(Resource.String.attic_host)));
        }

        [Export("NavigateBlindsAtticUp")]
        public async void NavigateBlindsAtticUp(View _)
        {
            await _commandHandler.Handle(new Command(CommandType.AllBlindsUp, string.Empty, GetString(Resource.String.ground_host)));
        }

        [Export("NavigateBlindsAtticDown")]
        public async void NavigateBlindsAtticDown(View _)
        {
            await _commandHandler.Handle(new Command(CommandType.AllBlindsUp, string.Empty, GetString(Resource.String.ground_host)));
        }

        [Export("NavigateLightsGroundOff")]
        public async void NavigateLightsGroundOff(View _)
        {
            await _commandHandler.Handle(new Command(CommandType.AllLightsOff, string.Empty, GetString(Resource.String.ground_host)));
        }

        [Export("NavigateLightsAtticOff")]
        public async void NavigateLightsAtticOff(View _)
        {
            await _commandHandler.Handle(new Command(CommandType.AllLightsOff, string.Empty, GetString(Resource.String.attic_host)));
        }
    }
}