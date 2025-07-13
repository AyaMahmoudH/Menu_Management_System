namespace Restaurant_Management_System.Models
{
    public class UserRoles : IEntity
    {
        public int Id { get; set; }
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        public const string Customer = "Customer";
    }
}
