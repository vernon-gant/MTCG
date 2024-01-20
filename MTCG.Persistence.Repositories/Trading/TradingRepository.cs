using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Trading;

public interface TradingRepository
{

    ValueTask<TradingDeal?> GetByIdAsync(Guid tradingDealId);

    ValueTask<List<TradingDeal>> GetAvailableAsync();

    Task CreateAsync(TradingDeal tradingDeal);

    Task CarryOutAsync(TradingDeal tradingDeal);

    Task DeleteAsync(Guid tradingDealId);

}