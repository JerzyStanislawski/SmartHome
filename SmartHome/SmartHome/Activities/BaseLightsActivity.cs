using Android.Widget;
using SmartHome.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHome.Activities
{
    public abstract class BaseLightsActivity : BasePageActivity
    {
        protected Dictionary<int, Light> _switches;
        bool _updatingStatus = false;
        ArduinoResponseParser _responseParser = new ArduinoResponseParser();

        private void UpdateState()
        {
            EnableButtons(false);

            Task.Run(async () =>
            {
                HttpResponseMessage response;
                try
                {
                    response = await _httpClient.GetAsync($"http://{GetHost()}/getStatus");
                }
                catch
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var state = _responseParser.ParseLightsStatus(content);

                    EnableButtons(true);
                    RunOnUiThread(() => UpdateSwitches(state));
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this.ApplicationContext, Resource.String.connection_failure_message, ToastLength.Short).Show());
                }
            });
        }

        private void UpdateSwitches(IDictionary<int, bool> state)
        {
            _updatingStatus = true;
            foreach (var (switchKey, _) in _switches)
            {
                var sButton = (Switch)FindViewById(switchKey);

                int lightId = _switches[switchKey].Output;
                if (state.ContainsKey(lightId))
                {
                    var turnedOn = state[lightId];
                    if (sButton.Checked != turnedOn)
                        sButton.Checked = turnedOn;
                }
            }
            _updatingStatus = false;
        }

        private void EnableButtons(bool enable)
        {
            RunOnUiThread(() =>
            {
                foreach (var switchKey in _switches.Keys)
                {
                    var @switch = (Switch)FindViewById(switchKey);
                    @switch.Enabled = enable;
                }
            });
        }

        protected void Initialize()
        {
            _switches = GetSwitchList();

            UpdateState();

            foreach (var (switchKey, switchValue) in _switches)
            {
                var sButton = (Switch)FindViewById(switchKey);
                sButton.SetOnCheckedChangeListener(new ChangeListener(this, switchValue));
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateState();
        }

        protected abstract Dictionary<int, Light> GetSwitchList();
        protected abstract string GetHost();

        private class ChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            private readonly BaseLightsActivity _activity;
            private readonly Light _light;

            public ChangeListener(BaseLightsActivity activity, Light light)
            {
                _activity = activity;
                _light = light;
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                if (_activity._updatingStatus)
                    return;

                Task.Run(async () =>
                {
                    HttpStatusCode responseCode;
                    try
                    {
                        responseCode = (await _activity._httpClient.PostAsync($"http://{_activity.GetHost()}/impulsOswietlenie",
                            new StringContent($"{_light.Name}={buttonView.Checked.ToString().ToLowerInvariant()}"))).StatusCode;
                    }
                    catch
                    {
                        responseCode = HttpStatusCode.InternalServerError;
                    }

                    if (responseCode != HttpStatusCode.OK)
                    {
                        _activity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(_activity.ApplicationContext,
                                $"{_activity.GetString(Resource.String.arduino_response_message)}{responseCode}", ToastLength.Short).Show();
                            buttonView.Checked = !isChecked;
                        });
                    }
                });
            }
        }
    }
}