using Microsoft.AspNetCore.Mvc;
using FavoriteMoviesSummerFest2023.Server.Models;
using FavoriteMoviesSummerFest2023.Server.Data;
using FavoriteMoviesSummerFest2023.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public IActionResult Index()
    {
        return View();
    }
}
