using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Model;

namespace NethereumGenerators.Deploy
{
    public class DeployModel : TypeMessageModel
    {
        public ContractABI ContractABI { get; }
        public string CQSNamespace { get; }
        public string FunctionOutputNamespace { get; }
        public ContractDeploymentCQSMessageModel ContractDeploymentCQSMessageModel { get; }

        public DeployModel(ContractABI contractABI, string contractName,
                            string byteCode, string @namespace,
                            string cqsNamespace, string functionOutputNamespace) :
            base(@namespace, contractName, "DeployingService")
        {
            ContractABI = contractABI;
            CQSNamespace = cqsNamespace;
            FunctionOutputNamespace = functionOutputNamespace;
            ContractDeploymentCQSMessageModel = new ContractDeploymentCQSMessageModel(contractABI.Constructor, cqsNamespace, byteCode, contractName);
            InitialiseNamespaceDependencies();

            if (!string.IsNullOrEmpty(cqsNamespace))
                NamespaceDependencies.Add(cqsNamespace);

            if (!string.IsNullOrEmpty(functionOutputNamespace))
                NamespaceDependencies.Add(functionOutputNamespace);
        }

        private void InitialiseNamespaceDependencies()
        {
            NamespaceDependencies.AddRange(new[] {
                "System",
                "System.Threading.Tasks",
                "System.Collections.Generic",
                "System.Numerics",
                "Nethereum.Hex.HexTypes",
                "Nethereum.ABI.FunctionEncoding.Attributes",
                "Nethereum.Web3",
                "Nethereum.RPC.Eth.DTOs",
                "Nethereum.Contracts.CQS",
                "Nethereum.Contracts.ContractHandlers",
                "Nethereum.Contracts",
                "System.Threading" });
        }
    }
}
