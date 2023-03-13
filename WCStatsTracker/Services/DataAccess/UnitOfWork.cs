using System;
namespace WCStatsTracker.Services.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly WcDbContext _context;

    // Allows getting the context for debug viewing
    public WcDbContext Context { get => _context; }

    public UnitOfWork(WcDbContext context)
    {
        _context = context;
        Ability = new AbilityRepository(_context);
        Character = new CharacterRepository(_context);
        Flag = new FlagRepository(_context);
        WcRun = new WcRunRepository(_context);
    }
    public bool IsDisposed { get; private set; }

    public IAbilityRepository Ability { get; }
    public ICharacterRepository Character { get; }
    public IFlagRepository Flag { get; }
    public IWcRunRepository WcRun { get; }

    /// <summary>
    ///     Save all changes done by this unit of work
    /// </summary>
    /// <returns>The number of state entries written to the Database</returns>
    public int Save()
    {
        return _context.SaveChanges();
    }

    /// <summary>
    ///     Clear all changes done by this unit of work?
    /// </summary>
    public void Clear()
    {
        _context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            IsDisposed = true;
        }
    }
}
