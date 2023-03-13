using System;
namespace WCStatsTracker.Services.DataAccess;

public interface IUnitOfWork : IDisposable
{
    WcDbContext Context { get; }
    IAbilityRepository Ability { get; }
    ICharacterRepository Character { get; }
    IFlagRepository Flag { get; }
    IWcRunRepository WcRun { get; }

    int Save();

    void Clear();
}
