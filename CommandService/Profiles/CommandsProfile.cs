using CommandService.Dtos;
using CommandService.Models;
using AutoMapper;

namespace CommandService.Profiles;
public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Platform, PlatformReadDto>();
    }
}