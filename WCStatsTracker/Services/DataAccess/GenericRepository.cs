using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace WCStatsTracker.Services.DataAccess;

/// <summary>
///     Class to handle the database insertions deletions and updates
/// </summary>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
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
    public TEntity? GetById(int id)
    {
        var entity = _context.Set<TEntity>().Find(id);
        if (entity is null)
        {
            Log.Logger.Error("No entry found in database for key:{0} type:{1}", id, typeof(TEntity));
            return null;
        }
        return entity;
    }

    /// <summary>
    ///     Gets all the entities in the db context
    /// </summary>
    /// <returns>A list of the entities</returns>
    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    /// <summary>
    ///     Finds a set of entities based on a predicate
    /// </summary>
    /// <param name="predicate">The predicate to search for</param>
    /// <returns>A list of entities that match the predicate</returns>
    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate);
    }

    /// <summary>
    ///     Adds an entity to the db context
    /// </summary>
    /// <param name="entity">The entity to add</param>
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    /// <summary>
    ///     Adds a range of entities to the db context
    /// </summary>
    /// <param name="entities">An IEnumerable of entities to add</param>
    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    /// <summary>
    ///     Removes an entity from the db context
    /// </summary>
    /// <param name="entity">The entity to remove</param>
    public void Remove(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<TEntity>().Attach(entity);
        }
        _context.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    ///     Removes a range of entities from the db context
    /// </summary>
    /// <param name="entities">An IEnumerable of entities to remove</param>
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public ObservableCollection<TEntity> GetAllObservable()
    {
        return _context.Set<TEntity>().Local.ToObservableCollection();
    }

    public void Load()
    {
        _context.Set<TEntity>().Load();
    }

    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }
}
