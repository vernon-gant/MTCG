using AutoMapper;

using MTCG.Domain;
using MTCG.Persistence.Repositories.Users;

namespace MTCG.Services.UserService;

public class DefaultAuthenticatedUserService : AuthenticatedUserService
{
    private readonly UserRepository _userRepository;

    private readonly IMapper _mapper;

    public DefaultAuthenticatedUserService(IMapper mapper, UserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async ValueTask<UserDetailsViewModel> GetUserAsync(string username)
    {
        User? user = await _userRepository.GetByUserName(username);
        if (user == null) throw new UserNotFoundException();
        return _mapper.Map<UserDetailsViewModel>(user);
    }

    public async ValueTask<UserDetailsViewModel> UpdateUserAsync(string userName, UserUpdateDto userUpdateDto)
    {
        User? user = await _userRepository.GetByUserName(userName);
        if (user == null) throw new UserNotFoundException();
        User updatedUser = await _userRepository.Update(_mapper.Map(userUpdateDto, user));
        return _mapper.Map<UserDetailsViewModel>(updatedUser);
    }

}