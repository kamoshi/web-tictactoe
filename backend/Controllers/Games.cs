using Microsoft.AspNetCore.Mvc;
using Tictactoe.Configuration;

namespace Tictactoe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Games : ControllerBase
{
    private readonly ILogger<Games> _logger;
    private readonly DatabaseContext _context;

    public Games(ILogger<Games> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("/{userId}")]
    public async Task<ActionResult<IEnumerable<long>>> GetGames(long userId)
    {
        if (_context.Game is null)
            return NotFound();

        return NotFound();
    }
}