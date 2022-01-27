using Android.App;
using Android.OS;
using SmartHome.Model;
using System.Collections.Generic;

namespace SmartHome.Activities
{
    [Activity(Label = "LightsGroundActivity")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "com.smarthome.MainActivity")]
    public class LightsGroundActivity : BaseLightsActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_lights_ground);

            Initialize();
        }

        protected override Dictionary<int, Light> GetSwitchList() => StaticData.GroundLights;

        protected override string GetHost() => GetString(Resource.String.ground_host);
    }
}