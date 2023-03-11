using System.Linq;
using Microsoft.EntityFrameworkCore;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services.DataAccess;

public class WcDbContext : DbContext
{
    public WcDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    ///     Set for the Runs in the database
    /// </summary>
    public DbSet<WcRun> WCRuns
    {
        get => Set<WcRun>();
    }

    /// <summary>
    ///     Set for the flags in the database
    /// </summary>
    public DbSet<Flag> Flags
    {
        get => Set<Flag>();
    }

    /// <summary>
    ///     Protected set to allow characters to be accessed as no tracking
    ///     since it is a read only table
    /// </summary>
    protected DbSet<Character> CharactersProtected
    {
        get => Set<Character>();
    }

    public IQueryable<Character> Characters
    {
        get => CharactersProtected.AsNoTracking();
    }

    /// <summary>
    ///     Protected set to allow abilities to be accessed as no tracking
    ///     since it is a read only table
    /// </summary>
    protected DbSet<Ability> AbilitiesProtected
    {
        get => Set<Ability>();
    }

    public IQueryable<Ability> Abilities
    {
        get => AbilitiesProtected.AsNoTracking();
    }

    /// <summary>
    ///     Override the creation of the models and insert some seed data for
    ///     characters and abilities
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Using properties instead of backing fields when loading entities so their correct
        // validation logic is called on loading
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);
        // Seed data for abilities and characters
        modelBuilder.Entity<Character>().HasData(
            new { Name = "Terra", CharacterId = 1 },
            new { Name = "Locke", CharacterId = 2 },
            new { Name = "Cyan", CharacterId = 3 },
            new { Name = "Shadow", CharacterId = 4 },
            new { Name = "Edgar", CharacterId = 5 },
            new { Name = "Sabin", CharacterId = 6 },
            new { Name = "Celes", CharacterId = 7 },
            new { Name = "Strage", CharacterId = 8 },
            new { Name = "Relm", CharacterId = 9 },
            new { Name = "Setzer", CharacterId = 10 },
            new { Name = "Mog", CharacterId = 11 },
            new { Name = "Gau", CharacterId = 12 },
            new { Name = "Gogo", CharacterId = 13 },
            new { Name = "Umaro", CharacterId = 14 }
        );
        modelBuilder.Entity<Ability>().HasData(
            new { Name = "Blitz", AbilityId = 21 },
            new { Name = "Capture", AbilityId = 1 },
            new { Name = "Control", AbilityId = 2 },
            new { Name = "GP Rain", AbilityId = 3 },
            new { Name = "Dance", AbilityId = 4 },
            new { Name = "Health", AbilityId = 5 },
            new { Name = "Jump", AbilityId = 6 },
            new { Name = "Lore", AbilityId = 7 },
            new { Name = "Morph", AbilityId = 8 },
            new { Name = "Rage", AbilityId = 9 },
            new { Name = "Runic", AbilityId = 10 },
            new { Name = "Sketch", AbilityId = 11 },
            new { Name = "Slot", AbilityId = 12 },
            new { Name = "Steal", AbilityId = 13 },
            new { Name = "SwdTech", AbilityId = 14 },
            new { Name = "Throw", AbilityId = 15 },
            new { Name = "Tools", AbilityId = 16 },
            new { Name = "X Magic", AbilityId = 17 },
            new { Name = "Shock", AbilityId = 18 },
            new { Name = "MagiTek", AbilityId = 19 },
            new { Name = "Possess", AbilityId = 20 }
        );
    }
}