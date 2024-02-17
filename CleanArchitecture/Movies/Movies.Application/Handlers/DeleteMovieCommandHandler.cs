using MediatR;
using Movies.Application.Commands;
using Movies.Core.Entities;
using Movies.Core.Repositories;

namespace Movies.Application.Handlers;

public class DeleteMovieCommandHandler(IMovieRepository movieRepository)
    : IRequestHandler<DeleteMovieCommand, bool>
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        Movie movieToDelete =
            await _movieRepository.GetByIdAsync(request.Id)
            ?? throw new NullReferenceException("Could not find requested id");

        bool deleted = await _movieRepository.DeleteAsync(movieToDelete);

        return deleted;
    }
}
