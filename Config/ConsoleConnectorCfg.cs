﻿using dpaLinkTool.Connectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace dpaLinkTool.Config
{
    public class ConsoleConnectorCfg: ConnectorCfgBase
    {
        public string Format { get; set; }

        public override ConnectorBase CreateConnector(string actionName)
        {
            return new ConsoleConnector(this, actionName);
        }
    }
}
