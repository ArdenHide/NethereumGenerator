using Microsoft.Extensions.CommandLineUtils;

namespace NethereumGenerator.ExtendedConsole.Commands
{
    public static class Extensions
    {
        public static void AddHelpOption(this CommandLineApplication app)
        {
            app.HelpOption("-? | -h | --help");
        }
    }
}
