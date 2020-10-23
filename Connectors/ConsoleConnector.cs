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
            var paramLambdas = Cfg.Params
                    .Select(p => new { Name = p.Name, Lambda = GetParamValueLambda(p.Value) })
                    .ToDictionary(p => p.Name, p => p.Lambda);

            foreach (var value in values) {

                var paramValues = Cfg.Params
                    .Select(p => new { Name = p.Name, Value = GetParamValue(paramLambdas[p.Name], equipment, indicator, value) })
                    .ToArray();

                string outString = Cfg.Format;
                foreach (var paramValue in paramValues)
                    outString = outString.Replace("{" + paramValue.Name + "}", paramValue.Value.ToString());

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
