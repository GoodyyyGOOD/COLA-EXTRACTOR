using System;

namespace Sofos2toDatawarehouseSyncer.Application.Interfaces.Shared
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}