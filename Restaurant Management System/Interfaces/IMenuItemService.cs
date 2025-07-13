using Restaurant_Management_System.DTOs;

namespace Restaurant_Management_System.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync();
        Task<MenuItemDto> GetMenuItemByIdAsync(int id);
        Task<MenuItemDto> CreateMenuItemAsync(MenuItemDto menuItemDto);
        Task<MenuItemDto> UpdateMenuItemAsync(MenuItemDto menuItemDto);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<IEnumerable<MenuItemDto>> GetMenuItemsByCategoryAsync(int categoryId);
    }
}
