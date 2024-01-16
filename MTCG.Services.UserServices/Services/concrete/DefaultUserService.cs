using AutoMapper;

using Microsoft.Extensions.Logging;

using MTCG.Domain;
using MTCG.Persistance.Repositories.users;

namespace MTCG.Services.UserService;

public class DefaultUserService : UserService
{
    private readonly IMapper _mapper;

    private readonly ILogger<DefaultUserService> _logger;

    private readonly UserRepository _userRepository;

    private readonly SecurityService _securityService;

    public DefaultUserService(UserRepository userRepository, ILogger<DefaultUserService> logger, SecurityService securityService, IMapper mapper)
    {
        _userRepository = userRepository;
        _logger = logger;
        _securityService = securityService;
        _mapper = mapper;
    }

    public async ValueTask<User> RegisterUserAsync(UserRegistrationDto userDto)
    {
        User user = new ()
        {
            UserName = userDto.UserName,
            Password = _securityService.HashPassword(userDto.Password),
        };

        return await _userRepository.Create(user);
    }

    public async ValueTask<string> LoginUserAsync(UserLoginDto userDto)
    {
        User? foundUser = await _userRepository.GetByUserName(userDto.UserName);

        if (foundUser == null) throw new UserNotFoundException();

        if (!_securityService.VerifyPassword(foundUser.Password, userDto.Password)) throw new WrongPasswordException();

        return _securityService.GenerateToken(foundUser.UserName, foundUser.IsAdmin ? "admin" : "user");
    }

    public async ValueTask<User?> GetUserAsync(string username)
    {
        User? foundUser = await _userRepository.GetByUserName(username);

        return foundUser;
    }

}