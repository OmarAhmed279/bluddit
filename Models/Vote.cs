using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bluddit.Api.Models;

public class Vote
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;
    // made both PostId and CommentId so I am always sure what kind of Id I extracted from the vote
    [BsonRepresentation(BsonType.ObjectId)]
    public string? PostId { get; set; } 
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CommentId  { get; set; }
    public int Value { get; set; } = 0; //1 -> like, -1 -> dislike
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}