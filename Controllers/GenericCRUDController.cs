using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

using CommanderREST.Dtos;
using CommanderData.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CommanderREST.Controllers
{
    [ApiController]
    public class GenericCRUDController<TEntity, TCreateDto, TUpdateDto, TReadDto> : ControllerBase
        where TCreateDto : class
        where TUpdateDto : class, IIdDto
        where TReadDto : class, IIdDto
    {
        private readonly ILogger<GenericCRUDController<TEntity, TCreateDto, TUpdateDto, TReadDto>> _logger;
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericCRUDController(ILogger<GenericCRUDController<TEntity, TCreateDto, TUpdateDto, TReadDto>> logger, IRepository<TEntity> repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/tools
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TReadDto>>> GetAll()
        {
            _logger.LogInformation("Starting controller action GetAll");
            var entities = _repository.GetAll();

            return Ok(_mapper.Map<IEnumerable<TReadDto>>(entities));
        }

        //GET api/tools/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TReadDto>> Get(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TReadDto>(entity));
        }

        //POST api/tools/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TReadDto>> Create(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            _repository.Add(entity);
            var t = _repository.SaveChangesAsync();
            await t;

            var readDto = _mapper.Map<TReadDto>(entity);

            return CreatedAtAction(nameof(Get), new { Id = readDto.Id }, readDto);
        }

        //PUT api/tools/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id, TUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, entity);

            _repository.Update(entity);
            
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        //PATCH api/tools/{id}
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<TUpdateDto> patchDoc)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var enitityToPatch = _mapper.Map<TUpdateDto>(entity);
            patchDoc.ApplyTo(enitityToPatch, ModelState);
            if (!TryValidateModel(enitityToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(enitityToPatch, entity);

            _repository.Update(entity);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        //DELETE api/tools/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            _repository.Remove(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}