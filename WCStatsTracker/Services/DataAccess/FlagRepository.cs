using WCStatsTracker.Models;
namespace WCStatsTracker.Services.DataAccess;

public class FlagRepository : GenericRepository<Flag>, IFlagRepository
{
    public FlagRepository(WcDbContext context) : base(context)
    {
    }
}
