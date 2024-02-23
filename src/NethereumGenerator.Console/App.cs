using NethereumGenerator.ExtendedConsole.Commands;
using Microsoft.Extensions.CommandLineUtils;

namespace NethereumGenerator.ExtendedConsole
{
    public class App : CommandLineApplication
    {
        public App()
        {
            Commands.Add(new GenerateCommand());
            HelpOption("-h | -? | --help");
        }
    }
}