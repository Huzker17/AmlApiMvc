using AmlApiMvc.Models;

namespace AmlApiMvc.Interfaces
{
    public interface IAmlService
    {
        Task<AmlResponse> SendToAmlAsync(WalletAddress walletAddress);
        Task<AmlResponse> ResendToAmlApi(AmlRecheckRequest reCheckRequest);
        Task<IEnumerable<NetworkType>> GetNetworkTypesAsync();
        Task<IEnumerable<AmlResponse>> GetAmlResponsesAsync();
    }
}
