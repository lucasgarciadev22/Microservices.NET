using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Commands;
using Movies.Application.Queries;
using Movies.Application.Responses;

namespace Movies.API.Controllers;

public class MovieController(IMediator mediator) : ApiController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMoviesByDirectorName(
        string directorName
    )
    {
        GetMovieByDirectorNameQuery query = new(directorName);

        IEnumerable<MovieResponse> result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult<MovieResponse>> CreateMovie(
        [FromBody] CreateMovieCommand command
    )
    {
        MovieResponse result = await _mediator.Send(command);

        return Ok(result);
    }
}
