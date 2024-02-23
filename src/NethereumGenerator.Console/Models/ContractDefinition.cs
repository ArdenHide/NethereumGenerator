using Nethereum.Generators.Model;
using Nethereum.Generators.Net;

namespace NethereumGenerator.ExtendedConsole.Models
{
    public class ContractDefinition
    {
        public string ContractName { get; set; }

        public ContractABI Abi { get; set; }

        public string Bytecode { get; set; }

        public ContractDefinition(string abi)
        {
            Abi = new GeneratorModelABIDeserialiser().DeserialiseABI(abi);
        }
    }
}
