using dpaLinkTool.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dpaLinkTool.Client
{
    public class DpaRestClient : IDisposable
    {
        private CookieContainer CookieContainer { get; set; }

        private HttpClient Client { get; set; }

        /// <summary>
        /// User login. Cookies are automatically stored in the client.
        /// </summary>
        public async Task Login(string userName, string password)
        {
            var payload = JsonSerializer.Serialize(new {
                userName = userName,
                password = password
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Account/Login", content);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Login failed: {response.StatusCode}");
        }

        public async Task<Equipment[]> GetEquipment()
        {
            var payload = JsonSerializer.Serialize(new {
                skip = 0
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/DpaEnterpriseStrusture/getEquipments", content);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Action failed: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStreamAsync();
            var equipmentResponse = await JsonSerializer.DeserializeAsync<Equipment[]>(responseContent);

            return equipmentResponse;
        }

        public async Task<Indicator[]> GetIndicators(Equipment equipment)
        {
            var payload = JsonSerializer.Serialize(new long[] { equipment.ID });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Dashboard/getIndicators", content);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Action failed: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStreamAsync();
            var indicatorsResponse = await JsonSerializer.DeserializeAsync<Indicator[]>(responseContent);

            return indicatorsResponse;
        }

        // ctor
        public DpaRestClient(string baseAddress)
        {
            CookieContainer = new CookieContainer();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = CookieContainer;

            Client = new HttpClient(handler) {
                BaseAddress = new Uri(baseAddress)
            };
        }

        protected virtual void Dispose(bool full)
        {
            Client.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
