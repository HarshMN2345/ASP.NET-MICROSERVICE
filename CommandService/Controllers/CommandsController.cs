using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController:ControllerBase{
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;
    public CommandsController(ICommandRepo repository,IMapper mapper){
        _repository=repository;
        _mapper=mapper;
    }
    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId){
        Console.WriteLine("Get Commands For Platform {platformId}");
        if(!_repository.PlatformExists(platformId)){
            return NotFound();
        }
        var commands=_repository.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandCreateDto>>(commands));
    }
    [HttpGet("{commandId}",Name ="GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId,int commandId){
        Console.WriteLine("Get Command For Platform {platformId}");
        if(!_repository.PlatformExists(platformId)){
            return NotFound();
        }
        var command=_repository.GetCommand(platformId,commandId);
        if(command==null){
            return NotFound();
        }
        return Ok(_mapper.Map<CommandReadDto>(command));
    }
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId,CommandCreateDto commandDto){
        Console.WriteLine("Create Command For Platform {platformId}");
        if(!_repository.PlatformExists(platformId)){
            return NotFound();
        }
        var command=_mapper.Map<Command>(commandDto);
        _repository.CreateCommand(platformId,command);
        _repository.SaveChanges();
        var commandReadDto=_mapper.Map<CommandReadDto>(command);
        return CreatedAtRoute(nameof(GetCommandForPlatform),new {platformId=platformId,commandId=commandReadDto.Id},commandReadDto);
    }

}
