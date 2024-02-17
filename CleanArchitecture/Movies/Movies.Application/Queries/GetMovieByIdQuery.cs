using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Queries;

public class GetMovieByIdQuery(int id) : IRequest<MovieResponse>
{
    public int Id { get; } = id;
}
