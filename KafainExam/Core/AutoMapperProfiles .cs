using AutoMapper;
using KafainExam.Entity;

namespace KafainExam.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TaskEntity, TaskEntity>().ReverseMap();
        }
    }
}
