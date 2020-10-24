using dpaLinkTool.Models;
using System;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace dpaLinkTool.Connectors
{
    public abstract class ConnectorBase: IDisposable
    {
        public string ActionName { get; set; }

        public abstract void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values);

        public void Dispose()
        {
            Dispose(true);
        }

        protected abstract void Dispose(bool full);

        // ctor
        public ConnectorBase(string actionName)
        {
            this.ActionName = actionName;
        }
    }
}
