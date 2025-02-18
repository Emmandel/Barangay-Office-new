using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SQLite;

namespace Barangay_Office.Models
{
    public class AdminUserInfo
    {
        [PrimaryKey]
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } //super admin or customer
    }
}
