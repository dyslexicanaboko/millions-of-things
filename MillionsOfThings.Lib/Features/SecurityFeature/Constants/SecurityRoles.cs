namespace MillionsOfThings.Lib.Features.SecurityFeature.Constants;

/// <summary>
/// In memory instance of the roles from public.security_roles table.
/// This is being used to validate the role before hitting the database.
/// </summary>
public class SecurityRoles
{
  private SecurityRoles(string name)
  {
    Name = name;
  }

  public string Name { get; }

  public static SecurityRoles Standard => new ("Standard");
  public static SecurityRoles Administrator => new ("Administrator");

  private static readonly Dictionary<string, SecurityRoles> Roles = new(StringComparer.OrdinalIgnoreCase)
  {
    { "Standard", Standard },
    { "Administrator", Administrator }
  };

  public static bool Contains(string role)
    => Roles.ContainsKey(role);
}
