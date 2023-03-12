using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.DataAccess;

/// <summary>
///     Repository for WcRun objects
///     Implements specific logic for accessing WcRuns
/// </summary>
public class WcRunRepository : GenericRepository<WcRun>, IWcRunRepository
{
    /// <summary>
    ///     The DbContext for this repository
    /// </summary>
    private readonly WcDbContext _context;


    public WcRunRepository(WcDbContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<WcRun> GetRunsWithIncludes()
    {
        var runsWithIncludes = _context.WCRuns
            .Include(u => u.Flag)
            .Include(u => u.Abilities)
            .Include(u => u.Characters);
        return runsWithIncludes;
    }
}
