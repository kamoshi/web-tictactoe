using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tictactoe.Configuration;
using Tictactoe.Engine;
using Tictactoe.Models;
using Tictactoe.Models.Dtos;
using Tictactoe.Models.Entities;

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
    [Route("/join/{playerId}")]
    public async Task<ActionResult<long>> Start(long playerId)
    {
        if (_context.Game == null || _context.Player == null)
            return NotFound();

        var me = await _context.Player.FirstOrDefaultAsync(p => p.Id == playerId);
        if (me == null) return NotFound();

        var mine = await _context.Game
            .Where(g => g.State == GameState.None)
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
        await _context.SaveChangesAsync();

        return newGame.Id;
    }

    [HttpGet]
    [Route("{gameId:long}")]
    public async Task<ActionResult<GameDto>> GetGame(long gameId)
    {
        if (_context.Game == null) return NotFound();

        var game = await _context.Game
            .Include(g => g.PlayerO)
            .Include(g => g.PlayerX)
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null) return NotFound();
        
        var engine = new GameEngine(new[]{game.Top, game.Mid, game.Bot});
        
        return new GameDto(game.PlayerO?.Name, game.PlayerX?.Name, engine.GetBoardChars(), engine.CheckMove(), engine.CheckState());
    }

    [HttpPut]
    [Route("{gameId:long}")]
    public async Task<ActionResult<GameDto>> MakeMove(long gameId, [FromBody] GameMoveDto moveDto)
    {
        if (_context.Game == null) return NotFound();

        var game = await _context.Game
            .Include(g => g.PlayerO)
            .Include(g => g.PlayerX)
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null || game.PlayerO?.Id != moveDto.PlayerId && game.PlayerX?.Id != moveDto.PlayerId)
            return NotFound();
        
        var engine = new GameEngine(new[]{game.Top, game.Mid, game.Bot});
        
        if (game.State == GameState.None)
        {
            var isO = game.PlayerO?.Id == moveDto.PlayerId;
            if (isO && engine.CheckMove() == Move.X)
                return BadRequest();

            var result = engine.MakeMove(isO ? Move.O : Move.X, moveDto.Row, moveDto.Col);
            if (result == null) return BadRequest();

            var rows = engine.GetBoardRows();
            game.State = result.Value;
            game.Top = rows[0];
            game.Mid = rows[1];
            game.Bot = rows[2];
            await _context.SaveChangesAsync();
        }
        
        return new GameDto(game.PlayerO?.Name, game.PlayerX?.Name, engine.GetBoardChars(), engine.CheckMove(), engine.CheckState());
    }
    
}