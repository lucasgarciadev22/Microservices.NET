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
    /// Obtém todos os filmes.
    /// </summary>
    /// <returns>Uma lista de todos os filmes.</returns>
    [HttpGet("movies/")]
    [ProducesResponseType(typeof(IEnumerable<MovieResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<MovieResponse>>> GetAllMovies()
    {
        GetAllMoviesQuery query = new();
        IEnumerable<MovieResponse> result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Obtém os filmes pelo id.
    /// </summary>
    /// <param name="id">O id do filme no banco de dados.</param>
    /// <returns>O filme com o id correspondente</returns>
    [HttpGet("/movies/{id}")]
    [ProducesResponseType(typeof(MovieResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<MovieResponse>> GetMovieById(int id)
    {
        GetMovieByIdQuery query = new(id);
        MovieResponse result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Obtém os filmes pelo nome do diretor.
    /// </summary>
    /// <param name="directorName">O nome do diretor.</param>
    /// <returns>Uma lista de filmes correspondentes ao nome do diretor.</returns>
    [HttpGet("movies/directors/{directorName}")]
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
    /// Atualiza um filme existente.
    /// </summary>
    /// <param name="id">O ID do filme a ser atualizado.</param>
    /// <param name="command">Os dados para atualizar o filme.</param>
    /// <returns>True se foi atualizado, false se não</returns>
    [HttpPut("movies/update/{id}")]
    [ProducesResponseType(typeof(MovieResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<MovieResponse>> UpdateMovie(
        int id,
        [FromBody] UpdateMovieCommand command
    )
    {
        command.Id = id;
        bool result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Deleta um filme existente.
    /// </summary>
    /// <param name="id">O ID do filme a ser deletado.</param>
    /// <returns>True se foi deletado, false se não</returns>
    [HttpDelete("movies/delete/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        DeleteMovieCommand command = new(id);
        bool result = await _mediator.Send(command);

        return Ok(result);
    }
}
