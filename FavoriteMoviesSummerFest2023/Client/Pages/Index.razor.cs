﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using FavoriteMoviesSummerFest2023.Shared;
using System.Net.Http.Json;


namespace FavoriteMoviesSummerFest2023.Client.Pages;

public partial class Index
{
    [Inject]
    public HttpClient Http { get; set; } = new()!;
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    public UserDto? User { get; set; }
    public List<OMDBMovie> MovieDetails { get; set; } = new List<OMDBMovie>();
    public bool IsLoading { get; set; } = true;
    protected override async Task OnInitializedAsync()
    {
        try {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                User = await Http.GetFromJsonAsync<UserDto>("api/get-movies");
                if (User is not null)
                {
                    foreach (var movie in User.FavoriteMovies)
                    {
                        var movieDetails = await Http.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                        if (movieDetails is not null)
                        {
                            MovieDetails.Add(movieDetails!);
                        }
                    }
                }
                IsLoading = false;
            }
        } catch
        {
            Console.WriteLine("An error occured.");
        }
    }
}
