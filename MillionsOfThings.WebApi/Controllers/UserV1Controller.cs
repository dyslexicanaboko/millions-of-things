using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Features.SecurityFeature;
using MillionsOfThings.Lib.Features.UserFeature;
using MillionsOfThings.Lib.Features.UserFeature.Models;
using MillionsOfThings.Lib.Models;

namespace MillionsOfThings.WebApi.Controllers;

[Route("millionsofthings/v1/users")]
[ApiController]
public class UserV1Controller(
  IUserManager manager,
  IUserMapper mapper,
  ISecurityUserMapper securityUserMapper,
  ISecurityUserManager securityUserManager)
  : BaseApiSecureController
{
  // GET millionsofthings/v1/users/5
  [HttpGet("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  public async Task<ActionResult<UserV1Model>> Get(int id)
  {
    var entity = await manager.Get(id);

    if (entity == null) throw Lib.Exceptions.NotFound.User(id);

    return Ok(mapper.ToModel(entity));
  }

  // GET millionsofthings/v1/users
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
  public async Task<ActionResult<UserV1Model>> GetAll()
  {
    //TODO: This has to be paged eventually.
    var entity = await manager.GetAll(CurrentUser.Value);

    return Ok(mapper.ToModel(entity));
  }

  // POST millionsofthings/v1/users
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserV1CreatedModel))]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
  public async Task<ActionResult<UserV1CreatedModel>> Post([FromBody] UserV1CreateModel model, CancellationToken cancellationToken)
  {
    if (model == null) throw Lib.Exceptions.InvalidArgument.Null(nameof(model));

    var result = await securityUserManager.Add(CurrentUser.Value, model, cancellationToken);

    var m = securityUserMapper.ToModel(result);

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
    await manager.Remove(id);

    return NoContent();
  }
}