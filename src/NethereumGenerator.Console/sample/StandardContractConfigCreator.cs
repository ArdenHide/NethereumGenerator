using System.Collections.Generic;
using NethereumGenerator.Console.Configuration;

namespace NethereumGenerator.Console.sample
{
    public class StandardContractConfigCreator
    {
        public static void CreateTestGeneratorConfigFile(string outputFilePath)
        {
            var config = new ABICollectionConfiguration
            {
                ABIConfigurations = new List<ABIConfiguration>
                {
                    new ABIConfiguration
                    {
                        ContractName = "StandardContract",
                        ABIFile = "StandardContract.abi",
                        BinFile = "StandardContract.bin"
                    }
                }
            };

            config.SaveToJson(outputFilePath);
        }
    }
}
