using Android.Views;
using Android.Widget;
using System;

namespace SmartHome.Settings
{
    internal class TimePopupWindow : WeekDaysView
    {
        private readonly View _popupView;

        public TimePopupWindow(View popupView) 
            : base(popupView.FindViewById(Resource.Id.popup_week_days))
        {
            _popupView = popupView;
        }

        public TimeSpan GetTime()
        {
            var timePicker = (TimePicker) _popupView.FindViewById(Resource.Id.popupTimePicker);
            return new TimeSpan(timePicker.Hour, timePicker.Minute, 0);
        }

        public void Init()
        {
            var timePicker = (TimePicker)_popupView.FindViewById(Resource.Id.popupTimePicker);
            timePicker.SetIs24HourView((Java.Lang.Boolean)true);
            timePicker.Hour = 7;
            timePicker.Minute = 0;
        }
    }
}