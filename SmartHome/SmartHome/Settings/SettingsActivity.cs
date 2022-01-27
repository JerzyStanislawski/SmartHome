using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SmartHome.Activities;
using Android.App;

namespace SmartHome.Settings
{
    [Activity(Label = "SettingsActivity")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "com.smarthome.MainActivity")]
    public class SettingsActivity : BasePageActivity
    {
        private readonly ArduinoResponseParser _responseParser = new ArduinoResponseParser();

        private readonly Dictionary<int, UIControls> _controls = new Dictionary<int, UIControls>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);

            _controls[Resource.String.ground_host] = new UIControls(Resource.Id.arduino1_ping, Resource.Id.arduino1_time, Resource.Id.arduino1_data, Resource.Id.arduino1_holiday_mode, Resource.Id.arduino1_twilight_mode, Resource.Id.arduino1_morning_mode, Resource.Id.arduino1_morning_time);
            _controls[Resource.String.attic_host] = new UIControls(Resource.Id.arduino2_ping, Resource.Id.arduino2_time, Resource.Id.arduino2_data, Resource.Id.arduino2_holiday_mode, Resource.Id.arduino2_twilight_mode, Resource.Id.arduino2_morning_mode, Resource.Id.arduino2_morning_time);

            var textArduino1 = (TextView)FindViewById(Resource.Id.arduino1_caption);
            textArduino1.Text = $"Arduino 1 - {Resource.String.ground_host}";

            var textArduino2 = (TextView)FindViewById(Resource.Id.arduino2_caption);
            textArduino2.SetText("Arduino 2 - " + Resource.String.attic_host, TextView.BufferType.Normal);

            RetrieveData(Resource.String.ground_host);
            RetrieveData(Resource.String.attic_host);
        }

        void DisplayError(string response, int host)
        {
            var controls = _controls[host];

            var layout = (LinearLayout)FindViewById(controls.IdLayout);
            layout.Visibility = ViewStates.Invisible;

            var pingText = (TextView)FindViewById(controls.IdPing);
            pingText.Text = response;
        }

        private void UpdateData(Model.Settings settings, long duration, int host)
        {
            var controls = _controls[host];

            var layout = (LinearLayout)FindViewById(controls.IdLayout);
            layout.Visibility = ViewStates.Visible;

            var pingText = (TextView)FindViewById(controls.IdPing);
            pingText.Text = $"Ping: {duration} ms";

            var timeView = (TextView)FindViewById(controls.IdTimeText);
            timeView.Text = settings.DateTime.ToString("yyyy-MM-dd HH:mm:ss");

            SetSwitch(controls.IdHolidayMode, settings.HolidayMode);
            SetSwitch(controls.IdTwilightMode, settings.TwilightMode);
            SetSwitch(controls.IdMorningMode, settings.MorningMode);

            if (settings.MorningMode)
            {
                TextView morningText = (TextView)FindViewById(controls.IdMorningText);
                SetMorningText(morningText, settings.MorningTime, WeekDaysHelper.WeekDaysFromMask(settings.MorningDays));
                morningText.Visibility = ViewStates.Visible;
            }
        }

        private void SetMorningText(TextView morningText, TimeSpan morningTime, IEnumerable<DayOfWeek> morningDays)
        {
            var time = morningTime.ToString("HH:mm");
            time += ": ";

            foreach (var weekDay in morningDays)
            {
                switch (weekDay)
                {
                    case DayOfWeek.Monday:
                        time += "Pon, ";
                        break;
                    case DayOfWeek.Tuesday:
                        time += "Wt, ";
                        break;
                    case DayOfWeek.Wednesday:
                        time += "Śr, ";
                        break;
                    case DayOfWeek.Thursday:
                        time += "Czw, ";
                        break;
                    case DayOfWeek.Friday:
                        time += "Pt, ";
                        break;
                    case DayOfWeek.Saturday:
                        time += "Sob, ";
                        break;
                    case DayOfWeek.Sunday:
                        time += "Nd, ";
                        break;
                }
            }

            time = time.Substring(0, time.Length - 2);
            morningText.Text = time;
        }

        private void SetSwitch(int switchId, bool value)
        {
            var switchView = (Switch)FindViewById(switchId);
            switchView.Checked = value;
        }

        private void RetrieveData(int host)
        {
            Task.Run(async () =>
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                var response = await _httpClient.GetAsync($"http://{GetString(host)}/getAllSettings");

                stopwatch.Stop();
                return (response, stopwatch.ElapsedMilliseconds);
            })
            .ContinueWith(async task =>
            {
                var response = task.Result.response;
                var content = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    DisplayError(content, host);
                }
                else
                {
                    var settings = _responseParser.ParseAllSettingsResponse(content);
                    UpdateData(settings, task.Result.ElapsedMilliseconds, host);
                }
            });
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu_settings, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.submit_item:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();

            RetrieveData(Resource.String.ground_host);
            RetrieveData(Resource.String.attic_host);
        }

        [Java.Interop.Export("NavigateSwitchTwilightMode")]
        public void NavigateSwitchTwilightMode(View view)
        {
            var host = view.Id == Resource.Id.arduino1_twilight_mode
                ? GetString(Resource.String.ground_host)
                : GetString(Resource.String.attic_host);

            var switchView = (Switch)view;
            if (switchView.Checked)
            {
                if (!_httpClient.PostAsync($"http://{host}/enableTwilightMode", new StringContent("")).Result.IsSuccessStatusCode)
                {
                    switchView.Checked = false;
                    DisplayFailureMessage();
                }
            }
            else
            {
                if (!_httpClient.PostAsync($"http://{host}/disableTwilightMode", new StringContent("")).Result.IsSuccessStatusCode)
                {
                    switchView.Checked = true;
                    DisplayFailureMessage();
                }
            }
        }

        [Java.Interop.Export("NavigateSwitchHolidayMode")]
        public void NavigateSwitchHolidayMode(View view)
        {
            var host = view.Id == Resource.Id.arduino1_holiday_mode 
                ? GetString(Resource.String.ground_host) 
                : GetString(Resource.String.attic_host);
            var switchView = (Switch)view;
            if (switchView.Checked)
            {
                if (!_httpClient.PostAsync($"http://{host}/enableHolidayMode", new StringContent("")).Result.IsSuccessStatusCode)                    
                {
                    switchView.Checked = false;
                    DisplayFailureMessage();
                }
            }
            else
            {
                if (!_httpClient.PostAsync($"http://{host}/disableHolidayMode", new StringContent("")).Result.IsSuccessStatusCode)
                {
                    switchView.Checked = true;
                    DisplayFailureMessage();
                }
            }
        }

        [Java.Interop.Export("NavigateUpdateTime")]
        public void NavigateUpdateTime(View view)
        {
            int host = view.Id == Resource.Id.arduino1_buttonUpdateTime 
                ? Resource.String.ground_host 
                : Resource.String.attic_host;

            var now = DateTime.Now;
            var parameters = "time=" + now.ToString("HH:mm:ss_dd-MM-yyyy");
            var url = $"http://{GetString(host)}/setTime";
            if (_httpClient.PostAsync(url, new StringContent(parameters)).Result.IsSuccessStatusCode)
            {
                var timeView = (TextView)FindViewById(_controls[host].IdTimeText);
                timeView.Text = now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
                DisplayFailureMessage();
        }

        [Java.Interop.Export("NavigateTimePopup")]
        public void NavigateTimePopup(View view)
        {
            Switch morningMode = (Switch)view;
            var morningText = (TextView)(view.Id == Resource.Id.arduino1_morning_mode 
                ? FindViewById(Resource.Id.arduino1_morning_time) 
                : FindViewById(Resource.Id.arduino2_morning_time));

            int host = view.Id == Resource.Id.arduino1_morning_mode 
                ? Resource.String.ground_host 
                : Resource.String.attic_host;

            var url = $"http://{GetString(host)}/setMorningMode";

            if (!morningMode.Checked)
            {
                if (!_httpClient.PostAsync(url, new StringContent("false")).Result.IsSuccessStatusCode)
                {
                    morningMode.Checked = false;
                    DisplayFailureMessage();
                }
                else
                {
                    morningText.Visibility = ViewStates.Gone;
                }
                return;
            }

            var inflater = (LayoutInflater)GetSystemService(LayoutInflaterService);
            var popupView = inflater.Inflate(Resource.Layout.time_popup, null);

            var timePopupWindow = new TimePopupWindow(popupView);
            timePopupWindow.Init();

            int width = LinearLayout.LayoutParams.MatchParent;
            int height = LinearLayout.LayoutParams.WrapContent;
            var popupWindow = new PopupWindow(popupView, width, height, false);

            popupWindow.ShowAtLocation(view, GravityFlags.Center, 0, 0);

            var okButton = (Button)popupView.FindViewById(Resource.Id.time_popup_ok);
            okButton.SetOnClickListener(new ClickListener(timePopupWindow, _httpClient, url,
                () =>
                {
                    SetMorningText(morningText, timePopupWindow.GetTime(), timePopupWindow.GetSelectedDays());
                    morningText.Visibility = ViewStates.Visible;
                    popupWindow.Dismiss();
                },
                () =>
                {
                    morningMode.Checked = false;
                    DisplayFailureMessage();
                    popupWindow.Dismiss();
                }));
        }

        private class ClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private readonly TimePopupWindow _timePopupWindow;
            private readonly HttpClient _httpClient;
            private readonly string _url;
            private readonly Action _onSuccess;
            private readonly Action _onFailure;

            public ClickListener(TimePopupWindow timePopupWindow, HttpClient httpClient, string url, Action onSuccess, Action onFailure)
            {
                _timePopupWindow = timePopupWindow;
                _httpClient = httpClient;
                _url = url;
                _onSuccess = onSuccess;
                _onFailure = onFailure;
            }

            public void OnClick(View v)
            {
                var parameters = String.Format("true;days=%3d;time=%s",
                    WeekDaysHelper.GetDaysMask(_timePopupWindow.GetSelectedDays()),
                    _timePopupWindow.GetTime().ToString("HH:mm"));
                if (!_httpClient.PostAsync(_url, new StringContent(parameters)).Result.IsSuccessStatusCode)
                {
                    _onFailure();
                }
                else
                {
                    _onSuccess();
                }
            }
        }

        private void DisplayFailureMessage()
        {
            Toast.MakeText(this.ApplicationContext, Resource.String.connection_failure_message, ToastLength.Short);
        }

        private class UIControls
        {
            public int IdPing { get; set; }
            public int IdTimeText { get; set; }
            public int IdLayout { get; set; }
            public int IdHolidayMode { get; set; }
            public int IdTwilightMode { get; set; }
            public int IdMorningMode { get; set; }
            public int IdMorningText { get; set; }

            public UIControls(int idPing, int idTimeText, int idLayout, int idHolidayMode, int idTwilightMode, int idMorningMode, int idMorningText)
            {
                IdPing = idPing;
                IdTimeText = idTimeText;
                IdLayout = idLayout;
                IdHolidayMode = idHolidayMode;
                IdTwilightMode = idTwilightMode;
                IdMorningMode = idMorningMode;
                IdMorningText = idMorningText;
            }
        }
    }
}