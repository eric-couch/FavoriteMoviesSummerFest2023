using Duende.IdentityServer.EntityFramework.Options;
using FavoriteMoviesSummerFest2023.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FavoriteMoviesSummerFest2023.Shared;

namespace FavoriteMoviesSummerFest2023.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Movie> Movies => Set<Movie>();
    }
}