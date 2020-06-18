using AutoMapper;
using ecommwebapi.Data.Dtos;
using ecommwebapi.Data.Models;

namespace ecommwebapi.Profiles
{
    public class DataProfiles : Profile
    {
        public DataProfiles()
        {
            CreateMap<Product, ProductReadDto>().ReverseMap();
        }
    }
}