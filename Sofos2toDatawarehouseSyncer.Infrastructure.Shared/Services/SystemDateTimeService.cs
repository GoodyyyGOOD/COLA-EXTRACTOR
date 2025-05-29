using Sofos2toDatawarehouseSyncer.Application.Interfaces.Shared;
using System;

namespace Sofos2toDatawarehouseSyncer.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}