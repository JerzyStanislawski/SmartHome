using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartHome.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

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
                var responseCode = _activity._httpClient.PostAsync($"http://{_activity.GetHost()}/impulsRolety",
                    new StringContent(_blind.Name)).Result.StatusCode;

                if (responseCode != HttpStatusCode.OK)
                {
                    Toast.MakeText(this._activity.ApplicationContext,
                        $"{Resource.String.arduino_response_message}{responseCode}", ToastLength.Short).Show();
                }
            }
        }
    }
}