using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MTCG.Services.DeckServices.Dto;

public class DeckCreationDTO
{

    [MaxLength(50)]
    public string? Description { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(4)]
    [JsonPropertyName("cardIds")]
    public required Guid[] ProvidedUserCardIds { get; set; }

}