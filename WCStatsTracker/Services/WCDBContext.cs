using Microsoft.EntityFrameworkCore;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services;

public class WCDBContext : DbContext
{
    public WCDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WCRun> WCRuns
    {
        get => Set<WCRun>();
    }

    public DbSet<FlagSet> Flags
    {
        get => Set<FlagSet>();
    }
}