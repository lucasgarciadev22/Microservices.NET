using MediatR;
using Movies.Application.Queries;
using Movies.Application.Responses;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class GetAllMoviesQueryHandler(IMovieRepository movieRepository)
    : IRequestHandler<GetAllMoviesQuery, IEnumerable<MovieResponse>>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<IEnumerable<MovieResponse>> Handle(
        GetAllMoviesQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<MovieResponse> movies =
            (IEnumerable<MovieResponse>)await _movieRepository.GetAllAsync();

        return movies;
    }
}
