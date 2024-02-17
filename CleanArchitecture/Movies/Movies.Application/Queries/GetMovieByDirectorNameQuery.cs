using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Queries;

public class GetMovieByDirectorNameQuery(string directorName) : IRequest<IEnumerable<MovieResponse>>
{
    public string DirectorName { get; } = directorName;
}
