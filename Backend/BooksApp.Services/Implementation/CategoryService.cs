using AutoMapper;
using BooksApp.Data.Entities;
using BooksApp.Data.Interfaces;
using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;

namespace BooksApp.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryModel> GetByIdAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryModel>(entity);
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            var entities = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryModel>>(entities);
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel model)
        {
            var entity = _mapper.Map<CategoryEntity>(model);
            await _categoryRepository.CreateAsync(entity);
            await _categoryRepository.SaveChangesAsync();
            return _mapper.Map<CategoryModel>(entity);
        }

        public async Task UpdateAsync(CategoryModel model)
        {
            var entity = _mapper.Map<CategoryEntity>(model);
            _categoryRepository.Update(entity);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity != null)
            {
                _categoryRepository.Delete(entity);
                await _categoryRepository.SaveChangesAsync();
            }
        }
    }
}
