using AutoMapper;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

using Moq;

using MTCG.Domain;
using MTCG.Domain.Cards;
using MTCG.Domain.Elements;
using MTCG.Persistence.Repositories.Cards;
using MTCG.Persistence.Repositories.Decks;
using MTCG.Persistence.Repositories.Trading;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.Cards.Services;
using MTCG.Services.TradingServices.DTO;
using MTCG.Services.TradingServices.Exceptions;
using MTCG.Services.TradingServices.Services.Concrete;
using MTCG.Services.TradingServices.ViewModels;
using MTCG.Services.UserService;

namespace MTCG.Services.TradingServices.Test;

[TestClass]
public class DefaultTradingServiceTests
{

    private Mock<TradingRepository> _mockTradingRepository;

    private Mock<IMapper> _mockMapper;

    private Mock<UserRepository> _mockUserRepository;

    private Mock<CardRepository> _mockCardRepository;

    private Mock<DeckRepository> _mockDeckRepository;

    private Mock<CardMapperService> _mockCardMapperService;

    private DefaultTradingService _service;

    [TestInitialize]
    public void SetUp()
    {
        _mockTradingRepository = new Mock<TradingRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockUserRepository = new Mock<UserRepository>();
        _mockCardRepository = new Mock<CardRepository>();
        _mockDeckRepository = new Mock<DeckRepository>();
        _mockCardMapperService = new Mock<CardMapperService>();

        _service = new DefaultTradingService(_mockTradingRepository.Object, _mockMapper.Object,
                                             _mockUserRepository.Object, _mockCardRepository.Object, _mockDeckRepository.Object,
                                             _mockCardMapperService.Object);
    }

    [TestMethod]
    public async Task GetAvailableTradingDealsAsync_ReturnsTradingDeals()
    {
        List<TradingDeal> tradingDeals = new ()
        {
            new TradingDeal(),
            new TradingDeal(),
            new TradingDeal()
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(tradingDeals);

        _mockMapper.Setup(mapper => mapper.Map<List<TradingDealViewModel>>(It.IsAny<List<TradingDeal>>()))
                   .Returns(new List<TradingDealViewModel>());

        List<TradingDealViewModel> result = await _service.GetAvailableTradingDealsAsync();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(List<TradingDealViewModel>));
        Assert.AreEqual(0, result.Count);
        _mockTradingRepository.Verify(repo => repo.GetAvailableAsync(), Times.Once);
    }

