using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Features;
using MillionsOfThings.Lib.Features.TaskF;
using MillionsOfThings.Lib.Features.TaskF.Models;
using MillionsOfThings.Lib.Models;

namespace MillionsOfThings.WebApi.Controllers
{
  [Route("api/v1/tasks")]
  public class TaskV1Controller
    : BaseApiSecureController
  {
    private readonly ITaskMapper _mapper;

    private readonly ITaskManager _service;

    public TaskV1Controller(
      ITaskManager service,
      ITaskMapper mapper)
    {
      _service = service;

      _mapper = mapper;
    }

    // GET api/v1/tasks/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ITask))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public async Task<ActionResult<ITask>> Get(int id)
    {
      //Null will be returned if the user does not have access to the resource
      var entity = await _service.Get(UserId, id);

      if (entity == null) throw Lib.Exceptions.NotFound.Task(id);

      return Ok(_mapper.ToModel(entity));
    }

    // GET api/v1/tasks
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ITask))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public async Task<ActionResult<List<ITask>>> GetAll()
    {
      var entity = await _service.GetAll(UserId);

      return Ok(_mapper.ToModel(entity));
    }

    // POST api/v1/tasks
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ITask))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public async Task<ActionResult<TaskEntity>> Post([FromBody] TaskV1CreateModel model)
    {
      var entity = _mapper.ToEntity(UserId, model);

      Validations.IsNotNull(entity, nameof(model));

      var result = await _service.Add(entity);

      var m = _mapper.ToModel(result);

      return CreatedAtAction(nameof(Get), new { id = m.TaskId }, m);
    }

    // PATCH api/v1/tasks/5
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<TaskV1PatchModel> patchDoc)
    {
      await _service.EditPartial(UserId, id, patchDoc);

      return NoContent();
    }

    // DELETE api/v1/tasks/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
      await _service.Remove(UserId, id);

      return NoContent();
    }
  }
}
