using AutoMapper;

using MTCG.Domain;

namespace MTCG.Services.UserService;

public class UserProfile : Profile
{

    public UserProfile()
    {
        CreateMap<User, UserDetailsViewModel>();
        CreateMap<UserUpdateDto, User>();
    }

}