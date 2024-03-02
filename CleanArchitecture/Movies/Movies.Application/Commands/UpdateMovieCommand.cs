using MediatR;

namespace Movies.Application.Commands;

public class UpdateMovieCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = null!;
    public string DirectorName { get; set; } = null!;
    public string ReleaseYear { get; set; } = null!;
    public int Duration { get; set; }
}
