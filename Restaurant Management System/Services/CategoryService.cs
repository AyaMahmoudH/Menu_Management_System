using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Interfaces;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        
    }
}
