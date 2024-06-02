using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using UserService.DTO;
using UserService.Model;

namespace UserService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => (RoleType)src.RoleId));
        }
    }
}