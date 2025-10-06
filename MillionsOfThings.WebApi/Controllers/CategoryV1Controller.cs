using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Mappers;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.Lib.Models.Client;
using MillionsOfThings.Lib.Services;
using MillionsOfThings.Lib.Validation;
using E = MillionsOfThings.Lib.Exceptions;

namespace MillionsOfThings.WebApi.Controllers
{
  [Route("api/v1/categories")]
  public class CategoryV1Controller
    : BaseApiSecureController
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

    // GET api/v1/categories/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICategory))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public async Task<ActionResult<CategoryV1Model>> Get(int id)
    {
      var entity = await _service.Get(UserId, id);

      if (entity == null) throw E.NotFound.Category(id);

      return Ok(_mapper.ToModel(entity));
    }

    // GET api/v1/categories
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICategory))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public async Task<ActionResult<CategoryV1Model>> GetAll()
    {
      var lst = await _service.GetAll(UserId);

      return Ok(_mapper.ToModel(lst));
    }

    // POST api/v1/categories
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

    // PATCH api/v1/categories/5
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<CategoryV1PatchModel> patchDoc)
    {
      var db = await _service.Get(UserId, id);

      var model = _mapper.ToPatchModel(db);

      if (model == null) throw E.NotFound.Category(id);

      patchDoc.ApplyTo(model);

      var entity = _mapper.ToEntity(UserId, id, model);

      await _service.Edit(entity);

      return NoContent();
    }

    // DELETE api/v1/categories/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
      await _service.Remove(UserId, id);

      return NoContent();
    }
  }
}
