namespace MTCG.Services.TradingServices.ViewModels;

public class TradingDealViewModel
{

    public Guid TradingDealId { get; set; }

    public string OfferingUserName { get; set; } = string.Empty;

    public string OfferingCardName { get; set; } = string.Empty;

    public int OfferingCardDamage { get; set; }

    public string RequiredCardType { get; set; } = string.Empty;

    public int RequiredMinimumDamage { get; set; }

}