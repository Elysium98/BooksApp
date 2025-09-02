using BooksApp.Services.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Interfaces
{
    public interface IRoleService 
    {
        Task<IEnumerable<RoleModel>> GetAllRolesAsync();
        Task<RoleModel> GetRoleByIdAsync(string id);
        Task<(bool Succeeded, string Message)> CreateRoleAsync(RoleModel model);
        Task<(bool Succeeded, string Message)> UpdateRoleAsync(string id, RoleModel model);
        Task<(bool Succeeded, string Message)> DeleteRoleAsync(string id);
    }
}
