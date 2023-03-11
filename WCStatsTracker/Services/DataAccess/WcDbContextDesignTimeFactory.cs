using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace WCStatsTracker.Services.DataAccess;

public class WcDbContextDesignTimeFactory : IDesignTimeDbContextFactory<WcDbContext>
{
    public WcDbContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<WcDbContext>();
        var fixedConnectionString = configuration.GetConnectionString("Default")
            .Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        optionsBuilder.UseSqlite(fixedConnectionString);

        return new WcDbContext(optionsBuilder.Options);
    }
}