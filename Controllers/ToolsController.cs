using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

using CommanderDA.Entities;
using CommanderREST.Data;
using CommanderREST.Dtos;


namespace CommanderREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        
        private readonly IToolRepo _repository;
        private readonly IMapper _mapper;

        public ToolsController(IToolRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        
        //GET api/tools

        [HttpGet]
        public ActionResult<IEnumerable<ToolReadDto>> GetAllTools()
        {
            var toolItems = _repository.GetAllTools();

            return Ok(_mapper.Map<IEnumerable<ToolReadDto>>(toolItems));
        }

        //GET api/tools/{id}
        //[Authorize]       //Apply this attribute to lockdown this ActionResult (or others)
        [HttpGet("{id}", Name = "GetToolById")]
        public ActionResult<ToolReadDto> GetToolById(int id)
        {
            var toolItem = _repository.GetToolById(id);
            if (toolItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ToolReadDto>(toolItem));
        }

        //POST api/tools/
        [HttpPost]
        public ActionResult<ToolReadDto> CreateTool(ToolCreateDto toolCreateDto)
        {
            var toolModel = _mapper.Map<Tool>(toolCreateDto);
            _repository.CreateTool(toolModel);
            _repository.SaveChanges();

            var toolReadDto = _mapper.Map<ToolReadDto>(toolModel);

            return CreatedAtRoute(nameof(GetToolById), new { Id = toolReadDto.Id }, toolReadDto);
        }

        //PUT api/tools/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateTool(int id, ToolUpdateDto toolUpdateDto)
        {
            var toolModelFromRepo = _repository.GetToolById(id);
            if (toolModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(toolUpdateDto, toolModelFromRepo);

            _repository.UpdateTool(toolModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/tools/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialToolUpdate(int id, JsonPatchDocument<ToolUpdateDto> patchDoc)
        {
            var toolModelFromRepo = _repository.GetToolById(id);
            if (toolModelFromRepo == null)
            {
                return NotFound();
            }

            var toolToPatch = _mapper.Map<ToolUpdateDto>(toolModelFromRepo);
            patchDoc.ApplyTo(toolToPatch, ModelState);
            if (!TryValidateModel(toolToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(toolToPatch, toolModelFromRepo);

            _repository.UpdateTool(toolModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/tools/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTool(int id)
        {
            var toolModelFromRepo = _repository.GetToolById(id);
            if(toolModelFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteTool(toolModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }        
    }
}