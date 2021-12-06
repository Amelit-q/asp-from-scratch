using AspFromScratch.WebApi.Helpers;
using AutoMapper;

namespace AspFromScratch.WebApi.User;

internal class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ReadUserDto, UserEntity>();
        CreateMap<RegisterUserDto, UserEntity>();
        CreateMap<JwtTokenHelper.TokenPair, TokenPairDto>();
    }
}
