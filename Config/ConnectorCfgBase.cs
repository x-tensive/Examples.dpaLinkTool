using dpaLinkTool.Connectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace dpaLinkTool.Config
{
    public abstract class ConnectorCfgBase
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public ConnectorParamCfg[] Params { get; set; }

        public abstract ConnectorBase CreateConnector();
    }
}
