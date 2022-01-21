using System;
using System.Net.Http;

namespace SmartHome
{
    public static class HttpClientFactory
    {
        static HttpClient _httpClient;
        static HttpClientFactory()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public static HttpClient Get() => _httpClient;
    }
}