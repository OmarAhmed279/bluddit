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

    private static async Task<IResult> GetAll(string? sortBy, int? order, int? num, IMongoCollection<Community> communities)
    {
        var result = communities.Find(x => true);
        if (sortBy == "name")
        {
            switch (order)
            {
                case 1:
                    result = result.SortBy(x => x.Name);
                    break;
                case -1:
                    result = result.SortByDescending(x => x.Name);
                    break;
                default:
                    return Results.BadRequest(new { message = "Invalid order format." });
            }
            
        }
        else if (sortBy != null)
        {
            return Results.BadRequest(new {message = "Bad SortBy format."});
        }

        var output = await result.Limit(num).ToListAsync();
        return Results.Ok(output);
    }

    private static async Task<IResult> GetById(string id, IMongoCollection<Community> communities)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
        
        var result = await communities.Find(x => x.Id == id).FirstOrDefaultAsync();
        return result != null
            ? Results.Ok(new { message = "Community found.", result }) : Results.NotFound(new { message = "Community not found." });
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