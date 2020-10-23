using dpaLinkTool.Connectors;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpaLinkTool.Config
{
    public class ConnectorsCfg
    {
        private Dictionary<string, ConnectorCfgBase> connectorCfgHash;

        private IEnumerable<ConnectorParamCfg> GetParams(IConfigurationSection configuration)
        {
            foreach (var childConfig in configuration.GetChildren())
                yield return new ConnectorParamCfg { Name = childConfig.Key, Value = childConfig.Value };
        }

        private ConsoleConnectorCfg CreateConsoleConnectorCfg(string name, IConfigurationSection configuration)
        {
            return new ConsoleConnectorCfg {
                Name = name,
                Type = "console",
                Format = configuration.GetValue<string>("format"),
                Params = GetParams(configuration.GetSection("params")).ToArray()
            };
        }

        public ConnectorCfgBase GetConnectorCfg(string name)
        {
            return connectorCfgHash[name];
        }

        public ConnectorBase CreateConnector(string name)
        {
            return connectorCfgHash[name].CreateConnector();
        }

        public void Setup(IConfigurationSection configuration)
        {
            connectorCfgHash = new Dictionary<string, ConnectorCfgBase>();

            foreach (var childConfig in configuration.GetChildren()) {
                var connectorType = childConfig.GetValue<string>("type");
                if (connectorType == "console")
                    connectorCfgHash.Add(childConfig.Key, CreateConsoleConnectorCfg(childConfig.Key, childConfig));
                else
                    throw new NotSupportedException();
            }
        }
    }
}
