using WCStatsTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace WCStatsTracker.Services;

public class WCDBContext : DbContext
{

    
    public DbSet<WCRun> WCRuns => Set<WCRun>();
    public DbSet<FlagSet> Flags => Set<FlagSet>();

    public WCDBContext(DbContextOptions options) : base(options) { }
}
