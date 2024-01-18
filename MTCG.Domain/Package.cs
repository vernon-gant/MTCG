namespace MTCG.Domain;

public class Package
{
    public int PackageId { get; set; }

    public string Name { get; set; } = "";

    public int CreatedById { get; set; }

    public string CreatedBy { get; set; } = "";

    public List<Card> Cards { get; set; } = new ();

}