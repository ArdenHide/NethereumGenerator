using System;
using Nethereum.Web3;

namespace NethereumGenerators.Interfaces
{
    public interface IChainProvider
    {
        IWeb3 Web3(long chainId);
        string ContractAddress(long chainId, Enum contractType);
    }
}
