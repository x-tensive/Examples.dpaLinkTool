using dpaLinkTool.Models;
using System;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace dpaLinkTool.Connectors
{
    public abstract class ConnectorBase
    {
        public abstract void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values);

        protected Delegate GetParamValueLambda(string expression)
        {
            var equipmentParam = Expression.Parameter(typeof(EquipmentConnectorCfg), "equipment");
            var indicatorParam = Expression.Parameter(typeof(IndicatorConnectorCfg), "indicator");
            var valueParam = Expression.Parameter(typeof(IndicatorValue), "value");

            var lambda = DynamicExpressionParser.ParseLambda(new[] { equipmentParam, indicatorParam, valueParam }, null, expression);
            return lambda.Compile();
        }

        protected object GetParamValue(Delegate lambda, EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue value)
        {
            return lambda.DynamicInvoke(equipment, indicator, value);
        }
    }
}
