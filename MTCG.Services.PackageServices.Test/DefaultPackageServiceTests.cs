using AutoMapper;

using Moq;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Cards.Mappings;
using MTCG.Persistence.Repositories.Packages;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.PackageServices.Dto;
using MTCG.Services.PackageServices.Exceptions;
using MTCG.Services.PackageServices.Services.Concrete;
using MTCG.Services.PackageServices.ViewModels;

namespace MTCG.Services.PackageServices.Test;

[TestClass]
public class DefaultPackageServiceTests
{

    private readonly Dictionary<string, CardMapping> _cardMappings = new ()
    {
        { "Dragon", new CardMapping { CardId = 1, DomainClass = "Dragon" } },
        { "FireElf", new CardMapping { CardId = 2, DomainClass = "FireElf" } },
        { "Wizard", new CardMapping { CardId = 3, DomainClass = "Wizard" } },
        { "Ork", new CardMapping { CardId = 4, DomainClass = "Ork" } },
        { "Knight", new CardMapping { CardId = 5, DomainClass = "Knight" } },
        { "Kraken", new CardMapping { CardId = 6, DomainClass = "Kraken" } },
        { "Goblin", new CardMapping { CardId = 7, DomainClass = "Goblin" } }
    };

    private readonly Mock<IMapper> _mapperMock = new ();

    private readonly Mock<PackageRepository> _packageRepositoryMock = new ();

    private readonly Mock<UserRepository> _userRepositoryMock = new ();

    private DefaultPackageService _defaultPackageService;

    [TestInitialize]
    public void SetUp()
    {
        _defaultPackageService = new DefaultPackageService(_packageRepositoryMock.Object, _mapperMock.Object, _cardMappings, _userRepositoryMock.Object);
    }

    [TestMethod]
    public async Task CreatePackageAsync_WithValidPackage_ReturnsPackageId()
    {
        _userRepositoryMock.Setup(userRepository => userRepository.GetByUserName(It.IsAny<string>())).ReturnsAsync(new User());

        _packageRepositoryMock.Setup(packageRepository => packageRepository.Create(It.IsAny<Package>())).ReturnsAsync(new Package { PackageId = 1 });

        CardPackageCreationDto cardPackageCreationDto = new ()
        {
            Name = "TestPackage",
            Cards = new List<CardPackageItem>
            {
                new () { UserCardId = Guid.NewGuid(), Name = "Dragon" },
                new () { UserCardId = Guid.NewGuid(), Name = "FireElf" },
                new () { UserCardId = Guid.NewGuid(), Name = "Wizard" },
                new () { UserCardId = Guid.NewGuid(), Name = "Ork" },
                new () { UserCardId = Guid.NewGuid(), Name = "Knight" }
            }
        };
        _mapperMock.Setup(mapper => mapper.Map<Package>(cardPackageCreationDto)).Returns(new Package());

        int packageId = await _defaultPackageService.CreatePackageAsync(cardPackageCreationDto, "TestAdmin");

        Assert.AreEqual(1, packageId);
        _packageRepositoryMock.Verify(packageRepository => packageRepository.Create(It.IsAny<Package>()), Times.Once);
    }


    [TestMethod]
    public async Task CreatePackageAsync_WithDuplicateCardId_ThrowsDuplicateCardGuidException()
    {
        Guid duplicateGuid = Guid.NewGuid();
        CardPackageCreationDto cardPackageCreationDto = new ()
        {
            Name = "TestPackage",
            Cards = new List<CardPackageItem>
            {
                new () { UserCardId = duplicateGuid, Name = "Dragon" },
                new () { UserCardId = duplicateGuid, Name = "FireElf" },
                new () { UserCardId = duplicateGuid, Name = "Wizard" },
                new () { UserCardId = Guid.NewGuid(), Name = "Ork" },
                new () { UserCardId = Guid.NewGuid(), Name = "Knight" },
            }
        };

        Assert.ThrowsException<DuplicateCardGuidException>(() => _defaultPackageService.CreatePackageAsync(cardPackageCreationDto, "TestAdmin").GetAwaiter().GetResult());
    }

    [TestMethod]
    public async Task CreatePackageAsync_WithUnexistingCard_ThrowsUnexistingCardException()
    {
        CardPackageCreationDto cardPackageCreationDto = new ()
        {
            Name = "TestPackage",
            Cards = new List<CardPackageItem>
            {
                new () { UserCardId = Guid.NewGuid(), Name = "Dragon" },
                new () { UserCardId = Guid.NewGuid(), Name = "FireElf" },
                new () { UserCardId = Guid.NewGuid(), Name = "Wizard" },
                new () { UserCardId = Guid.NewGuid(), Name = "Ork" },
                new () { UserCardId = Guid.NewGuid(), Name = "UnexistingCard" }
            }
        };

        Assert.ThrowsException<UnexistingCardException>(() => _defaultPackageService.CreatePackageAsync(cardPackageCreationDto, "TestAdmin").GetAwaiter().GetResult());
    }


    [TestMethod]
    public async Task AcquireCardPackageAsync_WithValidPackage_ReturnsCardPackageViewModel()
    {
        _userRepositoryMock.Setup(userRepository => userRepository.GetByUserName(It.IsAny<string>())).ReturnsAsync(new User { Coins = 10 });

        _packageRepositoryMock.Setup(packageRepository => packageRepository.GetFirstNotAcquiredPackage()).ReturnsAsync(new Package { PackageId = 1 });

        _packageRepositoryMock.Setup(packageRepository => packageRepository.AddPackageToUser(It.IsAny<int>(), It.IsAny<Package>(), It.IsAny<int>())).ReturnsAsync(new Package { PackageId = 1 });

        _mapperMock.Setup(mapper => mapper.Map<CardPackageViewModel>(It.IsAny<Package>())).Returns(new CardPackageViewModel { Name = "TestPackage" });

        CardPackageViewModel cardPackageViewModel = await _defaultPackageService.AcquirePackageAsync("TestUser");

        Assert.AreEqual("TestPackage", cardPackageViewModel.Name);
        _packageRepositoryMock.Verify(packageRepository => packageRepository.AddPackageToUser(It.IsAny<int>(), It.IsAny<Package>(), It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public async Task AcquireCardPackageAsync_WithNotEnoughCoins_ThrowsNotEnoughCoinsException()
    {
        _userRepositoryMock.Setup(userRepository => userRepository.GetByUserName(It.IsAny<string>())).ReturnsAsync(new User { Coins = 0 });

        Assert.ThrowsException<NotEnoughCoinsException>(() => _defaultPackageService.AcquirePackageAsync("TestUser").GetAwaiter().GetResult());
    }

    [TestMethod]
    public async Task AcquireCardPackge_WithNoAvailablePackage_ThrowsNoPackageAvailableException()
    {
        _userRepositoryMock.Setup(userRepository => userRepository.GetByUserName(It.IsAny<string>())).ReturnsAsync(new User { Coins = 10 });

        _packageRepositoryMock.Setup(packageRepository => packageRepository.GetFirstNotAcquiredPackage()).ReturnsAsync((Package?)null);

        Assert.ThrowsException<NoPackageAvailableException>(() => _defaultPackageService.AcquirePackageAsync("TestUser").GetAwaiter().GetResult());
    }



}