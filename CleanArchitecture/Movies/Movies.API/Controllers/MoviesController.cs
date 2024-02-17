using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Commands;
using Movies.Application.Queries;
using Movies.Application.Responses;

namespace Movies.API.Controllers;

public class MoviesController(IMediator mediator) : ApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Obtém os filmes pelo nome do diretor.
    /// </summary>
    /// <param name="directorName">O nome do diretor.</param>
    /// <returns>Uma lista de filmes correspondentes ao nome do diretor.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovieResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMoviesByDirectorName(
        string directorName
    )
    {
        GetMovieByDirectorNameQuery query = new(directorName);
        IEnumerable<MovieResponse> result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo filme.
    /// </summary>
    /// <param name="command">Os dados para criar o filme.</param>
    /// <returns>O filme criado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<MovieResponse>> CreateMovie(
        [FromBody] CreateMovieCommand command
    )
    {
        MovieResponse result = await _mediator.Send(command);
        return Ok(result);
    }
}
