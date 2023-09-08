﻿using Buccacard.Domain;
using Buccacard.Infrastructure;
using Buccacard.Infrastructure.DTO;
using Buccacard.Infrastructure.Utility;
using Buccacard.Repository.DbContext;
using HermesApp.Infrastructure.Dictionary;
using Microsoft.AspNetCore.Identity;

namespace Buccacard.Services.UserManagementService
{
    public interface IAuthService
    {
        Task<ServiceResponse<Token>> Login(LoginDTO login);
        Task<ServiceResponse<string>> Register(RegisterDTO model);
        Task<ServiceResponse<string>> ComfirmToken(string userId, string token);
        Task<ServiceResponse<string>> Register_Admin(RegisterDTO model);

    }
    public class AuthService : IAuthService
    {
        private readonly UserDbContext _userDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IResponseService _responseService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IBaseHttpClient _baseHttpClient;

        public AuthService(UserDbContext userDbContext, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, IResponseService responseService,
            IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration, IBaseHttpClient baseHttpClient)
        {
            _userDbContext = userDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _responseService = responseService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
            _baseHttpClient = baseHttpClient;
        }

        public async Task<ServiceResponse<string>> ComfirmToken(string userId, string token)
        {
            var retrieveUser = _userManager.Users.FirstOrDefault(n => n.Id == userId);
            if (retrieveUser != null)
            {
                var result = await _userManager.ConfirmEmailAsync(retrieveUser, token);
                if (result.Succeeded)
                {
                    _responseService.SuccessResponse(data: "Account Comfirmed.");
                }
                return _responseService.ErrorResponse<string>($"Account comfirmation failed {result.Errors.First().Description}");

            }
            return _responseService.ErrorResponse<string>("Invalid token");
        }

        public async Task<ServiceResponse<Token>> Login(LoginDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email.ToLower().Trim());
            if (user == null)
            {
                return _responseService.ErrorResponse<Token>("User does not exist.");
            }
            if (!await _userManager.CheckPasswordAsync(user, login.PassWord))
            {
                return _responseService.ErrorResponse<Token>("Invalid Credential.");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(user, userRoles);
            return _responseService.SuccessResponse(data: new Token { Jwt = token, RefreshToken = "" });

        }

        public async Task<ServiceResponse<string>> Register(RegisterDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return _responseService.ErrorResponse<string>("User already exist.");
            }
            var newUser = new AppUser()
            {
                Email = model.Email.ToLower().Trim(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return _responseService.ErrorResponse<string>("Unable to create user.");
            }

            if (!await _roleManager.RoleExistsAsync(UserRole.User.ToString()) && string.IsNullOrWhiteSpace(model.Role.DisplayName()))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRole.User.ToString()));
            }

            if (await _roleManager.RoleExistsAsync(UserRole.User.ToString()) && string.IsNullOrWhiteSpace(model.Role.DisplayName()))
            {
                await _userManager.AddToRoleAsync(newUser, UserRole.User.ToString());
            }

            var comfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var baseUrl = _configuration.GetValue<string>("GateWayUrl");
            var notificationUrl = _configuration.GetValue<string>("NotificationUrl");
            var url = $"{baseUrl}/Auth/ConfirmEmail?userId ={newUser.Id}&token={comfirmationToken}";
            var tokenLink = new Uri(url);
            var emailBody = string.Format(Constants.ConfirmPassWordLink, newUser.FirstName, tokenLink);
            Console.WriteLine($"This is the token link {0}", tokenLink);
            var emailReq = new EmailDTO
            {
                Email = model.Email,
                Name = newUser.FirstName,
                Subject = Constants.ComfirmationSubject,
                Message = emailBody
            };
            await _baseHttpClient.JSendPostAsync<string>(notificationUrl, "home/sendemail", emailReq, "N/A", "N/A");
            return _responseService.SuccessResponse("Successfully Create User.");
        }

        public async Task<ServiceResponse<string>> Register_Admin(RegisterDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email.ToLower().Trim());
            if (user != null)
            {
                return _responseService.ErrorResponse<string>("User already exist.");
            }
            var newUser = new AppUser()
            {
                Email = model.Email.ToLower().Trim(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return _responseService.ErrorResponse<string>("Unable to create user.");
            }

            if (!await _roleManager.RoleExistsAsync(UserRole.Admin.ToString()) && !string.IsNullOrWhiteSpace(model.Role.DisplayName()))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin.ToString()));
            }

            if (await _roleManager.RoleExistsAsync(UserRole.Admin.ToString()) && !string.IsNullOrWhiteSpace(model.Role.DisplayName()))
            {
                await _userManager.AddToRoleAsync(newUser, UserRole.Admin.ToString());
            }

            var comfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var baseUrl = _configuration.GetValue<string>("GateWayUrl");
            var notificationUrl = _configuration.GetValue<string>("NotificationUrl");
            var url = $"{baseUrl}/Auth/ConfirmEmail?userId ={newUser.Id}&token={comfirmationToken}";
            var tokenLink = new Uri(url);
            var emailBody = string.Format(Constants.ConfirmPassWordLink, newUser.FirstName, tokenLink);
            Console.WriteLine($"This is the token link {0}", tokenLink);
            var emailReq = new EmailDTO
            {
                Email = model.Email,
                Name = newUser?.FirstName ?? "N/A",
                Subject = Constants.ComfirmationSubject,
                Message = emailBody
            };
            await _baseHttpClient.JSendPostAsync<string>(notificationUrl, "home/sendemail", emailReq, "N/A", "N/A");
            return _responseService.SuccessResponse("Successfully Create User.");
        }


    }
}