using ClientCardManager.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CientCardManager.Core.Application.Interfaces.Repositorios
{
    public interface IRepositorioGenerico<T>
        where T : EntidadBaseAuditoria
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int Id, Expression<Func<T, dynamic>> include);
        Task<T> FindWhere(Expression<Func<T, bool>> predicate, Expression<Func<T, dynamic>> include);
        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<bool> Add(T entity);
        Task<bool> Delete(int Id);
        Task<bool> DeleteAll();
        Task<bool> Update(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
    }
}
