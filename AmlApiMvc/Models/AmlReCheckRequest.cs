using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmlApiMvc.Models
{
    public class AmlRecheckRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        [Required]
        public string AccesId { get; init; }
        [Required]
        public string Uid { get; init; }

        [Required]
        public string Token { get; init; }

        [ForeignKey(nameof(AmlRequest))]
        [JsonIgnore]
        public Guid AmlRequestId { get; set; }
        [JsonIgnore]
        public AmlRequest AmlRequest { get; set; } = null!;

        public AmlRecheckRequest(string accesId, string uid, string token)
        {
            AccesId = accesId ?? throw new ArgumentNullException(nameof(accesId));
            Uid = uid ?? throw new ArgumentNullException(nameof(uid));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

    }
}
