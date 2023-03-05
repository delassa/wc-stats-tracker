namespace WCStatsTracker.Services.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly WcDbContext _context;

    public UnitOfWork(WcDbContext context)
    {
        _context = context;
        Ability = new AbilityRepository(_context);
        Character = new CharacterRepository(_context);
        Flag = new FlagRepository(_context);
        WcRun = new WcRunRepository(_context);
    }

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

    public void Clear()
    {
        _context.ChangeTracker.Clear();
    }

    /// <summary>
    ///     Make sure the context is disposed after we are done with this unit of work
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}
