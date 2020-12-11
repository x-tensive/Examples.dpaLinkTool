using dpaLinkTool.Connectors;

namespace dpaLinkTool.Config
{
    public class MssqlConnectorCfg: ConnectorCfgBase
    {
        public string Connection { get; set; }

        public string Command { get; set; }

        public override ConnectorBase CreateConnector(string actionName)
        {
            return new MssqlConnector(this, actionName);
        }
    }
}
