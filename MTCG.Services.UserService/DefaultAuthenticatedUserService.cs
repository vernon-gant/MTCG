using AutoMapper;

using MTCG.Domain;
using MTCG.Persistance.Repositories.users;

namespace MTCG.Services.UserService;

public class DefaultAuthenticatedUserService : AuthenticatedUserService
{
    private readonly UserService _userService;

    private readonly UserRepository _userRepository;

    private readonly IMapper _mapper;

    public DefaultAuthenticatedUserService(UserService userService, IMapper mapper, UserRepository userRepository)
    {
        _userService = userService;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async ValueTask<UserDetails> Get(string username)
    {
        User? user = await _userService.Get(username);
        if (user == null) throw new UserNotFoundException();
        return _mapper.Map<UserDetails>(user);
    }

    public async ValueTask<User> Update(string userName, UserUpdateDto userUpdateDto)
    {
        User? user = await _userService.Get(userName);
        if (user == null) throw new UserNotFoundException();
        return await _userRepository.Update(_mapper.Map(userUpdateDto, user));
    }

}