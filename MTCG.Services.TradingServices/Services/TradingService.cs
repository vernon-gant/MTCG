using MTCG.Services.TradingServices.DTO;
using MTCG.Services.TradingServices.ViewModels;

namespace MTCG.Services.TradingServices.Services;

public interface TradingService
{

    ValueTask<List<TradingDealViewModel>> GetAvailableTradingDealsAsync();

    Task CreateTradingDealAsync(TradingDealCreationDTO tradingDealCreationDTO, string userName);

}