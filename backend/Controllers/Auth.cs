using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tictactoe.Configuration;
using Tictactoe.Models;

namespace Tictactoe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Auth : ControllerBase
{
    private readonly ILogger<Auth> _logger;
    private readonly DatabaseContext _context;

    public Auth(ILogger<Auth> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost(Name = "Login")]
    public async Task<ActionResult<long>> Login(string name)
    {
        if (_context.Player is null)
            return NotFound();

        var player = await _context.Player.Where(p => p.Name == name).FirstOrDefaultAsync();
        if (player is null)
        {
            player = new Player(name);
            await _context.Player.AddAsync(player);
            await _context.SaveChangesAsync();
        }

        return player.Id;
    }
}
