using Movies.Core.Entities;

namespace Movies.Core.Helpers;

public static class MovieExtensions
{
    public static Movie ToModel(this Movie updatingMovie, Movie updateMovieRequest)
    {
        updatingMovie.Title = updateMovieRequest.Title;
        updatingMovie.DirectorName = updateMovieRequest.DirectorName;
        updatingMovie.ReleaseYear = updateMovieRequest.ReleaseYear;

        return updatingMovie;
    }
}
