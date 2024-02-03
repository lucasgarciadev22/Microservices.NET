using Microsoft.EntityFrameworkCore;
using Movies.Core.Entities;

namespace Movies.Infrastructure.Data;

public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
}
