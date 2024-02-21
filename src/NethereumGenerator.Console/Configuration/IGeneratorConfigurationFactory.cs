using Nethereum.Generators.Core;
using Nethereum.Generators;
using System.Collections.Generic;

namespace NethereumGenerator.Console.Configuration
{
    public interface IGeneratorConfigurationFactory
    {
        IEnumerable<ContractProjectGenerator> FromAbi(string contractName, string abiFilePath, string binFilePath, string baseNamespace, string outputFolder);
        IEnumerable<ContractProjectGenerator> FromProject(string destinationProjectFolderOrFileName, string assemblyName);
        IEnumerable<ContractProjectGenerator> FromTruffle(string directory, string outputFolder, string baseNamespace, CodeGenLanguage language);
    }
}
