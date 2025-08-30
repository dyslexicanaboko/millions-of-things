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
  [Route("api/v1/task")]
  [ApiController]
  public class TaskV1Controller
      : AppBaseController
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

    // GET api/task/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ITask))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public async Task<ActionResult<ITask>> Get(int id)
    {
      //TODO: Need to verify that the user has access to the requested resource
      var entity = await _service.GetTask(id); //TODO: UserId needs to be passed

      if (entity == null) throw Lib.Exceptions.NotFound.Task(id);

      return Ok(_mapper.ToModel(entity));
    }

    // POST api/task
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

    // PATCH api/task/5
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<TaskV1PatchModel> patchDoc)
    {
      //TODO: Need more sophisticated patching that only updates what has changed #23
      //TODO: Needs proper validation #24
      var db = await _service.GetTask(id);

      var model = _mapper.ToPatchModel(db);

      if (model == null) throw Lib.Exceptions.NotFound.Task(id);

      patchDoc.ApplyTo(model);

      var entity = _mapper.ToEntity(UserId, model);

      await _service.Edit(entity);

      return NoContent();
    }

    // DELETE api/task/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
      await _service.Remove(id);

      return NoContent();
    }
  }
}
