using System.Text;
using Bluddit.Api.Configurations;
using Bluddit.Api.Models;
using Bluddit.Api.Endpoints;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbUser = Environment.GetEnvironmentVariable("MONGODB_USERNAME") ?? throw new InvalidOperationException("MONGODB_USERNAME is missing.");
var dbPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD") ?? throw new InvalidOperationException("MONGODB_PASSWORD is missing.");
var dbClusterUrl = Environment.GetEnvironmentVariable("MONGODB_URI") ?? throw new InvalidOperationException("MONGODB_URI is missing.");
var databaseName = "BludditDb";

var jwt = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET is missing.");

builder.Services.Configure<MongoDbOptions>(options =>
{
    options.DbUsername = dbUser;
    options.DbPassword = dbPassword;
    options.DbClusterUrl = dbClusterUrl;
}); //probably useless for now

builder.Services.Configure<JwtOptions>(options =>
{
    options.Secret = jwt;
    options.ExpiresInHours = 2;
});

var connectionString = $"mongodb+srv://{dbUser}:{dbPassword}@{dbClusterUrl}/{databaseName}";

var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(databaseName);

builder.Services.AddSingleton(database);
builder.Services.AddSingleton(database.GetCollection<User>("users"));
builder.Services.AddSingleton(database.GetCollection<Post>("posts"));
builder.Services.AddSingleton(database.GetCollection<Comment>("comments"));
builder.Services.AddSingleton(database.GetCollection<Community>("communities"));
builder.Services.AddSingleton(database.GetCollection<Vote>("votes"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt!))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(); 
}

app.MapAuthEndpoints(); //LOGIN & SIGNUP
app.MapUserEndpoints();
app.MapPostEndpoints();
app.MapCommentEndpoints();
app.MapCommunityEndpoints();
app.MapVoteEndpoints();

app.Run();