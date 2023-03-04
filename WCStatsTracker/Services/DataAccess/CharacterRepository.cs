using WCStatsTracker.Models;
namespace WCStatsTracker.Services.DataAccess;

public class CharacterRepository : GenericRepository<Character>, ICharacterRepository
{
    public CharacterRepository(WcDbContext context) : base(context)
    {
    }
}
