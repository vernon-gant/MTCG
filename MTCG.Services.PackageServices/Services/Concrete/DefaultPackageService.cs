using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Cards.Mappings;
using MTCG.Persistence.Repositories.Packages;
using MTCG.Persistence.Repositories.Users;
using MTCG.Services.PackageServices.Dto;
using MTCG.Services.PackageServices.Exceptions;
using MTCG.Services.PackageServices.ViewModels;
using MTCG.Services.UserService;

namespace MTCG.Services.PackageServices.Services.Concrete;

public class DefaultPackageService : PackageService
{

    private const int PACKAGE_PRICE = 5;

    private readonly Dictionary<string, CardMapping> _cardMappings;

    private readonly IMapper _mapper;

    private readonly PackageRepository _packageRepository;

    private readonly UserRepository _userRepository;

    public DefaultPackageService(PackageRepository packageRepository, IMapper mapper, Dictionary<string, CardMapping> cardMappings, UserRepository userRepository)
    {
        _packageRepository = packageRepository;
        _cardMappings = cardMappings;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async ValueTask<int> CreatePackageAsync(CardPackageCreationDto cardPackageCreationDto, string adminName)
    {
        if (IsDuplicateIdInPackage(cardPackageCreationDto)) throw new DuplicateCardGuidException();
        if (IsUnexistingCardInPackage(cardPackageCreationDto)) throw new UnexistingCardException();

        Package package = _mapper.Map<Package>(cardPackageCreationDto);
        package.CreatedBy = adminName;
        package.CreatedById = (await _userRepository.GetByUserName(adminName))!.UserId;
        Package createdPackage = await _packageRepository.Create(package);

        return createdPackage.PackageId;
    }

    public async ValueTask<CardPackageViewModel> AcquirePackageAsync(string userName)
    {
        User? foundUser = await _userRepository.GetByUserName(userName);

        if (foundUser is null) throw new UserNotFoundException();

        if (foundUser.Coins < PACKAGE_PRICE) throw new NotEnoughCoinsException();

        Package? firstNotAcquiredPackage = await _packageRepository.GetFirstNotAcquiredPackage();

        if (firstNotAcquiredPackage is null) throw new NoPackageAvailableException();

        Package addedPackage = await _packageRepository.AddPackageToUser(foundUser.UserId, firstNotAcquiredPackage, PACKAGE_PRICE);

        return _mapper.Map<CardPackageViewModel>(addedPackage);
    }

    private bool IsDuplicateIdInPackage(CardPackageCreationDto packageViewModel) =>
        packageViewModel.Cards.Select(card => card.UserCardId).Distinct().Count() != packageViewModel.Cards.Count;

    private bool IsUnexistingCardInPackage(CardPackageCreationDto packageViewModel) => packageViewModel.Cards.Any(card => !_cardMappings.ContainsKey(card.Name));

}