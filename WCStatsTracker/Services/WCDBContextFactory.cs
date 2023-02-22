using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCStatsTracker.Services;
public class WCDBContextFactory : IDesignTimeDbContextFactory<WCDBContext>
{
    public static string? DbFile => ConfigurationManager.AppSettings["DBName"];
    public WCDBContext CreateDbContext(string[] args = null)
    {
        var options = new DbContextOptionsBuilder<WCDBContext>();
        options.UseSqlite($"Data Source={DbFile}");
        return new WCDBContext(options.Options);
    }
}
