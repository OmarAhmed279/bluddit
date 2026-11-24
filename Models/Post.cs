using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bluddit.Api.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string AuthorId { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.ObjectId)]
    public string CommunityId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Upvotes { get; set; } = 0;
    public int Downvotes { get; set; } = 0;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}