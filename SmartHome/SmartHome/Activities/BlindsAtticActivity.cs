using Android.App;
using Android.OS;
using SmartHome.Model;
using System.Collections.Generic;

namespace SmartHome.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ParentActivity = typeof(MainActivity))]
    public class BlindsAtticActivity : BaseBlindsActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_blinds_attic);

            Initialize();
        }

        protected override Dictionary<int, Blind> GetButtonsList() => StaticData.AtticBlinds;

        protected override string GetHost() => GetString(Resource.String.ground_host);
    }
}