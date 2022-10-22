namespace Tictactoe.Models.Dtos;

public record GameDto(
    string? playerO,
    string? playerX,
    char[][] board,
    Move move,
    GameState state
);
