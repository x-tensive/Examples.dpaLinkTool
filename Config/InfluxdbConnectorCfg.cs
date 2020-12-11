using dpaLinkTool.Connectors;

namespace dpaLinkTool.Config
{
    public class InfluxdbConnectorCfg : ConnectorCfgBase
    {
        public string Url { get; set; }

        public string Token { get; set; }

        public string Org { get; set; }

        public string Bucket { get; set; }

        public override ConnectorBase CreateConnector(string actionName)
        {
            return new InfluxdbConnector(this, actionName);
        }
    }
}
