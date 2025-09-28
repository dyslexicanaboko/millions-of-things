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
  [Route("api/v1/tasks")]
  public class TaskV1Controller
    : BaseApiSecureController
  {
    private readonly ITaskMapper _mapper;

    private readonly ITaskService _service;

    public TaskV1Controller(
        ITaskService service,
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
      //TODO: Need to verify that the user has access to the requested resource
      var entity = await _service.Get(id, UserId); //TODO: UserId needs to be passed

      if (entity == null) throw Lib.Exceptions.NotFound.Task(id);

      return Ok(_mapper.ToModel(entity));
    }

    // GET api/v1/tasks
    [HttpGet()]
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

      return CreatedAtAction(nameof(Get), new { id = m!.TaskId }, m);
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
    public async Task<ActionResult> Delete(int id)
    {
      await _service.Remove(id);

      return NoContent();
    }
  }
}
