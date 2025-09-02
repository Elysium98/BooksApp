using AutoMapper;
using BooksApp.Data.Entities;
using BooksApp.Data.Implementation;
using BooksApp.Data.Interfaces;
using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BooksApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JWTConfig _jWTConfig;

        public UserService(IUserRepository userRepository, IMapper mapper, IOptions<JWTConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jWTConfig = jwtConfig.Value;
        }

        public async Task<(bool Succeeded, object Result)> RegisterUserAsync(RegisterModel model)
        {
            var user = _mapper.Map<UserEntity>(model);
            user.UserName = model.Email;
            user.DateCreated = DateTime.UtcNow;
            user.DateModified = DateTime.UtcNow;

            return await _userRepository.RegisterUserAsync(user, model.Password, model.Role);
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            var model = _mapper.Map<UserModel>(user);

            var role = await _userRepository.GetUserRoleAsync(user);
            model.Role = role;

            return model;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var entities = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserModel>>(entities);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string role = null)
        {
            var users = await _userRepository.GetAllAsync();
            var userModels = new List<UserModel>();

            foreach (var user in users)
            {
                var userRole = await _userRepository.GetUserRoleAsync(user);

                if (string.IsNullOrEmpty(role) || userRole == role)
                {
                    var model = _mapper.Map<UserModel>(user);
                    model.Role = userRole; 
                    userModels.Add(model);
                }
            }

            return userModels;
        }

        public Task<UserModel> CreateAsync(UserModel model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id '{id}' not found.");

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }

        public string GenerateToken(UserModel user, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTConfig.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = credentials,
                Audience = _jWTConfig.Audience,
                Issuer = _jWTConfig.Issuer
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.CreateToken(tokenDescriptor);

            return jwtHandler.WriteToken(token);
        }

        public async Task<(bool Succeeded, object Result)> LoginAsync(LoginModel model)
        {
            var user = await _userRepository.ValidateUserAsync(model.Email, model.Password);
            if (user == null)
                return (false, "Invalid email or password");

            var role = await _userRepository.GetUserRoleAsync(user);

            var userModel = _mapper.Map<UserModel>(user);

            var response = new UserLoginResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                BirthDate = user.BirthDate,
                DateCreated = user.DateCreated,
                DateModified = user.DateModified,
                Image = user.Image,
                Role = role,
                Token = GenerateToken(userModel, role)
            };

            return (true, response);
        }

        public async Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(ChangePasswordModel model, string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            var isValid = await _userRepository.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isValid)
                return (false, "Invalid current password");

            var result = await _userRepository.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userRepository.RefreshSignInAsync(user);
            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateUserAsync(string id, UpdateUserModel model)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return (false, "User not found");

            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Study = model.Study;
            user.PhoneNumber = model.PhoneNumber;
            user.DateModified = DateTime.UtcNow;

            var result = await _userRepository.UpdateUserAsync(user);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userRepository.RefreshSignInAsync(user);

            return (true, null);
        }

        public async Task<bool> UpdateUserImagePathAsync(string userId, string imagePath)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.Image = imagePath;
            var result = await _userRepository.UpdateUserAsync(user);
            return result.Succeeded;
        }
    }
}
