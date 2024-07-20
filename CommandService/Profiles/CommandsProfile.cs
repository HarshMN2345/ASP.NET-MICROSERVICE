using CommandService.Dtos;
using CommandService.Models;
using AutoMapper;
using PlatformService;

namespace CommandService.Profiles;
public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformPublishedDto,Platform>().ForMember(dest=>dest.ExternalId,opt=>opt.MapFrom(src=>src.Id));
        CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}