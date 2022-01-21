using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace SmartHome.Settings
{
    public class WeekDaysView
    {
        protected readonly View _view;

        public WeekDaysView(View view)
        {
            _view = view;
        }

        public IEnumerable<DayOfWeek> GetSelectedDays()
        {
            var days = new List<DayOfWeek>();

            CheckToggleDayButton(Resource.Id.btn_monday, days, DayOfWeek.Monday);
            CheckToggleDayButton(Resource.Id.btn_tuesday, days, DayOfWeek.Tuesday);
            CheckToggleDayButton(Resource.Id.btn_wednesday, days, DayOfWeek.Wednesday);
            CheckToggleDayButton(Resource.Id.btn_thursday, days, DayOfWeek.Thursday);
            CheckToggleDayButton(Resource.Id.btn_friday, days, DayOfWeek.Friday);
            CheckToggleDayButton(Resource.Id.btn_saturday, days, DayOfWeek.Saturday);
            CheckToggleDayButton(Resource.Id.btn_sunday, days, DayOfWeek.Sunday);

            return days;
        }

        private void CheckToggleDayButton(int buttonId, List<DayOfWeek> days, DayOfWeek dayOfWeek)
        {
            ToggleButton toggleButton = (ToggleButton)_view.FindViewById(buttonId);
            if (toggleButton.Checked)
            {
                days.Add(dayOfWeek);
            }
        }

        public void SetDays(List<DayOfWeek> days)
        {
            var layout = (LinearLayout)_view;
            var childCount = layout.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                View v = layout.GetChildAt(i);
                if (v is ToggleButton button)
                {
                    switch (v.Id)
                    {
                        case Resource.Id.btn_monday:
                            button.Checked = days.Contains(DayOfWeek.Monday);
                            break;
                        case Resource.Id.btn_tuesday:
                            button.Checked = days.Contains(DayOfWeek.Tuesday);
                            break;
                        case Resource.Id.btn_wednesday:
                            button.Checked = days.Contains(DayOfWeek.Wednesday);
                            break;
                        case Resource.Id.btn_thursday:
                            button.Checked = days.Contains(DayOfWeek.Thursday);
                            break;
                        case Resource.Id.btn_friday:
                            button.Checked = days.Contains(DayOfWeek.Friday);
                            break;
                        case Resource.Id.btn_saturday:
                            button.Checked = days.Contains(DayOfWeek.Saturday);
                            break;
                        case Resource.Id.btn_sunday:
                            button.Checked = days.Contains(DayOfWeek.Sunday);
                            break;
                        default:
                            break;
                    }

                }
            }
        }
    }
}