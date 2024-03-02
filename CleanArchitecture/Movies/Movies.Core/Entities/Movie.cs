using Movies.Core.Entities.Base;

namespace Movies.Core.Entities;

public sealed class Movie : Entity
{
    public string Title { get; set; } = null!;
    public string DirectorName { get; set; } = null!;
    public string ReleaseYear { get; set; } = null!;
    public int Duration { get; set; }
}
