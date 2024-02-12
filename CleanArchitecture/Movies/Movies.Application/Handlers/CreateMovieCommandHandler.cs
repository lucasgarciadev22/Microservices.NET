using MediatR;
using Movies.Application.Commands;
using Movies.Application.Mappers;
using Movies.Application.Responses;
using Movies.Core.Entities;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class CreateMovieCommandHandler(IMovieRepository movieRepository)
    : IRequestHandler<CreateMovieCommand, MovieResponse>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<MovieResponse> Handle(
        CreateMovieCommand request,
        CancellationToken cancellationToken
    )
    {
        Movie movieToAdd =
            MovieMapper.Mapper.Map<Movie>(request)
            ?? throw new NullReferenceException("Could not resolve the mapping...");

        Movie addedMovie = await _movieRepository.AddAsync(movieToAdd);

        MovieResponse response = MovieMapper.Mapper.Map<MovieResponse>(addedMovie);
        return response;
    }
}
