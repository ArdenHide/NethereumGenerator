using System;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Core;
using Nethereum.Generators.Model;
using NethereumGenerators.Deploy.CSharp;

namespace NethereumGenerators.Deploy
{
    public class DeployGenerator : ClassGeneratorBase<ClassTemplateBase<DeployModel>, DeployModel>
    {
        public ContractABI ContractABI { get; }

        public DeployGenerator(ContractABI contractABI, string contractName, string byteCode, string @namespace, string cqsNamespace, string functionOutputNamespace, CodeGenLanguage codeGenLanguage)
        {
            ContractABI = contractABI;
            ClassModel = new DeployModel(contractABI, contractName, byteCode, @namespace, cqsNamespace, functionOutputNamespace);
            ClassModel.CodeGenLanguage = codeGenLanguage;
            InitialiseTemplate(codeGenLanguage);
        }

        public void InitialiseTemplate(CodeGenLanguage codeGenLanguage)
        {
            switch (codeGenLanguage)
            {
                case CodeGenLanguage.CSharp:
                    ClassTemplate = new DeployCSharpTemplate(ClassModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codeGenLanguage), codeGenLanguage, "Code generation not implemented for this language");
            }

        }
    }
}
