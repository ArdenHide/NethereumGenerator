using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;

namespace Nethereum.Generators.ServiceInterface.CSharp
{
    public class ServiceInterfaceCSharpTemplate : ClassTemplateBase<ServiceInterfaceModel>
    {
        private FunctionServiceInterfaceMethodCSharpTemplate _functionServiceMethodCSharpTemplate;
        public ServiceInterfaceCSharpTemplate(ServiceInterfaceModel model) : base(model)
        {
            _functionServiceMethodCSharpTemplate = new FunctionServiceInterfaceMethodCSharpTemplate(model);
            ClassFileTemplate = new CSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            return
                $@"{SpaceUtils.OneTab}public interface {Model.GetTypeName()}<in TContractType>
{SpaceUtils.TwoTabs}where TContractType : Enum
{SpaceUtils.OneTab}{{
{_functionServiceMethodCSharpTemplate.GenerateMethods()}
{SpaceUtils.OneTab}}}";
        }
    }
}
