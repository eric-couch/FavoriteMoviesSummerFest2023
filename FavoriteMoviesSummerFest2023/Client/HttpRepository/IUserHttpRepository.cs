using FavoriteMoviesSummerFest2023.Shared;
using FavoriteMoviesSummerFest2023.Shared.Wrapper;

namespace FavoriteMoviesSummerFest2023.Client.HttpRepository;

public interface IUserHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetMovies();
    //Task<UserDto> AddMovie(Movie movie);
    //Task<bool> RemoveMovie(Movie movie);
}
