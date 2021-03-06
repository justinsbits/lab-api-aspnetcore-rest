using AutoMapper;
using CommanderREST.Dtos;
using CommanderData.Entities;

namespace CommanderREST.Profiles
{
    public class ToolsProfile : Profile
    {
        public ToolsProfile()
        {
            //Source -> Target
            CreateMap<Tool, ToolReadDto>();
            CreateMap<ToolCreateDto, Tool>();
            CreateMap<ToolUpdateDto, Tool>();
            CreateMap<Tool, ToolUpdateDto>();
        }
    } 
}