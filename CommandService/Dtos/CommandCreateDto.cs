using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos;
public class CommandCreateDto{
    [Required]
    public required string HowTo{get;set;}
      [Required]
    public required string CommandLine{get;set;}
}