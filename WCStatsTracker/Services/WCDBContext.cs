using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services;

public class WCDBContext : DbContext
{
    public WCDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WCRun> WCRuns { get => Set<WCRun>(); }

    public DbSet<FlagSet> Flags { get => Set<FlagSet>(); }

    /// <summary>
    /// Protected set to allow characters to be accessed as no tracking
    /// since it is a read only table
    /// </summary>
    protected DbSet<Character> CharactersProtected { get => Set<Character>(); }
    public IQueryable<Character> Characters => CharactersProtected.AsNoTracking();
    /// <summary>
    /// Protected set to allow abilities to be accessed as no tracking
    /// since it is a read only table
    /// </summary>
    protected DbSet<Ability> AbilitiesProtected { get => Set<Ability>(); }
    public IQueryable<Ability> Abilities => AbilitiesProtected.AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>().HasData(
            new { Name = "Terra", Id = 1},
            new { Name = "Locke", Id = 2 },
            new { Name = "Cyan", Id = 3 },
            new { Name = "Shadow", Id = 4 },
            new { Name = "Edgar", Id = 5 },
            new { Name = "Sabin", Id = 6 },
            new { Name = "Celes", Id = 7 },
            new { Name = "Strage", Id = 8 },
            new { Name = "Relm", Id = 9 },
            new { Name = "Setzer", Id = 10 },
            new { Name = "Mog", Id = 11 },
            new { Name = "Gau", Id = 12 },
            new { Name = "Gogo", Id = 13 },
            new { Name = "Umaro" , Id = 14 }
        );
        modelBuilder.Entity<Ability>().HasData(
            new { Name = "Blitz", Id = 21 },
            new { Name = "Capture", Id = 1 },
            new { Name = "Control", Id = 2 },
            new { Name = "GP Rain", Id = 3 },
            new { Name = "Dance", Id = 4 },
            new { Name = "Health", Id = 5 },
            new { Name = "Jump", Id = 6 },
            new { Name = "Lore", Id = 7 },
            new { Name = "Morph", Id = 8 },
            new { Name = "Rage", Id = 9 },
            new { Name = "Runic", Id = 10 },
            new { Name = "Sketch", Id = 11 },
            new { Name = "Slot", Id = 12 },
            new { Name = "Steal", Id = 13 },
            new { Name = "SwdTech", Id = 14 },
            new { Name = "Throw", Id = 15 },
            new { Name = "Tools", Id = 16 },
            new { Name = "X Magic", Id = 17 },
            new { Name = "Shock", Id = 18 },
            new { Name = "MagiTek", Id = 19 },
            new { Name = "Possess", Id = 20 }
        );
    }
}