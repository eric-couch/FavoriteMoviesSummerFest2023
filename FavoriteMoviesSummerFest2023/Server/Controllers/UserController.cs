using Microsoft.AspNetCore.Mvc;
using FavoriteMoviesSummerFest2023.Server.Models;
using FavoriteMoviesSummerFest2023.Server.Data;
using FavoriteMoviesSummerFest2023.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FavoriteMoviesSummerFest2023.Server.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("api/get-movies")]
    public async Task<ActionResult<UserDto>> GetMovies()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var movies = await _context.Users
                     .Include(m => m.FavoriteMovies)
                     .Select(u => new UserDto
                     {
                         Id = u.Id,
                         UserName = u.UserName!,
                         FavoriteMovies = u.FavoriteMovies
                     }).FirstOrDefaultAsync(u => u.Id == user.Id);
        
        if (movies is null)
        {
            return NotFound();
        }

        return Ok(movies);
    }

    [HttpPost("api/add-movie")]
    public async Task<ActionResult> AddMovie([FromBody] Movie movie)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user is  null) {
            return NotFound();
        }
        user.FavoriteMovies.Add(movie);
        await _userManager.UpdateAsync(user);

        return Ok();
    }

    [HttpPost("api/remove-movie")]
    public async Task<ActionResult> RemoveMovie([FromBody] string imdbId)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user is null)
        {
            return NotFound();
        }
        //var movie = user.FavoriteMovies.FirstOrDefault(m => m.imdbId == imdbId);
        var movie = _context.Users.Include(u => u.FavoriteMovies)
                    .FirstOrDefault(u => u.Id == user.Id)?.FavoriteMovies
                    .FirstOrDefault(m => m.imdbId == imdbId);
        if (movie is null)
        {
            return NotFound();
        }
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        //user.FavoriteMovies.Remove(movie);
        //await _userManager.UpdateAsync(user);

        return Ok();
    }

    [HttpGet]
    [Route("api/get-roles")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> GetUserRoles()
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user is not null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            } else
            {
                return NotFound();
            }

        } catch (Exception ex)
        {
            return Problem(
                // all params are optional
                detail: "Error while retrieving roles.",    // explanation of issue
                title: "An error occured",
                statusCode: StatusCodes.Status500InternalServerError
                );
        }
    }

    public IActionResult Index()
    {
        return View();
    }
}
