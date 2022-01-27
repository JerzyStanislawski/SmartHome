﻿using Android.App;
using Android.OS;
using SmartHome.Model;
using System.Collections.Generic;

namespace SmartHome.Activities
{
    [Activity(Label = "BlindsGroundActivity")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "com.smarthome.MainActivity")]
    public class BlindsGroundActivity : BaseBlindsActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_blinds_ground);

            Initialize();
        }

        protected override Dictionary<int, Blind> GetButtonsList() => StaticData.GroundBlinds;

        protected override string GetHost() => GetString(Resource.String.attic_host);
    }
}