using Microsoft.AspNetCore.Identity;
using FavoriteMoviesSummerFest2023.Shared;

namespace FavoriteMoviesSummerFest2023.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
    }
}