using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;

namespace Nethereum.Generators.Service
{
    public class ServiceCSharpTemplate : ClassTemplateBase<ServiceModel>
    {
        private FunctionServiceMethodCSharpTemplate _functionServiceMethodCSharpTemplate;
        public ServiceCSharpTemplate(ServiceModel model) : base(model)
        {
            _functionServiceMethodCSharpTemplate = new FunctionServiceMethodCSharpTemplate(model);
            ClassFileTemplate = new CSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            return
                $@"{SpaceUtils.OneTab}public partial class {Model.GetTypeName()}<TContractType> : I{Model.GetTypeName()}<TContractType>
{SpaceUtils.TwoTabs}where TContractType : Enum
{SpaceUtils.OneTab}{{
{SpaceUtils.TwoTabs}public IChainProvider<TContractType> ChainProvider {{ get; }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}(IChainProvider<TContractType> chainProvider)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}ChainProvider = chainProvider;
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}private ContractHandler InitializeContractHandler(long chainId, TContractType contractType)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractAddress = ChainProvider.ContractAddress(chainId, contractType);
{SpaceUtils.ThreeTabs}var web3 = ChainProvider.Web3(chainId);
{SpaceUtils.ThreeTabs}var contractHandler = web3.Eth.GetContractHandler(contractAddress);
{SpaceUtils.ThreeTabs}return contractHandler;
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{_functionServiceMethodCSharpTemplate.GenerateMethods()}
{SpaceUtils.OneTab}}}";
        }
    }
}
