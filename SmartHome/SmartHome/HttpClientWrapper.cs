using Android.App;
using Android.Widget;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHome
{
    public static class HttpClientWrapper
    {
        static HttpClient _httpClient;
        static HttpClientWrapper()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public static HttpClient GetClient() => _httpClient;

        public static async Task<HttpStatusCode> Post(string url, string content, Activity activity)
        {
            HttpStatusCode responseCode;
            try
            {
                responseCode = (await _httpClient.PostAsync(url, new StringContent(content))).StatusCode;
            }
            catch
            {
                responseCode = HttpStatusCode.InternalServerError;
            }

            if (responseCode != HttpStatusCode.OK)
            {
                activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(activity.ApplicationContext,
                        $"{activity.GetString(Resource.String.arduino_response_message)}{responseCode}", ToastLength.Short).Show();
                });
            }
            return responseCode;
        }
    }
}