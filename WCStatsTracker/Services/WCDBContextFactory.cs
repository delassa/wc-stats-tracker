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
    public WCDBContext CreateDbContext(string[]? args = null)
    {
        var options = new DbContextOptionsBuilder<WCDBContext>();
        string FixedConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        options.UseSqlite(FixedConnectionString);
        return new WCDBContext(options.Options);
    }
}
