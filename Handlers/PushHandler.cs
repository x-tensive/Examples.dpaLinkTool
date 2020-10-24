using dpaLinkTool.Client;
using dpaLinkTool.Config;
using dpaLinkTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace dpaLinkTool.Handlers
{
    public class PushHandler
    {
        public async static Task PushIndicators(DateTime from, DateTime to, string cfgFileName)
        {
            EquipmentConnectorCfg[] cfg;

            using (var xmlFile = new FileStream(cfgFileName, FileMode.Open)) {
                using (var xmlReader = XmlReader.Create(xmlFile)) {
                    var xmlSerializer = new XmlSerializer(typeof(EquipmentConnectorCfg[]));
                    cfg = (EquipmentConnectorCfg[])xmlSerializer.Deserialize(xmlReader);
                }
            }

            using (var client = new DpaRestClient(LinkConfig.DPA.BaseUrl)) {
                await client.Login(LinkConfig.DPA.UserName, LinkConfig.DPA.Password);
                
                foreach (var equipmentCfg in cfg) {
                    foreach (var indicatorCfg in equipmentCfg.Indicators) {
                        using (var connector = LinkConfig.Connectors.CreateConnector(indicatorCfg.Connector, $"{equipmentCfg.Name} / {indicatorCfg.Name}")) {
                            var values = await client.GetIndicatorValues(indicatorCfg.ID, from, to);
                            connector.Push(equipmentCfg, indicatorCfg, values);
                        }
                    }
                }
            }
        }
    }
}
