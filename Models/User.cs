using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bluddit.Api.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public int Karma { get; set; } = 0;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}