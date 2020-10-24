using dpaLinkTool.Config;
using dpaLinkTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpaLinkTool.Connectors
{
    public class ConsoleConnector: ConnectorBase
    {
        public ConsoleConnectorCfg Cfg { get; private set; }

        public override void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values)
        {
            foreach (var value in values) {

                string outString = Cfg.Format;
                foreach (var param in Cfg.Params) {
                    var paramValue = param.GetParamValue(equipment, indicator, value);
                    outString = outString.Replace("{" + param.Name + "}", paramValue.ToString());
                }

                Console.WriteLine(outString);
            }
        }

        // ctor
        public ConsoleConnector(ConsoleConnectorCfg cfg)
        {
            this.Cfg = cfg;
        }
    }
}
