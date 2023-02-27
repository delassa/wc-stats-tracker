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
    protected DbSet<otected { get => Set<blic IQueryable<> Abilities.AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>().HasData(
            new { Name = "Terra" },
            new { Name = "Locke" },
            new { Name = "Cyan" },
            new { Name = "Shadow" },
            new { Name = "Edgar" },
            new { Name = "Sabin" },
            new { Name = "Celes" },
            new { Name = "Strage" },
            new { Name = "Relm" },
            new { Name = "Setzer" },
            new { Name = "Mog" },
            new { Name = "Gau" },
            new { Name = "Gogo" },
            new { Name = "Umaro" }
        );
        modelBuilder.Entity<Ability>().HasData(
            new { Name = "Blitz" },
            new { Name = "Capture" },
            new { Name = "Control" },
            new { Name = "GP Rain" },
            new { Name = "Dance" },
            new { Name = "Health" },
            new { Name = "Jump" },
            new { Name = "Lore" },
            new { Name = "Morph" },
            new { Name = "Rage" },
            new { Name = "Runic" },
            new { Name = "Sketch" },
            new { Name = "Slot" },
            new { Name = "Steal" },
            new { Name = "SwdTech" },
            new { Name = "Throw" },
            new { Name = "Tools" },
            new { Name = "X Magic" },
            new { Name = "Shock" },
            new { Name = "MagiTek" },
            new { Name = "Possess" }
        );
    }
}