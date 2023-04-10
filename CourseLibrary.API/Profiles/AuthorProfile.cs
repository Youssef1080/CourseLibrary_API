using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();
            CreateMap<AuthorToUpdate, Author>();
        }
    }
}