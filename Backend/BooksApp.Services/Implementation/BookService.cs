using AutoMapper;
using BooksApp.Data.Entities;
using BooksApp.Data.Interfaces;
using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;

namespace BooksApp.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookModel> GetByIdAsync(string id)
        {
            var entity = await _bookRepository.GetByIdAsync(id);
            return _mapper.Map<BookModel>(entity);
        }

        public async Task<IEnumerable<BookModel>> GetAllAsync()
        {
            var entities = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookModel>>(entities);
        }

        public async Task<List<BookModel>> GetBooksByFilters(string? userId = null, bool? isSold = null, string? categoryName = null)
        {
            var entities = await _bookRepository.GetBooksByFilters(userId, isSold, categoryName);
            return _mapper.Map<List<BookModel>>(entities);
        }

        public async Task<BookModel> CreateAsync(BookModel model)
        {
            var entity = _mapper.Map<BookEntity>(model);
            await _bookRepository.CreateAsync(entity);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookModel>(entity);
        }

        public async Task UpdateAsync(BookModel model)
        {
            var entity = _mapper.Map<BookEntity>(model);
            _bookRepository.Update(entity);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _bookRepository.GetByIdAsync(id);
            if (entity != null)
            {
                _bookRepository.Delete(entity);
                await _bookRepository.SaveChangesAsync();
            }
        }

    }
}
