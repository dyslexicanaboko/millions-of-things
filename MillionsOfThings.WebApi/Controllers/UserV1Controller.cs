using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Features.UserFeature;
using MillionsOfThings.Lib.Features.UserFeature.Models;
using MillionsOfThings.Lib.Models;

namespace MillionsOfThings.WebApi.Controllers;

[Route("millionsofthings/v1/users")]
[ApiController]
public class UserV1Controller
  : BaseApiSecureController
{
  private readonly IUserMapper _mapper;

  private readonly IUserManager _manager;

  public UserV1Controller(
    IUserManager manager,
    IUserMapper mapper)
  {
    _manager = manager;

    _mapper = mapper;
  }

  // GET millionsofthings/v1/users/5
  [HttpGet("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  public async Task<ActionResult<UserV1Model>> Get(int id)
  {
    var entity = await _manager.Get(id);

    if (entity == null) throw Lib.Exceptions.NotFound.User(id);

    return Ok(_mapper.ToModel(entity));
  }

  // GET millionsofthings/v1/users
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
  public async Task<ActionResult<UserV1Model>> GetAll()
  {
    //TODO: This has to be paged eventually.
    var entity = await _manager.GetAll(CurrentUser.Value);

    return Ok(_mapper.ToModel(entity));
  }

  // POST millionsofthings/v1/users
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserV1CreatedModel))]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
  public async Task<ActionResult<UserV1CreatedModel>> Post([FromBody] UserV1CreateModel model)
  {
    //TODO: Remap all of this to the SecurityUserManager.
    var entity = _mapper.ToEntity(model);

    if (entity == null) throw Lib.Exceptions.InvalidArgument.Null(nameof(model));

    var result = await _manager.Add(entity);

    var m = _mapper.ToModel(result);

    return CreatedAtAction(nameof(Get), new { id = m!.UserId }, m);
  }

  // PATCH millionsofthings/v1/users/5
  [HttpPatch("{id:int}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
  public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument patchDoc)
  {
    var instructions = GetInstructions(patchDoc, [
      "IsAllowed", 
      "Password", 
      "FirstName", 
      "LastName", 
      "EmailAddress"]);

    //TODO: Continue working this out.

    return NoContent();
  }

  // DELETE millionsofthings/v1/users/5
  [HttpDelete("{id:int}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> Delete(int id)
  {
    await _manager.Remove(id);

    return NoContent();
  }
}