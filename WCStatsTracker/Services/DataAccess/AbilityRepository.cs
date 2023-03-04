using WCStatsTracker.Models;
namespace WCStatsTracker.Services.DataAccess;

public class AbilityRepository : GenericRepository<Ability>, IAbilityRepository
{
    public AbilityRepository(WcDbContext context) : base(context)
    {
    }
}
