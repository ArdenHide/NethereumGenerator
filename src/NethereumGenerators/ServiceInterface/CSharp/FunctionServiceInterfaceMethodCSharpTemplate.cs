using System;
using System.Linq;
using Nethereum.Generators.CQS;
using Nethereum.Generators.Core;
using Nethereum.Generators.DTOs;
using Nethereum.Generators.Model;

namespace Nethereum.Generators.ServiceInterface.CSharp
{
    public class FunctionServiceInterfaceMethodCSharpTemplate
    {
        private readonly ServiceInterfaceModel _model;
        private CommonGenerators _commonGenerators;
        private ITypeConvertor _typeConvertor;
        private ParameterABIFunctionDTOCSharpTemplate _parameterAbiFunctionDtocSharpTemplate;

        public FunctionServiceInterfaceMethodCSharpTemplate(ServiceInterfaceModel model)
        {
            _model = model;
            _typeConvertor = new ABITypeToCSharpType();
            _commonGenerators = new CommonGenerators();
            _parameterAbiFunctionDtocSharpTemplate = new ParameterABIFunctionDTOCSharpTemplate();
        }

        public string GenerateMethods()
        {
            var functions = _model.ContractABI.Functions;
            var methods = string.Join(GenerateLineBreak(), functions.Select(GenerateMethod));
            methods += $"{GenerateLineBreak()}{GenerateInitializeMethod()}";
            return methods;
        }

        private string GenerateInitializeMethod() => $@"{SpaceUtils.TwoTabs}public void Initialize(IWeb3 web3, string contractAddress);";

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
$@"{SpaceUtils.TwoTabs}public Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync({messageType} {messageVariableName}, BlockParameter blockParameter = null);";

                var returnWithoutInputParam =
$@"{SpaceUtils.TwoTabs}public Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(BlockParameter blockParameter = null);";

                var returnWithSimpleParams =
$@"{SpaceUtils.TwoTabs}public Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync({_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, BlockParameter blockParameter = null);";

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
                        $@"{SpaceUtils.TwoTabs}public Task<{type}> {functionNameUpper}QueryAsync({messageType} {messageVariableName}, BlockParameter blockParameter = null);";

                    var returnWithoutInputParam =
                        $@"{SpaceUtils.TwoTabs}public Task<{type}> {functionNameUpper}QueryAsync(BlockParameter blockParameter = null);";

                    var returnWithSimpleParams =
                        $@"{SpaceUtils.TwoTabs}public Task<{type}> {functionNameUpper}QueryAsync({_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, BlockParameter blockParameter = null);";

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
                    $@"{SpaceUtils.TwoTabs}public Task<string> {functionNameUpper}RequestAsync({messageType} {messageVariableName});";

                var transactionRequestWithoutInput =
                    $@"{SpaceUtils.TwoTabs}public Task<string> {functionNameUpper}RequestAsync();";

                var transactionRequestWithSimpleParams =
                    $@"{SpaceUtils.TwoTabs}public Task<string> {functionNameUpper}RequestAsync({_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)});";

                var transactionRequestAndReceiptWithInput =
                    $@"{SpaceUtils.TwoTabs}public Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync({messageType} {messageVariableName}, CancellationTokenSource cancellationToken = null);";

                var transactionRequestAndReceiptWithoutInput =
                    $@"{SpaceUtils.TwoTabs}public Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null);";

                var transactionRequestAndReceiptWithSimpleParams =
                    $@"{SpaceUtils.TwoTabs}public Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync({_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, CancellationTokenSource cancellationToken = null);";

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
