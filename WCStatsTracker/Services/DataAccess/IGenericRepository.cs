using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
namespace WCStatsTracker.Services.DataAccess;

/// <summary>
///     Basic database access service
/// </summary>
/// <typeparam name="TEntity">Type of entity to use for the db</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Get an entity by id from the database
    /// </summary>
    /// <param name="id">The id of the entity to get</param>
    /// <returns>The entity requested</returns>
    TEntity GetById(int id);

    /// <summary>
    ///     Get all records of a specific entity
    /// </summary>
    /// <returns>An IEnumerable of all the entities</returns>
    IQueryable<TEntity> GetAll();

    /// <summary>
    ///     Finds entities with the specified func
    /// </summary>
    /// <param name="predicate">The func to search on</param>
    /// <returns>An IEnumerable of the entities specified by the predicate</returns>
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Create a new entity in the database
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <returns>The entity created</returns>
    void Add(TEntity entity);

    /// <summary>
    ///     Adds a range of entities
    /// </summary>
    /// <param name="entities">The entities to add to the db</param>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    ///     Removes an Entity in the database
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    void Remove(TEntity entity);

    /// <summary>
    ///     Removes a range of entities in the database
    /// </summary>
    /// <param name="entities"></param>
    void RemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    ///     Grabs the local from the dbcontext as an observable collection
    /// </summary>
    /// <returns>An Observable collection of the local from the dbcontext</returns>
    ObservableCollection<TEntity> GetAllObservable();

    /// <summary>
    ///     Loads the data from the DB to populate the local value of the dbcontext
    /// </summary>
    void Load();
}
