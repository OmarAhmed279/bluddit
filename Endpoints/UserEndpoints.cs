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
        
        //private
        group.MapPut("/{id}", UpdateUser).RequireAuthorization();
        group.MapDelete("/{id}", DeleteUser).RequireAuthorization();
    }

    private static async Task<IResult> GetAll(string? sortBy, string? order, IMongoCollection<User> users)
    {
        return null;
    }

    private static async Task<IResult> GetById(string id, IMongoCollection<User> users)
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