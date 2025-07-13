using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Models
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
