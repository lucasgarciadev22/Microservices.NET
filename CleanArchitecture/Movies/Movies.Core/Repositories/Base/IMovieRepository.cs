using Movies.Core.Entities;

namespace Movies.Core.Repositories.Base;

public interface IMovieRepository : IRepository<Movie>
{
    Task<IEnumerable<Movie>> GetMoviesByDirectorName(string directorName);
}
