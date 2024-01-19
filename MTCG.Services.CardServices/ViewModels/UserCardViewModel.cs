using System.Text.Json.Serialization;

namespace MTCG.Services.UserService.ViewModels;

public class UserCardViewModel
{

    [JsonPropertyName("id")]
    public string UserCardId { get; set; }

    public string Name { get; set; }

    public int Damage { get; set; }

}