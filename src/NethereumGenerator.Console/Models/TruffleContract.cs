using Newtonsoft.Json.Linq;

namespace NethereumGenerator.Console.Models
{
    public class TruffleContract
    {
        public string ContractName { get; set; }

        public string Bytecode { get; set; }

        public JToken Abi { get; set; }

        public ContractDefinition GetContractConfiguration()
        {
            return new ContractDefinition(Abi.ToString())
            {
                ContractName = ContractName,
                Bytecode = Bytecode
            };
        }
    }
}
