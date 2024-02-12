using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Commands;

public class CreateMovieCommand : IRequest<MovieResponse> { }
