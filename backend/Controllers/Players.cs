using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tictactoe.Configuration;
using Tictactoe.Models;

namespace Tictactoe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Players : ControllerBase
{
    private readonly ILogger<Games> _logger;
    private readonly DatabaseContext _context;

    public Players(ILogger<Games> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    [Route("{id:long}/games")]
    public async Task<ActionResult<IEnumerable<long>>> GetGames(long id)
    {
        if (_context.Player == null || _context.Game == null)
            return NotFound();
        
        var user = await _context.Player.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (user is null) return NotFound();

        var games = await _context.Game
            .Where(g => g.Result == GameResult.None)
            .Where(g => g.PlayerO != null && g.PlayerO.Id == id || g.PlayerX != null && g.PlayerX.Id == id)
            .Select(g => g.Id)
            .ToListAsync();

        return games;
    }

    [HttpGet]
    [Route("{id:long}/stats")]
    public async Task<ActionResult<Models.Dtos.PlayerStats>> GetStats(long id)
    {
        if (_context.Player == null || _context.Game == null)
            return NotFound();

        if (await _context.Player.Where(p => p.Id == id).FirstOrDefaultAsync() == null)
            return NotFound();
        
        var games = await _context.Game
            .Where(g => g.Result != GameResult.None)
            .Where(g => g.PlayerO != null && g.PlayerO.Id == id || g.PlayerX != null && g.PlayerX.Id == id)
            .ToListAsync();

        var asO = games.Where(g => g.PlayerO!.Id == id).ToList();
        var asX = games.Where(g => g.PlayerX!.Id == id).ToList();

        var w = asO.Count(g => g.Result == GameResult.WinO) + asX.Count(g => g.Result == GameResult.WinX);
        var l = asO.Count(g => g.Result == GameResult.WinX) + asX.Count(g => g.Result == GameResult.WinO);
        var t = games.Count(g => g.Result == GameResult.Tied);

        return new Models.Dtos.PlayerStats(w, l, t);
    }
}