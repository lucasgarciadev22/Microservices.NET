using MediatR;

namespace Movies.Application.Commands;

public class DeleteMovieCommand(int id) : IRequest<bool>
{
    public int Id { get; } = id;
}