    [TestMethod]
    public async Task CreateTradingDealAsync_UserNotFound_ThrowsUserNotFoundException()
    {
        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync((User)null);

        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _service.CreateTradingDealAsync(new TradingDealCreationDTO(), "username"));
    }


    [TestMethod]
    public async Task CreateTradingDealAsync_CardNotOwnedByUser_ThrowsCardNotOwnedException()
    {
        User mockUser = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(mockUser);

        List<Card> mockedUserCards = new ()
        {
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(mockedUserCards);

        TradingDealCreationDTO tradingDealCreationDTO = new ()
        {
            OfferingUserCardId = Guid.NewGuid()
        };

        await Assert.ThrowsExceptionAsync<CardNotOwnedException>(() => _service.CreateTradingDealAsync(tradingDealCreationDTO, "username"));
    }


    [TestMethod]
    public async Task CreateTradingDealAsync_TradingDealIdIsAlreadyTaken_ThrowsTradingDealIdIsAlreadyTakenException()
    {
        User mockUser = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(mockUser);

        List<Card> mockedUserCards = new ()
        {
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(mockedUserCards);

        TradingDealCreationDTO tradingDealCreationDTO = new ()
        {
            OfferingUserCardId = mockedUserCards[0].UserCardId,
            TradingDealId = Guid.NewGuid()
        };

        List<TradingDeal> tradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = tradingDealCreationDTO.TradingDealId
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(tradingDeals);

        await Assert.ThrowsExceptionAsync<TradingDealIdIsAlreadyTakenException>(() => _service.CreateTradingDealAsync(tradingDealCreationDTO, "username"));
    }


    [TestMethod]
    public async Task CreateTradingDealAsync_CardAlreadyInDeal_ThrowsCardAlreadyInDealException()
    {
        User mockUser = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(mockUser);

        List<Card> mockedUserCards = new ()
        {
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(mockedUserCards);

        TradingDealCreationDTO tradingDealCreationDTO = new ()
        {
            OfferingUserCardId = mockedUserCards[0].UserCardId,
            TradingDealId = Guid.NewGuid()
        };

        List<TradingDeal> tradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = Guid.NewGuid(),
                OfferingUserCardId = tradingDealCreationDTO.OfferingUserCardId
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(tradingDeals);

        await Assert.ThrowsExceptionAsync<CardAlreadyInDealException>(() => _service.CreateTradingDealAsync(tradingDealCreationDTO, "username"));
    }


    [TestMethod]
    public async Task CreateTradingDealAsync_CardInDeck_ThrowsCardInDeckException()
    {
        User mockUser = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(mockUser);

        List<Card> mockedUserCards = new ()
        {
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(mockedUserCards);

        TradingDealCreationDTO tradingDealCreationDTO = new ()
        {
            OfferingUserCardId = mockedUserCards[0].UserCardId,
            TradingDealId = Guid.NewGuid()
        };

        List<TradingDeal> tradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = Guid.NewGuid(),
                OfferingUserCardId = Guid.NewGuid()
            }
        };

        List<Deck> mockedUserDecks = new ()
        {
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = tradingDealCreationDTO.OfferingUserCardId
                    }
                }
            },
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(tradingDeals);

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(mockedUserDecks);

        await Assert.ThrowsExceptionAsync<CardInDeckException>(() => _service.CreateTradingDealAsync(tradingDealCreationDTO, "username"));
    }


    [TestMethod]
    public async Task CreateTradingDealAsync_CreatesTradingDeal()
    {
        User user = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        List<Card> userCards = new ()
        {
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(userCards);

        TradingDealCreationDTO tradingDealCreationDTO = new ()
        {
            OfferingUserCardId = userCards[0].UserCardId,
            TradingDealId = Guid.NewGuid()
        };

        List<TradingDeal> availableTradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = Guid.NewGuid(),
                OfferingUserCardId = Guid.NewGuid()
            }
        };

        List<Deck> userDecks = new ()
        {
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(availableTradingDeals);

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(userDecks);

        _mockTradingRepository.Setup(repo => repo.CreateAsync(It.IsAny<TradingDeal>()))
                              .Returns(Task.CompletedTask);

        TradingDeal tradingDeal = new ();

        _mockMapper.Setup(mapper => mapper.Map<TradingDeal>(It.IsAny<TradingDealCreationDTO>()))
                   .Returns(tradingDeal);

        await _service.CreateTradingDealAsync(tradingDealCreationDTO, "username");

        _mockTradingRepository.Verify(repo => repo.CreateAsync(tradingDeal), Times.Once);
    }


    [TestMethod]
    public async Task CarryOutTradingDealAsync_TradingDealNotFound_ThrowsTradingDealNotFoundException()
    {
        string userName = "username";
        Guid tradingDealId = Guid.NewGuid();
        Guid respondingUserCardId = Guid.NewGuid();

        User user = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        List<Card> userCards = new ()
        {
            new Card
            {
                UserCardId = respondingUserCardId
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(userCards);

        List<TradingDeal> availableTradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = Guid.NewGuid(),
                OfferingUserCardId = Guid.NewGuid()
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(availableTradingDeals);

        List<Deck> userDecks = new ()
        {
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
        };

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(userDecks);

        await Assert.ThrowsExceptionAsync<TradingDealNotFoundException>(() => _service.CarryOutTradingDealAsync(userName, tradingDealId, respondingUserCardId));
    }


    [TestMethod]
    public async Task CarryOutTradingDealAsync_SelfDeal_ThrowsSelfDealException()
    {
        string userName = "username";
        Guid tradingDealId = Guid.NewGuid();
        Guid respondingUserCardId = Guid.NewGuid();

        User user = new ()
        {
            UserId = 1
        };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        List<Card> userCards = new ()
        {
            new Card
            {
                UserCardId = respondingUserCardId
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            },
            new Card
            {
                UserCardId = Guid.NewGuid()
            }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(userCards);

        List<TradingDeal> availableTradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = tradingDealId,
                OfferingUserCardId = Guid.NewGuid(),
                OfferingUserId = user.UserId
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(availableTradingDeals);

        List<Deck> userDecks = new ()
        {
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
            new Deck
            {
                Cards = new ()
                {
                    new Card
                    {
                        UserCardId = Guid.NewGuid()
                    }
                }
            },
        };

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(userDecks);

        await Assert.ThrowsExceptionAsync<SelfDealException>(() => _service.CarryOutTradingDealAsync(userName, tradingDealId, respondingUserCardId));
    }


    [TestMethod]
    public async Task CarryOutTradingDealAsync_RequiredTypeDoesNotMatch_ThrowsRequiredTypeDoesNotMatchException()
    {
        string userName = "username";
        Guid tradingDealId = Guid.NewGuid();
        Guid respondingUserCardId = Guid.NewGuid();

        User user = new () { UserId = 1 };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        List<Card> userCards = new ()
        {
            new Card { UserCardId = respondingUserCardId },
            new Card { UserCardId = Guid.NewGuid() },
            new Card { UserCardId = Guid.NewGuid() }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(userCards);

        List<TradingDeal> availableTradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = tradingDealId,
                OfferingUserCardId = Guid.NewGuid(),
                OfferingUserId = 2,
                RequiredCardType = "monster"
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(availableTradingDeals);

        List<Deck> userDecks = new ()
        {
            new Deck { Cards = new () { new Card { UserCardId = Guid.NewGuid() } } },
            new Deck { Cards = new () { new Card { UserCardId = Guid.NewGuid() } } },
        };

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(userDecks);

        List<Card> mappedUserCards = new ()
        {
            new WaterBlast { UserCardId = respondingUserCardId },
            new WaterBlast { UserCardId = Guid.NewGuid() },
            new FireElf { UserCardId = Guid.NewGuid() }
        };

        _mockCardMapperService.Setup(service => service.MapCardsAsync(It.IsAny<List<Card>>()))
                              .ReturnsAsync(mappedUserCards);

        await Assert.ThrowsExceptionAsync<RequiredTypeDoesNotMatchException>(() => _service.CarryOutTradingDealAsync(userName, tradingDealId, respondingUserCardId));
    }


    [TestMethod]
    public async Task CarryOutTradingDealAsync_RequiredDamageNotReached_ThrowsRequiredDamageNotReachedException()
    {
        string userName = "username";
        Guid tradingDealId = Guid.NewGuid();
        Guid respondingUserCardId = Guid.NewGuid();

        User user = new () { UserId = 1 };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        List<Card> userCards = new ()
        {
            new Card { UserCardId = respondingUserCardId },
            new Card { UserCardId = Guid.NewGuid() },
            new Card { UserCardId = Guid.NewGuid() }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(userCards);

        List<TradingDeal> availableTradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = tradingDealId,
                OfferingUserCardId = Guid.NewGuid(),
                OfferingUserId = 2,
                RequiredCardType = "monster",
                RequiredMinimumDamage = 90
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(availableTradingDeals);

        List<Deck> userDecks = new ()
        {
            new Deck { Cards = new () { new Card { UserCardId = Guid.NewGuid() } } },
            new Deck { Cards = new () { new Card { UserCardId = Guid.NewGuid() } } },
        };

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(userDecks);

        List<Card> mappedUserCards = new ()
        {
            new FireElf { UserCardId = respondingUserCardId, Damage = 50 },
            new WaterBlast { UserCardId = Guid.NewGuid(), Damage = 50 },
            new FireElf { UserCardId = Guid.NewGuid(), Damage = 50 }
        };

        _mockCardMapperService.Setup(service => service.MapCardsAsync(It.IsAny<List<Card>>()))
                              .ReturnsAsync(mappedUserCards);

        await Assert.ThrowsExceptionAsync<RequiredDamageNotReachedException>(() => _service.CarryOutTradingDealAsync(userName, tradingDealId, respondingUserCardId));
    }


    [TestMethod]
    public async Task CarryOutTradingDealAsync_CarriesOutTradingDeal()
    {
        User user = new () { UserId = 1 };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        Guid respondingUserCardId = Guid.NewGuid();

        List<Card> userCards = new ()
        {
            new Card { UserCardId = respondingUserCardId },
            new Card { UserCardId = Guid.NewGuid() },
            new Card { UserCardId = Guid.NewGuid() }
        };

        _mockCardRepository.Setup(repo => repo.GetUserCardsAsync(It.IsAny<int>()))
                           .ReturnsAsync(userCards);

        Guid tradingDealId = Guid.NewGuid();

        List<TradingDeal> availableTradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = tradingDealId,
                OfferingUserCardId = Guid.NewGuid(),
                OfferingUserId = 2,
                RequiredCardType = "monster",
                RequiredMinimumDamage = 90
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(availableTradingDeals);

        List<Deck> userDecks = new ()
        {
            new Deck { Cards = new () { new Card { UserCardId = Guid.NewGuid() } } },
            new Deck { Cards = new () { new Card { UserCardId = Guid.NewGuid() } } },
        };

        _mockDeckRepository.Setup(repo => repo.GetUserDecksAsync(It.IsAny<int>()))
                           .ReturnsAsync(userDecks);

        List<Card> mappedUserCards = new ()
        {
            new FireElf { UserCardId = respondingUserCardId, Damage = 100 },
            new WaterBlast { UserCardId = Guid.NewGuid(), Damage = 50 },
            new FireElf { UserCardId = Guid.NewGuid(), Damage = 50 }
        };

        _mockCardMapperService.Setup(service => service.MapCardsAsync(It.IsAny<List<Card>>()))
                              .ReturnsAsync(mappedUserCards);

        TradingDeal tradingDeal = availableTradingDeals[0];

        await _service.CarryOutTradingDealAsync("username", tradingDealId, respondingUserCardId);

        _mockTradingRepository.Verify(repo => repo.CarryOutAsync(tradingDeal), Times.Once);
    }


    [TestMethod]
    public async Task DeleteTradingDealAsync_TradingDealNotOwnedByUser_ThrowsTradingDealNotOwnedException()
    {
        User user = new () { UserId = 1 };

        _mockUserRepository.Setup(repo => repo.GetByUserName(It.IsAny<string>()))
                           .ReturnsAsync(user);

        Guid tradingDealId = Guid.NewGuid();

        List<TradingDeal> tradingDeals = new ()
        {
            new TradingDeal
            {
                TradingDealId = tradingDealId,
                OfferingUserId = 2
            }
        };

        _mockTradingRepository.Setup(repo => repo.GetAvailableAsync())
                              .ReturnsAsync(tradingDeals);

        await Assert.ThrowsExceptionAsync<TradingDealNotOwnedException>(() => _service.DeleteTradingDealAsync("username", tradingDealId));
    }

}