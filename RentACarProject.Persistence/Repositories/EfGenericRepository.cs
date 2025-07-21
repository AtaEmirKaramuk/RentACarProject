using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Domain.Common;
using RentACarProject.Persistence.Context;
using System.Linq.Expressions;

namespace RentACarProject.Persistence.Repositories
{
    /// <summary>
    /// Generic repository implementasyonu.
    /// Tüm entity'ler için temel CRUD operasyonlarını içerir.
    /// Soft delete desteği entegre edilmiştir.
    /// </summary>
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly RentACarDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public EfGenericRepository(RentACarDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(expression);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? expression = null)
        {
            var query = _dbSet.Where(e => !e.IsDeleted);

            if (expression != null)
                query = query.Where(expression);

            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet.Where(e => !e.IsDeleted).AsQueryable();
        }

        public IQueryable<TEntity> GetAll()
        {
            return Query(); // Alternatif olarak doğrudan: return _dbSet.Where(e => !e.IsDeleted);
        }
    }
}
