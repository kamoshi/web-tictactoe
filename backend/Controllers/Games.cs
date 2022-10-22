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

        var me = await _context.Player.FirstOrDefaultAsync(p => p.Id == playerId);
        if (me == null) return NotFound();

        var mine = await _context.Game
            .Where(g => g.Result == GameResult.None)
            .Where(g => g.PlayerO != null && g.PlayerO.Id == playerId || g.PlayerX != null && g.PlayerX.Id == playerId)
            .FirstOrDefaultAsync();
        if (mine != null) return mine.Id;

        var other = await _context.Game.Where(g => g.PlayerX == null).FirstOrDefaultAsync();
        if (other != null)
        {
            other.PlayerX = me;
            await _context.SaveChangesAsync();
            return other.Id;
        }
        
        var newGame = new Game(me);
        await _context.Game.AddAsync(newGame);

        return newGame.Id;
    }
}