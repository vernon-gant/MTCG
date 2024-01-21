using MTCG.Services.BattleServices.ViewModels;

namespace MTCG.Services.BattleServices.Services;

public interface BattleService
{

    ValueTask<BattleResultViewModel> BattleAsync(string userName);

}