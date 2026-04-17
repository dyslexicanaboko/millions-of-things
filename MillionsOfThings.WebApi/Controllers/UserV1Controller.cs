using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Features.UserF;
using MillionsOfThings.Lib.Features.UserF.Models;
using MillionsOfThings.Lib.Models;
using MillionsOfThings.WebApi.Controllers;

namespace MillionsOfThings.Lib.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController
  : BaseApiSecureController
{
  private readonly IUserMapper _mapper;

  private readonly IUserManager _manager;

  public UserController(
    IUserManager manager,
    IUserMapper mapper)
  {
    _manager = manager;

    _mapper = mapper;
  }

  // GET api/v1/users/5
  [HttpGet("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  public async Task<ActionResult<UserV1Model>> Get(int id)
  {
    var entity = await _manager.Get(id);

    if (entity == null) throw new Exception($"User:{id} not found.");

    return Ok(_mapper.ToModel(entity));
  }

  // GET api/v1/users
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
  public async Task<ActionResult<UserV1Model>> GetAll()
  {
    var entity = await _manager.GetAll();

    return Ok(_mapper.ToModel(entity));
  }

  // POST api/v1/users
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserV1CreatedModel))]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
  public async Task<ActionResult<UserV1CreatedModel>> Post([FromBody] UserV1CreateModel model)
  {
    var entity = _mapper.ToEntity(model);

    if (entity == null) throw new Exception("Model cannot be null.");

    var result = await _manager.Add(entity);

    var m = _mapper.ToModel(result);

    return CreatedAtAction(nameof(Get), new { id = m!.UserId }, m);
  }

  // PATCH api/v1/users/5
  [HttpPatch("{id:int}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
  public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<UserV1PatchModel> patchDoc)
  {
    var db = await _manager.Get(id);

    var model = _mapper.ToPatchModel(db);

    if (model == null) throw new Exception($"User:{id} not found.");

    patchDoc.ApplyTo(model);

    var entity = _mapper.ToEntity(model);

    await _manager.Edit(entity);

    return NoContent();
  }

  // DELETE api/v1/users/5
  [HttpDelete("{id:int}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> Delete(int id)
  {
    await _manager.Remove(id);

    return NoContent();
  }
}