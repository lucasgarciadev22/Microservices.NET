using Microsoft.EntityFrameworkCore;
using Movies.Core.Entities;

public sealed class MovieContext : DbContext
{
    public MovieContext() { }

    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options) { }

    public DbSet<Movie> Movies { get; set; }
}
