using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AmlApiMvc.Models
{
    public class AmlRequest
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; init; }
        [Required]
        public string AccessId { get; init; }
        [Required]
        public string Locale { get; init; }
        [Required]
        public string Hash { get; init; }

        [Required]
        public string Asset { get; init; }

        [Required]
        public string Token { get; init; }

        public int Expanded = 1;

        [ForeignKey(nameof(AmlResponse))]
        [JsonIgnore]
        public int AmlResponseId { get; set; }
        [JsonIgnore]
        public AmlResponse AmlResponse { get; set; } = null!;


        public AmlRequest(string accessId, string locale, string hash, string asset, string token)
        {
            AccessId = accessId ?? throw new ArgumentNullException(nameof(accessId));
            Locale = string.IsNullOrEmpty(locale) ? "en-US" : locale;
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            Asset = asset ?? throw new ArgumentNullException(nameof(asset));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
