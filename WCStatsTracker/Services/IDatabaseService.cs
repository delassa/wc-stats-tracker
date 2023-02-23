using System.Collections.Generic;
using System.Threading.Tasks;

namespace WCStatsTracker.Services;

/// <summary>
/// Basic database access service
/// </summary>
/// <typeparam name="T">Type of entity to use for the db</typeparam>
public interface IDatabaseService<T> where T : class
{
    /// <summary>
    /// Get all of a certain type from the database
    /// </summary>
    /// <returns>An IEnumerable of the type</returns>
    Task<IList<T>> GetAll();
    
    /// <summary>
    /// Get an entity by id from the database
    /// </summary>
    /// <param name="id">The id of the entity to get</param>
    /// <returns>The entity requested</returns>
    Task<T> Get(int id);
    
    /// <summary>
    /// Create a new entity in the database
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <returns>The entity created</returns>
    Task<T> Create(T entity);
    
    /// <summary>
    /// Update an entity in the database
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <returns>The updated entity</returns>
    Task<T> Update(T entity, int id);

    /// <summary>
    /// Deletes an entry in the database by id
    /// </summary>
    /// <param name="id">The id of the entry to delete</param>
    /// <returns>true on success, false otherwise</returns>
    Task<bool> Delete(int id);

    /// <summary>
    /// Deletes an entity in the database
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <returns>True on success false otherwise</returns>
    Task<bool> Delete(T entity);
}
