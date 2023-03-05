using System.Collections.Generic;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.DataAccess;

public interface IWcRunRepository : IGenericRepository<WcRun>
{
    IEnumerable<WcRun> GetRunsWithIncludes();
}
