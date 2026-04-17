using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MillionsOfThings.Lib.Exceptions;
using MillionsOfThings.Lib.Features.Security.Models;
using MillionsOfThings.Lib.Features.UserF;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MillionsOfThings.Lib.Features.Security
{
  public class TokenService
    : ITokenService
  {
    private readonly IAuthenticationService _authenticationService;

    private readonly IConfiguration _configuration;

    private readonly IDateTimeService _dateTimeService;

    private readonly ILogger<TokenService> _logger;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private readonly IUserManager _userService;

    public TokenService(
      ILogger<TokenService> logger,
      IConfiguration config,
      IDateTimeService dateTimeService,
      IUserManager userService,
      IRefreshTokenRepository refreshTokenRepository,
      IAuthenticationService authenticationService)
    {
      _logger = logger;
      _configuration = config;
      _dateTimeService = dateTimeService;
      _userService = userService;
      _refreshTokenRepository = refreshTokenRepository;
      _authenticationService = authenticationService;
    }

    public async Task<JwtTokenV1Model> GetToken(AuthenticationV1PostModel model, string ipAddress)
    {
      var user = await _authenticationService.Authenticate(model.Username, model.Password);

      return await GetToken(user, ipAddress);
    }

    public async Task<JwtTokenV1Model> GetToken(RefreshTokenV1PostModel model, string ipAddress)
    {
      //Hit the DB one time - lookup user by refresh-token
      var record = await _refreshTokenRepository.Select(model.Token);

      //If the token is not found, an error must be thrown
      if (record == null) throw Unauthorized.FailedAuthentication();

      var refreshToken = new RefreshTokenEntity(record);

      //On the off chance the refresh token has expired
      if (refreshToken.IsExpired()) throw Unauthorized.NotAuthenticated();

      var user = await _userService.Get(refreshToken.UserId);

      //TODO: Re-authenticate the user - as in, are they still allowed to login? #26

      //If the user is not found, an error must be thrown
      if (user == null)
      {
        _logger.LogCritical($"User not found during token refresh. {refreshToken.UserId}");

        throw Unauthorized.FailedAuthentication();
      }

      var token = await GetToken(user, ipAddress);

      //Only delete if and only if the token is returned successfully
      await _refreshTokenRepository.Delete(user.UserId, refreshToken.Token);

      return token;
    }

    //NOTE: You have to make sure that every part of the JWT adheres to the standard
    // Otherwise you can get an error like this: `IDX14101: Unable to decode the payload as Base64Url encoded string.`
    // And authentication will fail. In my case `iat` was being sent as a date instead of a long integer.
    private async Task<JwtTokenV1Model> GetToken(UserEntity user, string ipAddress)
    {
      var utcNow = _dateTimeService.UtcNow;
      var offSet = new DateTimeOffset(utcNow);

      var refreshToken = await GenerateRefreshToken(user.UserId, utcNow, ipAddress);

      await _refreshTokenRepository.DeleteExpired(user.UserId);

      //create claims details based on the user information
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, offSet.ToUnixTimeSeconds().ToString()),
        new Claim(Constants.RefreshToken, refreshToken.Token),
        new Claim(Constants.ClaimsUserId, user.UserId.ToString()),
        new Claim(Constants.Name, "TODO: Name is not implemented yet"),
        new Claim(Constants.Username, user.Username),
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        _configuration["Jwt:Issuer"],
        _configuration["Jwt:Audience"],
        claims,
        expires: utcNow.AddMinutes(10),
        signingCredentials: signIn);

      var jwt = new JwtSecurityTokenHandler().WriteToken(token);

      var record = new RefreshTokenRecord
      {
        RefreshTokenId = refreshToken.RefreshTokenId,
        UserId = refreshToken.UserId,
        Token = refreshToken.Token,
        CreatedOn = refreshToken.CreatedOn,
        ExpiresOn = refreshToken.ExpiresOn,
        CreatedByIp = refreshToken.CreatedByIp
      };

      //Only record the new refresh token after we have a successful generation
      await _refreshTokenRepository.Insert(record);

      return new JwtTokenV1Model(jwt, Convert.ToInt32((token.ValidTo - utcNow).TotalSeconds));
    }

    //https://github.com/cornflourblue/dotnet-6-jwt-refresh-tokens-api
    private async Task<string> GetUniqueToken()
    {
      //TODO: Run away train situation? Probably very unlikely to be a problem
      while (true)
      {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        // ensure token is unique by checking against db
        if ((await _refreshTokenRepository.Select(token)) != null) continue;

        return token;
      }
    }

    private async Task<RefreshTokenEntity> GenerateRefreshToken(int userId, DateTime utcNow, string ipAddress)
    {
      var refreshToken = new RefreshTokenEntity
      {
        RefreshTokenId = Guid.NewGuid(),
        UserId = userId,
        Token = await GetUniqueToken(),
        CreatedOn = utcNow,
        ExpiresOn = utcNow.AddDays(7),
        CreatedByIp = ipAddress
      };

      return refreshToken;
    }
  }
}
