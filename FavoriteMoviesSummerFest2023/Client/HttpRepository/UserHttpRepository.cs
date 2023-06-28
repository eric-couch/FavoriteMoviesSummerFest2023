using FavoriteMoviesSummerFest2023.Shared;
using FavoriteMoviesSummerFest2023.Shared.Wrapper;
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

    public async Task<DataResponse<List<OMDBMovie>>> GetMovies()
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
                return new DataResponse<List<OMDBMovie>>() {
                    Data = MovieDetails,
                    Message = "Success",
                    Succeeded = true
                };
            }
            return new DataResponse<List<OMDBMovie>>()
            {
                Data = new List<OMDBMovie>(),
                Message = "Success",
                Succeeded = true
            };
        } catch (NotSupportedException)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Data = new List<OMDBMovie>(),
                Message = "Not Supported",
                Succeeded = false,
                Errors = new Dictionary<string, string[]> { { "Not Supported", new string[] { "This content type is not supported." } } }
            };
        }
        catch (JsonException)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Data = new List<OMDBMovie>(),
                Message = "Invalid Json",
                Succeeded = false,
                Errors = new Dictionary<string, string[]> { { "Invalid Json", new string[] { "The Json is invalid." } } }
            };
        }
        catch (Exception ex)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Data = new List<OMDBMovie>(),
                Message = ex.Message,
                Succeeded = false,
                Errors = new Dictionary<string, string[]> { { ex.Message, new string[] { ex.Message } } }
            };
        }
        
    }
}
