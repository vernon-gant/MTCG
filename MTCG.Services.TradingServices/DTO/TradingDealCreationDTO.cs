using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MTCG.Services.TradingServices.DTO;

public class TradingDealCreationDTO
{

    [Required]
    [JsonPropertyName("id")]
    public Guid TradingDealId { get; set; }

    [Required]
    [JsonPropertyName("cardToTradeId")]
    public Guid OfferingUserCardId { get; set; }

    [Required]
    [RegularExpression(@"^(monster|spell)$")]
    [JsonPropertyName("type")]
    public string RequiredCardType { get; set; } = string.Empty;

    [Required]
    [Range(1, 100)]
    [JsonPropertyName("minimumDamage")]
    public int RequiredMinimumDamage { get; set; }

}