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
    [Route("join")]
    public async Task<ActionResult<long>> Start([FromBody] JoinGameDto joinGameDto)
    {
        if (_context.Game == null || _context.Player == null)
            return NotFound();

        var me = await _context.Player.FirstOrDefaultAsync(p => p.Id == joinGameDto.PlayerId);
        if (me == null) return NotFound();

        var mine = await _context.Game
            .Where(g => g.State == GameState.None && (g.PlayerO!.Id == me.Id || g.PlayerX!.Id == me.Id))
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
        
        var engine = new GameEngine(game.Board, game.Size);
        
        return new GameDto(game.PlayerO?.Name, game.PlayerX?.Name, engine.GetBoardChars(), engine.CheckMove(), game.State);
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
        
        var engine = new GameEngine(game.Board, game.Size);
        
        if (game.State == GameState.None)
        {
            var isO = game.PlayerO?.Id == moveDto.PlayerId;
            var currentMove = engine.CheckMove();
            
            if ((isO && currentMove != Move.O) || !isO && currentMove == Move.O )
                return BadRequest();

            var result = engine.MakeMove(isO ? Move.O : Move.X, moveDto.Row, moveDto.Col);
            if (result == null) return BadRequest();
            
            game.State = engine.CheckState();
            game.Board = engine.Board;
            await _context.SaveChangesAsync();
        }
        
        return new GameDto(game.PlayerO?.Name, game.PlayerX?.Name, engine.GetBoardChars(), engine.CheckMove(), engine.CheckState());
    }
    
}