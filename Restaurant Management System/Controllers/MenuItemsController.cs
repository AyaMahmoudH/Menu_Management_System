using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.DTOs;
using Restaurant_Management_System.Interfaces;
using Restaurant_Management_System.Models;
using Restaurant_Management_System.Services;
using X.PagedList.Extensions;

namespace Restaurant_Management_System.Controllers
{
    [Authorize]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IRepository<Category> _categoryRepository;
        public MenuItemsController(
           IMenuItemService menuItemService,
           IRepository<Category> categoryRepository
           )
        {
            _menuItemService = menuItemService;
            _categoryRepository = categoryRepository;
           
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> Index(int? categoryId, int? page)
        {
            var categories = await _categoryRepository.GetAllAsync();
            IEnumerable<MenuItemDto> menuItems;

            if (categoryId.HasValue)
                menuItems = await _menuItemService.GetMenuItemsByCategoryAsync(categoryId.Value); 
            else
                menuItems = await _menuItemService.GetAllMenuItemsAsync();

            int pageSize = 6;
            int pageNumber = page ?? 1;
            var pagedItems = menuItems.ToPagedList(pageNumber, pageSize);

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            return View(pagedItems);
        }



        public async Task<IActionResult> Details(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var distinctCategories = categories
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .ToList();

            ViewBag.Categories = distinctCategories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(MenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepository.GetAllAsync();
                var distinctCategories = categories
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .ToList();

                ViewBag.Categories = distinctCategories;

                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, errors });
            }

            menuItemDto.Id = 0;

            var result = await _menuItemService.CreateMenuItemAsync(menuItemDto);
            if (result != null)
            {
                return Json(new
                {
                    success = true,
                    message = "Menu item created successfully!",
                    redirectUrl = Url.Action("Index", "MenuItems")
                });
            }

            return Json(new
            {
                success = false,
                message = "Something went wrong while creating item."
            });
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            var distinctCategories = categories
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .ToList();

            ViewBag.Categories = distinctCategories;
            return View(menuItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(MenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, errors });
            }

            var result = await _menuItemService.UpdateMenuItemAsync(menuItemDto);
            if (result != null)
            {
                return Json(new
                {
                    success = true,
                    message = "Menu item updated successfully!",
                    redirectUrl = Url.Action("Index", "MenuItems")
                });
            }

            return Json(new
            {
                success = false,
                message = "Menu item not found."
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _menuItemService.DeleteMenuItemAsync(id);

            if (result)
            {
                return Json(new
                {
                    success = true,
                    message = "Menu item deleted successfully!",
                    redirectUrl = Url.Action("Index", "MenuItems")
                });
            }

            return Json(new
            {
                success = false,
                message = "Menu item not found or already deleted."
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _menuItemService.GetAllMenuItemsAsync();
            return Json(menuItems);
        }
    } 

}

