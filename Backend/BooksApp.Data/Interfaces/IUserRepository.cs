using BooksApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<(bool Succeeded, object Result)> RegisterUserAsync(UserEntity user, string password, string role);

        Task<UserEntity?> ValidateUserAsync(string email, string password);

        Task<string?> GetUserRoleAsync(UserEntity user);

        Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(UserEntity user, string password);

        Task RefreshSignInAsync(UserEntity user);

        Task<IdentityResult> UpdateUserAsync(UserEntity user);
    }
}
