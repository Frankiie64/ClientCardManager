using AutoMapper;
using CientCardManager.Core.Application.Interfaces.Repositorios;
using CientCardManager.Core.Application.Interfaces.Servicios;
using ClientCardManager.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CientCardManager.Core.Application.Servicios
{
    public class ServicioGenerico<sv,dto,model> : IServicioGenerico<sv, dto, model>
        where dto : class
        where model : EntidadBaseAuditoria
        where sv : class
    {
        private readonly IRepositorioGenerico<model> _repository;
        private readonly IMapper _mapper;

        public ServicioGenerico(IMapper mapper, IRepositorioGenerico<model> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> Add(sv entity)
        {
            try
            {
                model model = _mapper.Map<model>(entity);
                return await _repository.Add(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> Delete(int Id)
        {
            try
            {
                return await _repository.Delete(Id);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<bool> Exists(Expression<Func<model, bool>> predicate)
        {
            try
            {
                return await _repository.Exists(predicate);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<dto> FindWhere(Expression<Func<model, bool>> predicate, Expression<Func<model, dynamic>> include)
        {
            try
            {
                var result = await _repository.FindWhere(predicate, include);
                return _mapper.Map<dto>(result);

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<dto>> GetAll()
        {
            try
            {
                var result = await _repository.GetAll();
                return _mapper.Map<IEnumerable<dto>>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<dto> GetById(int Id, Expression<Func<model, dynamic>> include = null)
        {
            try
            {
                var result = await _repository.GetById(Id,include);
                return _mapper.Map<dto>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<sv> GetByIdSv(int Id, Expression<Func<model, dynamic>> include = null)
        {
            try
            {
                var result = await _repository.GetById(Id,include);
                return _mapper.Map<sv>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<dto>> GetList(Expression<Func<model, bool>> predicate = null, Func<IQueryable<model>, IIncludableQueryable<model, object>> include = null)
        {
            try
            {
                var result = await _repository.GetList(predicate, include);
                return _mapper.Map<IEnumerable<dto>>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public sv MapepVmToSv(dto vm)
        {
            return _mapper.Map<sv>(vm);
        }

        public async Task<bool> Update(sv entity)
        {
            try
            {
                var model = _mapper.Map<model>(entity);
                return await _repository.Update(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}