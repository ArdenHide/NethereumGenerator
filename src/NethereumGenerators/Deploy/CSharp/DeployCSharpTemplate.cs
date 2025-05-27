using Nethereum.Generators.CQS;
using Nethereum.Generators.Core;

namespace NethereumGenerators.Deploy.CSharp
{
    public class DeployCSharpTemplate : ClassTemplateBase<DeployModel>
    {
        private ContractDeploymentServiceMethodsCSharpTemplate _deploymentServiceMethodsCSharpTemplate;
        public DeployCSharpTemplate(DeployModel model) : base(model)
        {
            _deploymentServiceMethodsCSharpTemplate = new ContractDeploymentServiceMethodsCSharpTemplate(model);
            ClassFileTemplate = new CSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            return
                $@"{SpaceUtils.OneTab}public partial class {Model.GetTypeName()}<TContractType>
{SpaceUtils.TwoTabs}where TContractType : Enum
{SpaceUtils.OneTab}{{
{SpaceUtils.TwoTabs}public IChainProvider<TContractType> ChainProvider {{ get; }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}(IChainProvider<TContractType> chainProvider)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}ChainProvider = chainProvider;
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{_deploymentServiceMethodsCSharpTemplate.GenerateMethods()}
{SpaceUtils.OneTab}}}";
        }
    }
}
