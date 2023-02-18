using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;
using WCStatsTracker.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace WCStatsTracker.Services;
public class WCDatabaseService : IDatabaseService
{
    private WCContext _dbContext;

    public WCDatabaseService()
    {
        _dbContext = new WCContext();
        _dbContext.Database.EnsureCreated();
    }
    public void DeleteFlag(FlagSet flag)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteRun(WCRun run)
    {
        throw new System.NotImplementedException();
    }

    public ObservableCollection<FlagSet> GetFlagSet()
    {
        _dbContext.Flags.Load();
        if (_dbContext.Flags.Local.Count == 0)
            return new ObservableCollection<FlagSet>();
        return _dbContext.Flags.Local.ToObservableCollection();
    }

    public ObservableCollection<WCRun> GetWCRuns()
    {
        _dbContext.WCRuns.Load();
        if (_dbContext.Flags.Local.Count == 0)
            return new ObservableCollection<WCRun>();
        return _dbContext.WCRuns.Local.ToObservableCollection();
    }

    public void SaveFlag(FlagSet flagSet)
    {
        throw new System.NotImplementedException();
    }

    public void SaveRun(WCRun run)
    {
        throw new System.NotImplementedException();
    }

    ~WCDatabaseService()
    {
        _dbContext.Dispose();
    }
}
