using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

using CommanderREST.Dtos;
using CommanderData.Repositories;
using Microsoft.Extensions.Logging;

namespace CommanderREST.Controllers
{
    [ApiController]
    public class GenericCRUDController<TEntity, TCreateDto, TUpdateDto, TReadDto> : ControllerBase
        where TCreateDto : class
        where TUpdateDto : class
        where TReadDto : class, IReadDto
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
        public ActionResult<IEnumerable<TReadDto>> GetAll()
        {
            _logger.LogInformation("Starting controller action GetAll");
            var entities = _repository.GetAll();

            return Ok(_mapper.Map<IEnumerable<TReadDto>>(entities));
        }

        //GET api/tools/{id}
        [HttpGet("{id}")]
        public ActionResult<TReadDto> Get(int id)
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
        public ActionResult<TReadDto> Create(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            _repository.Add(entity);
            _repository.SaveChanges();

            var readDto = _mapper.Map<TReadDto>(entity);

            return CreatedAtAction(nameof(Get), new { Id = readDto.Id }, readDto);
        }

        //PUT api/tools/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, TUpdateDto updateDto)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, entity);

            _repository.Update(entity);

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/tools/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialUpdate(int id, JsonPatchDocument<TUpdateDto> patchDoc)
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

            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/tools/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            _repository.Remove(entity);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}