using ClientCardManager.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ClientCardManager.Core.Domain.Common;
using CientCardManager.Core.Application.Interfaces.Repositorios;

namespace ClientCardManager.Infrastructure.Persistence.Repositorios
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T>
        where T : EntidadBaseAuditoria
    {
        private readonly ApplicationDbContext _db;

        public RepositorioGenerico(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int Id, Expression<Func<T, dynamic>> include)
        {
            if (include != null)
            {
                return await _db.Set<T>().Include(include).FirstOrDefaultAsync(x => x.Id == Id);
            }
            else
            {
                return await _db.Set<T>().FindAsync(Id);
            }
        }

        public async Task<bool> Add(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            return await CommitChanges();
        }

        public async Task<bool> Delete(int Id)
        {
            var objectToDelete = await _db.Set<T>().FindAsync(Id);
            _db.Set<T>().Remove(objectToDelete);
            return await CommitChanges();
        }

        public async Task<bool> DeleteAll()
        {
            var objectToDelete = await _db.Set<T>().ToListAsync();
            _db.Set<T>().RemoveRange(objectToDelete);
            return await CommitChanges();
        }

        public async virtual Task<bool> Update(T entity)
        {
            var entry = await _db.Set<T>().FindAsync(entity.Id);

            if (entry == null)
            {
                return false;
            }

            entity.Creado = entry.Creado;

            _db.Entry(entry).CurrentValues.SetValues(entity);


            return await CommitChanges();
        }

        public async Task<T> FindWhere(Expression<Func<T, bool>> predicate, Expression<Func<T, dynamic>> include)
        {
            IQueryable<T> query = _db.Set<T>();


            if (predicate != null)
            {
                query.Where(predicate);
            }

            if (include != null)
            {
                query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                IQueryable<T> query = _db.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                return await _db.Set<T>().AnyAsync(predicate);
            }

            return false;

        }
        private async Task<bool> CommitChanges()
        {
            return await _db.SaveChangesAsync() >= 0;
        }


    }
}