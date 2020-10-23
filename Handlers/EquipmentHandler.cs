using dpaLinkTool.Client;
using dpaLinkTool.Config;
using dpaLinkTool.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace dpaLinkTool.Handlers
{
    public class EquipmentHandler
    {
        public async static Task GetEquipment()
        {
            using (var client = new DpaRestClient(LinkConfig.DPA.BaseUrl)) {
                await client.Login(LinkConfig.DPA.UserName, LinkConfig.DPA.Password);
                var equipment = await client.GetEquipment();

                Console.WriteLine(JsonSerializer.Serialize(equipment, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            }
        }

        public async static Task GetIndicators()
        {
            using (var client = new DpaRestClient(LinkConfig.DPA.BaseUrl)) {
                await client.Login(LinkConfig.DPA.UserName, LinkConfig.DPA.Password);
                var equipment = await client.GetEquipment();

                foreach (var equipmentInstance in equipment) {
                    var indicators = await client.GetIndicators(equipmentInstance);

                    Console.WriteLine(JsonSerializer.Serialize(indicators, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
                }
            }
        }
    }
}
