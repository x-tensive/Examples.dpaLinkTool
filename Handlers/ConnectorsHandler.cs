using dpaLinkTool.Client;
using dpaLinkTool.Config;
using dpaLinkTool.Models;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace dpaLinkTool.Handlers
{
    public class ConnectorsHandler
    {
        private static async Task<EquipmentConnectorCfg> CreateEquipmentConnectorCfg(DpaRestClient client, Equipment equipment)
        {
            var cfg = new EquipmentConnectorCfg {
                ID = equipment.ID,
                Name = equipment.Name,
                DepartmentName = equipment.DepartmentName
            };

            var indicators = await client.GetIndicators(equipment);

            cfg.Indicators = indicators
                .Select(ind => new IndicatorConnectorCfg {
                    ID = ind.ID,
                    Name = ind.Name,
                    ModuleName = ind.ModuleName,
                    Device = ind.Device,
                    DeviceClass = ind.DeviceClass,
                    Connector = "connectorName"
                })
                .ToArray();

            return cfg;
        }

        private static EquipmentConnectorCfg[] CreateCfg(DpaRestClient client, Equipment[] equipment)
        {
            using (var progressBar = new ProgressBar(equipment.Length, "", ConsoleColor.DarkGray)) {

                var cfg = equipment
                    .Select(async (Equipment equipmentInstance) => {
                        var equipmentCfg = await CreateEquipmentConnectorCfg(client, equipmentInstance);
                        progressBar.Tick(equipmentCfg.Name);
                        return equipmentCfg;
                    })
                    .Select(action => action.Result)
                    .ToArray();

                return cfg;
            }
        }

        public async static Task CreateIndicators(string fileName)
        {
            using (var client = new DpaRestClient(LinkConfig.DPA.BaseUrl)) {
                await client.Login(LinkConfig.DPA.UserName, LinkConfig.DPA.Password);
                var equipment = await client.GetEquipment();
                var cfg = CreateCfg(client, equipment);
                
                using (var xmlFile = new FileStream(fileName, FileMode.Create)) {
                    using (var xmlWriter = XmlWriter.Create(xmlFile, new XmlWriterSettings { Indent = true })) {
                        var xmlSerializer = new XmlSerializer(typeof(EquipmentConnectorCfg[]));
                        xmlSerializer.Serialize(xmlWriter, cfg);
                    }
                }
            }
        }
    }
}
