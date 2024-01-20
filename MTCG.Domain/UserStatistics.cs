namespace MTCG.Domain;

public class UserStatistics
{

    public string UserName { get; set; } = string.Empty;

    public int ELO { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }

    public int PointsWon { get; set; }

    public int PointsLost { get; set; }

    public int CardsTraded { get; set; }

}