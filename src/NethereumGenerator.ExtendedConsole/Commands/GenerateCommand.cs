﻿using Microsoft.Extensions.CommandLineUtils;

namespace NethereumGenerator.ExtendedConsole.Commands
{
    public class GenerateCommand : CommandLineApplication
    {
        public GenerateCommand()
        {
            Name = "generate";
            Description = "Generates Nethereum code for Ethereum integration and interaction.";

            Commands.Add(new GenerateFromProjectCommand());
            Commands.Add(new GenerateFromAbiCommand());
            Commands.Add(new GenerateFromTruffleCommand());

            this.AddHelpOption();
        }
    }
}
