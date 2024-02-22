using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Commands;

public class CreateMovieCommand : IRequest<MovieResponse>
{
    public string Title { get; set; } = null!;
    public string DirectorName { get; set; } = null!;
    public string ReleaseYear { get; set; } = null!;
}
