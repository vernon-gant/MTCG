using System.ComponentModel.DataAnnotations;

namespace MTCG.Services.PackageServices.Dto;

public class CardPackageCreationDto
{

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = "";

    [Required]
    [MinLength(5)]
    [MaxLength(5)]
    public List<CardPackageItemWithId> Cards { get; set; } = new ();

}