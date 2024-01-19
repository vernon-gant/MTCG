using System.Text.Json.Serialization;

using MTCG.Services.UserService.ViewModels;

namespace MTCG.Services.DeckServices.ViewModels;

public class DeckViewModel
{

    [JsonPropertyName("id")]
    public int DeckId { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public List<UserCardViewModel> Cards { get; set; } = new ();

}