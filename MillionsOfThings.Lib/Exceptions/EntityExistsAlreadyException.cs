namespace MillionsOfThings.Lib.Exceptions
{
  public abstract class EntityExistsAlreadyException
    : BadRequestException
  {
    protected EntityExistsAlreadyException(string message)
      : base(message)
    {
    }

    public override int ErrorCode { get; set; } = ErrorCodes.BadRequest.EntityExistsAlready;
  }
}
