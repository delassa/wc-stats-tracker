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

    public DbSet<Character> Characters
    {
        get => Set<Character>();
    }

    public DbSet<Ability> Abilities
    {
        get => Set<Ability>();
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
            new { Name = "Strago", CharacterId = 8 },
            new { Name = "Relm", CharacterId = 9 },
            new { Name = "Setzer", CharacterId = 10 },
            new { Name = "Mog", CharacterId = 11 },
            new { Name = "Gau", CharacterId = 12 },
            new { Name = "Gogo", CharacterId = 13 },
            new { Name = "Umaro", CharacterId = 14 }
        );
        modelBuilder.Entity<Ability>().HasData(
            new { Name = "Blitz", AbilityId = 1 },
            new { Name = "Capture", AbilityId = 2 },
            new { Name = "Control", AbilityId = 3 },
            new { Name = "GP Rain", AbilityId = 4 },
            new { Name = "Dance", AbilityId = 5 },
            new { Name = "Health", AbilityId = 6 },
            new { Name = "Jump", AbilityId = 7 },
            new { Name = "Lore", AbilityId = 8 },
            new { Name = "Morph", AbilityId = 9 },
            new { Name = "Rage", AbilityId = 10 },
            new { Name = "Runic", AbilityId = 11 },
            new { Name = "Sketch", AbilityId = 12 },
            new { Name = "Slot", AbilityId = 13 },
            new { Name = "Steal", AbilityId = 14 },
            new { Name = "SwdTech", AbilityId = 15 },
            new { Name = "Throw", AbilityId = 16 },
            new { Name = "Tools", AbilityId = 17 },
            new { Name = "X Magic", AbilityId = 18 },
            new { Name = "Shock", AbilityId = 19 },
            new { Name = "MagiTek", AbilityId = 20 },
            new { Name = "Possess", AbilityId = 21 }
        );
    }
}
