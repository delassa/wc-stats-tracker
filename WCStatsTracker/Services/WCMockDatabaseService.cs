using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WCStatsTracker.Models;
using WCStatsTracker.Utility.Data;
namespace WCStatsTracker.Services;

/// <summary>
///     A Mocking database service to generate data mostly for design time preview
/// </summary>
/// <typeparam name="T">The type of Database Service to mock</typeparam>
public class WCMockDatabaseService<T> : IDatabaseService<T> where T : BaseModelObject
{
    private ObservableCollection<FlagSet> FlagSetCollection;
    private ObservableCollection<WCRun> RunCollection;

    /// <summary>
    ///     Has same signature as the real database service constructor
    /// </summary>
    /// <param name="context">Just a null context that is unused</param>
    public WCMockDatabaseService(WCDBContextFactory? context = null)
    {
        FlagSetCollection = new ObservableCollection<FlagSet>();
        RunCollection = new ObservableCollection<WCRun>();
        GenerateFakeData(20, 30);
    }

    // public Task<bool> Create(T entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<bool> Delete(int id)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<bool> Delete(T entity)
    // {
    //     if (entity is WCRun run) return Task.FromResult(RunCollection.Remove(run));
    //     if (entity is FlagSet flag) return Task.FromResult(FlagSetCollection.Remove(flag));
    //     throw new ArgumentException($"Cannot delete a type of {typeof(T)}.");
    // }
    //
    // public Task<T> Get(int id)
    // {
    //     T? checkType = default;
    //
    //     if (checkType is WCRun)
    //     {
    //         dynamic? run = RunCollection.FirstOrDefault(e => e.Id == id);
    //         if (run is null) throw new ArgumentException($"{id} is not a valid id in FakeRuns");
    //         return Task.FromResult(run);
    //     }
    //
    //     if (checkType is FlagSet)
    //     {
    //         dynamic? flag = FlagSetCollection.FirstOrDefault(e => e.Id == id);
    //         if (flag is null) throw new ArgumentException($"{id} is not a valid id in FakeRuns");
    //         return Task.FromResult(flag);
    //     }
    //
    //     throw new ArgumentException($"Cannot get a type of {typeof(T)}.");
    // }
    //
    //
    // public Task<IList<T>> GetAll()
    // {
    //     T? checkType = default;
    //     if (checkType is IEnumerable<WCRun>)
    //     {
    //         dynamic runs = RunCollection;
    //         return Task.FromResult(runs);
    //     }
    //
    //     if (checkType is IEnumerable<FlagSet>)
    //     {
    //         dynamic flags = FlagSetCollection;
    //         return Task.FromResult(flags);
    //     }
    //
    //     throw new ArgumentException($"Cannot Get All with type of {typeof(T)}.");
    // }
    //
    // public Task<bool> Update(T entity, int id)
    // {
    //     entity.Id = id;
    //     dynamic d = entity;
    //     T? checkType = default;
    //
    //     switch (checkType)
    //     {
    //         case WCRun:
    //         {
    //             var item = RunCollection.First(e => e.Id == id);
    //             var index = RunCollection.IndexOf(item);
    //             if (index != -1)
    //             {
    //                 RunCollection[index] = d;
    //                 return Task.FromResult(true);
    //             }
    //
    //             throw new ArgumentException($"No item found with id : {id} of {typeof(T)}.");
    //         }
    //
    //         case FlagSet:
    //         {
    //             var item = FlagSetCollection.First(e => e.Id == id);
    //             var index = FlagSetCollection.IndexOf(item);
    //             if (index != -1)
    //             {
    //                 FlagSetCollection[index] = d;
    //                 return Task.FromResult(true);
    //             }
    //
    //             throw new ArgumentException($"No item found with id : {id} of {typeof(T)}.");
    //         }
    //         default:
    //         {
    //             throw new ArgumentException($"Cannot Update with type of {typeof(T)}.");
    //         }
    //     }
    // }

    public T Get(int id)
    {
        T? checkType = default;

        if (checkType is WCRun)
        {
            dynamic? run = RunCollection.FirstOrDefault(e => e.Id == id);
            if (run is null) throw new ArgumentException($"{id} is not a valid id in FakeRuns");
            return Task.FromResult(run);
        }

        if (checkType is FlagSet)
        {
            dynamic? flag = FlagSetCollection.FirstOrDefault(e => e.Id == id);
            if (flag is null) throw new ArgumentException($"{id} is not a valid id in FakeRuns");
            return Task.FromResult(flag);
        }

        throw new ArgumentException($"Cannot get a type of {typeof(T)}.");
    }

    public bool Create(T entityToCreate)
    {
        throw new NotImplementedException();

    }

    public bool Delete(T entityToDelete)
    {
        throw new NotImplementedException();
    }

    public bool Update(T entityToUpdate)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<T> GetAll()
    {
        return new List<T>();
    }

    public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        string includeProperties = "")
    {
        return new List<T>();
    }

    /// <summary>
    ///     Loads up the fake data set
    /// </summary>
    /// <param name="FlagCount">Number of flags to generate</param>
    /// <param name="RunCount">Number of runs to generate</param>
    public void GenerateFakeData(int FlagCount, int RunCount)
    {
        FlagSetCollection = (ObservableCollection<FlagSet>)GenerateData.GenerateFlags(FlagCount);
        RunCollection = (ObservableCollection<WCRun>)GenerateData.GenerateRuns(RunCount);
    }
}