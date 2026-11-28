using Bluddit.Api.Configurations;
using Bluddit.Api.Models;
using Bluddit.Api.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bluddit.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");
        
        //public
        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapGet("/{id}/vote", GetVoteByUserId);
        
        //private
        group.MapPut("/{id}", UpdateUser).RequireAuthorization();
        group.MapDelete("/{id}", DeleteUser).RequireAuthorization();
    }

    private static async Task<IResult> GetAll(string? sortBy, int? order, int? num, IMongoCollection<User> users)
    {
        var result = users.Find(x => true);
        switch (sortBy)
        {
            case "karma":
                switch (order)
                {
                    case 1:
                        result.SortBy(x => x.Karma);
                        break;
                    case -1:
                        result.SortByDescending(x => x.Karma);
                        break;
                    default:
                        return Results.BadRequest(new { message = "Invalid order format."});
                }
                break;
            case "username":
                switch (order)
                {
                    case 1:
                        result.SortBy(x => x.Username);
                        break;
                    case -1:
                        result.SortByDescending(x => x.Username);
                        break;
                    default:
                        return Results.BadRequest(new { message = "Invalid order format."});
                }
                break;
            case "date":
                switch (order)
                {
                    case 1:
                        result.SortBy(x => x.CreationDate);
                        break;
                    case -1:
                        result.SortByDescending(x => x.CreationDate);
                        break;
                    default:
                        return Results.BadRequest(new { message = "Invalid order format."});
                }
                break;
            case null:
                break;
            default:
                return Results.BadRequest(new { message = "Invalid sort format."});
        }

        var output = await result.Limit(num).ToListAsync();
        return Results.Ok(output);
    }

    private static async Task<IResult> GetById(string id, IMongoCollection<User> users)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
        
        var result = await users.Find(x => x.Id == id).FirstOrDefaultAsync();
        return result != null
            ? Results.Ok(new { message = "User found.", result }) : Results.NotFound(new { message = "User not found." });
    }

    private static async Task<IResult> GetVoteByUserId(string id, IMongoCollection<User> users)
    {
        return null;
    }
    
    private static async Task<IResult> UpdateUser(string id, UpdateUserRequest request, IMongoCollection<User> users, IOptions<JwtOptions> options)
    {
        return null;
    }
    
    private static async Task<IResult> DeleteUser(string id, DeleteUserRequest request, IMongoCollection<User> users, IOptions<JwtOptions> options)
    {
        //TODO: actually add authorization check before deleting
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
    
        var result = await users.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount == 0
            ? Results.NotFound(new { message = $"user '{id}' not found." })
            : Results.Ok(new { message = "user deleted successfully.", id });
    }
}