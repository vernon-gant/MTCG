using AutoMapper;

using MTCG.Domain;
using MTCG.Persistance.Repositories;
using MTCG.Persistance.Repositories.Packages;
using MTCG.Persistance.Repositories.users;
using MTCG.Services.PackageServices.Dto;
using MTCG.Services.PackageServices.Exceptions;
using MTCG.Services.PackageServices.ViewModels;

namespace MTCG.Services.PackageServices.Services.Concrete;

public class DefaultPackageService : PackageService
{
    private readonly UserRepository _userRepository;

    private readonly Dictionary<string, CardMapping> _cardMappings;

    private readonly IMapper _mapper;

    private readonly PackageRepository _packageRepository;

    public DefaultPackageService(PackageRepository packageRepository, IMapper mapper, Dictionary<string, CardMapping> cardMappings, UserRepository userRepository)
    {
        _packageRepository = packageRepository;
        _cardMappings = cardMappings;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async ValueTask<CardPackageViewModel> CreatePackageAsync(CardPackageCreationDto cardPackageCreationDto, string createdBy)
    {
        if (IsDuplicateIdInPackage(cardPackageCreationDto)) throw new DuplicateCardGuidException();
        if (IsUnexistingCardInPackage(cardPackageCreationDto)) throw new UnexistingCardException();

        CardPackage cardPackage = _mapper.Map<CardPackage>(cardPackageCreationDto);
        cardPackage.CreatedBy = createdBy;
        cardPackage.CreatedById = (await _userRepository.GetByUserName(createdBy))!.UserId;
        cardPackage = await _packageRepository.Create(cardPackage);

        return _mapper.Map<CardPackageViewModel>(cardPackage);
    }

    private bool IsDuplicateIdInPackage(CardPackageCreationDto packageViewModel) =>
        packageViewModel.Cards.Select(card => card.UserCardId).Distinct().Count() != packageViewModel.Cards.Count;

    private bool IsUnexistingCardInPackage(CardPackageCreationDto packageViewModel) => packageViewModel.Cards.Any(card => !_cardMappings.ContainsKey(card.Name));

    public async ValueTask<CardPackageViewModel> AcquirePackageAsync(string username)
    {
        throw new NotImplementedException();
    }

}