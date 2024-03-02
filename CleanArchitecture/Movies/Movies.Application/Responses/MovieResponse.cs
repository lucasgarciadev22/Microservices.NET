namespace Movies.Application.Responses;

public class MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string DirectorName { get; set; } = null!;
    public string ReleaseYear { get; set; } = null!;
    public int Duration { get; set; }
}
