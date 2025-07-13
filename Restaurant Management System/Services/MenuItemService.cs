using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Restaurant_Management_System.DTOs;
using Restaurant_Management_System.Hubs;
using Restaurant_Management_System.Interfaces;
using Restaurant_Management_System.Models;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace Restaurant_Management_System.Services
{
    public class MenuItemService: IMenuItemService
    {
        private readonly IMapper _mapper;

        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<MenuItemService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public MenuItemService(
            IRepository<MenuItem> menuItemRepository,
            IRepository<Category> categoryRepository,
            IMemoryCache cache,
            ILogger<MenuItemService> logger,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext
            )
        {
            _menuItemRepository = menuItemRepository;
            _categoryRepository = categoryRepository;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
            _hubContext = hubContext;

        }
        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync()
        {
            const string cacheKey = "all_menu_items";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<MenuItemDto> cachedItems))
            {
                return cachedItems;
            }

            var menuItems = await _menuItemRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            foreach (var item in menuItems)
            {
                item.Category = categories.FirstOrDefault(c => c.Id == item.CategoryId);
            }

            var menuItemDtos = _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);


            _cache.Set(cacheKey, menuItemDtos, TimeSpan.FromMinutes(30));
            return menuItemDtos;
        }
        public async Task<MenuItemDto> GetMenuItemByIdAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null) return null;

            var category = await _categoryRepository.GetByIdAsync(menuItem.CategoryId);
            menuItem.Category = category;

            var menuItemDto = _mapper.Map<MenuItemDto>(menuItem);
            return menuItemDto;

        }
        private async Task<string?> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/Images/" + uniqueFileName;
        }
        public async Task<MenuItemDto> CreateMenuItemAsync(MenuItemDto menuItemDto)
        {
            var category = await _categoryRepository.GetByIdAsync(menuItemDto.CategoryId);
            menuItemDto.CategoryName = category?.Name;
            menuItemDto.Id = 0;

            var menuItem = _mapper.Map<MenuItem>(menuItemDto);
            menuItem.CategoryId = menuItemDto.CategoryId;

            if (menuItemDto.clientFile != null)
            {
                menuItem.ImageUrl = await SaveImageAsync(menuItemDto.clientFile);
            }

            var createdItem = await _menuItemRepository.AddAsync(menuItem);
            _cache.Remove("all_menu_items");

            _logger.LogInformation("Menu item created: {Name}", menuItem.Name);
            await _hubContext.Clients.Group("Customers")
          .SendAsync("ReceiveNewItemNotification", $"New item added: {menuItem.Name}");

            return await GetMenuItemByIdAsync(createdItem.Id);
        }




        public async Task<MenuItemDto> UpdateMenuItemAsync(MenuItemDto menuItemDto)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(menuItemDto.Id);
            if (menuItem == null) return null;

            var category = await _categoryRepository.GetByIdAsync(menuItemDto.CategoryId);
            menuItemDto.CategoryName = category?.Name;

            var existingImageUrl = menuItem.ImageUrl; 

            _mapper.Map(menuItemDto, menuItem);
            menuItem.CategoryId = menuItemDto.CategoryId;

            if (menuItemDto.clientFile != null && menuItemDto.clientFile.Length > 0)
            {
                menuItem.ImageUrl = await SaveImageAsync(menuItemDto.clientFile);
            }
            else
            {
                menuItem.ImageUrl = existingImageUrl; 
            }

            await _menuItemRepository.UpdateAsync(menuItem);
            _cache.Remove("all_menu_items");

            _logger.LogInformation("Menu item updated: {Name}", menuItem.Name);

            return await GetMenuItemByIdAsync(menuItem.Id);
        }




        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null) return false;

            await _menuItemRepository.DeleteAsync(menuItem);
            _cache.Remove("all_menu_items");

            _logger.LogInformation("Menu item deleted: {Name}", menuItem.Name);

            return true;

        }
       

        public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByCategoryAsync(int categoryId)
        {
            var menuItems = await _menuItemRepository.FindAsync(m => m.CategoryId == categoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            return _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);

        }

  
       
    }
}
