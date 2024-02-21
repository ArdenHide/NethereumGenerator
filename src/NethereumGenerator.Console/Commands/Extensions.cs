using Microsoft.Extensions.CommandLineUtils;

namespace NethereumGenerator.Console.Commands
{
    public static class Extensions
    {
        public static void AddHelpOption(this CommandLineApplication app)
        {
            app.HelpOption("-? | -h | --help");
        }
    }
}
