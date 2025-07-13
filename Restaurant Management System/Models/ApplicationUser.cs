using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
       
        public string FullName => $"{FirstName} {LastName}";

    }
}
