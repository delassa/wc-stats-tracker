using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCStatsTracker.Models;

namespace WCStatsTracker.Services;


/// <summary>
/// Class to handle the database insertions deletions and updates
/// </summary>
public class WCDatabaseService<T> : IDatabaseService<T> where T : BaseModelObject
{
    private readonly WCDBContextFactory _contextFactory;

    public WCDatabaseService(WCDBContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<T> Create(T entity)
    {
        using (WCDBContext context = _contextFactory.CreateDbContext())
        {
            var createdEntity = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();

            return createdEntity.Entity;
        }
    }
    public async Task<bool> Delete(int id)
    {
        using (WCDBContext context = _contextFactory.CreateDbContext())
        {
            T entity = await context.Set<T>().FirstOrDefaultAsync<T>((e) => e.Id == id);
            context.Set<T>().Remove(entity!);
            await context.SaveChangesAsync();

            return true;
        }
    }

    public async Task<bool> Delete(T entity)
    {
        using (WCDBContext context = _contextFactory.CreateDbContext())
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }
    }

    public async Task<T> Update(T entity, int id)
    {
        using (WCDBContext context = _contextFactory.CreateDbContext())
        {

            entity.Id = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }


    public async Task<IEnumerable<T>> GetAll()
    {
        using (WCDBContext context = _contextFactory.CreateDbContext())
        {
            IEnumerable<T> entities = await context.Set<T>().ToListAsync();
            return entities;
        }
    }

    public async Task<T> Get(int id)
    {
        using (WCDBContext context = _contextFactory.CreateDbContext())
        {
            T entity = await context.Set<T>().FirstOrDefaultAsync<T>((e) => e.Id == id);
            return entity;
        }
    }
}
