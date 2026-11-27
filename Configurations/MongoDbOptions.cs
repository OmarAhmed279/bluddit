namespace Bluddit.Api.Configurations;

public record MongoDbOptions
{
    public string DbName { get; set; } = "BludditDb";
    public string DbUsername { get; set; } = String.Empty;
    public string DbPassword { get; set; } = String.Empty;
    public string DbClusterUrl { get; set; } = String.Empty;
} //this might be useless for now