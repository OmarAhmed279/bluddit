using Bluddit.Api.Configurations;
using Bluddit.Api.Models;
using Bluddit.Api.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bluddit.Api.Endpoints;

public static class CommunityEndpoints
{
    public static void MapCommunityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/communities");
        
        //public
        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        
        //private
        group.MapPost("/", CreateCommunity).RequireAuthorization();
        group.MapPut("/{id}", UpdateCommunity).RequireAuthorization();
        group.MapDelete("/{id}", DeleteCommunity).RequireAuthorization();
    }

    private static async Task<IResult> GetAll(string? sortBy, string? order, IMongoCollection<Community> communities)
    {
        return null;
    }

    private static async Task<IResult> GetById(string id, IMongoCollection<Community> communities)
    {
        return null;
    }

    private static async Task<IResult> CreateCommunity(CreateCommunityRequest request, IMongoCollection<Community> communities, IOptions<JwtOptions> options)
    {
        return null;
    }

    private static async Task<IResult> UpdateCommunity(string id, UpdateCommunityRequest request, IMongoCollection<Community> communities, IOptions<JwtOptions> options)
    {
        return null;
    }
    
    private static async Task<IResult> DeleteCommunity(string id, DeleteCommunityRequest request, IMongoCollection<Community> communities, IOptions<JwtOptions> options)
    {
        //TODO: actually add authorization check before deleting
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
    
        var result = await communities.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount == 0
            ? Results.NotFound(new { message = $"Community '{id}' not found." })
            : Results.Ok(new { message = "Community deleted successfully.", id });
    }
}