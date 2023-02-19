using WCStatsTracker.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace WCStatsTracker.Services;
/// <summary>
/// Class to handle the database insertions deletions and updates
/// </summary>
public class WCDatabaseService : IDatabaseService
{
    private WCContext _dbContext;

    public WCDatabaseService()
    {
        _dbContext = new WCContext();
        _dbContext.Database.EnsureCreated();
    }

    public void DeleteRun(WCRun run)
    {
        _dbContext.WCRuns.Remove(run);
        _dbContext.SaveChanges();
    }
    public void SaveRun(WCRun run)
    {
        _dbContext.WCRuns.Add(run);
        _dbContext.SaveChanges();
    }
    public void DeleteFlag(FlagSet flagSet)
    {
        _dbContext.Remove(flagSet);
        _dbContext.SaveChanges();
    }
    public void SaveFlag(FlagSet flagSet)
    {
        _dbContext.Add(flagSet);
        _dbContext.SaveChanges();
    }

    public ObservableCollection<FlagSet> GetFlagSet()
    {
        _dbContext.Flags.Load();
        return _dbContext.Flags.Local.ToObservableCollection();
    }

    public ObservableCollection<WCRun> GetWCRuns()
    {
        _dbContext.WCRuns.Load();
        return _dbContext.WCRuns.Local.ToObservableCollection();
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
    ~WCDatabaseService()
    {
        _dbContext.Dispose();
    }
}
