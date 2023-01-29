using AutoMapper;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Models.DTO.GuardianTask;

namespace Guardian_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<GuardianDTO, GuardianCreateDTO>().ReverseMap();
            CreateMap<GuardianDTO, GuardianUpdateDTO>().ReverseMap();

            CreateMap<GuardianTaskDTO, GuardianCreateTaskDTO>().ReverseMap();
            CreateMap<GuardianTaskDTO, GuardianUpdateTaskDTO>().ReverseMap();
        }
    }
}
