using MediatR;
using Movies.Application.Commands;
using Movies.Application.Mappers;
using Movies.Core.Entities;
using Movies.Core.Helpers;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class UpdateMovieCommandHandler(IMovieRepository movieRepository)
    : IRequestHandler<UpdateMovieCommand, bool>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        Movie movieToUpdate =
            await _movieRepository.GetByIdAsync(request.Id)
            ?? throw new NullReferenceException("Filme não encontrado!");

        Movie movieRequest = MovieMapper.Mapper.Map<Movie>(request);

        movieToUpdate.ToModel(movieRequest);

        bool updated = await _movieRepository.UpdateAsync(movieToUpdate);

        return updated;
    }
}
