using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Barangay_Office.Models
{
    public class CustomerService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sender_id")]
        public string SenderID { get; set; }

        [BsonElement("recipient_id")]
        public string RecipientId { get; set; }

        [BsonElement("sender_role")]
        public string SenderRole { get; set; } // "Admin" or "Customer"

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("is_read")]
        public bool IsRead { get; set; }
    }
}
