using AutoMapper;
using Guardian.Application.DTO;
using Guardian.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Data
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<GuardianModel, GuardianDTO>().ReverseMap();           
            CreateMap<GuardianModel, GuardianCreateDTO>().ReverseMap();
            CreateMap<GuardianModel, GuardianUpdateDTO>().ReverseMap();
        }
    }
}
