using AutoMapper;
using Data.Entity;
using Service.DTOs.Account;


namespace Service.Mappings
{
    public class ModelToDto : Profile
    {
        public ModelToDto()
        {
            CreateMap<AppUser, UserDto>();
        }
    }
}
