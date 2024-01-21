using AutoMapper;

using MTCG.Domain;
using MTCG.Services.BattleServices.ViewModels;

namespace MTCG.Services.BattleServices;

public class BattleResultProfile : Profile
{

    public BattleResultProfile()
    {
        CreateMap<BattleResult, BattleResultViewModel>()
            .ForMember(dest => dest.EnemyUserName, opt => opt.Ignore()) // We'll set this manually
            .ForMember(dest => dest.Result, opt => opt.Ignore()) // Manually set based on conditions
            .ForMember(dest => dest.ELOChange, opt => opt.Ignore()) // Determine dynamically
            .AfterMap((src, dest, context) =>
            {
                string userName = (string)context.Items["userName"];
                bool isPlayerOne = src.PlayerOne.UserName == userName;

                dest.EnemyUserName = isPlayerOne ? src.PlayerTwo.UserName : src.PlayerOne.UserName;

                dest.Result = GetResultText(src.Result, isPlayerOne);

                dest.ELOChange = isPlayerOne ? src.PlayerOneELOChange : src.PlayerTwoELOChange;
            });
    }

    private string GetResultText(string matchResult, bool isPlayerOne)
    {
        if (matchResult == "PlayerOneWin") return isPlayerOne ? "You Won!" : "You Lost!";

        if (matchResult == "PlayerTwoWin") return isPlayerOne ? "You Lost!" : "You Won!";

        return "Draw!";
    }

}