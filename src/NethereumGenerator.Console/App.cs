using NethereumGenerator.Console.Commands;
using Microsoft.Extensions.CommandLineUtils;

namespace NethereumGenerator.Console
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