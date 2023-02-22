using Avalonia.Controls.Documents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;

namespace WCStatsTracker.Services;

public class WCMockDatabaseService<T> : IDatabaseService<T> where T : BaseModelObject
{
    private ObservableCollection<FlagSet> FakeFlagSet;
    private ObservableCollection<WCRun> FakeRuns;

    public WCMockDatabaseService(WCDBContextFactory context)
    {
        FakeFlagSet = new ObservableCollection<FlagSet>();
        FakeRuns = new ObservableCollection<WCRun>();
    }

    public void GenerateFakeData(int FlagCount, int RunCount)
    {
        FakeFlagSet = (ObservableCollection<FlagSet>)GenerateData.GenerateFlags(FlagCount);
        FakeRuns = (ObservableCollection<WCRun>)GenerateData.GenerateRuns(RunCount);
    }

    public async Task<T> Create(T entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> Delete(T entity)
    {
        WCRun run = entity as WCRun;
        if (run != null)
        {
            return Task.FromResult(FakeRuns.Remove(run));
        }
        FlagSet flag = entity as FlagSet;
        if (flag != null)
        {
            return Task.FromResult(FakeFlagSet.Remove(flag));
        }
        throw new ArgumentException($"Cannot delete a type of {typeof(T)}.");
    }

    public Task<T> Get(int id)
    {
        T checkType = default(T);
        if (checkType is WCRun)
        {
            dynamic run = FakeRuns.FirstOrDefault((e) => e.Id == id);
            if (run is null) { throw new ArgumentException($"{id} is not a valid id in FakeRuns"); }
            return Task.FromResult(run);
        }
        if (checkType is FlagSet)
        {
            dynamic flag = FakeFlagSet.FirstOrDefault((e) => e.Id == id);
            if (flag is null) { throw new ArgumentException($"{id} is not a valid id in FakeRuns"); }
            return Task.FromResult(flag);
        }
        throw new ArgumentException($"Cannot get a type of {typeof(T)}.");
    }


    public Task<IEnumerable<T>> GetAll()
    {
        T checkType = default(T);
        if (checkType is IEnumerable<WCRun>)
        {
            dynamic runs = FakeRuns;
            return Task.FromResult(runs);
        }
        if (checkType is IEnumerable<FlagSet>)
        {
            dynamic flags = FakeFlagSet;
            return Task.FromResult(flags);
        }
        throw new ArgumentException($"Cannot Get All with type of {typeof(T)}.");
    }

    public Task<T> Update(T entity, int id)
    {
        entity.Id = id;
        dynamic d = entity;
        T checkType = default(T);
        if (checkType is WCRun)
        {
            var item = FakeRuns.FirstOrDefault((e) => e.Id == id);
            var index = FakeRuns.IndexOf(item);
            if (index != -1) 
            {
                FakeRuns[index] = entity as WCRun;
                return Task.FromResult(d);
            }
            throw new ArgumentException($"No item found with id : {id} of {typeof(T)}.");
        }
        if (checkType is FlagSet)
        {
            var item = FakeFlagSet.FirstOrDefault((e) => e.Id == id);
            var index = FakeFlagSet.IndexOf(item);
            if (index != -1) 
            {
                FakeFlagSet[index] = entity as FlagSet; 
                return Task.FromResult(d);
            }
            throw new ArgumentException($"No item found with id : {id} of {typeof(T)}.");
        }
        throw new ArgumentException($"Cannot Update with type of {typeof(T)}.");
    }
}
