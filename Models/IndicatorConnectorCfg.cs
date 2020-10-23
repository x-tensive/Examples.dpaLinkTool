using System;
using System.Collections.Generic;
using System.Text;

namespace dpaLinkTool.Models
{
    [Serializable]
    public class IndicatorConnectorCfg
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string ModuleName { get; set; }

        public string Device { get; set; }

        public string DeviceClass { get; set; }

        public string Connector { get; set; }
    }
}
