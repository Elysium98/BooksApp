using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Interfaces
{
    public interface IBaseService<TModel, TId>
    {
        Task<TModel> GetByIdAsync(TId id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> CreateAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(TId id);
    }
}
