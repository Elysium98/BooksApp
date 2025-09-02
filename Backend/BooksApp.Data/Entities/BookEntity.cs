using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Entities
{
    public class BookEntity
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Page { get; set; }
        public int Price { get; set; }
        public string Language { get; set; }
        public string Condition { get; set; }
        public bool isSold { get; set; }
        public string Image { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryEntity? Category { get; set; }
        public string UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
