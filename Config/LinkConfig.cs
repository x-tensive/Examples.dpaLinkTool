﻿using Microsoft.Extensions.Configuration;

namespace dpaLinkTool.Config
{
    public class LinkConfig
    {
        public static DpaConfig DPA { get; private set; }

        public static ConnectorsCfg Connectors { get; private set; }

        public static void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            LinkConfig.DPA = new DpaConfig();
            configuration.Bind("dpa", LinkConfig.DPA);

            LinkConfig.Connectors = new ConnectorsCfg();
            LinkConfig.Connectors.Setup(configuration.GetSection("connectors"));
        }
    }
}
