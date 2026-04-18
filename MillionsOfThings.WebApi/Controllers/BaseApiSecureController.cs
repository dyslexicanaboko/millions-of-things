using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MillionsOfThings.Lib;
using MillionsOfThings.Lib.Utility;
using System.IdentityModel.Tokens.Jwt;

namespace MillionsOfThings.WebApi.Controllers
{
  [Authorize]
  [ApiController]
  public abstract class BaseApiSecureController
    : ControllerBase
  {
    //REMINDER: You cannot access the HttpContext in the constructor so don't try it will be null.

    private int _userId;

    protected int UserId
    {
      get
      {
        if (_userId == 0)
        {
          _userId = GetUserId();
        }

        return _userId;
      }
    }

    protected int GetUserId()
    {
      if (!Request.Headers.TryGetValue("Authorization", out var headerAuth))
        throw Lib.Exceptions.Unauthorized.NotAuthenticated();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
      var token = headerAuth
        .First()
        .Split([' '], StringSplitOptions.RemoveEmptyEntries)[1];
#pragma warning restore CS8602 // Dereference of a possibly null reference.

      var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

      var claim = jwt.Claims.Single(x => x.Type == Constants.ClaimsUserId);

      return Convert.ToInt32(claim.Value);
    }

    /// <summary>
    /// Extract the Patch Document as a list of properties that are allowed to be updated.
    /// Any unsupported properties are ignored.
    /// </summary>
    /// <param name="patchDocument">Patch document to extract instructions from.</param>
    /// <param name="allowedProperties">List of properties that are allowed to be updated.</param>
    /// <returns>List of update instructions.</returns>
    protected List<UpdateInstruction> GetInstructions(JsonPatchDocument patchDocument, List<string> allowedProperties)
    {
      var instructions = new List<UpdateInstruction>(patchDocument.Operations.Count);

      foreach (var op in patchDocument.Operations)
      {
        //For each item in the operations list (which is what was explicitly provided)
        //Match the incoming operation to the property of the entity in question
        //Create an instruction set
        var prop = allowedProperties.SingleOrDefault(x
          => string.Equals(x, op.path.TrimStart('/'), StringComparison.OrdinalIgnoreCase));

        //If there is no match then skip the property, but this would indicate that there is a problem with the patch document
        //Maybe log this as a problem?
        if (prop == null) continue;

        instructions.Add(new UpdateInstruction(prop, op.value));
      }

      return instructions;
    }
  }
}
