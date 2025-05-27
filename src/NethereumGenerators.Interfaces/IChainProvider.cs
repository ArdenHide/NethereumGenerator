using System;
using Nethereum.Web3;

namespace NethereumGenerators.Interfaces
{
    public interface IChainProvider<in TContractType>
        where TContractType : Enum
    {
        IWeb3 Web3(long chainId);
        string ContractAddress(long chainId, TContractType contractType);
    }
}
