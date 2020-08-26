using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;
            // gets key based on repo type
            if(!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                // if we dont have a repository we say...
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType
                (typeof(TEntity)), _context);
                // passing in the data context that our unit of work owns
                _repositories.Add(type, repositoryInstance);
                // giving it a key and a subject within the hashtable

            }

            return (IGenericRepository<TEntity>) _repositories[type];
        }
    }
}