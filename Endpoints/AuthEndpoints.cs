using Bluddit.Api.Models;
using Bluddit.Api.Auth;
using Bluddit.Api.Configurations;
using Bluddit.Api.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bluddit.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", Register);
        app.MapPost("auth/login", Login);
    }

    private static async Task<IResult> Register(RegisterRequest request, IMongoCollection<User> users)
    {
        var exists = await users.Find(x => x.Username == request.Username).AnyAsync();
        if (exists)
        {
            return Results.Conflict(new { message = "Username already exists." });
        }
    
        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };
    
        await users.InsertOneAsync(user);
    
        return Results.Created($"/users/{user.Id}", new { user.Id, user.Username });
    }

    private static async Task<IResult> Login(LoginRequest request, IMongoCollection<User> users, IOptions<JwtOptions> options)
    {
        var user = await users.Find(x => x.Username == request.Username).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }
    
        return Results.Ok(new { token = TokenService.CreateToken(user, options.Value.Secret) });
    }
}