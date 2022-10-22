namespace Tictactoe.Models.Dtos;

public record GameMoveDto(
    long PlayerId,
    int Row,
    int Col
);