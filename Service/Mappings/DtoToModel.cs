using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entity;
using Service.Account.DTOs;

namespace Service.Mappings
{
    public class DtoToModel : Profile
    {
        public DtoToModel()
        {
            CreateMap<RegisterDto, AppUser>();
        }
    }
}