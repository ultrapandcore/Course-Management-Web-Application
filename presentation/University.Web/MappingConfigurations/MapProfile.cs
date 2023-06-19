using AutoMapper;
using University.Web.Models;

namespace University.Web.MappingConfigurations
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Student, StudentModel>().ReverseMap();
            CreateMap<Course, CourseModel>().ReverseMap();
            CreateMap<Group, GroupModel>().ReverseMap();
        }
    }
}
