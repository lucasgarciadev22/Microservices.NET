using MediatR;
using Movies.Application.Commands;
using Movies.Application.Mappers;
using Movies.Core.Entities;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class UpdateMovieCommandHandler(IMovieRepository movieRepository)
    : IRequestHandler<UpdateMovieCommand, bool>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        Movie movieToUpdate = MovieMapper.Mapper.Map<Movie>(request);

        bool updated = await _movieRepository.UpdateAsync(movieToUpdate);

        return updated;
    }
}
