using FavoriteMoviesSummerFest2023.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FavoriteMoviesSummerFest2023.Client.HttpRepository;
using Syncfusion.Blazor;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("FavoriteMoviesSummerFest2023.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("FavoriteMoviesSummerFest2023.ServerAPI"));
builder.Services.AddScoped<IUserHttpRepository, UserHttpRepository>();

builder.Services.AddApiAuthorization();

builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
