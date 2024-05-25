using AutoMapper;
using MessageService.DTO;
using MessageService.Models;

namespace MessageService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageViewModel>().ReverseMap();
        }
    }
}
