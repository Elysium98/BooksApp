using BooksApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Interfaces
{
    public interface IBookRepository : IBaseRepository<BookEntity>
    {
        Task<List<BookEntity>> GetBooksByFilters(string? userId = null, bool? isSold = null, string? categoryName = null);
    }
}
