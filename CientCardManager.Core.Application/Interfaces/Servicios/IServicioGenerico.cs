using ClientCardManager.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CientCardManager.Core.Application.Interfaces.Servicios
{
    public interface IServicioGenerico<sv, dto, model>
       where dto : class
       where model : EntidadBaseAuditoria
       where sv : class
    {
        Task<IEnumerable<dto>> GetAll();
        Task<dto> GetById(int Id, Expression<Func<model, dynamic>> include = null);
        Task<sv> GetByIdSv(int Id, Expression<Func<model, dynamic>> include = null);
        Task<dto> FindWhere(Expression<Func<model, bool>> predicate, Expression<Func<model, dynamic>> include);
        Task<IEnumerable<dto>> GetList(Expression<Func<model, bool>> predicate = null, Func<IQueryable<model>, IIncludableQueryable<model, object>> include = null);
        Task<IEnumerable<dto>> GetListAvance(Func<IQueryable<model>, IQueryable<model>> queryConfigurator);
        Task<int> GetTotalCount(Func<IQueryable<model>, IQueryable<model>> queryConfigurator);
        Task<bool> Add(sv entity);
        Task<bool> Delete(int Id);
        Task<bool> Update(sv entity);
        sv MapepVmToSv(dto vm);
        Task<bool> Exists(Expression<Func<model, bool>> predicate);
    }
}