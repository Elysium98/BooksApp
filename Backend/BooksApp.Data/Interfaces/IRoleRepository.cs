using BooksApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<IdentityResult> CreateRoleAsync(string name);
        Task<IdentityResult> UpdateRoleAsync(string id, string name);
        Task<IdentityResult> DeleteRoleAsync(string id);
        Task<bool> RoleExistsAsync(string name);
    }
}
