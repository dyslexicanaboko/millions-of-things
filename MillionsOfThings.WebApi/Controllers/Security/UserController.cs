using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Features.UserF;
using MillionsOfThings.Lib.Features.UserF.Models;
using MillionsOfThings.Lib.Models;

namespace MillionsOfThings.WebApi.Controllers.Security
{
  [Route("api/v1/users")]
  public class UserController
    : BaseApiSecureController
  {
    private readonly IUserMapper _mapper;

    private readonly IUserManager _service;

    public UserController(
      IUserManager service,
      IUserMapper mapper)
    {
      _service = service;

      _mapper = mapper;
    }

    // GET api/v1/users/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserV1Model))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
    public async Task<ActionResult<UserV1Model>> Get(int id)
    {
      var entity = await _service.Get(id);

      if (entity == null) throw Lib.Exceptions.NotFound.User(id);

      return Ok(_mapper.ToModel(entity));
    }

    // GET api/v1/users
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<UserV1Model>))]
    public async Task<ActionResult<List<UserV1Model>>> GetAll()
    {
      var lst = await _service.GetAll();

      return Ok(_mapper.ToModel(lst));
    }

    // POST api/v1/users
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserV1CreatedModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorModel))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorModel))]
    public async Task<ActionResult<UserV1CreatedModel>> Post([FromBody] UserV1CreateModel model)
    {
      var entity = await _service.Add(_mapper.ToEntity(model));

      var m = _mapper.ToCreatedModel(entity);

      return CreatedAtAction(nameof(Get), new { id = m!.UserId }, m);
    }

    //No PATCH or DELETE endpoints for now on purpose
  }
}
