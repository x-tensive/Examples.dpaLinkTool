using dpaLinkTool.Models;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace dpaLinkTool.Config
{
    public class ConnectorParamCfg
    {
        private Lazy<Delegate> lambda0;

        public string Name { get; set; }

        public string Value { get; set; }

        public object GetParamValue(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue value)
        {
            return lambda0.Value.DynamicInvoke(equipment, indicator, value);
        }

        private Delegate CreateLambda0()
        {
            var equipmentParam = Expression.Parameter(typeof(EquipmentConnectorCfg), "equipment");
            var indicatorParam = Expression.Parameter(typeof(IndicatorConnectorCfg), "indicator");
            var valueParam = Expression.Parameter(typeof(IndicatorValue), "value");

            var lambda = DynamicExpressionParser.ParseLambda(new[] { equipmentParam, indicatorParam, valueParam }, null, this.Value);
            return lambda.Compile();
        }

        // ctor
        public ConnectorParamCfg()
        {
            lambda0 = new Lazy<Delegate>(CreateLambda0);
        }
    }
}
