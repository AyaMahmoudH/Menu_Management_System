using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Interfaces;
using Restaurant_Management_System.Models;
using X.PagedList.Extensions;

namespace Restaurant_Management_System.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryService CategoryService, UserManager<ApplicationUser> userManager)
        {
            _CategoryService = CategoryService;
            _userManager = userManager;
        }
       



    }
}
