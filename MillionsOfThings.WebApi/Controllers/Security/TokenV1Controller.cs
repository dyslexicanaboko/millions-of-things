using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib.Features;
using MillionsOfThings.Lib.Features.SecurityFeature;
using MillionsOfThings.Lib.Features.SecurityFeature.Models;

namespace MillionsOfThings.WebApi.Controllers.Security
{
  [Route("millionsofthings/v1/token")]
  [ApiController]
  public class TokenV1Controller : Controller
  {
    private readonly ITokenManager _service;

    public TokenV1Controller(ITokenManager service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Post(AuthenticationV1PostModel? model, CancellationToken cancellationToken)
    {
      if (model == null) throw Validations.IsMalformedModel();

      var token = await _service.GetToken(model, GetIpAddress(), cancellationToken);

      return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Post(RefreshTokenV1PostModel? model, CancellationToken cancellationToken)
    {
      if (model == null) throw Validations.IsMalformedModel();

      var token = await _service.GetToken(model, GetIpAddress(), cancellationToken);

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
