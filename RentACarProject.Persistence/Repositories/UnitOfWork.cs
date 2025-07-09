using Microsoft.EntityFrameworkCore.Storage;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Persistence.Context;

namespace RentACarProject.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RentACarDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(RentACarDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
