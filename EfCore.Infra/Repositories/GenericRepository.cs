using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Infra;


public class GenericRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;

    public GenericRepository(DbContext context)
    {
        this.Context = context;
    }
    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }
    
    public  void Add(TEntity entity)
    {
         Context.Set<TEntity>().Add(entity);
    }


    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities);
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public ValueTask<TEntity> GetByIdAsync(int id)
    {
        return Context.Set<TEntity>().FindAsync(id);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }
}


public class GenericRepositoryWithProperty<TEntity> where TEntity:class
{
    private readonly DbContext _context;
    private DbSet<TEntity> DBSet { get; set; }

    public GenericRepositoryWithProperty(DbContext context)
    {
        _context = context;
        DBSet = context.Set<TEntity>();
    }
    public  void Add(TEntity entity)
    {
        DBSet.Add(entity);
    }
    
    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return DBSet.FirstOrDefault(predicate);
    }
    
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DBSet.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

