using FavoriteMoviesSummerFest2023.Shared;

namespace FavoriteMoviesSummerFest2023.Client.HttpRepository;

public interface IUserHttpRepository
{
    Task<List<OMDBMovie>?> GetMovies();
    //Task<UserDto> AddMovie(Movie movie);
    //Task<bool> RemoveMovie(Movie movie);
}
