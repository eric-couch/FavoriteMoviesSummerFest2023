using Microsoft.AspNetCore.Components;
using FavoriteMoviesSummerFest2023.Shared;

namespace FavoriteMoviesSummerFest2023.Client.Pages;

public partial class MovieDetails
{
    [Parameter]
    public OMDBMovie Movie { get; set; }

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIPosterURL = "http://img.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
}
