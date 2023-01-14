using AutoMapper;
using Guardian.Domain.DTO.Guardian;
using Guardian.Domain.DTO.GuardianTask;
using Guardian.Domain.Models;

namespace Guardian.Data
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<GuardianModel, GuardianDTO>().ReverseMap();           
            CreateMap<GuardianModel, GuardianCreateDTO>().ReverseMap();
            CreateMap<GuardianModel, GuardianUpdateDTO>().ReverseMap();

            CreateMap<GuardianTaskModel, GuardianTaskDTO>().ReverseMap();
            CreateMap<GuardianTaskModel, GuardianCreateTaskDTO>().ReverseMap();
            CreateMap<GuardianTaskModel, GuardianUpdateTaskDTO>().ReverseMap();
        }
    }
}
