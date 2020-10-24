using dpaLinkTool.Models;
using System;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace dpaLinkTool.Connectors
{
    public abstract class ConnectorBase
    {
        public abstract void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values);
    }
}
