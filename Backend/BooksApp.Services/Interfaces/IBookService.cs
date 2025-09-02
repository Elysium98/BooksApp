using BooksApp.Data.Entities;
using BooksApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Interfaces
{
    public interface IBookService : IBaseService<BookModel, string>
    {
        Task<List<BookModel>> GetBooksByFilters(string? userId = null, bool? isSold = null, string? categoryName = null);
    }
}
