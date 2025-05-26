using System;
using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Service;

namespace NethereumGenerators.Deploy.CSharp
{
    public class ContractDeploymentServiceMethodsCSharpTemplate
    {
        private ContractDeploymentCQSMessageModel _contractDeploymentCQSMessageModel;
        private static readonly string SpaceFollowingFunction = (Environment.NewLine + Environment.NewLine);

        public ContractDeploymentServiceMethodsCSharpTemplate(DeployModel model)
        {
            _contractDeploymentCQSMessageModel = model.ContractDeploymentCQSMessageModel;
        }

        public string GenerateMethods()
        {
            var messageType = _contractDeploymentCQSMessageModel.GetTypeName();
            var messageVariableName =_contractDeploymentCQSMessageModel.GetVariableName();

            var sendRequestReceipt =
                $@"{SpaceUtils.TwoTabs}public virtual Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(long chainId, {messageType} {messageVariableName}, CancellationTokenSource cancellationTokenSource = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var web3 = ChainProvider.Web3(chainId);
{SpaceUtils.ThreeTabs}return web3.Eth.GetContractDeploymentHandler<{messageType}>().SendRequestAndWaitForReceiptAsync({messageVariableName}, cancellationTokenSource);
{SpaceUtils.TwoTabs}}}";

            var sendRequest =
                $@"{SpaceUtils.TwoTabs}public virtual Task<string> DeployContractAsync(long chainId, {messageType} {messageVariableName})
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var web3 = ChainProvider.Web3(chainId);
{SpaceUtils.ThreeTabs}return web3.Eth.GetContractDeploymentHandler<{messageType}>().SendRequestAsync({messageVariableName});
{SpaceUtils.TwoTabs}}}";

            return string.Join(SpaceFollowingFunction, sendRequestReceipt, sendRequest);
        }
    }
}
