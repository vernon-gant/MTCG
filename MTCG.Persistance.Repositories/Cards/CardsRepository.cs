using MTCG.Domain;

namespace MTCG.Persistance.Repositories;

public interface CardsRepository
{

    ValueTask<Card?> GetCardByUserCardIdAsync(Guid userCardId);

}