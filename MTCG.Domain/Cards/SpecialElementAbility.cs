using MTCG.Domain.Elements;

namespace MTCG.Domain.Cards;

public interface SpecialElementAbility
{

    void Activate(Element enemyElement);

}