using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management_System.Models
{
    public class MenuItem : IEntity
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Range(0.01, 10000)]
        public decimal Price { get; set; }
        [Url]
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [NotMapped]
        public IFormFile? clientFile { get; set; }
        public byte[]? dbImage { get; set; }
        [NotMapped]
        public string? imageSrc
        {
            get
            {
                if (dbImage != null)
                {
                    string base64String = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
            }
        }

    }
}
