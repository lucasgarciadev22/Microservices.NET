using MediatR;
using Movies.Application.Mappers;
using Movies.Application.Queries;
using Movies.Application.Responses;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class GetMoviesByDirectorNameHandler(IMovieRepository movieRepository)
    : IRequestHandler<GetMovieByDirectorNameQuery, IEnumerable<MovieResponse>>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<IEnumerable<MovieResponse>> Handle(
        GetMovieByDirectorNameQuery request,
        CancellationToken cancellationToken
    )
    {
        var movies = await _movieRepository.GetMoviesByDirectorName(request.DirectorName);
        var movieResponses = MovieMapper.Mapper.Map<IEnumerable<MovieResponse>>(movies);

        return movieResponses;
    }
}
