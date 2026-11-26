namespace Bluddit.Api.Requests;

//TODO: finish PostRequests.cs
public record RegisterRequest
{
    public required string Username {get; init;}
    public required string Password {get; init;}
}

public record LoginRequest
{
    public required string Username {get; init;}
    public required string Password {get; init;}
}