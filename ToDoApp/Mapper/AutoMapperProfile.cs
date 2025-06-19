using AutoMapper;
using ToDoApp.DataContext;
using ToDoApp.Models;

namespace ToDoApp.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ToDo, ToDoModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (StatusType)src.StatusId));

            CreateMap<ToDoModel, ToDo>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));
        }
    }
}
