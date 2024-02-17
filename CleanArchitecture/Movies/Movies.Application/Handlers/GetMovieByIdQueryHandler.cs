using MediatR;
using Movies.Application.Mappers;
using Movies.Application.Queries;
using Movies.Application.Responses;
using Movies.Core.Entities;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class GetMovieByIdQueryHandler(IMovieRepository movieRepository)
    : IRequestHandler<GetMovieByIdQuery, MovieResponse>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<MovieResponse> Handle(
        GetMovieByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        Movie movieToGet = await _movieRepository.GetByIdAsync(request.Id);

        MovieResponse response =
            MovieMapper.Mapper.Map<MovieResponse>(movieToGet)
            ?? throw new NullReferenceException("Could not resolve mapping...");

        return response;
    }
}
