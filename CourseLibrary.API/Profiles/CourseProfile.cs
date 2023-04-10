using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseDto, Course>();
            CreateMap<CourseToAdd, Course>();
        }
    }
}