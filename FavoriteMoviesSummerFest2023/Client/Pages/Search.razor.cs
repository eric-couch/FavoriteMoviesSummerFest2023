using Microsoft.AspNetCore.Components;
using FavoriteMoviesSummerFest2023.Shared;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;

namespace FavoriteMoviesSummerFest2023.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; } = new()!;
    [Inject]
    public IJSRuntime JS { get; set; }
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    private string SearchTitle = String.Empty;
    private List<MovieSearchResultItems> MovieSearchResults { get; set; } = new();
    //IQueryable<MovieSearchResultItems>? movies { get; set; } = null;
    public List<MovieSearchResultItems>? SelectedMovies { get; set; }
    private SfGrid<MovieSearchResultItems>? MoviesGrid;
    private SfToast? ToastObj;
    private string toastContent = String.Empty;
    List<MovieSearchResultItems> OMDBMovies = new();

    private int pageNum = 1;
    private int totalPages = 2;
    private int totalResults = 0;
    private async Task SearchOMDB()
    {
        await GetMovies();
        StateHasChanged();
    }

    public async Task PagerClick(PagerItemClickEventArgs args)
    {
        pageNum = args.CurrentPage;
        await GetMovies();
        StateHasChanged();
    }

    public async Task GetSelectedRecords(RowSelectEventArgs<MovieSearchResultItems> args)
    {
        SelectedMovies = await MoviesGrid.GetSelectedRecordsAsync();
        //MovieSearchResultItems ItemsSelected = args.Data;
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        switch (args.Item.Id)
        {
            case "GridMoviesAdd":
                AddMovie(SelectedMovies.FirstOrDefault());
                break;
            default:
                break;
        }
    }

    private async void AddMovie(MovieSearchResultItems m)
    {
        Movie newMovie = new() { imdbId = m.imdbID };
        var res = await Http.PostAsJsonAsync("api/add-movie", newMovie);
        if (!res.IsSuccessStatusCode)
        {
            await JS.InvokeVoidAsync("userFeedback", "Post to add user movie favorite failed (api/add-movie)");
        } else
        {
            toastContent = $"Added {m.Title} to user favorites!";
            StateHasChanged();
            await Task.Delay(100);
            await ToastObj.ShowAsync();
        }
    }

    private async Task GetMovies()
    {
        try
        {
            MovieSearchResult? searchResults = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&page={pageNum}");
            if (searchResults is not null)
            {
                //movies = searchResults.Search.AsQueryable();
                OMDBMovies = searchResults.Search.ToList();
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
