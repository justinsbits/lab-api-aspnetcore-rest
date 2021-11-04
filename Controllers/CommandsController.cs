using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using CommanderData.Entities;
using CommanderREST.Data;
using CommanderREST.Dtos;
using Microsoft.Extensions.Logging;
using CommanderData.Repositories;

namespace CommanderREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandsController : GenericCRUDController<Command, CommandCreateDto, CommandUpdateDto, CommandReadDto>
    {
        public CommandsController(
            ILogger<GenericCRUDController<Command, CommandCreateDto, CommandUpdateDto, CommandReadDto>> logger, 
            IRepository<Command> repository, 
            IMapper mapper) 
            : base(logger, repository, mapper)
        {

        }
    }
}