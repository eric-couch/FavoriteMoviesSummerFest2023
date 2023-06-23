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
    private int pageNum = 1;
    private int totalPages = 2;
    private int totalResults = 0;
    private async Task SearchOMDB()
    {
        await GetMovies();
        StateHasChanged();
    }

    private async Task NextPage()
    {
        if (pageNum < totalPages)
        {
            pageNum++;
            await GetMovies();
        }
    }

    private async Task PreviousPage()
    {
        if (pageNum > 1)
        {
            pageNum--;
            await GetMovies();
        }
    }

    private async void AddMovie(MovieSearchResultItems m)
    {
        Movie newMovie = new() { imdbId = m.imdbID };
        var res = await Http.PostAsJsonAsync("api/add-movie", newMovie);
        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine("Post to add user movie favorite failed (api/add-movie)");
        } else
        {
            Console.WriteLine("Post to add user movie favorite was successful");
        }
    }

    private async Task GetMovies()
    {
        try
        {
            MovieSearchResult? searchResults = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&page={pageNum}");
            if (searchResults is not null)
            {
                movies = searchResults.Search.AsQueryable();
                if (Double.TryParse(searchResults.totalResults, out double total))
                {
                    totalResults = (int)total;
                    totalPages = (int)Math.Ceiling(total / 10);
                }
                else
                {
                    totalPages = 1;
                }
            }
        }
        catch
        {
            Console.WriteLine("An error occured.");
        }
    }
}
