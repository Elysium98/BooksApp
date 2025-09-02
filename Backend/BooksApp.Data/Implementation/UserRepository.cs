using BooksApp.Data.Entities;
using BooksApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Implementation
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(ApplicationDBContext applicationDbContext, UserManager<UserEntity> userManager,
                          RoleManager<IdentityRole> roleManager, SignInManager<UserEntity> signInManager)
            : base(applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = applicationDbContext;
            _signInManager = signInManager;
        }

        public async Task<(bool Succeeded, object Result)> RegisterUserAsync(UserEntity user, string password, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                return (false, "Role does not exist");

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, result.Errors);

            await _userManager.AddToRoleAsync(user, role);
            return (true, user);
        }

        public async Task<UserEntity?> ValidateUserAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            if (!result.Succeeded) return null;

            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string?> GetUserRoleAsync(UserEntity user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task RefreshSignInAsync(UserEntity user)
        {
            await _signInManager.RefreshSignInAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserEntity user)
        {
            return await _userManager.UpdateAsync(user);
        }

    }
}
