using dpaLinkTool.Config;
using dpaLinkTool.Models;
using ShellProgressBar;
using System;
using System.Data;
using System.Data.SqlClient;

namespace dpaLinkTool.Connectors
{
    public class MssqlConnector : ConnectorBase
    {
        private ProgressBar progressBar;

        public MssqlConnectorCfg Cfg { get; private set; }

        public override void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values)
        {
            progressBar.MaxTicks = values.Length;

            using (var connection = new SqlConnection(Cfg.Connection)) {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                foreach (var value in values) {

                    using (var cmd = new SqlCommand(Cfg.Command, connection)) {
                        foreach (var param in Cfg.Params) {
                            var paramValue = param.GetParamValue(equipment, indicator, value);
                            var sqlParam = new SqlParameter(param.Name, paramValue);
                            cmd.Parameters.Add(sqlParam);
                        }
                        cmd.ExecuteNonQuery();
                    }

                    progressBar.Tick();
                }
            }
        }

        protected override void Dispose(bool full)
        {
            progressBar.Dispose();
        }

        // ctor
        public MssqlConnector(MssqlConnectorCfg cfg, string actionName): base(actionName)
        {
            this.Cfg = cfg;

            var progressBarOptions = new ProgressBarOptions {
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
