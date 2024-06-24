﻿using MillionsOfThings.Lib;

namespace MillionsOfThings.Lib.Services
{
  [ExcludeFromDiScan]
  public interface IDateTimeService
  {
    DateTime Now { get; }

    DateTime UtcNow { get; }
  }

  [ExcludeFromDiScan]
  public class DateTimeService
    : IDateTimeService
  {
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
  }
}
