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
            return methods;
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
$@"{SpaceUtils.TwoTabs}Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(long chainId, Enum contractType, {messageType} {messageVariableName}, BlockParameter blockParameter = null);";

                var returnWithoutInputParam =
$@"{SpaceUtils.TwoTabs}Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(long chainId, Enum contractType, BlockParameter blockParameter = null);";

                var returnWithSimpleParams =
$@"{SpaceUtils.TwoTabs}Task<{functionOutputDTOType}> {functionNameUpper}QueryAsync(long chainId, Enum contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, BlockParameter blockParameter = null);";

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
                        $@"{SpaceUtils.TwoTabs}Task<{type}> {functionNameUpper}QueryAsync(long chainId, Enum contractType, {messageType} {messageVariableName}, BlockParameter blockParameter = null);";

                    var returnWithoutInputParam =
                        $@"{SpaceUtils.TwoTabs}Task<{type}> {functionNameUpper}QueryAsync(long chainId, Enum contractType, BlockParameter blockParameter = null);";

                    var returnWithSimpleParams =
                        $@"{SpaceUtils.TwoTabs}Task<{type}> {functionNameUpper}QueryAsync(long chainId, Enum contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, BlockParameter blockParameter = null);";

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
                    $@"{SpaceUtils.TwoTabs}Task<string> {functionNameUpper}RequestAsync(long chainId, Enum contractType, {messageType} {messageVariableName});";

                var transactionRequestWithoutInput =
                    $@"{SpaceUtils.TwoTabs}Task<string> {functionNameUpper}RequestAsync(long chainId, Enum contractType);";

                var transactionRequestWithSimpleParams =
                    $@"{SpaceUtils.TwoTabs}Task<string> {functionNameUpper}RequestAsync(long chainId, Enum contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)});";

                var transactionRequestAndReceiptWithInput =
                    $@"{SpaceUtils.TwoTabs}Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(long chainId, Enum contractType, {messageType} {messageVariableName}, CancellationTokenSource cancellationToken = null);";

                var transactionRequestAndReceiptWithoutInput =
                    $@"{SpaceUtils.TwoTabs}Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(long chainId, Enum contractType, CancellationTokenSource cancellationToken = null);";

                var transactionRequestAndReceiptWithSimpleParams =
                    $@"{SpaceUtils.TwoTabs}Task<TransactionReceipt> {functionNameUpper}RequestAndWaitForReceiptAsync(long chainId, Enum contractType, {_parameterAbiFunctionDtocSharpTemplate.GenerateAllFunctionParameters(functionABIModel.FunctionABI.InputParameters)}, CancellationTokenSource cancellationToken = null);";

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
