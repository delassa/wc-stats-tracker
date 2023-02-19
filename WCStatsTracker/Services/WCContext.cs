using WCStatsTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace WCStatsTracker.Services;

public class WCContext : DbContext
{
    public static string? DbFile => ConfigurationManager.AppSettings["DBName"];
    public DbSet<WCRun> WCRuns => Set<WCRun>();
    public DbSet<FlagSet> Flags => Set<FlagSet>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbFile}");
    }
}
