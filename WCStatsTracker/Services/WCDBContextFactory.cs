using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace WCStatsTracker.Services;

public class WCDBContextFactory : IDesignTimeDbContextFactory<WCDBContext>
{
    public WCDBContext CreateDbContext(string[]? args = null)
    {
        var options = new DbContextOptionsBuilder<WCDBContext>().LogTo(message => Debug.WriteLine(message)).EnableSensitiveDataLogging();
        var FixedConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString
            .Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        options.UseSqlite(FixedConnectionString);
        return new WCDBContext(options.Options);
    }
}