namespace MillionsOfThings.Lib.Features.UserFeature.Models
{
  public class UserV1CreateModel
  {
    public UserV1CreateModel(string name)
    {
      Name = name;
    }

    public string Name { get; set; }
  }
}
