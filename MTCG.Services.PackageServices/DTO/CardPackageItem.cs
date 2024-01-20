using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MTCG.Services.PackageServices.Dto;

public class CardPackageItem
{

    [JsonPropertyName("id")]
    public Guid UserCardId { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    [Range(1, 100)]
    public int Damage { get; set; }

}