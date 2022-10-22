using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tictactoe.Configuration;
using Tictactoe.Models;

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

    [HttpPost]
    [Route("/start/{playerId}")]
    public async Task<ActionResult<long>> Start(long playerId)
    {
        if (_context.Game == null || _context.Player == null)
            return NotFound();

        var ongoing = await _context.Game
            .Where(g => g.Result == GameResult.None)
            .Where(g => g.PlayerO != null && g.PlayerO.Id == playerId || g.PlayerX != null && g.PlayerX.Id == playerId)
            .ToListAsync();

        if (ongoing.Any())
            return ongoing.First().Id;
        
        
        return NotFound();
    }
}