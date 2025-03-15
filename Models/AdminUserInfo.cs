using SQLite;

namespace Barangay_Office.Models
{
    public class AdminUserInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public  string Email { get; set; }
        public  string Password { get; set; }
        public 
         string Role { get; set; } //admin or customer
    }
}
