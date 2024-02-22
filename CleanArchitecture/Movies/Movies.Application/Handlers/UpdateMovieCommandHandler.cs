using MediatR;
using Movies.Application.Commands;
using Movies.Core.Entities;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class UpdateMovieCommandHandler(IMovieRepository movieRepository)
    : IRequestHandler<UpdateMovieCommand, bool>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        _ =
            await _movieRepository.GetByIdAsync(request.Id)
            ?? throw new NullReferenceException("Filme não encontrado!");

        Movie movieToUpdate =
            new()
            {
                Id = request.Id,
                Title = request.Title,
                DirectorName = request.DirectorName,
                ReleaseYear = request.ReleaseYear
            };

        bool updated = await _movieRepository.UpdateAsync(movieToUpdate);

        return updated;
    }
}
