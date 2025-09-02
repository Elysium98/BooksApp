using BooksApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace BooksApp.Services.Implementation
{
    public class PhotoService : IPhotoService
    {
        private readonly IWebHostEnvironment _env;

        public PhotoService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<(bool Success, string Path, string ErrorMessage)> SavePhotoAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return (false, null, "No file uploaded.");

            var folderName = Path.Combine("Resources", "Images", folder);
            var pathToSave = Path.Combine(_env.ContentRootPath, folderName);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName).Replace("\\", "/");

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (true, dbPath, null);
        }
    }
}
