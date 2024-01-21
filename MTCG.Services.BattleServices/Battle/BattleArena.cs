using MTCG.Domain;
using MTCG.Domain.Cards;

namespace MTCG.Services.BattleServices.Battle;

public class BattleArena
{

    private readonly BattleResultsStorage _battleResultsStorage;

    private const int ROUND_LIMIT = 100;

    public BattleArena(BattleResultsStorage battleResultsStorage)
    {
        _battleResultsStorage = battleResultsStorage;
    }

    public void Battle(BattleRequest battleRequest, BattleRequest opponentRequest)
    {
        BattleResult battleResult = new ()
        {
            PlayerOne = battleRequest.User,
            PlayerTwo = opponentRequest.User,
            PlayerOneCards = battleRequest.Deck.Cards,
            PlayerTwoCards = opponentRequest.Deck.Cards,
        };

        List<Card> playerOneBattleCards = new (battleRequest.Deck.Cards);
        List<Card> playerTwoBattleCards = new (opponentRequest.Deck.Cards);
        PerformBattle(battleResult, playerOneBattleCards, playerTwoBattleCards);
        _battleResultsStorage.StoreBattleResult(battleRequest, opponentRequest, battleResult);
    }

    private void PerformBattle(BattleResult battleResult, List<Card> playerOneBattleCards, List<Card> playerTwoBattleCards)
    {
        for (int i = 1; i <= ROUND_LIMIT && playerOneBattleCards.Any() && playerTwoBattleCards.Any(); i++)
        {
            Card playerOneCard = ChooseRandomCard(playerOneBattleCards);
            Card playerTwoCard = ChooseRandomCard(playerTwoBattleCards);

            Card playerOneCardForBattle = Card.ForBattle(playerOneCard);
            Card playerTwoCardForBattle = Card.ForBattle(playerTwoCard);

            playerOneCardForBattle.Ability.Apply(playerOneCardForBattle, playerTwoCardForBattle);
            playerTwoCardForBattle.Ability.Apply(playerTwoCardForBattle, playerOneCardForBattle);

            ProcessRoundResult(battleResult, playerOneCardForBattle, playerTwoCardForBattle, playerOneBattleCards, playerTwoBattleCards, i, playerOneCard, playerTwoCard);
        }

        battleResult.Result = DetermineBattleWinner(playerOneBattleCards, playerTwoBattleCards);
        ComputeELOChanges(battleResult);
    }

    private void ProcessRoundResult(BattleResult battleResult, Card playerOneCardForBattle, Card playerTwoCardForBattle, List<Card> playerOneCards, List<Card> playerTwoCards, int roundNumber, Card playerOneCard, Card playerTwoCard)
    {
        Card? winner = DetermineRoundWinner(playerOneCardForBattle, playerTwoCardForBattle);

        if (winner is not null && winner.Equals(playerOneCardForBattle))
        {
            playerOneCards.Add(playerTwoCard);
            playerTwoCards.Remove(playerTwoCard);
        }
        else if (winner is not null && winner.Equals(playerTwoCardForBattle))
        {
            playerTwoCards.Add(playerOneCard);
            playerOneCards.Remove(playerOneCard);
        }

        battleResult.BattleEvents.Add(new BattleEvent
        {
            Round = roundNumber,
            FirstCardName = playerOneCardForBattle.Name,
            FirstCardEvents = playerOneCardForBattle.Events,
            FirstCardFinalDamage = playerOneCardForBattle.Damage,
            SecondCardName = playerTwoCardForBattle.Name,
            SecondCardEvents = playerTwoCardForBattle.Events,
            SecondCardFinalDamage = playerTwoCardForBattle.Damage,
            WinnerCardName = winner?.Name ?? "No winner"
        });
    }

    private string DetermineBattleWinner(List<Card> playerOneCards, List<Card> playerTwoCards)
    {
        if (playerOneCards.Any() && playerTwoCards.Any()) return "Draw";

        if (playerOneCards.Any()) return "PlayerOneWin";

        return "PlayerTwoWin";
    }

    private void ComputeELOChanges(BattleResult battleResult)
    {
        int playerOneELOChange = 0;
        int playerTwoELOChange = 0;

        if (battleResult.Result == "PlayerOneWin")
        {
            playerOneELOChange = 3;
            playerTwoELOChange = -5;
        }
        else if (battleResult.Result == "PlayerTwoWin")
        {
            playerOneELOChange = -5;
            playerTwoELOChange = 3;
        }

        battleResult.PlayerOneELOChange = playerOneELOChange;
        battleResult.PlayerTwoELOChange = playerTwoELOChange;
    }

    private Card? DetermineRoundWinner(Card playerOneCard, Card playerTwoCard)
    {
        if (playerOneCard > playerTwoCard) return playerOneCard;

        if (playerOneCard < playerTwoCard) return playerTwoCard;

        return null;
    }

    private Card ChooseRandomCard(List<Card> cards)
    {
        Random random = new ();
        int randomIndex = random.Next(0, cards.Count);

        return cards[randomIndex];
    }

}