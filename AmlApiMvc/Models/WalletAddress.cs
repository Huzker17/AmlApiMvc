using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmlApiMvc.Models
{
    public class WalletAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        [Required]
        public string Address { get; init; }

        [Required]
        public string NetworkType { get; init; }

        public WalletAddress(string? address, string networkType)
        {
            if (CheckAddressAndType(address, networkType))
            {
                Address = address ?? throw new ArgumentNullException(nameof(address));
                NetworkType = networkType ?? throw new ArgumentNullException(nameof(networkType));
            }

            throw new Exception();
        }

        public bool CheckAddressAndType(string? address, string type)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            return true;
        }
    }
}
