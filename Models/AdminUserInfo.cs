using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Barangay_Office.Models
{
    public class AdminUserInfo
    {
        [PrimaryKey, AutoIncrement]
        public required string Id { get; set; }

        [Unique]
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } //super admin or customer
    }
}
