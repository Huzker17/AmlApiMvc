using System.ComponentModel.DataAnnotations;

namespace AmlApiMvc.Models
{
    public class AmlRequest
    {
        [Key]
        public Guid Id { get; init; }
        [Required]
        public string AccessId { get; init; }
        [Required]
        public string Locale { get; init; } = "en-US";
        [Required]
        public string Hash { get; init; }

        [Required]
        public string Asset { get; init; }

        [Required]
        public string Token { get; init; }

        public int Expanded = 1;



        public AmlRequest(string accessId, string locale, string hash, string asset, string token)
        {
            AccessId = accessId ?? throw new ArgumentNullException(nameof(accessId));
            Locale = locale;
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            Asset = asset ?? throw new ArgumentNullException(nameof(asset));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
