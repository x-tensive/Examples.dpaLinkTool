using dpaLinkTool.Handlers;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpaLinkTool
{
    public static class LinkCommandLine
    {
        private static Command BuildRootCommand(string description)
        {
            return new RootCommand(description);
        }

        private static Command BuildCommand(string name, bool treatUnmatchedTokensAsErrors = false)
        {
            return new Command(name) { TreatUnmatchedTokensAsErrors = treatUnmatchedTokensAsErrors };
        }

        private static Command WithSubCommand(this Command command, string name, bool treatUnmatchedTokensAsErrors, Action<Command> initSubCommand)
        {
            var subCommand = new Command(name) { TreatUnmatchedTokensAsErrors = treatUnmatchedTokensAsErrors };
            initSubCommand(subCommand);
            command.AddCommand(subCommand);
            return command;
        }

        private static Command WithHandler(this Command command, ICommandHandler handler)
        {
            command.Handler = handler;
            return command;
        }

        private static Command WithOption<T>(this Command command, string alias, string description, bool isRequired = false)
        {
            command.AddOption(new System.CommandLine.Option(alias, description) {
                IsRequired = isRequired,
                Argument = new Argument<T>()
            });
            return command;
        }

        private static Command WithDateTimeOption(this Command command, string alias, string description, bool isRequired = false)
        {
            command.AddOption(new System.CommandLine.Option(alias, description) {
                IsRequired = isRequired,
                Argument = new Argument<DateTime>((ArgumentResult argumentResult) => DateTime.Parse(argumentResult.Tokens.Single().Value))
            }); ;
            return command;
        }

        private static Command WithGetCommands(this Command parent)
        {
            var extCmd = BuildCommand("get", true)
                .WithSubCommand("equipment", true, subCommand => {
                    subCommand
                    .WithHandler(CommandHandler.Create(EquipmentHandler.GetEquipment));
                })
                .WithSubCommand("indicators", true, subCommand => {
                    subCommand
                    .WithHandler(CommandHandler.Create(EquipmentHandler.GetIndicators));
                });

            parent.AddCommand(extCmd);
            return parent;
        }

        private static Command WithCreateConnectorsConfigCommands(this Command parent)
        {
            var extCmd = BuildCommand("createConnectorsConfig", true)
                .WithSubCommand("indicators", true, subCommand => {
                    subCommand
                    .WithHandler(CommandHandler.Create((string fileName) => ConnectorsHandler.CreateIndicators(fileName)))
                    .WithOption<string>("--fileName", "file name", true);
                });

            parent.AddCommand(extCmd);
            return parent;
        }

        private static Command WithPushCommands(this Command parent)
        {
            var extCmd = BuildCommand("push", true)
                .WithSubCommand("indicators", true, subCommand => {
                    subCommand
                    .WithHandler(CommandHandler.Create((DateTime from, DateTime to, string cfg) => PushHandler.PushIndicators(from, to, cfg)))
                    .WithDateTimeOption("--from", "period FROM", true)
                    .WithDateTimeOption("--to", "period TO", true)
                    .WithOption<string>("--cfg", "cfg file name", true);
                });

            parent.AddCommand(extCmd);
            return parent;
        }

        public static async Task<int> Run(params string[] args)
        {
            var rootCmd = BuildRootCommand("DPA LINK tool")
                .WithGetCommands()
                .WithCreateConnectorsConfigCommands()
                .WithPushCommands();

            return await rootCmd.InvokeAsync(args);
        }

    }
}
