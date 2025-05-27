using System;
using System.Linq;
using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;
using Nethereum.Generators.DTOs;
using Nethereum.Generators.Model;

namespace Nethereum.Generators.Service
{
    public class FunctionServiceMethodCSharpTemplate
    {
        private readonly ServiceModel _model;
        private CommonGenerators _commonGenerators;
        private ITypeConvertor _typeConvertor;
        private ParameterABIFunctionDTOCSharpTemplate _parameterAbiFunctionDtocSharpTemplate;

        public FunctionServiceMethodCSharpTemplate(ServiceModel model)
        {
            _model = model;
            _typeConvertor = new ABITypeToCSharpType();
            _commonGenerators = new CommonGenerators();
            _parameterAbiFunctionDtocSharpTemplate = new ParameterABIFunctionDTOCSharpTemplate();
        }

        public string GenerateMethods()
        {
            var functions = _model.ContractABI.Functions;
            return string.Join(GenerateLineBreak(), functions.Select(GenerateMethod));
        }

        public string GenerateMethod(FunctionABI functionABI)
        {
            var functionCQSMessageModel = new FunctionCQSMessageModel(functionABI, _model.CQSNamespace);
            var functionOutputDTOModel = new FunctionOutputDTOModel(functionABI, _model.FunctionOutputNamespace);
            var functionABIModel = new FunctionABIModel(functionABI, _typeConvertor, CodeGenLanguage.CSharp);

            var messageType = functionCQSMessageModel.GetTypeName();
            var messageVariableName = functionCQSMessageModel.GetVariableName();
            var functionNameUpper = _commonGenerators.GenerateClassName(functionABI.Name);

            if (functionABIModel.IsMultipleOutput() && !functionABIModel.IsTransaction())
            {
                var functionOutputDTOType = functionOutputDTOModel.GetTypeName();

                var returnWithInputParam =
$@"{SpaceUtils.TwoTabs}public virtual Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(long chainId, TContractType contractType, {messageType} {messageVariableName}, BlockParameter blockParameter = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.QueryDeserializingToObjectAsync<{messageType}, {functionOutputDTOType}>({messageVariableName}, blockParameter);
{SpaceUtils.TwoTabs}}}";

                var returnWithoutInputParam =
$@"{SpaceUtils.TwoTabs}public virtual Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(long chainId, TContractType contractType, BlockParameter blockParameter = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.QueryDeserializingToObjectAsync<{messageType}, {functionOutputDTOType}>(null, blockParameter);
{SpaceUtils.TwoTabs}}}";

                var returnWithSimpleParams =
