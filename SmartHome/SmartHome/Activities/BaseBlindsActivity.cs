using Android.Views;
using Android.Widget;
using SmartHome.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHome.Activities
{
    public abstract class BaseBlindsActivity : BasePageActivity
    {

        protected Dictionary<int, Blind> _buttons;

        protected void Initialize()
        {
            _buttons = GetButtonsList();

            foreach (var (buttonKey, buttonValue) in _buttons)
            {
                Button sButton = (Button)FindViewById(buttonKey);
                sButton.SetOnClickListener(new ChangeListener(this, buttonValue));
            }
        }

        protected abstract Dictionary<int, Blind> GetButtonsList();
        protected abstract string GetHost();

        private class ChangeListener : Java.Lang.Object, View.IOnClickListener
        {
            private readonly BaseBlindsActivity _activity;
            private readonly Blind _blind;

            public ChangeListener(BaseBlindsActivity activity, Blind blind)
            {
                _activity = activity;
                _blind = blind;
            }

            public void OnClick(View v)
            {
                Task.Run(async () =>
                {
                    HttpStatusCode responseCode;
                    try
                    {
                        responseCode = (await _activity._httpClient.PostAsync($"http://{_activity.GetHost()}/impulsRolety",
                            new StringContent(_blind.Name))).StatusCode;
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
                        });
                    }
                });
            }
        }
    }
}