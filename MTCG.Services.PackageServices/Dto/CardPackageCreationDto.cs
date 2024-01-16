using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MTCG.Services.PackageServices.Dto;

public class CardPackageCreationDto
{

    [Required]
    public string Name { get; set; } = "";

    [Required]
    [MinLength(5)]
    [MaxLength(5)]
    public List<CardPackageItem> Cards { get; set; } = new ();

    public class CardPackageItem
    {

        [JsonPropertyName("id")]
        public Guid UserCardId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "";

        public int Damage { get; set; }

    }

}