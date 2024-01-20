using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MTCG.Services.TradingServices.DTO;

public class TradingDealCarryOutDTO
{

    [Required]
    [JsonPropertyName("cardId")]
    public Guid RespondingUserCardId { get; set; }

}