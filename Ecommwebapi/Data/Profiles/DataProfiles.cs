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
            CreateMap<Product, ProductAddWriteDto>().ReverseMap();

            CreateMap<UserRegisterWriteDto, User>().ReverseMap();
            CreateMap<UserUpdateWriteDto, User>().ReverseMap();
            CreateMap<UserReadDto, User>().ReverseMap();
            CreateMap<UserAuthenticateReadDto, User>().ReverseMap();
            

            CreateMap<Category, CategoryReadDto>().ReverseMap();
            CreateMap<Category, CategoryAddWriteDto>().ReverseMap();

            CreateMap<CartItem, CartItemAddWriteDto>()
                //.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap();
            CreateMap<CartItem, CartItemReadDto>()
                //.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap();

            CreateMap<WishlistItem, WishlistItemReadDto>().ReverseMap();
            CreateMap<WishlistItem, WishlistItemAddWriteDto>().ReverseMap();

            CreateMap<Order, OrderReadDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemReadDto>().ReverseMap();
            

            //Example for individual member mapping
            //CreateMap<CalendarEvent, CalendarEventForm>()
            //    .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date.Date))
            //    .ForMember(dest => dest.EventHour, opt => opt.MapFrom(src => src.Date.Hour))
            //    .ForMember(dest => dest.EventMinute, opt => opt.MapFrom(src => src.Date.Minute)));
        }
    }
}