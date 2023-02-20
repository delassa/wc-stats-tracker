using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Views;

namespace WCStatsTracker.Services;

public interface IDatabaseService
{
    ObservableCollection<WCRun> GetWCRuns();
    void SaveRun(WCRun run);
    void DeleteRun(WCRun run);
    ObservableCollection<FlagSet> GetFlagSets();
    void SaveFlag(FlagSet flagSet);
    void DeleteFlag(FlagSet flag);
    void Save();
}
