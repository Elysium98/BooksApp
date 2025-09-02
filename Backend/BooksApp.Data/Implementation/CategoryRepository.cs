using BooksApp.Data.Entities;
using BooksApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Implementation
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public CategoryRepository(ApplicationDBContext applicationDbContext)
            : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
    }
}
