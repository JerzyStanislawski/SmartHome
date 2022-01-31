using Android.App;
using Android.OS;
using SmartHome.Model;
using System.Collections.Generic;

namespace SmartHome.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ParentActivity = typeof(MainActivity))]
    public class LightsAtticActivity : BaseLightsActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_lights_attic);

            Initialize();
        }

        protected override Dictionary<int, Light> GetSwitchList() => StaticData.AtticLights;

        protected override string GetHost() => GetString(Resource.String.attic_host);
    }
}