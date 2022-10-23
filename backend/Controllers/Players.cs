using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tictactoe.Configuration;
using Tictactoe.Engine;
using Tictactoe.Models;
using Tictactoe.Models.Dtos;

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
        if (_context.Player == null || _context.Game == null) return NotFound();
        
        // var user = await _context.Player.Where(p => p.Id == id).FirstOrDefaultAsync();
        // if (user is null) return NotFound();

        var games = await _context.Game
            .Where(g => g.State == GameState.None && (g.PlayerO!.Id == id || g.PlayerX!.Id == id))
            .Select(g => g.Id)
            .ToListAsync();

        return games;
    }

    [HttpGet]
    [Route("{id:long}/stats")]
    public async Task<ActionResult<Models.Dtos.PlayerStatsDto>> GetStats(long id)
    {
        if (_context.Player == null || _context.Game == null) return NotFound();

        var user = await _context.Player.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (user == null) return NotFound();
        
        var games = await _context.Game
            .Where(g => g.State != GameState.None && (g.PlayerO!.Id == id || g.PlayerX!.Id == id))
            .ToListAsync();

        var asO = games.Where(g => g.PlayerO?.Id == id).ToList();
        var asX = games.Where(g => g.PlayerX?.Id == id).ToList();

        var w = asO.Count(g => g.State == GameState.WinO) + asX.Count(g => g.State == GameState.WinX);
        var l = asO.Count(g => g.State == GameState.WinX) + asX.Count(g => g.State == GameState.WinO);
        var t = games.Count(g => g.State == GameState.Tied);

        return new Models.Dtos.PlayerStatsDto(user.Name, w, l, t);
    }

    [HttpGet]
    [Route("leaderboard")]
    public async Task<ActionResult<IEnumerable<LeaderboardDto>>> GetLeaderboard()
    {
        if (_context.Game == null || _context.Player == null) return NotFound();

        var winsAsO = from g in _context.Game
            where g.State == GameState.WinO && g.PlayerO != null
            join p in _context.Player on g.PlayerO.Id equals p.Id
            group g by new { g.PlayerO.Name }
            into gp
            select new { gp.Key.Name, Wins = gp.Count() };

        var winsAsX = from g in _context.Game
            where g.State == GameState.WinO && g.PlayerO != null
            join p in _context.Player on g.PlayerO.Id equals p.Id
            group g by new { g.PlayerO.Name }
            into gp
            select new { gp.Key.Name, Wins = gp.Count() };

        return await winsAsO.Union(winsAsX)
            .GroupBy(e => new {e.Name})
            .Select(g => new LeaderboardDto(g.Key.Name, g.Sum(x => x.Wins)))
            .ToListAsync();
    }
}