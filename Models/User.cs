using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bluddit.Api.Models;

public enum UserRole
{
    Normal = 0,
    Moderator = 1
}

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public int Karma { get; set; } = 0;
    public Boolean IsAdministrator { get; set; } = false; 
    public List<CommunityMembership>? CommunityMemberships { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}

public class CommunityMembership
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CommunityId { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Normal;
}