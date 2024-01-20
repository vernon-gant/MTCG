using MTCG.Services.TradingServices.DTO;
using MTCG.Services.TradingServices.ViewModels;

namespace MTCG.Services.TradingServices.Services;

public interface TradingService
{

    ValueTask<List<TradingDealViewModel>> GetAvailableTradingDealsAsync();

    Task CreateTradingDealAsync(TradingDealCreationDTO tradingDealCreationDTO, string userName);

    Task CarryOutTradingDealAsync(string userName, Guid tradingDealId, Guid respondingUserCardId);

    Task DeleteTradingDealAsync(string userName, Guid tradingDealId);

}