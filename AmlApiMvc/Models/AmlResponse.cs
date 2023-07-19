using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmlApiMvc.Models
{
    public class AmlResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        [Required]

        public DateTime DateTime { get; init; }
        [Required]

        public string Address { get; init; }
        [Required]

        public string Network { get; init; }
        [Required]

        public double RiskScore { get; init; }
        [Required]

        public string XlsReport { get; init; }
        [Required]
        public string Uid { get; init; }
        [Required]
        public AmlResponseStatus Status { get; init; }

        [ForeignKey(nameof(AmlRequest))]
        [JsonIgnore]
        public Guid AmlRequestId { get; set; }
        [JsonIgnore]
        public AmlRequest AmlRequest { get; set; } = null!;

        public AmlResponse(DateTime dateTime, string address, string network, double riskScore, string xlsReport, string uid, AmlResponseStatus status)
        {
            DateTime = dateTime;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Network = network ?? throw new ArgumentNullException(nameof(network));
            RiskScore = riskScore;
            XlsReport = xlsReport ?? throw new ArgumentNullException(nameof(xlsReport));
            Uid = uid ?? throw new ArgumentNullException(nameof(uid));
            Status = status;
        }
    }
     public enum AmlResponseStatus
     {
         Success,
         Pending
     }
}
