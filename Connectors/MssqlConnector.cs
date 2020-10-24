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
        public MssqlConnectorCfg Cfg { get; private set; }

        public override void Push(EquipmentConnectorCfg equipment, IndicatorConnectorCfg indicator, IndicatorValue[] values)
        {
            var progressBarOptions = new ProgressBarOptions {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true,
                ForegroundColor = ConsoleColor.White,
                BackgroundColor = ConsoleColor.DarkGray,
                ForegroundColorDone = ConsoleColor.Gray
            };

            using (var progressBar = new ProgressBar(values.Length, $"{equipment.Name} / {indicator.Name}", progressBarOptions)) {

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

                            progressBar.Tick();
                        }

                    }
                }

            }
        }

        // ctor
        public MssqlConnector(MssqlConnectorCfg cfg)
        {
            this.Cfg = cfg;
        }
    }
}
