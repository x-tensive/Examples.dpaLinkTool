using System;
using System.Collections.Generic;
using System.Text;

namespace dpaLinkTool.Models
{
    [Serializable]
    public class EquipmentConnectorCfg
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string DepartmentName { get; set; }

        public IndicatorConnectorCfg[] Indicators { get; set; }
    }
}
