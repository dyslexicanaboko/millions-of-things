using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.UserF;

public class UserManager 
  : BaseManager, IUserManager
{
  private readonly IUserRepository _repoUser;

  private readonly IUserMapper _mapper;

  public UserManager(
    IUserRepository repoUser,
    IUserMapper mapper)
  {
    _repoUser = repoUser;
    _mapper = mapper;
  }

  //Password is purposely not returned
  public async Task<UserEntity?> Get(int id)
    => _mapper.ToEntity(await _repoUser.Read(id));

  //Password is purposely not returned
  public async Task<List<UserEntity>> GetAll()
    => _mapper.ToList(await _repoUser.ReadAll());

  public async Task<UserEntity> Add(UserEntity? user)
  {
    Validations.IsNotNull(user, nameof(user));

    user.UserId = await _repoUser.Create(_mapper.ToRecord(user));

    return user;
  }

  public async Task PartialEdit(int userId, List<UpdateInstruction> instructions)
  {
    if(instructions == null || instructions.Count == 0) 
      throw Exceptions.InvalidArgument.Null(nameof(instructions));

    var db = await Get(userId);

    if (db == null) Exceptions.NotFound.User(userId);

    await _repoUser.UpdatePartial(userId, GetDifferences(db!, instructions));
  }

  //TODO: Permissions required
  // 1. Administrator can remove anyone except the main administrator's account
  // 2. User can remove themselves only
  public async Task Remove(int id)
    => await _repoUser.Delete(id);
}