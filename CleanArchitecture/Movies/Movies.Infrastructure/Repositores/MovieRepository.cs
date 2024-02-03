using Microsoft.EntityFrameworkCore;
using Movies.Core.Entities;
using Movies.Core.Repositories;
using Movies.Infrastructure.Data;
using Movies.Infrastructure.Repositores.Base;

namespace Movies.Infrastructure.Repositores;

public class MovieRepository(MovieContext movieContext)
    : Repository<Movie>(movieContext),
        IMovieRepository
{
    public async Task<IEnumerable<Movie>> GetMoviesByDirectorName(string directorName) =>
        await _movieContext.Movies.Where(m => m.DirectorName.Equals(directorName)).ToListAsync();
}
