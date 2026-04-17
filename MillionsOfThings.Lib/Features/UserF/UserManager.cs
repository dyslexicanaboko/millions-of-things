namespace MillionsOfThings.Lib.Features.UserF
{
  public class UserManager : IUserManager
  {
    private readonly IUserRepository _repoUser;

    private readonly IUserMapper _mapper;

    public UserManager(
      IUserRepository repoUser, IUserMapper mapper)
    {
      _repoUser = repoUser;
      _mapper = mapper;
    }

    //Password is purposely not returned
    public async Task<UserEntity?> Get(int id)
      => _mapper.ToEntity(await _repoUser.Select(id));

    //Password is purposely not returned
    public async Task<List<UserEntity>> GetAll()
      => _mapper.ToList(await _repoUser.SelectAll());

    public async Task<UserEntity> Add(UserEntity? user)
    {
      Validations.IsNotNull(user, nameof(user));

      user.UserId = await _repoUser.Insert(_mapper.ToRecord(user));

      return user;
    }
  }
}
