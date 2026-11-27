using System.ComponentModel.DataAnnotations;

namespace Bluddit.Api.Requests;

//TODO: finish PostRequests.cs
public record CreatePostRequest
{
    public required string CommunityId { get; init; }
    
    [StringLength(300, MinimumLength = 1)]
    public required string Title {get; init; }
    [StringLength(40000)]
    public required string Body {get; init; }
}

public record DeletePostRequest();
public record UpdatePostRequest();