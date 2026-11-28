using Bluddit.Api.Configurations;
using Bluddit.Api.Models;
using Bluddit.Api.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bluddit.Api.Endpoints;

public static class VoteEndpoints
{
    public static void MapVoteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/votes");
        
        //public
        //group.MapGet("/{id}", GetById); is this needed?
        
        //private
        group.MapPost("/", CreateVote).RequireAuthorization();
        group.MapPut("/{id}", UpdateVote).RequireAuthorization();
        group.MapDelete("/{id}", DeleteVote).RequireAuthorization();
    }

    /*private static async Task<IResult> GetById(string id, IMongoCollection<Vote> votes)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
        
        var result = await votes.Find(x => x.Id == id).FirstOrDefaultAsync();
        return result != null
            ? Results.Ok(new { message = "Vote found.", result }) : Results.NotFound(new { message = "Vote not found." });
    }*/

    private static async Task<IResult> GetByUserId(IMongoCollection<Vote> votes)
    {
        return null;
    }

    private static async Task<IResult> GetByPostId(IMongoCollection<Vote> votes)
    {
        return null;
    }
    
    private static async Task<IResult> CreateVote(CreateVoteRequest request, IMongoCollection<Vote> votes, IOptions<JwtOptions> options)
    {
        return null;
    }

    private static async Task<IResult> UpdateVote(string id, UpdateVoteRequest request, IMongoCollection<Vote> votes, IOptions<JwtOptions> options)
    {
        return null;
    }
    
    private static async Task<IResult> DeleteVote(string id, DeleteVoteRequest request, IMongoCollection<Vote> votes, IOptions<JwtOptions> options)
    {
        //TODO: actually add authorization check before deleting
        if (!ObjectId.TryParse(id, out _))
        {
            return Results.BadRequest(new { message = "Invalid ID format."});
        }
    
        var result = await votes.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount == 0
            ? Results.NotFound(new { message = $"Vote '{id}' not found." })
            : Results.Ok(new { message = "Vote deleted successfully.", id });
    }
}