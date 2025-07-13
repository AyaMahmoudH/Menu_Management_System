using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model);
        Task<(bool Success, string Message)> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
