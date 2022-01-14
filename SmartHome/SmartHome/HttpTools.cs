using System.Net.Http;

namespace SmartHome
{
    public static class HttpClientFactory
    {
        static HttpClient _httpClient;
        static HttpClientFactory()
        {
            _httpClient = new HttpClient();
        }

        public static HttpClient Get() => _httpClient;
    }
}