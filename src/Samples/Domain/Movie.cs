using EFCore.IncludeFlags.Attributes;

namespace Samples.Domain;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int ReleaseYear { get; set; }

    public int DirectorId { get; set; }

    public int WriterId { get; set; }

    public virtual Director Director { get; set; }

    public virtual Writer Writer { get; set; }

    public virtual List<Trivia> Trivia { get; set; } = new List<Trivia>();
}

[Flags]
public enum MovieInclude
{
    [IncludeAll]
    Everything = 1 << 0,
    Director = 1 << 1,
    Writer = 1 << 2,
    Trivia = 1 << 3,
    All = 1 << 4,
    [CompositeFlag]
    People = Director | Writer
}
