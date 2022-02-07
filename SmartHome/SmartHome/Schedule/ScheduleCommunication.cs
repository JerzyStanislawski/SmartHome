using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHome.Schedule
{
    public class ScheduleCommunication
    {
        private readonly Context _applicationContext;
        private readonly HttpClient _httpClient;

        public ScheduleCommunication(Context applicationContext)
        {
            _applicationContext = applicationContext;
            _httpClient = HttpClientWrapper.GetClient();
        }

        public async Task<IEnumerable<ScheduleEntity>> RetrieveData()
        {
            var entities = new List<ScheduleEntity>();

            await GetScheduledEvents(entities, _applicationContext.GetString(Resource.String.ground_host), Area.GROUNDFLOOR, Area.ATTIC);
            await GetScheduledEvents(entities, _applicationContext.GetString(Resource.String.attic_host), Area.ATTIC, Area.GROUNDFLOOR);

            return entities;
        }

        private async Task GetScheduledEvents(List<ScheduleEntity> entities, string baseUrl, Area lightsArea, Area blindsArea)
        {
            var response = await _httpClient.GetAsync($"http://{baseUrl}/getScheduledEvents");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(content))
                {
                    var lines = content.TrimEnd().Split(";\r\n");
                    int i = entities.Count();
                    foreach (var line in lines)
                    {
                        entities.Add(ScheduleEntityHelpers.FromHttpResponseLine(i++, line, lightsArea, blindsArea));
                    }
                }
            }
            else
            {
                throw new HttpRequestException("GetScheduledEvents error");
            }
        }

        public async Task<bool> SendData(IEnumerable<ScheduleEntity> entities)
        {
            var groundEntities = entities.Where(x => (x.Type == ScheduleType.LIGHTS && x.Area == Area.GROUNDFLOOR)
                || (x.Type == ScheduleType.BLINDS && x.Area == Area.ATTIC));
            var atticEntities = entities.Where(x => (x.Type == ScheduleType.LIGHTS && x.Area == Area.ATTIC)
                || (x.Type == ScheduleType.BLINDS && x.Area == Area.GROUNDFLOOR));

            var clearBasementResponseCode = await Clear(_applicationContext.GetString(Resource.String.ground_host));
            var clearAtticResponseCode = await Clear(_applicationContext.GetString(Resource.String.attic_host));
            bool basementSend = false, atticSend = false;

            if (clearBasementResponseCode == HttpStatusCode.OK)
                basementSend = await Send(groundEntities, _applicationContext.GetString(Resource.String.ground_host));

            if (clearAtticResponseCode == HttpStatusCode.OK)
                atticSend = await Send(atticEntities, _applicationContext.GetString(Resource.String.attic_host));

            return basementSend && atticSend;
        }

        private async Task<HttpStatusCode> Clear(string baseUrl)
        {
            var response = await _httpClient.PostAsync($"http://{baseUrl}/clearSchedule", new StringContent(""));
            return response.StatusCode;
        }

        private async Task<bool> Send(IEnumerable<ScheduleEntity> entities, String baseUrl)
        {
            int i = 0;
            String requestBody = "";

            foreach (var entity in entities)
            {
                String line = ScheduleEntityHelpers.BuildHttpRequestLine(entity);
                requestBody += line;
                i++;

                if (i == 10)
                {
                    var respone = await _httpClient.PostAsync($"http://{baseUrl}/schedule", new StringContent(requestBody));
                    if (!respone.IsSuccessStatusCode)
                        return false;

                    i = 0;
                    requestBody = "";
                }
            }

            if (!String.IsNullOrEmpty(requestBody))
            {
                var respone = await _httpClient.PostAsync($"http://{baseUrl}/schedule", new StringContent(requestBody));
                if (!respone.IsSuccessStatusCode)
                    return false;
            }

            return true;
        }
    }
}