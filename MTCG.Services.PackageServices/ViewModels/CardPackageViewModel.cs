using MTCG.Services.PackageServices.Dto;

namespace MTCG.Services.PackageServices.ViewModels;

public class CardPackageViewModel
{

    public string Name { get; set; } = "";

    public List<CardPackageItem> Cards { get; set; } = new ();

}