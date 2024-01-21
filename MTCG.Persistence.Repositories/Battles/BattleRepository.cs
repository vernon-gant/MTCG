using MTCG.Domain;

namespace MTCG.Persistence.Repositories.Battles;

public interface BattleRepository
{

    Task SaveBattleResult(BattleResult battleResult);

}