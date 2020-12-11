using dpaLinkTool.Config;
using dpaLinkTool.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Text;

namespace dpaLinkTool.Connectors
{
    public class InfluxdbConnector : ConnectorBase
    {
        private ProgressBar progressBar;

        public InfluxdbConnectorCfg Cfg { get; private set; }

        public override void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values)
        {
            var influxDBClient = InfluxDBClientFactory.Create(Cfg.Url, Cfg.Token);

            progressBar.MaxTicks = values.Length;

            using (var writeApi = influxDBClient.GetWriteApi()) {

                foreach (var value in values) {

                    var point = PointData.Measurement($"{equipment.Name}\\{indicator.Name}");
                    foreach (var param in Cfg.Params) {
                        var paramValue = param.GetParamValue(equipment, indicator, value);
                        point = point.Tag(param.Name, paramValue.ToString());
                    }
                    point = point.Field("value", value.Value.GetDouble())
                        .Timestamp(value.TimeStamp, WritePrecision.Ms);

                    writeApi.WritePoint(Cfg.Bucket, Cfg.Org, point);

                    progressBar.Tick();
                }
            }
        }

        protected override void Dispose(bool full)
        {
            progressBar.Dispose();
        }

        // ctor
        public InfluxdbConnector(InfluxdbConnectorCfg cfg, string actionName) : base(actionName)
        {
            this.Cfg = cfg;

            var progressBarOptions = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true,
                ForegroundColor = ConsoleColor.White,
                BackgroundColor = ConsoleColor.DarkGray,
                ForegroundColorDone = ConsoleColor.Gray
            };
            this.progressBar = new ProgressBar(1, actionName, progressBarOptions);
        }
    }
}
