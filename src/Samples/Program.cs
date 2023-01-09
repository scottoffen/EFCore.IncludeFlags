// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using EFCore.IncludeFlags;
using Samples.Database;
using Samples.Domain;

var options = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true
};

using (var context = new MoviesContext(DbProviderType.InMemory))
{
    context.Database.EnsureCreated();

    var flags = MovieInclude.Everything;

    var movie = context.Movies
        .Where(m => m.Id == 1)
        .IncludeFlags(flags)
        .Single();

    var json = JsonSerializer.Serialize(movie, options);
    Console.WriteLine(json);
}
