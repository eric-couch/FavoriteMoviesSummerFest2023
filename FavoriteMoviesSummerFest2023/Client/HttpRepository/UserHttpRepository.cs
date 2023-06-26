using FavoriteMoviesSummerFest2023.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace FavoriteMoviesSummerFest2023.Client.HttpRepository;

public class UserHttpRepository : IUserHttpRepository
{
    public readonly HttpClient _httpClient;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";

    public UserHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OMDBMovie>?> GetMovies()
    {
        try
        {
            var MovieDetails = new List<OMDBMovie>();
            UserDto User = await _httpClient.GetFromJsonAsync<UserDto>("api/get-movies");
            if (User?.FavoriteMovies?.Any() ?? false)
            {
                foreach (var movie in User.FavoriteMovies)
                {
                    var movieDetails = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    MovieDetails.Add(movieDetails);
                }
                return MovieDetails;
            }
            return new List<OMDBMovie>();
        } catch (Exception ex)
        {
            return null;
        }
        
    }
}
