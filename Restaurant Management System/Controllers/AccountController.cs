using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Restaurant_Management_System.Hubs;
using Restaurant_Management_System.Interfaces;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Services;
using Serilog;
using X.PagedList;
using X.PagedList.Extensions;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger, IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager)
    {
        _accountService = accountService;
        _logger = logger;
        _userManager = userManager;

    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = $"Invalid data: {string.Join(", ", errors)}" });
        }


        var (success, message) = await _accountService.LoginAsync(model);

        if (success)
        {
            _logger.LogInformation("User logged in: {Email}", model.Email);
            return Json(new { success = true, message = "Login successful!" });
        }

        _logger.LogWarning("Login failed for: {Email}", model.Email);
        return Json(new { success = false, message });
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Invalid data submitted." });
        }

        var (success, message) = await _accountService.RegisterAsync(model);

        if (success)
        {
            _logger.LogInformation("New user registered: {Email}", model.Email);
            return Json(new { success = true, message = "Registration successful!" });
        }

        _logger.LogWarning("Registration failed for: {Email}", model.Email);
        return Json(new { success = false, message });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AllCustomers(int? page)
    {
        var customers = await _userManager.GetUsersInRoleAsync("Customer");

        var result = customers.Select(u => new CustomerViewModel
        {
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email
        }).ToList();

        int pageSize = 5;
        int pageNumber = page ?? 1;

        var pagedList = result.ToPagedList(pageNumber, pageSize);

        return View(pagedList);
    }

}
