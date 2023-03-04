using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace WCStatsTracker.Services.DataAccess;

public class WcDbContextDesignTimeFactory : IDesignTimeDbContextFactory<WcDbContext>
{
    public WcDbContext CreateDbContext(string[] args = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WcDbContext>();
        var fixedConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString
            .Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        optionsBuilder.UseSqlite(fixedConnectionString);

        return new WcDbContext(optionsBuilder.Options);
    }
}