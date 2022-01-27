using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models;
using System;
using System.Linq;

namespace MvcMovie.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>()))
            {
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "Wizzard of Oz",
                        ReleaseDate = DateTime.Parse("1939-8-15"),
                        Genre = "Family",
                        Price = 2.78M,
                        Rating = "PG"
                    },

                    new Movie
                    {
                        Title = "Dr. Bean",
                        ReleaseDate = DateTime.Parse("1997-8-2"),
                        Genre = "Comedy",
                        Price = 18.00M,
                        Rating = "PG"
                    },

                    new Movie
                    {
                        Title = "Sound of Music",
                        ReleaseDate = DateTime.Parse("1965-3-2"),
                        Genre = "Musical",
                        Price = 8.20M,
                        Rating = "PG"
                    },

                    new Movie
                    {
                        Title = "Chariots of Fire",
                        ReleaseDate = DateTime.Parse("1981-4-15"),
                        Genre = "Historical Drama",
                        Price = 5.50M,
                        Rating = "PG"
                    },

                    new Movie
                    {
                        Title = "Amadeus",
                        ReleaseDate = DateTime.Parse("1984-9-19"),
                        Genre = "Biographical Drama",
                        Price = 17.99M,
                        Rating = "PG"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}