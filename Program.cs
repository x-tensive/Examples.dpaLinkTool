using dpaLinkTool.Config;
using System.Threading.Tasks;

namespace dpaLinkTool
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            LinkConfig.Setup();

            return await LinkCommandLine.Run(args);
        }
    }
}
