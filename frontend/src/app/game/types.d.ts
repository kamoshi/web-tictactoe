interface GameDto {
  playerO: string;
  playerX: string;
  board: string[][];
  move:
    | 0   // O
    | 1;  // X
  state:
    | 0   // None
    | 1   // WinO
    | 2   // WinX
    | 3;  // Tied
}
