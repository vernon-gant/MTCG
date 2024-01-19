using System;

namespace MTCG.Domain;

public class TradingDeal
{

    public Guid TradingDealId { get; set; }

    public int OfferingUserId { get; set; }

    public User? OfferingUser { get; set; }

    public Guid OfferingUserCardId { get; set; }

    public Card? OfferingUserCard { get; set; }

    public int RespondingUserId { get; set; }

    public User? RespondingUser { get; set; }

    public Guid RespondingUserCardId { get; set; }

    public Card? RespondingUserCard { get; set; }

    public string RequiredCardType { get; set; } = string.Empty;

    public int RequiredMinimumDamage { get; set; }

}