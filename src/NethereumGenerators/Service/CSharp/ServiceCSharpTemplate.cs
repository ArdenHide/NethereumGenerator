using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;

namespace Nethereum.Generators.Service
{
    public class ServiceCSharpTemplate: ClassTemplateBase<ServiceModel>
    {
        private FunctionServiceMethodCSharpTemplate _functionServiceMethodCSharpTemplate;
        public ServiceCSharpTemplate(ServiceModel model):base(model)
        {
            _functionServiceMethodCSharpTemplate = new FunctionServiceMethodCSharpTemplate(model);
            ClassFileTemplate = new CSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            return
                $@"{SpaceUtils.OneTab}public partial class {Model.GetTypeName()}
{SpaceUtils.OneTab}{{
{SpaceUtils.TwoTabs}protected virtual Nethereum.Web3.IWeb3 Web3 {{ get; }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public virtual ContractHandler ContractHandler {{ get; }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}(Nethereum.Web3.Web3 web3, string contractAddress)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}Web3 = web3;
{SpaceUtils.ThreeTabs}ContractHandler = web3.Eth.GetContractHandler(contractAddress);
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}(Nethereum.Web3.IWeb3 web3, string contractAddress)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}Web3 = web3;
{SpaceUtils.ThreeTabs}ContractHandler = web3.Eth.GetContractHandler(contractAddress);
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{_functionServiceMethodCSharpTemplate.GenerateMethods()}
{SpaceUtils.OneTab}}}";
        }
    }
}