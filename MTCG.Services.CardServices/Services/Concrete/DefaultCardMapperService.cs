using System.Reflection;

using MTCG.Domain.Cards;
using MTCG.Domain.Elements;
using MTCG.Persistence.Repositories.Cards.Mappings;

namespace MTCG.Services.Cards.Services.Concrete;

public class DefaultCardMapperService : CardMapperService
{

    private readonly Dictionary<string, CardMapping> _cardMappings;

    private readonly Assembly _domainAssembly = Assembly.Load("MTCG.Domain");

    private readonly Dictionary<string, ElementMapping> _elementMappings;

    public DefaultCardMapperService(Dictionary<string, CardMapping> cardMappings, Dictionary<string, ElementMapping> elementMappings)
    {
        _cardMappings = cardMappings;
        _elementMappings = elementMappings;
    }

    public ValueTask<List<Card>> MapCardsAsync(List<Card> cards)
    {
        return ValueTask.FromResult(cards.Select(MapCardAsync).Select(cardTask => cardTask.Result).ToList());
    }

    private ValueTask<Card> MapCardAsync(Card genericCard)
    {
        if (!_cardMappings.TryGetValue(genericCard.Name, out CardMapping? cardMapping)) throw new Exception("Card mapping not found");

        Type? concreteCardType = _domainAssembly.GetType($"MTCG.Domain.Cards.{cardMapping.DomainClass}");

        if (concreteCardType == null) throw new Exception("Card type not found");

        ConstructorInfo? constructorInfo = concreteCardType.GetConstructor(new[] { typeof(Card) });

        if (constructorInfo == null) throw new Exception("Constructor not found");

        Card concreteCard = (Card)constructorInfo.Invoke(new object[] { genericCard });

        concreteCard.Element = MapElementAsync(genericCard.Element!).Result;

        return ValueTask.FromResult(concreteCard);
    }

    private ValueTask<Element> MapElementAsync(Element element)
    {
        if (!_elementMappings.TryGetValue(element.Name, out ElementMapping? elementMapping)) throw new Exception("Element mapping not found");

        Type? elementType = _domainAssembly.GetType($"MTCG.Domain.Elements.{elementMapping.DomainClass}");

        if (elementType == null) throw new Exception("Element type not found");

        ConstructorInfo? constructorInfo = elementType.GetConstructor(new[] { typeof(Element) });

        if (constructorInfo == null) throw new Exception("Constructor not found");

        Element concreteElement = (Element)constructorInfo.Invoke(new object[] { element });

        return ValueTask.FromResult(concreteElement);
    }

}