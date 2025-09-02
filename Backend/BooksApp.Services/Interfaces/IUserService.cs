using BooksApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Interfaces
{
    public interface IUserService : IBaseService<UserModel, string>
    {
        Task<(bool Succeeded, object Result)> RegisterUserAsync(RegisterModel model);

        Task<(bool Succeeded, object Result)> LoginAsync(LoginModel model);

        string GenerateToken(UserModel user, string role);

        Task<IEnumerable<UserModel>> GetUsersAsync(string role);

        Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(ChangePasswordModel model, string userId);

        Task<(bool Success, string ErrorMessage)> UpdateUserAsync(string id, UpdateUserModel model);

        Task<bool> UpdateUserImagePathAsync(string userId, string imagePath);
    }
}
