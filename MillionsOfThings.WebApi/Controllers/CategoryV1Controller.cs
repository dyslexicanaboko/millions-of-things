using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Mappers;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;
using MillionsOfThings.Lib.Services;
using MillionsOfThings.Lib.Validation;

namespace MillionsOfThings.WebApi.Controllers
{
    [Route("api/v1/category")]
    [ApiController]
    public class CategoryV1Controller
        : AppBaseController
    {
        private readonly ICategoryMapper _mapper;

        private readonly ICategoryService _service;

        public CategoryV1Controller(
            ICategoryService service,
            ICategoryMapper mapper)
        {
            _service = service;

            _mapper = mapper;
        }

        // GET api/category/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICategory))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
        public async Task<ActionResult<ICategory>> Get(int id)
        {
            var entity = await _service.GetCategory(id);

            if (entity == null) throw Lib.Exceptions.NotFound.Category(id);

            return Ok(_mapper.ToModel(entity));
        }

        // POST api/category
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ICategory))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
        public async Task<ActionResult<CategoryV1Model>> Post([FromBody] CategoryV1CreateModel model)
        {
            var entity = _mapper.ToEntity(UserId, model);

            Validations.IsNotNull(entity, nameof(model));

            var result = await _service.Add(entity);

            var m = _mapper.ToModel(result);

            return CreatedAtAction(nameof(Get), new { id = m!.CategoryId }, m);
        }

        // PATCH api/category/5
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<CategoryV1PatchModel> patchDoc)
        {
            var db = await _service.GetCategory(id);

            var model = _mapper.ToPatchModel(db);

            if (model == null) throw Lib.Exceptions.NotFound.Category(id);

            patchDoc.ApplyTo(model);

            var entity = _mapper.ToEntity(model);

            await _service.Edit(entity);

            return NoContent();
        }

        // DELETE api/category/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.Remove(id);

            return NoContent();
        }
    }
}