$@"{SpaceUtils.TwoTabs}public virtual Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(long chainId, TContractType contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, BlockParameter blockParameter = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var {messageVariableName} = new {messageType}();
{_parameterAbiFunctionDtocSharpTemplate.GenerateAssigmentFunctionParametersToProperties(functionABIModel.FunctionABI.InputParameters, messageVariableName, SpaceUtils.FourTabs)}
{SpaceUtils.ThreeTabs}
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.QueryDeserializingToObjectAsync<{messageType}, {functionOutputDTOType}>({messageVariableName}, blockParameter);
{SpaceUtils.TwoTabs}}}";

                if (functionABIModel.HasNoInputParameters())
                {
                    return returnWithInputParam + GenerateLineBreak() + returnWithoutInputParam;
                }
                else
                {
                    return returnWithInputParam + GenerateLineBreak() + returnWithSimpleParams;
                }
            }

            if (functionABIModel.IsSingleOutput() && !functionABIModel.IsTransaction())
            {
                if (functionABI.OutputParameters != null && functionABI.OutputParameters.Length == 1 && functionABI.Constant)
                {
                    var type = functionABIModel.GetSingleOutputReturnType();

                    var returnWithInputParam =
                        $@"{SpaceUtils.TwoTabs}public virtual Task<{type}> {functionNameUpper}QueryAsync(long chainId, TContractType contractType, {messageType} {messageVariableName}, BlockParameter blockParameter = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.QueryAsync<{messageType}, {type}>({messageVariableName}, blockParameter);
{SpaceUtils.TwoTabs}}}";

                    var returnWithoutInputParam =
                        $@"{SpaceUtils.TwoTabs}public virtual Task<{type}> {functionNameUpper}QueryAsync(long chainId, TContractType contractType, BlockParameter blockParameter = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.QueryAsync<{messageType}, {type}>(null, blockParameter);
{SpaceUtils.TwoTabs}}}";

                    var returnWithSimpleParams =
                        $@"{SpaceUtils.TwoTabs}public virtual Task<{type}> {functionNameUpper}QueryAsync(long chainId, TContractType contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, BlockParameter blockParameter = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var {messageVariableName} = new {messageType}();
{_parameterAbiFunctionDtocSharpTemplate.GenerateAssigmentFunctionParametersToProperties(functionABIModel.FunctionABI.InputParameters, messageVariableName, SpaceUtils.FourTabs)}
{SpaceUtils.ThreeTabs}
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.QueryAsync<{messageType}, {type}>({messageVariableName}, blockParameter);
{SpaceUtils.TwoTabs}}}";

                    if (functionABIModel.HasNoInputParameters())
                    {
                        return returnWithInputParam + GenerateLineBreak() + returnWithoutInputParam;
                    }
                    else
                    {
                        return returnWithInputParam + GenerateLineBreak() + returnWithSimpleParams;
                    }
                }
            }

            if (functionABIModel.IsTransaction())
            {
                var transactionRequestWithInput =
                    $@"{SpaceUtils.TwoTabs}public virtual Task<string> {functionNameUpper}RequestAsync(long chainId, TContractType contractType, {messageType} {messageVariableName})
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.SendRequestAsync({messageVariableName});
{SpaceUtils.TwoTabs}}}";

                var transactionRequestWithoutInput =
                    $@"{SpaceUtils.TwoTabs}public virtual Task<string> {functionNameUpper}RequestAsync(long chainId, TContractType contractType)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.SendRequestAsync<{messageType}>();
{SpaceUtils.TwoTabs}}}";

                var transactionRequestWithSimpleParams =
                    $@"{SpaceUtils.TwoTabs}public virtual Task<string> {functionNameUpper}RequestAsync(long chainId, TContractType contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)})
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var {messageVariableName} = new {messageType}();
{_parameterAbiFunctionDtocSharpTemplate.GenerateAssigmentFunctionParametersToProperties(functionABIModel.FunctionABI.InputParameters, messageVariableName, SpaceUtils.FourTabs)}
{SpaceUtils.ThreeTabs}
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.SendRequestAsync({messageVariableName});
{SpaceUtils.TwoTabs}}}";

                var transactionRequestAndReceiptWithInput =
                    $@"{SpaceUtils.TwoTabs}public virtual Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(long chainId, TContractType contractType, {messageType} {messageVariableName}, CancellationTokenSource cancellationToken = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.SendRequestAndWaitForReceiptAsync({messageVariableName}, cancellationToken);
{SpaceUtils.TwoTabs}}}";

                var transactionRequestAndReceiptWithoutInput =
                    $@"{SpaceUtils.TwoTabs}public virtual Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(long chainId, TContractType contractType, CancellationTokenSource cancellationToken = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.SendRequestAndWaitForReceiptAsync<{messageType}>(null, cancellationToken);
{SpaceUtils.TwoTabs}}}";

                var transactionRequestAndReceiptWithSimpleParams =
                    $@"{SpaceUtils.TwoTabs}public virtual Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(long chainId, TContractType contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, CancellationTokenSource cancellationToken = null)
{SpaceUtils.TwoTabs}{{
{SpaceUtils.ThreeTabs}var {messageVariableName} = new {messageType}();
{_parameterAbiFunctionDtocSharpTemplate.GenerateAssigmentFunctionParametersToProperties(functionABIModel.FunctionABI.InputParameters, messageVariableName, SpaceUtils.FourTabs)}
{SpaceUtils.ThreeTabs}
{SpaceUtils.ThreeTabs}var contractHandler = InitializeContractHandler(chainId, contractType);
{SpaceUtils.ThreeTabs}return contractHandler.SendRequestAndWaitForReceiptAsync({messageVariableName}, cancellationToken);
{SpaceUtils.TwoTabs}}}";

                if (functionABIModel.HasNoInputParameters())
                {
                    return transactionRequestWithInput + GenerateLineBreak()
                           + transactionRequestWithoutInput +
                           GenerateLineBreak() +
                           transactionRequestAndReceiptWithInput +
                           GenerateLineBreak() +
                           transactionRequestAndReceiptWithoutInput;
                }

                return transactionRequestWithInput + GenerateLineBreak() + transactionRequestAndReceiptWithInput +
                       GenerateLineBreak() +
                       transactionRequestWithSimpleParams +
                       GenerateLineBreak() +
                       transactionRequestAndReceiptWithSimpleParams;

            }

            return null;
        }

        private string GenerateLineBreak()
        {
            return Environment.NewLine + Environment.NewLine;
        }
    }
}
