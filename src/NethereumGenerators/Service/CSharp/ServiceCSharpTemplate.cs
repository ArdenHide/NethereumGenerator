using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;

namespace Nethereum.Generators.Service
{
    public class ServiceCSharpTemplate : ClassTemplateBase<ServiceModel>
    {
        private readonly FunctionServiceMethodCSharpTemplate _functionServiceMethodCSharpTemplate;
        public ServiceCSharpTemplate(ServiceModel model) : base(model)
        {
            _functionServiceMethodCSharpTemplate = new FunctionServiceMethodCSharpTemplate(model);
            ClassFileTemplate = new CSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            return
                $@"{SpaceUtils.OneTab}public partial class {Model.GetTypeName()}
{SpaceUtils.OneTab}{{
{SpaceUtils.TwoTabs}protected virtual IWeb3 Web3 {{ get; private set; }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public virtual ContractHandler ContractHandler {{ get; private set; }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}() {{ }}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}(Web3 web3, string contractAddress)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}Initialize(web3, contractAddress);
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public {Model.GetTypeName()}(IWeb3 web3, string contractAddress)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}Initialize(web3, contractAddress);
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{SpaceUtils.TwoTabs}public void Initialize(IWeb3 web3, string contractAddress)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}Web3 = web3.ThrowIfNull();
{SpaceUtils.ThreeTabs}ContractHandler = web3.Eth.GetContractHandler(contractAddress).ThrowIfNull();
{SpaceUtils.TwoTabs}}}
{SpaceUtils.NoTabs}
{SpaceUtils.NoTabs}
{_functionServiceMethodCSharpTemplate.GenerateMethods()}
{SpaceUtils.OneTab}}}
{GenerateThrowIfNullMethod()}";
        }

        private string GenerateThrowIfNullMethod()
        {
            return
                $@"internal static class ObjectExtensions
{SpaceUtils.NoTabs}{{
{SpaceUtils.OneTab}[MethodImpl(MethodImplOptions.AggressiveInlining)]
{SpaceUtils.OneTab}internal static T ThrowIfNull<T>(
{SpaceUtils.OneTab}{SpaceUtils.TwoTabs}this T value,
{SpaceUtils.OneTab}{SpaceUtils.TwoTabs}[CallerMemberName] string memberName = """",
{SpaceUtils.OneTab}{SpaceUtils.TwoTabs}[CallerFilePath] string filePath = """",
{SpaceUtils.OneTab}{SpaceUtils.TwoTabs}[CallerLineNumber] int lineNumber = 0
{SpaceUtils.OneTab})
{SpaceUtils.OneTab}where T : class
{SpaceUtils.OneTab}{{
{SpaceUtils.TwoTabs}if (value == null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}throw new ArgumentNullException($""Value is null. Caller: {{memberName}}, File: {{filePath}}, Line: {{lineNumber}}"");
{SpaceUtils.TwoTabs}}}
{SpaceUtils.TwoTabs}return value;
{SpaceUtils.OneTab}}}
{SpaceUtils.NoTabs}}}";
        }
    }
}
