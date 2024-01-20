namespace MTCG.Domain;

public class User
{

    public int UserId { get; set; }

    public string UserName { get; set; } = "";

    public string Password { get; set; } = "";

    public string Name { get; set; } = "";

    public string BIO { get; set; } = "";

    public string Image { get; set; } = "";

    public int ELO { get; set; }

    public int Coins { get; set; }

    public bool IsAdmin { get; set; }

}