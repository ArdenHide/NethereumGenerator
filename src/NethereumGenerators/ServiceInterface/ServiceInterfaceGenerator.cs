using System;
using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Model;
using Nethereum.Generators.ServiceInterface.CSharp;

namespace Nethereum.Generators.ServiceInterface
{
    public class ServiceInterfaceGenerator : ClassGeneratorBase<ClassTemplateBase<ServiceInterfaceModel>, ServiceInterfaceModel>
    {
        public  ContractABI ContractABI { get; }

        public ServiceInterfaceGenerator(ContractABI contractABI, string contractName, string byteCode, string @namespace, string cqsNamespace, string functionOutputNamespace, CodeGenLanguage codeGenLanguage)
        {
            ContractABI = contractABI;
            ClassModel = new ServiceInterfaceModel(contractABI, contractName, byteCode, @namespace, cqsNamespace, functionOutputNamespace);
            ClassModel.CodeGenLanguage = codeGenLanguage;
            InitialiseTemplate(codeGenLanguage);
        }

        public void InitialiseTemplate(CodeGenLanguage codeGenLanguage)
        {
            switch (codeGenLanguage)
            {
                case CodeGenLanguage.CSharp:
                    ClassTemplate = new ServiceInterfaceCSharpTemplate(ClassModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codeGenLanguage), codeGenLanguage, "Code generation not implemented for this language");
            }

        }
    }
}