using AutoMapper;
using Ecommwebapi.Data.Dtos;
using Ecommwebapi.Data.Models;

namespace Ecommwebapi.Profiles
{
    public class DataProfiles : Profile
    {
        public DataProfiles()
        {
            CreateMap<Product, ProductReadDto>().ReverseMap();
            CreateMap<UserRegisterWriteDto, User>().ReverseMap();
            CreateMap<UserUpdateWriteDto, User>().ReverseMap();
            CreateMap<UserReadDto, User>().ReverseMap();
            CreateMap<UserAuthenticateReadDto, User>().ReverseMap();
        }
    }
}