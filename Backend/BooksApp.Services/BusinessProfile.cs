using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BooksApp.Data.Entities;
using BooksApp.Services.Models;

namespace BooksApp.Services
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<BookEntity, BookModel>().ReverseMap();
            CreateMap<CategoryEntity, CategoryModel>().ReverseMap();

            CreateMap<UserEntity, UserModel>().ReverseMap();
            CreateMap<RegisterModel, UserEntity>()
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                   .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                   .ForMember(dest => dest.DateModified, opt => opt.Ignore());
        }

    }
}
