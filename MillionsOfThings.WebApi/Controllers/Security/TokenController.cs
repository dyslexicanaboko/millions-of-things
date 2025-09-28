using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Models.Client;
using MillionsOfThings.Lib.Services.Security;
using MillionsOfThings.Lib.Validation;

namespace MillionsOfThings.WebApi.Controllers.Security
{
  [Route("api/token")]
  [ApiController]
  public class TokenController : Controller
  {
    private readonly ITokenService _service;

    public TokenController(ITokenService service)
    {
      _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post(AuthenticationV1PostModel? model)
    {
      if (model is not { Username: { }, Password: { } }) throw Validations.IsMalformedModel();

      var token = await _service.GetToken(model, GetIpAddress());

      return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Post(RefreshTokenV1PostModel? model)
    {
      if (model is not { Token: { } }) throw Validations.IsMalformedModel();

      var token = await _service.GetToken(model, GetIpAddress());

      return Ok(token);
    }

    private string GetIpAddress()
    {
      if (Request.Headers.ContainsKey("X-Forwarded-For")) 
        return Convert.ToString(Request.Headers["X-Forwarded-For"]);

      var ip = HttpContext.Connection.RemoteIpAddress;

      return ip == null ? string.Empty : ip.MapToIPv4().ToString();
    }
  }
}
