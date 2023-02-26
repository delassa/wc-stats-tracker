using System.Collections.Generic;
namespace WCStatsTracker.Services;

/// <summary>
///     Basic database access service
/// </summary>
/// <typeparam name="T">Type of entity to use for the db</typeparam>
public interface IDatabaseService<EntityType> where EntityType : class
{
    /// <summary>
    ///     Get an entity by id from the database
    /// </summary>
    /// <param name="id">The id of the entity to get</param>
    /// <returns>The entity requested</returns>
    EntityType Get(int id);

    /// <summary>
    ///     Create a new entity in the database
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <returns>The entity created</returns>
    void Create(EntityType entity);

    /// <summary>
    ///     Update an entity in the database
    /// </summary>
    /// <param name="entity"></param>
    void Update(EntityType entity);

    /// <summary>
    ///     Deletes an entity in the database
    /// </summary>
    /// <param EntityTypeame="entity">The entity to delete</param>
    /// <returns>True on success false otherwise</returns>
    void Delete(EntityType entity);

    public IList<EntityType> GetAll();
}