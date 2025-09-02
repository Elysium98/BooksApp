using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<(bool Success, string Path, string ErrorMessage)> SavePhotoAsync(IFormFile file, string folder);
    }
}
