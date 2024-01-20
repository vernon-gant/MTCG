namespace MTCG.Services.StatisticsServices.ViewModels;

public class UserStatisticsViewModel
{

    public string UserName { get; set; } = string.Empty;

    public int ELO { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }

}