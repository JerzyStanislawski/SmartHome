using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartHome.Activities;
using SmartHome.Model;
using SmartHome.Settings;
using System;
using System.Linq;

namespace SmartHome.Schedule
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ParentActivity = typeof(ScheduleActivity))]
    public class ScheduleEntityActivity : BasePageActivity
    {
        ArrayAdapter<object> _spinnerAdapter;
        object[] _spinnerArray;
        ScheduleEntity _originalEntity;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_schedule_entity);

            var timePicker = (TimePicker)FindViewById(Resource.Id.timePicker);
            timePicker.SetIs24HourView(new Java.Lang.Boolean(true));

            var toggleListener = new ToggleListener();
            ((RadioGroup)FindViewById(Resource.Id.toggleGroupArea)).SetOnCheckedChangeListener(toggleListener);
            ((RadioGroup)FindViewById(Resource.Id.toggleGroupType)).SetOnCheckedChangeListener(toggleListener);
            ((RadioGroup)FindViewById(Resource.Id.toggleGroupBlinds)).SetOnCheckedChangeListener(toggleListener);

            if (Intent?.Extras != null && Intent?.Extras.Size() > 0)
                LoadViewFromIntent(Intent);
            else
                SetSpinnerData();
        }

        private void LoadViewFromIntent(Intent data)
        {
            _originalEntity = ScheduleEntityHelpers.CreateFromIntent(data);

            if (_originalEntity.Type == ScheduleType.LIGHTS)
            {
                OnToggleType(FindViewById(Resource.Id.btn_lights));
            }
            else
            {
                OnToggleType(FindViewById(Resource.Id.btn_blinds));
            }

            if (_originalEntity.Area == Area.GROUNDFLOOR)
            {
                OnToggleArea(FindViewById(Resource.Id.btn_ground_floor));
            }
            else
            {
                OnToggleArea(FindViewById(Resource.Id.btn_attic));
            }

            int position;
            if (_originalEntity.Type == ScheduleType.LIGHTS)
            {
                var lightToSelect = (Light)_spinnerArray.FirstOrDefault(x => x is Light light && light.Name == _originalEntity.Room);
                position = _spinnerAdapter.GetPosition(lightToSelect);
            }
            else
            {
                var blindToSelect = (Blind)_spinnerArray.FirstOrDefault(x => x is Blind blind 
                    && blind.Name == StripBlindName(_originalEntity.Room));
                position = _spinnerAdapter.GetPosition(blindToSelect);                
            }

            var roomSpinner = (Spinner)FindViewById(Resource.Id.room_spinner);
            roomSpinner.SetSelection(position);

            var timePicker = (TimePicker)FindViewById(Resource.Id.timePicker);
            timePicker.Hour = _originalEntity.Time.Hours;
            timePicker.Minute = _originalEntity.Time.Minutes;

            if (_originalEntity.Type == ScheduleType.LIGHTS)
            {
                ((Switch)FindViewById(Resource.Id.lights_switch)).Checked = _originalEntity.OnOrUp;
            }
            else
            {
                ((ToggleButton)FindViewById(Resource.Id.blinds_up)).Checked = _originalEntity.OnOrUp;
                ((ToggleButton)FindViewById(Resource.Id.blinds_down)).Checked = !_originalEntity.OnOrUp;
            }

            var weekDaysView = new WeekDaysView(FindViewById(Resource.Id.schedule_week_days));
            weekDaysView.SetDays(WeekDaysHelper.WeekDaysFromMask(_originalEntity.DaysMask).ToList());
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu_schedule_entity, menu);

            if (Intent?.Extras == null || Intent.Extras.Size() == 0)
            {
                var item = menu.FindItem(Resource.Id.delete_item);
                item.SetVisible(false);
            }

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.submit_item:
                    Intent intent = CreateIntent();
                    SetResult(Result.Ok, intent);
                    Finish();
                    return true;
                case Resource.Id.delete_item:
                    Intent removeIntent = new Intent();
                    removeIntent.PutExtra("id", _originalEntity.Id.Value);
                    SetResult((Result)ScheduleActivity.RESULT_CODE_REMOVE, removeIntent);
                    Finish();
                    return true;
                case Android.Resource.Id.Home:
                    SetResult(Result.Canceled);
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private string StripBlindName(string blindName) => blindName.Substring(0, blindName.LastIndexOf('_'));

        private Intent CreateIntent()
        {
            var lightsButton = (ToggleButton)FindViewById(Resource.Id.btn_lights);
            ScheduleType type = lightsButton.Checked ? ScheduleType.LIGHTS : ScheduleType.BLINDS;

            var groundFloorButton = (ToggleButton)FindViewById(Resource.Id.btn_ground_floor);
            Area area = groundFloorButton.Checked ? Area.GROUNDFLOOR : Area.ATTIC;

            var timePicker = (TimePicker)FindViewById(Resource.Id.timePicker);
            int hour = timePicker.Hour;
            int minute = timePicker.Minute;

            var onOrUp = type == ScheduleType.LIGHTS
                    ? ((Switch)FindViewById(Resource.Id.lights_switch)).Checked
                    : ((ToggleButton)FindViewById(Resource.Id.blinds_up)).Checked;

            var room = "";
            var roomNiceName = "";
            var roomSpinner = (Spinner)FindViewById(Resource.Id.room_spinner);

            if (type == ScheduleType.LIGHTS)
            {
                var light = roomSpinner.SelectedItem.JavaCast<Light>();
                room = light.Name;
                roomNiceName = light.NiceName;
            }
            else if (type == ScheduleType.BLINDS)
            {
                var blind = roomSpinner.SelectedItem.JavaCast<Blind>();
                room = blind.Name;
                room += onOrUp ? "_up" : "_down";
                roomNiceName = blind.NiceName;
            }

            var weekDaysView = new WeekDaysView(FindViewById(Resource.Id.schedule_week_days));
            int daysMask = WeekDaysHelper.GetDaysMask(weekDaysView.GetSelectedDays());

            var entity = new ScheduleEntity(_originalEntity != null ? _originalEntity.Id : null, room, roomNiceName, type,
                new TimeSpan(hour, minute, 0), daysMask, onOrUp, area);
            var intent = new Intent();
            ScheduleEntityHelpers.FillIntent(entity, intent);

            return intent;
        }

        private void SetSpinnerData()
        {
            var lightsButton = (ToggleButton)FindViewById(Resource.Id.btn_lights);
            ScheduleType type = lightsButton.Checked ? ScheduleType.LIGHTS : ScheduleType.BLINDS;

            var groundFloorButton = (ToggleButton)FindViewById(Resource.Id.btn_ground_floor);
            Area area = groundFloorButton.Checked ? Area.GROUNDFLOOR : Area.ATTIC;

            if (type == ScheduleType.LIGHTS && area == Area.GROUNDFLOOR)
            {
                _spinnerArray = StaticData.GroundLights.Values.OrderBy(x => x.NiceName).ToArray();
            }
            else if (type == ScheduleType.LIGHTS && area == Area.ATTIC)
            {
                _spinnerArray = StaticData.AtticLights.Values.OrderBy(x => x.NiceName).ToArray();
            }
            else if (type == ScheduleType.BLINDS && area == Area.GROUNDFLOOR)
            {
                _spinnerArray = StaticData.GroundBlinds.Values.OrderBy(x => x.NiceName)
                    .Select(x => new Blind(StripBlindName(x.Name), x.NiceName, x.SpeechName))
                    .GroupBy(x => x.NiceName).Select(x => x.First()).ToArray();
            }
            else if (type == ScheduleType.BLINDS && area == Area.ATTIC)
            {
                _spinnerArray = StaticData.AtticBlinds.Values.OrderBy(x => x.NiceName)
                    .Select(x => new Blind(StripBlindName(x.Name), x.NiceName, x.SpeechName))
                    .GroupBy(x => x.NiceName).Select(x => x.First()).ToArray();
            }

            var spinner = (Spinner)FindViewById(Resource.Id.room_spinner);
            _spinnerAdapter = new ArrayAdapter<object>(this, Android.Resource.Layout.SimpleSpinnerItem, _spinnerArray);
            _spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = _spinnerAdapter;
        }

        private void HandleToggling(View view)
        {
            ((RadioGroup)view.Parent).ClearCheck();
            ((RadioGroup)view.Parent).Check(view.Id);
            ((ToggleButton)view).Checked = true;
        }

        [Java.Interop.Export("OnToggleType")]
        public void OnToggleType(View view)
        {
            HandleToggling(view);

            var lightsOptions = (LinearLayout)FindViewById(Resource.Id.ligts_options);
            var blindsOptions = (LinearLayout)FindViewById(Resource.Id.blinds_options);
            if (view.Id == Resource.Id.btn_lights)
            {
                lightsOptions.Visibility = ViewStates.Visible;
                blindsOptions.Visibility = ViewStates.Gone;
            }
            else if (view.Id == Resource.Id.btn_blinds)
            {
                lightsOptions.Visibility = ViewStates.Gone;
                blindsOptions.Visibility = ViewStates.Visible;
            }

            SetSpinnerData();
        }

        [Java.Interop.Export("OnToggleArea")]
        public void OnToggleArea(View view)
        {
            HandleToggling(view);
            SetSpinnerData();
        }

        [Java.Interop.Export("OnToggleBlinds")]
        public void OnToggleBlinds(View view)
        {
            HandleToggling(view);
        }

        private class ToggleListener : Java.Lang.Object, RadioGroup.IOnCheckedChangeListener
        {
            public void OnCheckedChanged(RadioGroup group, int checkedId)
            {
                for (int i = 0; i < group.ChildCount; i++)
                {
                    var view = (ToggleButton)group.GetChildAt(i);
                    view.Checked = view.Id == i;
                }
            }
        }
    }
}