using Microsoft.AspNetCore.Components;
using FavoriteMoviesSummerFest2023.Shared;
using System.Net.Http.Json;

namespace FavoriteMoviesSummerFest2023.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; } = new()!;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    private string SearchTitle = String.Empty;
    private List<MovieSearchResultItems> MovieSearchResults { get; set; } = new();
    IQueryable<MovieSearchResultItems>? movies { get; set; } = null;
    private async Task SearchOMDB()
    {
        try
        {
            MovieSearchResult? searchResults = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}");
            if (searchResults is not null)
            {
                movies = searchResults.Search.AsQueryable();
                // need to do something for pagination
            }
        }
        catch
        {
            Console.WriteLine("An error occured.");
        }
    }
}
