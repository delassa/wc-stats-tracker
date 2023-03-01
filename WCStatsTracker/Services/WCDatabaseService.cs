using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WCStatsTracker.Models;
namespace WCStatsTracker.Services;

/// <summary>
///     Class to handle the database insertions deletions and updates
/// </summary>
public class WCDatabaseService<T> : IDatabaseService<T> where T : BaseModelObject
{
    private readonly WCDBContextFactory _contextFactory;

    public WCDatabaseService(WCDBContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public bool Delete(T entityToDelete)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                context.Set<T>().Attach(entityToDelete);
                context.Remove(entityToDelete);
            }
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed Deleting from DB exception message: {message}", ex.Message);
            Log.Fatal(ex.InnerException, "Failed Deleting from DB exception message: {message}", ex.InnerException.Message);
            throw;
        }
    }

    public bool Create(T entityToCreate)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            if (!context.Set<T>().Contains(entityToCreate))
            {
                context.Set<T>().Attach(entityToCreate);
                context.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed Deleting from DB exception message: {message}", ex.Message);
            Log.Fatal(ex.InnerException, "Failed Deleting from DB exception message: {message}", ex.InnerException.Message);
            throw;
        }
    }

    public T Get(int id)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<T>().Find(id);
            return entity;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed retrieving from db id:{id} message: {ex.message}");
            throw;
        }
    }

    public IEnumerable<T> GetAll()
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<T>().ToList();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed retrieving all from db, message: {ex.message}");
            throw;
        }
    }

    public bool Update(T entityToUpdate)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<T>().Attach(entityToUpdate);
            return true;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to update entity in DB, message: {ex.message}");
            throw;
        }
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<T>().Where(predicate);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to find in db, message: {ex.message}");
            throw;
        }
    }

    // public async Task<bool> Create(T entity)
    // {
    //     using var context = _contextFactory.CreateDbContext();
    //     
    //         var item = context.Set<T>().Find(entity.Id);
    //         if (item is not null)
    //         {
    //             context.Entry(item).CurrentValues.SetValues(entity);
    //             await context.SaveChangesAsync();
    //         }
    //         else
    //         {
    //             context.Update(entity);
    //             await context.SaveChangesAsync();
    //         }
    //
    //         return true;
    // }

    // public async Task<bool> Delete(int id)
    // {
    //     using var context = _contextFactory.CreateDbContext();
    //     var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    //     if (entity is not null)
    //     {
    //         context.Set<T>().Remove(entity!);
    //         await context.SaveChangesAsync();
    //         return Task.FromResult(true).Result;
    //     }
    //     else
    //     {
    //         return Task.FromResult(false).Result;
    //     }
    // }

    // public async Task<bool> Delete(T entity)
    // {
    //     using var context = _contextFactory.CreateDbContext();
    //     context.Set<T>().Remove(entity);
    //     await context.SaveChangesAsync();
    //     return true;
    // }

    // public async Task<bool> Update(T entity, int id)
    // {
    //     using var context = _contextFactory.CreateDbContext();
    //     
    //     var newEntity  = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    //     if (newEntity is not null)
    //     {
    //             context.Entry(entity).CurrentValues.SetValues(newEntity);
    //             
    //             context.Set<T>().Update(entity);
    //             await context.SaveChangesAsync();
    //             return true;
    //     }
    //     return false;
    //
    // }
    //
    // public async Task<IList<T>> GetAll()
    // {
    //     using var context = _contextFactory.CreateDbContext();
    //     return await context.Set<T>().ToListAsync();
    // }
    //
    // public async Task<T> Get(int id)
    // {
    //     using var context = _contextFactory.CreateDbContext();
    //     var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    //     if (entity is null)
    //         throw new NullReferenceException($"Null reference from database of type {typeof(T)} entity");
    //     return entity;
    // }
}