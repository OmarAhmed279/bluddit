using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bluddit.Api;

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
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string AuthorId { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.ObjectId)]
    public String? ParentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}

public class Community
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string OwnerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public List<String> ModeratorIds { get; set; } = [];
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}

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