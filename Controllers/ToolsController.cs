using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using CommanderData.Entities;
using CommanderREST.Dtos;
using Microsoft.Extensions.Logging;
using CommanderData.Repositories;

namespace CommanderREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolsController : GenericCRUDController<Tool, ToolCreateDto, ToolUpdateDto, ToolReadDto>
    {
        public ToolsController(
            ILogger<GenericCRUDController<Tool, ToolCreateDto, ToolUpdateDto, ToolReadDto>> logger,
            IRepository<Tool> repository,
            IMapper mapper)
            : base(logger, repository, mapper)
        {

        }
    }
}