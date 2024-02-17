using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Queries;

public class GetAllMoviesQuery : IRequest<IEnumerable<MovieResponse>> { }
