using System;
using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Service;

namespace NethereumGenerators.Deploy.CSharp
{
    public class ContractDeploymentServiceMethodsCSharpTemplate
    {
        private ContractDeploymentCQSMessageModel _contractDeploymentCQSMessageModel;
        private DeployModel _serviceModel;
        private static readonly string SpaceFollowingFunction = (Environment.NewLine + Environment.NewLine);

        public ContractDeploymentServiceMethodsCSharpTemplate(DeployModel model)
        {
            _contractDeploymentCQSMessageModel = model.ContractDeploymentCQSMessageModel;
            _serviceModel = model;
        }

        public string GenerateMethods()
        {
            var messageType = _contractDeploymentCQSMessageModel.GetTypeName();
            var messageVariableName =_contractDeploymentCQSMessageModel.GetVariableName();

            var sendRequestReceipt =
                $@"{SpaceUtils.TwoTabs}public virtual Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.IWeb3 web3, {messageType} {messageVariableName}, CancellationTokenSource cancellationTokenSource = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}return web3.Eth.GetContractDeploymentHandler<{messageType}>().SendRequestAndWaitForReceiptAsync({messageVariableName}, cancellationTokenSource);
{SpaceUtils.TwoTabs}}}";

            var sendRequest =
                $@"{SpaceUtils.TwoTabs}public virtual Task<string> DeployContractAsync(Nethereum.Web3.IWeb3 web3, {messageType} {messageVariableName})
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}return web3.Eth.GetContractDeploymentHandler<{messageType}>().SendRequestAsync({messageVariableName});
{SpaceUtils.TwoTabs}}}";

            var contractServiceTypeName = _serviceModel.GetTypeName().Replace("Deploying", "");
            var sendRequestContract =
                $@"{SpaceUtils.TwoTabs}public virtual async Task<{contractServiceTypeName}> DeployContractAndGetServiceAsync(Nethereum.Web3.IWeb3 web3, {messageType} {messageVariableName}, CancellationTokenSource cancellationTokenSource = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var receipt = await DeployContractAndWaitForReceiptAsync(web3, {messageVariableName}, cancellationTokenSource);
{SpaceUtils.ThreeTabs}return new {contractServiceTypeName}(web3, receipt.ContractAddress);
{SpaceUtils.TwoTabs}}}";

            return string.Join(SpaceFollowingFunction, sendRequestReceipt, sendRequest, sendRequestContract);
        }
    }
}
