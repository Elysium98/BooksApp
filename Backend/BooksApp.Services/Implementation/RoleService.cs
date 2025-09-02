using AutoMapper;
using BooksApp.Data.Interfaces;
using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleModel>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return _mapper.Map<IEnumerable<RoleModel>>(roles);
        }

        public async Task<RoleModel> GetRoleByIdAsync(string id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            return _mapper.Map<RoleModel>(role);
        }

        public async Task<(bool Succeeded, string Message)> CreateRoleAsync(RoleModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
                return (false, "Parameters are missing");

            if (await _roleRepository.RoleExistsAsync(model.Name))
                return (false, "Role already exists");

            var result = await _roleRepository.CreateRoleAsync(model.Name);
            return result.Succeeded ? (true, "Role created") : (false, "Something went wrong, please try again");
        }

        public async Task<(bool Succeeded, string Message)> UpdateRoleAsync(string id, RoleModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
                return (false, "Parameters are missing");

            var result = await _roleRepository.UpdateRoleAsync(id, model.Name);

            if (result.Succeeded)
                return (true, "Role updated");

            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return (false, errors);
        }

        public async Task<(bool Succeeded, string Message)> DeleteRoleAsync(string id)
        {
            var result = await _roleRepository.DeleteRoleAsync(id);
            return result.Succeeded ? (true, "Role deleted successfully") : (false, "Something went wrong, please try again");
        }
    }
}
