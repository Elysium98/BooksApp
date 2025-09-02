using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Entities
{
    public class CategoryEntity
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<BookEntity>? Books { get; set; }
    }
}
