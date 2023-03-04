using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace WCStatsTracker.Services.DataAccess;

/// <summary>
///     Class to handle the database insertions deletions and updates
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    /// <summary>
    ///     The current db context we are using
    /// </summary>
    private readonly WcDbContext _context;

    /// <summary>
    ///     Initializes the repository with a db context
    /// </summary>
    /// <param name="context">The db context to use for operations</param>
    public GenericRepository(WcDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets an entity by its id from the db context
    /// </summary>
    /// <param name="id">The id to search for</param>
    /// <returns>The entity or null if not found</returns>
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    /// <summary>
    ///     Gets all the entities in the db context
    /// </summary>
    /// <returns>A list of the entities</returns>
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    /// <summary>
    ///     Finds a set of entities based on a predicate
    /// </summary>
    /// <param name="predicate">The predicate to search for</param>
    /// <returns>A list of entities that match the predicate</returns>
    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    /// <summary>
    ///     Adds an entity to the db context
    /// </summary>
    /// <param name="entity">The entity to add</param>
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    /// <summary>
    ///     Adds a range of entities to the db context
    /// </summary>
    /// <param name="entities">An IEnumerable of entities to add</param>
    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    /// <summary>
    ///     Removes an entity from the db context
    /// </summary>
    /// <param name="entity">The entity to remove</param>
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    /// <summary>
    ///     Removes a range of entities from the db context
    /// </summary>
    /// <param name="entities">An IEnumerable of entities to remove</param>
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}
