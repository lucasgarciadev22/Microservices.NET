using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Core.Entities;

namespace Movies.Infrastructure.Data;

public class MovieContextSeed
{
    public static IEnumerable<Movie> Movies => GetMovies();

    public static async Task SeedAsync(
        MovieContext movieContext,
        ILoggerFactory loggerFactory,
        int retry = 0
    )
    {
        int retryForAvailability = retry;
        try
        {
            movieContext.Database.Migrate();
            if (!movieContext.Movies.Any())
            {
                movieContext.Movies.AddRange(GetMovies());
                await movieContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability < 3)
            {
                retryForAvailability++;
                ILogger<MovieContextSeed> log = loggerFactory.CreateLogger<MovieContextSeed>();

                log.LogError("Exception occured while connecting:{Message}", ex.Message);

                await SeedAsync(movieContext, loggerFactory, retryForAvailability);
            }
        }
    }

    private static IEnumerable<Movie> GetMovies()
    {
        return [
            new()
            {
                Title = "Avatar",
                DirectorName = "James Cameron",
                ReleaseYear = "2009"
            },
            new()
            {
                Title = "Titanic",
                DirectorName = "James Cameron",
                ReleaseYear = "1997"
            },
            new()
            {
                Title = "Die Another Day",
                DirectorName = "Lee Tamahori",
                ReleaseYear = "2002"
            },
            new()
            {
                Title = "Godzilla",
                DirectorName = "Gareth Edwards",
                ReleaseYear = "2014"
            }];
    }
}
