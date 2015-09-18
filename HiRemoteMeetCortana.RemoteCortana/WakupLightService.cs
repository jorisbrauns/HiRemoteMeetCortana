using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using HiRemoteMeetCortana.RemoteCortana.models;

namespace HiRemoteMeetCortana.Services
{
    public interface IWakeupLightService
    {
        Task<Settings> get();
        Task<bool> Save(Settings settings);
    }

    public class WakupLightService : IWakeupLightService
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage webResponse = null;

        public object JObject { get; private set; }

        public async Task<Settings> get()
        {
            webResponse = await client.GetAsync("http://hiremotemeetcortana.azurewebsites.net/api/settings");
            if (webResponse.IsSuccessStatusCode)
            {
                string responseBody = await webResponse.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Settings>(responseBody);
            }

            throw new NullReferenceException("Could not parse response (settings) from service");
        }

        public async Task<bool> Save(Settings settings)
        {
            string postBody = JsonConvert.SerializeObject(settings);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var webResponse = await client.PostAsync("http://hiremotemeetcortana.azurewebsites.net/api/settings", new StringContent(postBody, Encoding.UTF8, "application/json"));

            return webResponse.IsSuccessStatusCode;
        }

    }

}
