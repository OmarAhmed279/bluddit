using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bluddit.Api.Models;

public class Community
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string OwnerId { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.ObjectId)]
    public List<String> ModeratorIds { get; set; } = [];
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}