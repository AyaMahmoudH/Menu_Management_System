using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Hubs;
using Restaurant_Management_System.Interfaces;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountService> logger, IHubContext<NotificationHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return (false, "This email is already registered!");
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var userByName = await _userManager.FindByNameAsync(model.Email);
            if (userByName != null)
            {
                return (false, "This username is already registered!");
            }


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                _logger.LogInformation("New user registered: {Email}", model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                await _hubContext.Clients.Group("Admins")
              .SendAsync("ReceiveUserNotification", $"New user registered: {user.UserName}");
                return (true, "Registration successful");
            }
          

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)> LoginAsync(LoginViewModel model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return (false, "Invalid login attempt");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in: {Email}", model.Email);
                return (true, "Login successful");
            }
            return (false, "Invalid login attempt");
        }


        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
        }
       
    }
}
