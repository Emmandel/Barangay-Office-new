using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Barangay_Office.Models
{
    [Table("customer_service_messages")]
    public class CustomerService : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("sender_id")]
        public string SenderID { get; set; }

        [Column("recipient_id")]
        public string RecipientId { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }
    }
}
