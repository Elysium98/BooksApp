using AutoMapper;
using BooksApp.Data.Interfaces;
using BooksApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Implementation
{
    public class BaseService<TEntity, TModel, TId> : IBaseService<TModel, TId>
      where TEntity : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TModel> GetByIdAsync(TId id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TModel>>(entities);
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            await _repository.CreateAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
