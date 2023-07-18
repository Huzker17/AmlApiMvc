using AmlApiMvc.Models;

namespace AmlApiMvc.Interfaces
{
    public interface IAmlService
    {
        Task<AmlResponse> SendToAmlAsync(WalletAddress walletAddress);
        Task<AmlResponse> ResendToAmlApi(AmlReCheckRequest reCheckRequest);
        Task<IEnumerable<NetworkType>> GetNetworkTypesAsync();
    }
}
