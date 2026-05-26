using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MillionsOfThings.Lib.Exceptions;
using MillionsOfThings.Lib.Features.SecurityFeature.Models;
using MillionsOfThings.Lib.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MillionsOfThings.Lib.Features.SecurityFeature
{
  public class TokenManager
    : ITokenManager
  {
    private readonly ICryptographyService _authenticationService;

    private readonly IOptions<JwtSettings> _configuration;

    private readonly IDateTimeService _dateTimeService;

    private readonly ILogger<TokenManager> _logger;

    private readonly ISecurityUserRepository _securityUserRepository;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenManager(
      ILogger<TokenManager> logger,
      IOptions<JwtSettings> config,
      IDateTimeService dateTimeService,
      ISecurityUserRepository securityUserRepository,
      IRefreshTokenRepository refreshTokenRepository,
      ICryptographyService authenticationService)
    {
      _logger = logger;
      _configuration = config;
      _dateTimeService = dateTimeService;
      _securityUserRepository = securityUserRepository;
      _refreshTokenRepository = refreshTokenRepository;
      _authenticationService = authenticationService;
    }

    public async Task<JwtTokenV1Model> GetToken(AuthenticationV1PostModel model, string ipAddress)
    {
      var user = await Authenticate(model.Username, model.Password);

      return await GetToken(user, ipAddress);
    }

    private async Task<SecurityUserRecord> Authenticate(string username, string password)
    {
      //Direct Repo access on purpose to have a separation of concerns between the UserService and Authentication
      //The password is needed only in this situation.
      var record = await _securityUserRepository.Read(username);

      //If user isn't found
      if (record == null)
      {
        //throw exception about user not being found
        throw NotFound.UserCredentials();
      }

      //Explicitly denied access
      if (!record.IsAllowed) throw Unauthorized.FailedAuthentication();

      if (!_authenticationService.IsPasswordValid(password, record.Password)) throw Unauthorized.InvalidPassword();

      //There is no conceivable scenario where the encrypted password is needed after this method call, just blank it out
      record.Password = string.Empty;

      return record;
    }

    public async Task<JwtTokenV1Model> GetToken(RefreshTokenV1PostModel model, string ipAddress)
    {
      //Hit the DB one time - lookup user by refresh-token
      var record = await _refreshTokenRepository.Read(model.Token);

      //If the token is not found, an error must be thrown
      if (record == null) throw Unauthorized.FailedAuthentication();

      var refreshToken = new RefreshTokenEntity(record);

      //On the off chance the refresh token has expired
      if (refreshToken.IsExpired()) throw Unauthorized.NotAuthenticated();

      var user = await _securityUserRepository.Read(refreshToken.UserId);

      //TODO: Re-authenticate the user - as in, are they still allowed to login? #26

      //If the user is not found, an error must be thrown
      if (user == null)
      {
        _logger.LogCritical("User not found during token refresh. {RefreshTokenUserId}", refreshToken.UserId);

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
    private async Task<JwtTokenV1Model> GetToken(SecurityUserRecord user, string ipAddress)
    {
      var utcNow = _dateTimeService.UtcNow;
      var offSet = new DateTimeOffset(utcNow);

      var refreshToken = await GenerateRefreshToken(user.UserId, utcNow, ipAddress);

      await _refreshTokenRepository.DeleteExpired(user.UserId);

      //create claims details based on the user information
      var claims = new List<Claim>
      {
        new (JwtRegisteredClaimNames.Sub, _configuration.Value.Subject),
        new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new (JwtRegisteredClaimNames.Iat, offSet.ToUnixTimeSeconds().ToString()),
        new (JwtClaims.RefreshToken, refreshToken.Token),
        new (JwtClaims.UserId, user.UserId.ToString()),
        new (JwtClaims.Name, $"{user.FirstName} {user.LastName}"),
        new (JwtClaims.Username, user.Username),
        new (JwtClaims.Role, user.Role)
      };

      //Get the permissions for this user and include them as claims.
      var permissions = await _securityUserRepository.ReadPermissions(user.SecurityRoleId);

      //Apparently best practice is to add each permission as a separate claim, instead of using a JSON array.
      claims.AddRange(permissions.Select(permission => new Claim(JwtClaims.Permission, permission.Permission)));

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
      var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        _configuration.Value.Issuer,
        _configuration.Value.Audience,
        claims,
        expires: utcNow.AddMinutes(_configuration.Value.ExpirationMinutes),
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
      await _refreshTokenRepository.Create(record);

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
        if (await _refreshTokenRepository.Read(token) != null) continue;

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
        ExpiresOn = utcNow.AddDays(_configuration.Value.RefreshTokenExpirationDays),
        CreatedByIp = ipAddress
      };

      return refreshToken;
    }
  }
}
