using dpaLinkTool.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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

        public async Task<IndicatorValue[]> GetIndicatorValues(long indicatorId, DateTime from, DateTime to)
        {
            var payload = JsonSerializer.Serialize(new {
                dateTimeFrom = (DateTimeOffset)from,
                dateTimeUntil = (DateTimeOffset)to,
                indicators = new long[] { indicatorId },
                gridOptions = new {
                    skip = 0
                }
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/dashboard/getIndicatorData", content);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Action failed: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStreamAsync();
            var indicatorValuesResponse = await JsonSerializer.DeserializeAsync<IndicatorValue[]>(responseContent);

            return indicatorValuesResponse;
        }

        // ctor
        public DpaRestClient(string baseAddress)
        {
            CookieContainer = new CookieContainer();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = CookieContainer;

            Client = new HttpClient(handler) {
                BaseAddress = new Uri(baseAddress),
            };

            Client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("dpaLinkTool", $"{assemblyVersion.Major}.{assemblyVersion.Minor}"));
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
