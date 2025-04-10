using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Barangay_Office.Models
{
    public class CustomerServiceMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("sender_id")]
        public string SenderID { get; set; } = string.Empty;

        [BsonElement("recipient_id")]
        public string RecipientId { get; set; } = string.Empty;

        [BsonElement("sender_role")]
        public string SenderRole { get; set; } = string.Empty; // "Admin" or "Customer"

        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("is_read")]
        public bool IsRead { get; set; }

        [BsonElement("is_auto_reply")]
        public bool IsAutoReply { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty; // "sent", "delivered", "read"

        [BsonElement("admin_response_time")]
        public DateTime? AdminResponseTime { get; set; }

        [BsonIgnore]
        public bool IsUserMessage { get; set; }
    }

}
