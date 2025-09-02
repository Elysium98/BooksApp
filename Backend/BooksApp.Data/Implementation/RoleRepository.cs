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
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleRepository(ApplicationDBContext applicationDbContext, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await Task.Run(() => _roleManager.Roles.ToList());
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string id)
            => await _roleManager.FindByIdAsync(id);

        public async Task<IdentityResult> CreateRoleAsync(string name)
            => await _roleManager.CreateAsync(new IdentityRole(name));

        public async Task<IdentityResult> UpdateRoleAsync(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            var existingRole = await _roleManager.FindByNameAsync(name);
            if (existingRole != null && existingRole.Id != id)
                return IdentityResult.Failed(new IdentityError { Description = "Another role with this name already exists" });

            role.Name = name;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string name)
            => await _roleManager.RoleExistsAsync(name);
    }
}
