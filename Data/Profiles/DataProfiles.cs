using AutoMapper;
using ecommwebapi.Data.Dtos;
using ecommwebapi.Data.Models;
using ecommwebapi.Models;

namespace ecommwebapi.Profiles
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