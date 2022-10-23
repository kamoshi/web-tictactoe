namespace Tictactoe.Models.Dtos;

public record PlayerStatsDto(
    string Name,
    int Wins,
    int Loses,
    int Ties
);
