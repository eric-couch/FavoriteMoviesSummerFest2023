﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using FavoriteMoviesSummerFest2023.Shared;
using FavoriteMoviesSummerFest2023.Shared.Wrapper;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using FavoriteMoviesSummerFest2023.Client.HttpRepository;

namespace FavoriteMoviesSummerFest2023.Client.Pages;

public partial class Index
{
    [Inject]
    public HttpClient Http { get; set; } = new()!;
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    public IJSRuntime JS { get; set; }
    [Inject]
    public IUserHttpRepository UserHttpRepository { get; set; }
    
    public UserDto? User { get; set; }
    public List<OMDBMovie> MovieDetails { get; set; } = new List<OMDBMovie>();
    public bool IsLoading { get; set; } = true;
    public List<string> roles { get; set; } = new List<string>();
    protected override async Task OnInitializedAsync()
    {
        try {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                DataResponse<List<OMDBMovie>> dataResponse = await UserHttpRepository.GetMovies();
                if (dataResponse.Succeeded)
                {
                    MovieDetails = dataResponse.Data;
                }
                else
                {
                    await JS.InvokeVoidAsync(dataResponse.Message, dataResponse.Message);
                }
                IsLoading = false;
            }
        } catch
        {
            Console.WriteLine("An error occured.");
        }
    }

    private async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        try
        {
            var res = await Http.PostAsJsonAsync("api/remove-movie", movie.imdbID);
            if (!res.IsSuccessStatusCode)
            {
                //Console.WriteLine("Post to remove user movie favorite failed (api/remove-movie)");
                await JS.InvokeVoidAsync("userFeedback", "Post to remove user movie favorite failed (api/remove-movie)");
                await Task.CompletedTask;
            }
            else
            {
                MovieDetails.Remove(movie);
                StateHasChanged();
                await JS.InvokeVoidAsync("userFeedback", $"Removed {movie.Title} from user favorites!");
                await Task.CompletedTask;
            }
        }
        catch
        {
            Console.WriteLine("An error occured.");
        }
    }
}
