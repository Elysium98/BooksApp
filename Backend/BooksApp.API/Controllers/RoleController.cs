using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksApp.API.Controllers
{
    [Route("/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] RoleModel model)
        {
            var (succeeded, message) = await _roleService.CreateRoleAsync(model);
            if (succeeded) return Ok(message);
            return BadRequest(message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleModel model)
        {
            var (succeeded, message) = await _roleService.UpdateRoleAsync(id, model);
            if (succeeded) return Ok(message);
            return BadRequest(message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var (succeeded, message) = await _roleService.DeleteRoleAsync(id);
            if (succeeded) return Ok(message);
            return BadRequest(message);
        }
    }
}
